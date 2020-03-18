using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using API.Models;
using Common.Exception;
using Data.Repositories;
using Data.Repositories.UserRepositories;
using Entities.Framework;
using Microsoft.AspNetCore.Mvc;
using Services;
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

        public override Task<AccessToken> AxToken(string username, string password, CancellationToken cancellationToken)
        {
            return base.AxToken(username, password, cancellationToken);
        }

        public override Task<ApiResult<UserDto>> Create(UserDto dto, CancellationToken cancellationToken)
        {
            return base.Create(dto, cancellationToken);
        }

        public override Task<ApiResult> Delete(int id, CancellationToken cancellationToken)
        {
            return base.Delete(id, cancellationToken);
        }

        public override ApiResult<IQueryable<UserDto>> Get()
        {
            return base.Get();
        }

        public override Task<ApiResult<UserDto>> Get(int id, CancellationToken cancellationToken)
        {
            return base.Get(id, cancellationToken);
        }

        [HttpGet("GetByUserName")]
        public async Task<ApiResult<UserDto>> GetByUserName(string username, CancellationToken cancellationToken)
        {
            var user = await _userRepository.GetFirstAsync(x => x.UserName == username, cancellationToken);
            if (user == null)
                throw new NotFoundException("کاربر یافت نشد");
            return UserDto.FromEntity(user);
        }

        public override Task<ApiResult<UserDto>> Update(UserDto dto, CancellationToken cancellationToken)
        {
            return base.Update(dto, cancellationToken);
        }



    }
}