using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using API.Models;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Common.Utilities;
using Data.Repositories.UserRepositories;
using Entities.Framework;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Services;
using Services.Services.Services;
using WebFramework.Api;
using WebFramework.Filters;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [ApiResultFilter]
    [AllowAnonymous]
    public class UserController : ControllerBase
    {
        private readonly IUserRepository _userRepository;
        private readonly IJwtService _jwtService;

        public UserController(IUserRepository userUserRepository, IJwtService jwtService)
        {
            _userRepository = userUserRepository;
            _jwtService = jwtService;
        }

        [HttpGet]
        public ApiResult<IQueryable<UserDto>> Get()
        {
            var users = _userRepository.GetAll().ProjectTo<UserDto>();
            return Ok(users); 
        }

        [HttpGet("{id:int}")]
        public async Task<ApiResult<UserDto>> Get(int id, CancellationToken cancellationToken)
        {
            var user = await _userRepository.GetByIdAsync(id, cancellationToken);
            var user2 = Mapper.Map<UserDto>(user);
            return user2;
        }

        [HttpPost]
        public async Task<ApiResult<User>> Create(UserDto userDto, CancellationToken cancellationToken)
        {
            var user = new User { UserName = userDto.UserName };
            await _userRepository.AddAsync(user, userDto.Password, cancellationToken);
            return user;
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