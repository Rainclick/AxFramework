using API.Models;
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

        public AddressController(IBaseRepository<Address> repository) : base(repository)
        {
        }
    }
}