using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using API.Models;
using AutoMapper.QueryableExtensions;
using Common;
using Common.Exception;
using Common.Utilities;
using Data.Repositories;
using Data.Repositories.UserRepositories;
using Entities.Framework;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using Services;
using Services.Services.Services;
using UAParser;
using WebFramework.Api;
using WebFramework.Filters;

namespace API.Controllers.v1.Basic
{
    /// <summary>
    /// Users Actions
    /// </summary>
    [ApiVersion("1")]
    public class UsersController : BaseController
    {
        private readonly IUserRepository _userRepository;
        private readonly IJwtService _jwtService;
        private readonly IMemoryCache _memoryCache;
        private readonly IBaseRepository<LoginLog> _loginlogRepository;
        private readonly IBaseRepository<Permission> _permissionRepository;
        private readonly IBaseRepository<UserToken> _userTokenRepository;
        private readonly IBaseRepository<Menu> _menuRepository;
        private readonly IBaseRepository<ConfigData> _configDataRepository;
        private readonly IBaseRepository<UserGroup> _userGroupRepository;

        /// <inheritdoc />
        public UsersController(IUserRepository userRepository, IJwtService jwtService, IMemoryCache memoryCache, IBaseRepository<LoginLog> loginlogRepository, IBaseRepository<Permission> permissionRepository,
            IBaseRepository<UserToken> userTokenRepository, IBaseRepository<Menu> menuRepository, IBaseRepository<ConfigData> configDataRepository, IBaseRepository<UserGroup> userGroupRepository)
        {
            _userRepository = userRepository;
            _jwtService = jwtService;
            _memoryCache = memoryCache;
            _loginlogRepository = loginlogRepository;
            _permissionRepository = permissionRepository;
            _userTokenRepository = userTokenRepository;
            _menuRepository = menuRepository;
            _configDataRepository = configDataRepository;
            _userGroupRepository = userGroupRepository;
        }

        /// <summary>
        /// This method is Login 
        /// </summary>
        /// <param name="loginDto"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [AxAuthorize(StateType = StateType.Ignore)]
        [HttpPost("[action]")]
        public async Task<ApiResult<AccessToken>> AxToken(LoginDto loginDto, CancellationToken cancellationToken)
        {
            var passwordHash = SecurityHelper.GetSha256Hash(loginDto.Password);
            var user = await _userRepository.GetFirstAsync(x => x.UserName == loginDto.Username && x.Password == passwordHash, cancellationToken);

            var address = Request.HttpContext.Connection.RemoteIpAddress;
            var computerName = address.GetDeviceName();
            var ip = address.GetIp();

            #region loginLog And configLoad

            var config = _memoryCache.GetOrCreate(CacheKeys.ConfigData, entry =>
            {
                var dataDto = _configDataRepository.TableNoTracking.ProjectTo<ConfigDataDto>().FirstOrDefault(x => x.Active);
                if (dataDto == null)
                    throw new NotFoundException("تنظیمات اولیه سامانه به درستی انجام نشده است");
                return dataDto;
            });
            var userAgent = Request.Headers["User-Agent"].ToString();
            var uaParser = Parser.GetDefault();
            var info = uaParser.Parse(userAgent);
            var loginlog = new LoginLog
            {
                AppVersion = config.VersionName,
                Browser = info.UA.Family,
                BrowserVersion = info.UA.Major + "." + info.UA.Minor,
                UserId = user?.Id,
                CreatorUserId = user?.Id ?? 0,
                InvalidPassword = user == null ? loginDto.Password : null,
                Ip = ip,
                MachineName = computerName,
                Os = info.Device + " " + info.OS,
                UserName = loginDto.Username,
                ValidSignIn = user != null
            };
            await _loginlogRepository.AddAsync(loginlog, cancellationToken);
            #endregion

            if (user == null)
                return new ApiResult<AccessToken>(false, ApiResultStatusCode.UnAuthenticated, null, "نام کاربری و یا رمز عبور اشتباه است");

            var clientId = Guid.NewGuid().ToString();
            var token = await _jwtService.GenerateAsync(user, clientId);

            var userToken = new UserToken
            {
                Active = true,
                Token = token.access_token,
                UserAgent = userAgent,
                Ip = ip,
                DeviceName = computerName,
                UserId = user.Id,
                ClientId = clientId,
                CreatorUserId = user.Id,
                InsertDateTime = DateTime.Now,
                Browser = info.UA.ToString(),
                ExpireDateTime = DateTime.Now.AddSeconds(token.expires_in)
            };

            //Response.Cookies.Append("AxToken", token.access_token);
            await _userTokenRepository.AddAsync(userToken, cancellationToken);

            await Task.Run(() =>
            {
                var oldTokens = _userTokenRepository.GetAll(t => t.ExpireDateTime < DateTime.Now);
                _userTokenRepository.DeleteRange(oldTokens);
            }, cancellationToken);


            var keys = _memoryCache.Get("user" + user.Id);
            if (keys == null)
            {
                await Task.Run(() =>
                {
                    var hashSet = new HashSet<string>();
                    var userPermissions = _permissionRepository.GetAll(x => x.Access && x.UserId == user.Id).Include(x => x.Menu);
                    foreach (var item in userPermissions)
                    {
                        if (!string.IsNullOrWhiteSpace(item.Menu.Key))
                            hashSet.Add(item.Menu.Key);
                    }

                    var userGroups = _userGroupRepository.GetAll(x => x.UserId == user.Id);
                    foreach (var item in userGroups)
                    {
                        var groupPermissions = _permissionRepository.GetAll(x => x.GroupId == item.GroupId && x.Access)
                            .Include(x => x.Menu);
                        foreach (var groupPermission in groupPermissions)
                        {
                            if (!string.IsNullOrWhiteSpace(groupPermission.Menu.Key))
                                hashSet.Add(groupPermission.Menu.Key);
                        }
                    }

                    var userDenied = _permissionRepository.GetAll(x => x.UserId == user.Id && !x.Access)
                        .Select(x => x.Menu.Key);
                    foreach (var item in userDenied)
                    {
                        hashSet.Remove(item);
                    }

                    //var NotShowInTreeKeys = _permissionRepository.GetAll(x => !x.ShowInTree && keys.Contains(x.ParentKey) && !x.Key.Contains("GetList")).ToList().Select(x=>x.Key);
                    //foreach (var item in NotShowInTreeKeys)
                    //{
                    //    keys.Add(item);
                    //}

                    _memoryCache.Set("user" + user.Id, hashSet);

                }, cancellationToken);
            }
            return token;
        }


