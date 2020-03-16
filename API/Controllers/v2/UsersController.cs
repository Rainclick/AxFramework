using System.Threading;
using System.Threading.Tasks;
using API.Models;
using Common.Exception;
using Data.Repositories;
using Data.Repositories.UserRepositories;
using Entities.Framework;
using Microsoft.AspNetCore.Mvc;
using Services.Services.Services;
using WebFramework.Api;

namespace API.Controllers.v2
{
    [ApiVersion("2")]
    public class UsersController : v1.UsersController
    {
        private readonly IUserRepository _userRepository;
        public UsersController(IUserRepository userRepository, IJwtService jwtService, IBaseRepository<UserToken> userTokenRepository) : base(userRepository, jwtService, userTokenRepository)
        {
            _userRepository = userRepository;
        }

        [HttpGet("GetByUserName")]
        public async Task<ApiResult<UserDto>> GetByUserName(string username, CancellationToken cancellationToken)
        {
            var user = await _userRepository.GetFirstAsync(x => x.UserName == username, cancellationToken);
            if (user == null)
                throw new NotFoundException("کاربر یافت نشد");
            return UserDto.FromEntity(user);
        }
    }
}