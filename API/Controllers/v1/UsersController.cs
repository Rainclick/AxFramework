using System;
using System.Threading;
using System.Threading.Tasks;
using API.Models;
using Common.Utilities;
using Data.Repositories;
using Data.Repositories.UserRepositories;
using Entities.Framework;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Services;
using Services.Services.Services;
using WebFramework.Api;

namespace API.Controllers.v1
{
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

        [HttpGet("[action]")]
        [AllowAnonymous]
        public async Task<AccessToken> AxToken(string username, string password, CancellationToken cancellationToken)
        {
            var passwordHash = SecurityHelper.GetSha256Hash(password);
            var user = await _userRepository.GetFirstAsync(x => x.UserName == username && x.Password == passwordHash, cancellationToken);
            if (user == null)
                throw new UnauthorizedAccessException("نام کاربری و یا رمز عبور اشتباه است");
            var token = await _jwtService.GenerateAsync(user);

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
                ClientId = Guid.NewGuid().ToString(),
                CreatorUserId = user.Id,
                Browser = ""
            };

            //Response.Cookies.Append("AxToken", token.access_token);
            await _userTokenRepository.AddAsync(userToken, cancellationToken);
            return token;
        }
    }
}