        /// <summary>
        /// SignOut user and Remove user Token from Db
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        /// <exception cref="UnauthorizedAccessException"></exception>
        [HttpGet("[action]")]
        [AxAuthorize(StateType = StateType.OnlyToken)]
        public async Task<ApiResult> SignOut(CancellationToken cancellationToken)
        {
            var clientId = User.Identity.GetClientId();
            var userToken = await _userTokenRepository.GetFirstAsync(x => x.ClientId == clientId, cancellationToken);

            if (userToken == null)
                throw new UnauthorizedAccessException("کاربر یافت نشد");

            await _userTokenRepository.DeleteAsync(userToken, cancellationToken);
            return Ok();
        }

        /// <summary>
        /// Get Init Information For Main Panel
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        /// <exception cref="UnauthorizedAccessException"></exception>
        [HttpGet("[action]")]
        [AxAuthorize(StateType = StateType.OnlyToken)]
        public async Task<ApiResult<UserInfo>> GetInitData(CancellationToken cancellationToken)
        {
            var user = await _userRepository.GetFirstAsync(x => x.Id == UserId, cancellationToken);
            _userRepository.LoadReference(user, t => t.UserSettings);


            if (user == null)
                return new ApiResult<UserInfo>(false, ApiResultStatusCode.NotFound, null, "کاربر یافت نشد");


            var config = _memoryCache.GetOrCreate(CacheKeys.ConfigData, entry =>
            {
                var dataDto = _configDataRepository.TableNoTracking.ProjectTo<ConfigDataDto>().FirstOrDefault(x => x.Active);
                if (dataDto == null)
                    throw new NotFoundException("تنظیمات اولیه سامانه به درستی انجام نشده است");
                return dataDto;
            });


            //var menus = _menuRepository.GetAll(x => x.Active && x.ParentId == null).ProjectTo<AxSystem>();

            _userRepository.LoadReference(user, t => t.UserSettings);
            var userInfo = new UserInfo
            {
                UserName = user.UserName,
                DateTimeNow = DateTime.Now.ToPerDateString(),
                OrganizationName = config.OrganizationName,
                OrganizationLogo = "/api/v1/General/GetOrganizationLogo",
                UserPicture = "/api/v1/users/GetUserPicture",
                UnReedMsgCount = 0,
                UserTheme = user.UserSettings?.Theme,
                UserDisplayName = user.FullName,
                VersionName = config.VersionName,
                DefaultSystemId = user.UserSettings?.DefaultSystemId,
                SystemsList = _menuRepository.GetAll(x => x.Active && x.ParentId == null).ProjectTo<AxSystem>()
            };
            return userInfo;
        }


        /// <summary>
        /// Get Signed User Permission keys
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpGet("[action]")]
        [AxAuthorize(StateType = StateType.OnlyToken)]
        public ApiResult<List<string>> GetUserPermissions(CancellationToken cancellationToken)
        {
            var keys = _memoryCache.Get<HashSet<string>>("user" + UserId).ToList();
            return keys;
        }

        /// <summary>
        /// Get All Users
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [AxAuthorize(StateType = StateType.Authorized, Order = 0, AxOp = AxOp.UserList, ShowInMenu = true)]
        public ApiResult<IQueryable<UserSelectDto>> Get([FromQuery] DataRequest request)
        {
            var predicate = request.GetFilter<User>();
            var users = _userRepository.GetAll(predicate).Skip(request.PageIndex * request.PageSize).Take(request.PageSize).OrderBy(request.Sort, request.SortType).ProjectTo<UserSelectDto>();
            return Ok(users);
        }

        /// <summary>
        /// Get User Instance By Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("{id}")]
        [AxAuthorize(StateType = StateType.Authorized, Order = 1, AxOp = AxOp.UserItem)]
        public ApiResult<UserSelectDto> Get(int id)
        {
            var user = _userRepository.GetAll(x => x.Id == id).ProjectTo<UserSelectDto>().FirstOrDefault();
            return Ok(user);
        }

    }

}