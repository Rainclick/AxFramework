using System;
using System.Threading;
using System.Threading.Tasks;
using API.Models;
using Common.Exception;
using Common.Utilities;
using Data.Repositories;
using Data.Repositories.UserRepositories;
using Entities.Framework;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis;
using Services;
using Services.Services.Services;
using WebFramework.Api;

namespace API.Controllers.v1
{
    /// <summary>
    /// Users Actions
    /// </summary>
    [ApiVersion("1")]
    public class UsersController : AxController<UserDto, User>
    {
        private readonly IUserRepository _userRepository;
        private readonly IJwtService _jwtService;
        private readonly IBaseRepository<UserToken> _userTokenRepository;

        public UsersController(IUserRepository userRepository, IJwtService jwtService, IBaseRepository<UserToken> userTokenRepository) : base(userRepository)
        {
            _userRepository = userRepository;
            _jwtService = jwtService;
            _userTokenRepository = userTokenRepository;
        }

        /// <summary>
        /// This method is Login 
        /// </summary>
        /// <param name="username"></param>
        /// <param name="password"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpGet("[action]")]
        [AllowAnonymous]
        public virtual async Task<AccessToken> AxToken(string username, string password, CancellationToken cancellationToken)
        {
            var passwordHash = SecurityHelper.GetSha256Hash(password);
            var user = await _userRepository.GetFirstAsync(x => x.UserName == username && x.Password == passwordHash, cancellationToken);
            if (user == null)
                throw new AppException("نام کاربری و یا رمز عبور اشتباه است");
            var clientId = Guid.NewGuid().ToString();
            var token = await _jwtService.GenerateAsync(user, clientId);

            var remoteIpAddress = Request.HttpContext.Connection.RemoteIpAddress;
            var ip = remoteIpAddress?.ToString();
            var computerName = CompNameHelper.DetermineCompName(ip);


            var userToken = new UserToken
            {
                Active = true,
                Token = token.access_token,
                UserAgent = Request.Headers["User-Agent"].ToString(),
                Ip = ip,
                DeviceName = computerName,
                UserId = user.Id,
                ClientId = clientId,
                CreatorUserId = user.Id,
                Browser = ""
            };

            //Response.Cookies.Append("AxToken", token.access_token);
            await _userTokenRepository.AddAsync(userToken, cancellationToken);
            return token;
        }


        /// <summary>
        /// SignOut user and Remove user Token from Db
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        /// <exception cref="UnauthorizedAccessException"></exception>
        [HttpGet("[action]")]
        public virtual async Task<ApiResult> SignOut(CancellationToken cancellationToken)
        {
            var clientId = User.Identity.GetClientId();
            var userToken = await _userTokenRepository.GetFirstAsync(x => x.ClientId == clientId, cancellationToken);

            if (userToken == null)
                throw new UnauthorizedAccessException("کاربر یافت نشد");

            await _userTokenRepository.DeleteAsync(userToken, cancellationToken);
            return Ok();
        }

  


    }
}