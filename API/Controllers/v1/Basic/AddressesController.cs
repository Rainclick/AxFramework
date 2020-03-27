using Data.Repositories;
using Entities.Framework;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers.v1.Basic
{
    [ApiVersion("1")]
    [AllowAnonymous]
    public class AddressesController
    {
        public AddressesController(IBaseRepository<Address> repository)
        {
        }
    }
}