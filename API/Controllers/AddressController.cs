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
    public class AddressController : AxController<AddressDto, AddressDto, Address>
    {
        private readonly IBaseRepository<Address> _repository;

        public AddressController(IBaseRepository<Address> repository) : base(repository)
        {
            _repository = repository;
        }
    }
}