using Common;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using WebFramework.Api;
using WebFramework.Filters;

namespace API.Controllers.v1.Capitar
{

    [ApiVersion("1")]
    public class testController : BaseController
    {
        public testController()
        {

        }
       
        [HttpGet("[action]")]
        [AxAuthorize(StateType = StateType.Authorized, Order = 1, AxOp = AxOp.Capitar)]
        public async Task<ApiResult> CapitarTest(CancellationToken cancellationToken)
        {
            return Ok();
        }
    }
}
