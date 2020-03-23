using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Common.Utilities;
using Data.Repositories;
using Entities.Framework;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebFramework.Api;

namespace API.Controllers.v1
{
    [ApiVersion("1")]
    public class GeneralController : BaseController
    {
        private readonly IBaseRepository<ConfigData> _repository;

        public GeneralController(IBaseRepository<ConfigData> repository)
        {
            _repository = repository;
        }


        [HttpGet("[action]")]
        [Authorize]
        public async Task<IActionResult> GetOrganizationLogo(CancellationToken cancellationToken)
        {
            var data = await _repository.GetFirstAsync(x => x.Active, cancellationToken);

            if (data == null)
                return NotFound();

            return File(data.OrganizationLogo.ToArray(), GeneralUtils.GetContentType("a.png"));
        }
    }
}