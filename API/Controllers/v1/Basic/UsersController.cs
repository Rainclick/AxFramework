using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using API.Hubs;
using API.Models;
using AutoMapper.QueryableExtensions;
using Common;
using Common.Exception;
using Common.Utilities;
using Data.Repositories;
using Data.Repositories.UserRepositories;
using Entities.Framework;
using Entities.Framework.AxCharts;
using Entities.Framework.Reports;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using Services;
using Services.Services.Services;
using UAParser;
using WebFramework.Api;
using WebFramework.Filters;
using WebFramework.UserData;

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
        private readonly IBaseRepository<UserConnection> _userConnectionRepository;
        private readonly IBaseRepository<AxChart> _chartRepository;
        private readonly IBaseRepository<BarChart> _barChartRepository;
        private readonly IBaseRepository<NumericWidget> _numberWidgetRepository;
        private readonly IHubContext<AxHub> _hub;

        /// <inheritdoc />
        public UsersController(IUserRepository userRepository, IJwtService jwtService, IMemoryCache memoryCache, IBaseRepository<LoginLog> loginlogRepository, IBaseRepository<Permission> permissionRepository,
            IBaseRepository<UserToken> userTokenRepository, IBaseRepository<Menu> menuRepository, IBaseRepository<ConfigData> configDataRepository,
            IBaseRepository<UserGroup> userGroupRepository, IBaseRepository<UserConnection> userConnectionRepository, IBaseRepository<AxChart> chartRepository, IBaseRepository<BarChart> barChartRepository, IBaseRepository<NumericWidget> numberWidgetRepository, IHubContext<AxHub> hub)
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
            _userConnectionRepository = userConnectionRepository;
            _chartRepository = chartRepository;
            _barChartRepository = barChartRepository;
            _numberWidgetRepository = numberWidgetRepository;
            _hub = hub;
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
                ValidSignIn = user != null,
                InsertDateTime = DateTime.Now
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
                ExpireDateTime = token.expires_in.UnixTimeStampToDateTime()
            };

            //Response.Cookies.Append("AxToken", token.access_token);
            await _userTokenRepository.AddAsync(userToken, cancellationToken);

            await Task.Run(() =>
            {
                var oldTokens = _userTokenRepository.GetAll(t => t.ExpireDateTime < DateTime.Now);
                _userTokenRepository.DeleteRange(oldTokens);
            }, cancellationToken);


            var connections = _userConnectionRepository.GetAll(x => x.Active).Select(x => x.ConnectionId).ToList();
            var barChart = _barChartRepository.GetAll(x => x.AxChartId == 5).ProjectTo<BarChartDto>().FirstOrDefault();
            if (barChart != null && barChart.Series?.Count > 0)
            {
                var date = DateTime.Now.AddDays(-15);
                var data0 = _loginlogRepository.GetAll(x => x.InsertDateTime.Date >= date.Date).ToList()
                    .GroupBy(x => x.InsertDateTime.Date).OrderBy(x => x.Key).Select(x => new
                    { Count = x.Count(), x.Key, UnScuccessCount = x.Count(t => t.ValidSignIn == false) }).ToList();
                //var data = chart.Report.Execute();
                var a = data0.Select(x => x.Count).ToList();
                var b = data0.Select(x => x.UnScuccessCount).ToList();
                barChart.Series[0] = new AxSeriesDto { Data = a, Name = "تعداد ورود به سیستم" };
                barChart.Series.Add(new AxSeriesDto { Data = b, Name = "تعداد ورود ناموفق" });
                barChart.Labels = data0.Select(x => x.Key.ToPerDateString("d MMMM")).ToList();
            }
            await _hub.Clients.Clients(connections).SendAsync("UpdateChart", barChart, cancellationToken);

            var chart = await _chartRepository.GetAll(x => x.Id == 9).Include(x => x.Report).FirstOrDefaultAsync(cancellationToken);
            var numericWidget = _numberWidgetRepository.GetAll(x => x.AxChartId == 9).ProjectTo<NumericWidgetDto>().FirstOrDefault();
            if (chart != null && numericWidget != null)
            {
                var data = chart.Report.Execute();
                numericWidget.Data = (int)data;
                numericWidget.LastUpdated = DateTime.Now.ToPerDateTimeString("yyyy/MM/dd HH:mm:ss");
            }
            await _hub.Clients.Clients(connections).SendAsync("UpdateChart", numericWidget, cancellationToken);



            await _memoryCache.GetOrCreateAsync("user" + user.Id, entry =>
              {
                  return Task.Run(() => PermissionHelper.GetKeysFromDb(_permissionRepository, _userGroupRepository, user.Id), cancellationToken);
              });

            await _userRepository.UpdateLastLoginDateAsync(user, cancellationToken);

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

            var connections = _userConnectionRepository.GetAll(x => x.Active).Select(x => x.ConnectionId).ToList();
            var chart = await _chartRepository.GetAll(x => x.Id == 9).Include(x => x.Report).FirstOrDefaultAsync(cancellationToken);
            var numericWidget = _numberWidgetRepository.GetAll(x => x.AxChartId == 9).ProjectTo<NumericWidgetDto>().FirstOrDefault();
            if (chart != null && numericWidget != null)
            {
                var data = chart.Report.Execute();
                numericWidget.Data = (int)data;
                numericWidget.LastUpdated = DateTime.Now.ToPerDateTimeString("yyyy/MM/dd HH:mm:ss");
            }
            await _hub.Clients.Clients(connections).SendAsync("UpdateChart", numericWidget, cancellationToken);

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
            var keys = _memoryCache.GetOrCreate("user" + UserId, entry =>
            {
                var data = PermissionHelper.GetKeysFromDb(_permissionRepository, _userGroupRepository, UserId);
                return data;
            }).ToList();
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

        [AxAuthorize(StateType = StateType.OnlyToken)]
        [HttpPost("[action]")]
        public async Task<ApiResult> SetUserConnectionId(UserConnectionDto connectionDto, CancellationToken cancellationToken)
        {
            var address = Request.HttpContext.Connection.RemoteIpAddress;
            var ip = address.GetIp();
            var userConnection = new UserConnection
            {
                UserId = UserId,
                Active = false,
                Ip = ip,
                ConnectionId = connectionDto.ConnectionId,
                CreatorUserId = UserId
            };
            await _userConnectionRepository.AddAsync(userConnection, cancellationToken);
            return Ok();
        }

        [AxAuthorize(StateType = StateType.OnlyToken)]
        [HttpPost("[action]")]
        public async Task<ApiResult> DisableUserConnectionId(UserConnectionDto connectionDto, CancellationToken cancellationToken)
        {
            var connection = await _userConnectionRepository.GetFirstAsync(x => x.ConnectionId == connectionDto.ConnectionId && x.UserId == UserId, cancellationToken);
            connection.Active = false;
            _userConnectionRepository.Update(connection);
            return Ok();
        }

    }

}