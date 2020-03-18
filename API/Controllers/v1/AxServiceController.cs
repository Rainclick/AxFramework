﻿using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using API.Models;
using Common;
using Common.Utilities;
using Dapper;
using Dapper.Contrib.Extensions;
using Data.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebFramework.Api;

namespace API.Controllers.v1
{
    [ApiVersion("1")]
    //[Authorize]
    public class AxServiceController : BaseController
    {

        [Route("GetConfig/{vc:int}/{vn}/{device}/{userId}")]
        [HttpGet]
        [AllowAnonymous]
        public async Task<ApiResult<ConfigDto>> GetConfig(int vc, string vn, string device, long userId)
        {
            using var qe = new QueryExecutor();
            var config = await qe.Connection.QueryFirstOrDefaultAsync<ConfigDto>($"select * from AxConfig where VersionCode > {vc} order by datetime desc");
            var apiResult = new ApiResult<ConfigDto>(true, ApiResultStatusCode.Success, config);
            var remoteIpAddress = Request.HttpContext.Connection.RemoteIpAddress;
            var logId = qe.Connection.ExecuteScalar<long>("SELECT NEXT VALUE FOR [dbo].idseq_$1203113500000000107");
            var loginLog = new AxUserLoginLog
            {
                DateTime = DateTime.Now,
                Device = device,
                Ip = remoteIpAddress?.ToString(),
                UserId = userId == 0 ? (long?)null : userId,
                VersionCode = vc,
                VersionName = vn,
                Id = logId
            };
            await qe.Connection.InsertAsync(loginLog);
            return apiResult;
        }


        [HttpGet("[action]")]
        public async Task<IEnumerable<AxServiceDtoReserve>> GetData()
        {
            var userId = User.Identity.GetUserId<long>();
            using var qe = new QueryExecutor();
            var data = await qe.Connection.QueryAsync<AxServiceDtoReserve>(@"select * from UserActiveFoodPlans WHERE UserId = @userId ORDER BY DeliveryDate DESC", new { userId });
            return data;
        }

        [HttpGet("[action]/{page}")]
        public async Task<ApiResult<IEnumerable<AxServiceDtoHistory>>> GetReservesHistory(int page)
        {
            var pageCount = 10;
            var userId = User.Identity.GetUserId<long>();
            var offset = page * pageCount;
            using var qe = new QueryExecutor();
            var data = await qe.Connection.QueryAsync<AxServiceDtoHistory>("SELECT * FROM UserReservationHistory WHERE UserId = @userId ORDER BY [Date] DESC OFFSET (@offset) ROWS FETCH NEXT (@pageCount) ROWS ONLY", new { userId, offset, pageCount });
            return new ApiResult<IEnumerable<AxServiceDtoHistory>>(true, ApiResultStatusCode.Success, data);
        }
    }

}