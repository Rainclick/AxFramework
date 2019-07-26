using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using API.Models;
using AutoMapper.QueryableExtensions;
using Data.Repositories;
using Entities.Framework;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebFramework.Api;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [AllowAnonymous]
    public class AddressController : ControllerBase
    {
        private readonly IBaseRepository<Address> _repository;

        public AddressController(IBaseRepository<Address> repository)
        {
            _repository = repository;
        }

        [HttpGet]
        public ApiResult<IQueryable<AddressDto>> Get()
        {
            var address = _repository.GetAll().ProjectTo<AddressDto>();
            return Ok(address);
        }

        [HttpPost]
        public async Task<ApiResult<AddressDto>> Create(AddressDto addressDto, CancellationToken cancellationToken)
        {
            var address = addressDto.ToEntity();
            await _repository.AddAsync(address, cancellationToken);
            var result = AddressDto.FromEntity(address);
            return result;
        }
    }
}