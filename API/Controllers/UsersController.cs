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

namespace API.Controllers
{
    public class UsersController : AxController<UserDto, User>
    {
        private readonly IUserRepository _userRepository;
        private readonly IJwtService _jwtService;

        public UsersController(IUserRepository userRepository, IJwtService jwtService) : base(userRepository)
        {
            _userRepository = userRepository;
            _jwtService = jwtService;
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
            return token;
        }
    }
}