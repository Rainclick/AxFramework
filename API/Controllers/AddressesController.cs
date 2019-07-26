using API.Models;
using Data.Repositories;
using Entities.Framework;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebFramework.Api;

namespace API.Controllers
{
    [ApiVersion("1")]
    [AllowAnonymous]
    public class AddressesController : AxController<AddressDto, AddressDto, Address>
    {
        public AddressesController(IBaseRepository<Address> repository) : base(repository)
        {
        }
    }
}