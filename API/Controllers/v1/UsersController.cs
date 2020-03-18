using System;
using System.Threading;
using System.Threading.Tasks;
using API.Models;
using Common.Utilities;
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

        public UsersController(IUserRepository userRepository, IJwtService jwtService) : base(userRepository)
        {
            _userRepository = userRepository;
            _jwtService = jwtService;
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
                throw new UnauthorizedAccessException("نام کاربری و یا رمز عبور اشتباه است");
            var token = await _jwtService.GenerateAsync(user);
            return token;
        }
    }
}