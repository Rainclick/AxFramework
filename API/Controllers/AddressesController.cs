using API.Models;
using Data.Repositories;
using Entities.Framework;
using Microsoft.AspNetCore.Authorization;
using WebFramework.Api;

namespace API.Controllers
{
    [AllowAnonymous]
    public class AddressesController : AxController<AddressDto, AddressDto, Address>
    {
        public AddressesController(IBaseRepository<Address> repository) : base(repository)
        {
        }
    }
}