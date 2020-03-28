using System;
using System.Collections.Generic;
using System.Globalization;
using System.Threading.Tasks;
using API.Models;
using Common;
using Common.Utilities;
using Dapper;
using Dapper.Contrib.Extensions;
using Data.Repositories;
using Entities.Framework;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebFramework.Api;
using Log = NLog.Fluent.Log;

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
            var config = await qe.Connection.QueryFirstOrDefaultAsync<ConfigDto>($"select * from AxConfig where VersionCode > {vc} and IsActive = 1 order by datetime desc");
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
        [Authorize]
        public async Task<IEnumerable<AxServiceDtoReserve>> GetData()
        {
            var userId = User.Identity.GetUserId<long>();
            using var qe = new QueryExecutor();
            var data = await qe.Connection.QueryAsync<AxServiceDtoReserve>(@"select * from UserActiveFoodPlans WHERE UserId = @userId ORDER BY DeliveryDate DESC", new { userId });
            return data;
        }

        [HttpGet("[action]/{page}/{month}")]
        [Authorize]
        public async Task<ApiResult<IEnumerable<AxServiceDtoHistory>>> GetReservesHistory(int page, int month)
        {
            IEnumerable<AxServiceDtoHistory> data;
            var pageCount = 10;
            var userId = User.Identity.GetUserId<long>();
            var offset = page * pageCount;
            using var qe = new QueryExecutor();
            if (month > 0 && month <= 12)
            {
                var p = new PersianCalendar();
                var year = p.GetYear(DateTime.Now);
                var fDate = DateExtensionMethods.GetMiladiDate(year + "-" + month + "-1");
                var tDate = month < 12 ? DateExtensionMethods.GetMiladiDate(year + "-" + (month + 1) + "-1") : DateExtensionMethods.GetMiladiDate((year + 1) + "-1-1");

                data = await qe.Connection.QueryAsync<AxServiceDtoHistory>("SELECT * FROM UserReservationHistory WHERE UserId = @userId And [Date] >= @fDate and [Date] < @tDate ORDER BY [Date] DESC OFFSET (@offset) ROWS FETCH NEXT (@pageCount) ROWS ONLY", new { userId, offset, pageCount, fDate, tDate });
            }
            else
                data = await qe.Connection.QueryAsync<AxServiceDtoHistory>("SELECT * FROM UserReservationHistory WHERE UserId = @userId ORDER BY [Date] DESC OFFSET (@offset) ROWS FETCH NEXT (@pageCount) ROWS ONLY", new { userId, offset, pageCount });
            return new ApiResult<IEnumerable<AxServiceDtoHistory>>(true, ApiResultStatusCode.Success, data);
        }


        [HttpPost("[action]")]
        [Authorize]
        public async Task<ApiResult> AddAxReserve(AxReserveRequest req)
        {
            var userId = User.Identity.GetUserId<long>();
            using var qe = new QueryExecutor();

            var personnel = await qe.Connection.ExecuteScalarAsync<long>("select Personnel  from Res_PersonelRestaurantSetting WHERE UserId = @userId", new { userId });
            var plan = await qe.Connection.QueryFirstOrDefaultAsync<AxServiceDtoReserve>("select * from UserActiveFoodPlans WHERE pid = @pid", new { pid = req.Id });
            if (plan == null)
                return new ApiResult(false, ApiResultStatusCode.NotFound, "رزرو یافت نشد");

            if (plan.RemainingBookable <= 0)
                return new ApiResult(false, ApiResultStatusCode.NotFound, "ظرفیت غذای مورد نظر تکمیل شد");

            var reserve = await qe.Connection.QueryFirstOrDefaultAsync<AxReserveRequest>("Select * from Res_PersonnelFoodReservation WHERE Personnel = @personnel and PersonnelDailyReservationDetails =@pid", new { personnel, pid = req.Id });
            if (reserve == null)
            {
                var nextId = qe.Connection.ExecuteScalar<long>("SELECT NEXT VALUE FOR [dbo].idseq_$1207113500000010123");
                var axReserveRequest = new AxReserveRequest
                {
                    Id = nextId,
                    Num = req.Num,
                    Personnel = personnel,
                    Progress = "APIV1",
                    DayOfWeek = plan.WeekDay,
                    Food = plan.Food,
                    Meal = plan.Meal,
                    PersonnelDailyReservationDetails = req.Id,
                    Restaurant = plan.Restaurant,
                    ServeFoodPlace = 1207027580000000101,
                    Category = 1,
                    Status = 1,
                    CategoryCaption = "سهمیه شخص",
                    StatusCaption = "رزرو شده",
                    Date = plan.DeliveryDate
                };
                qe.Connection.Insert(axReserveRequest);
                return new ApiResult(true, ApiResultStatusCode.NotFound, "رزرو با موفقیت انجام شد");
            }

            if (req.Num == 0)
            {
                await qe.Connection.DeleteAsync(reserve);
                return new ApiResult(true, ApiResultStatusCode.NotFound, "رزرو با موفقیت حذف شد");
            }
            reserve.Num = req.Num;
            await qe.Connection.UpdateAsync(reserve);
            return new ApiResult(true, ApiResultStatusCode.NotFound, "رزرو با موفقیت ویرایش شد");
        }
    }
}