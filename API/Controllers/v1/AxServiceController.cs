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


        [HttpGet("[action]/{date1?}/{date2?}")]
        [Authorize]
        public async Task<IEnumerable<AxServiceDtoReserve>> GetData(DateTime? date1 = null, DateTime? date2 = null)
        {
            var userId = User.Identity.GetUserId<long>();
            using var qe = new QueryExecutor();
            var str = "";
            IEnumerable<AxServiceDtoReserve> data = null;
            if (!date1.HasValue && !date2.HasValue)
                str = "select * from UserActiveFoodPlans WHERE UserId = @userId ORDER BY DeliveryDate";
            if (date1.HasValue && !date2.HasValue)
                str = "select * from UserActiveFoodPlans WHERE UserId = @userId And DeliveryDate >= @date1 ORDER BY DeliveryDate";
            if (!date1.HasValue && date2.HasValue)
                str = "select * from UserActiveFoodPlans WHERE UserId = @userId And DeliveryDate <= @date2 ORDER BY DeliveryDate";
            if (date1.HasValue && date2.HasValue)
                str = "select * from UserActiveFoodPlans WHERE UserId = @userId And DeliveryDate >= @date1 And DeliveryDate <= @date2 ORDER BY DeliveryDate";

            data = await qe.Connection.QueryAsync<AxServiceDtoReserve>(str, new { userId, date1, date2 });
            return data;
        }

        [HttpGet("[action]/{page?}/{date1?}/{date2?}")]
        [Authorize]
        public async Task<ApiResult<IEnumerable<AxServiceDtoHistory>>> GetReservesHistory(int? page = 0, DateTime? date1 = null, DateTime? date2 = null)
        {
            var pageCount = 10;
            var userId = User.Identity.GetUserId<long>();
            var offset = page * pageCount;
            using var qe = new QueryExecutor();
            var whereClause = "";
            if (date1.HasValue && !date2.HasValue)
                whereClause = " And [Date] >= @date1 ";
            if (!date1.HasValue && date2.HasValue)
                whereClause = " And [Date] <= @date2 ";
            if (date1.HasValue && date2.HasValue)
                whereClause = " And [Date] >= @date1 And [Date] <= @date2";

            var data = await qe.Connection.QueryAsync<AxServiceDtoHistory>($"SELECT * FROM UserReservationHistory WHERE UserId = @userId {whereClause} ORDER BY [Date] DESC OFFSET (@offset) ROWS FETCH NEXT (@pageCount) ROWS ONLY", new { userId, offset, pageCount, date1, date2 });
            return new ApiResult<IEnumerable<AxServiceDtoHistory>>(true, ApiResultStatusCode.Success, data);
        }


        [HttpPost("[action]")]
        [Authorize]
        public ApiResult AddAxReserve(AxReserveRequest req)
        {
            var userId = User.Identity.GetUserId<long>();
            var userName = User.Identity.GetUserName();
            using var qe = new QueryExecutor();

            var personnelSettingId = qe.Connection.ExecuteScalar<long>("select Id  from Res_PersonelRestaurantSetting WHERE UserId = @userId", new { userId });
            var plan = qe.Connection.QueryFirstOrDefault<AxServiceDtoReserve>("select * from UserActiveFoodPlans WHERE Id = @Id", new { req.Id });
            if (plan == null)
                return new ApiResult(false, ApiResultStatusCode.NotFound, "رزرو یافت نشد");

            if (plan.RemainingBookable <= 0)
                return new ApiResult(false, ApiResultStatusCode.NotFound, "ظرفیت غذای مورد نظر تکمیل شده است");

            var userLimit = qe.Connection.QueryFirstOrDefault<AxResUserOrgLimit>(@"SELECT A.Personel,A.Quota,A.Meal, A_Meal.Title as MealTitle FROM Res_PersonelMealQuota AS A left outer join Res_Meal A_Meal on (A_Meal.id=A.Meal) left outer join Res_PersonelRestaurantSetting A_Personel on (A_Personel.id=A.Personel)
            WHERE
            (
                A.Meal = @meal AND 
            A_Personel.UserId = @userId)
            SELECT * from Res_Meal", new { meal = plan.Meal, userId });

            if (userLimit != null && userLimit.Quota < req.Num)
                return new ApiResult(false, ApiResultStatusCode.NotFound, $"کاربر عزیز {userName} گرامی، سهمیه ی شما در وعده  {userLimit.MealTitle} حداکثر {userLimit.Quota} عدد می باشد");

            if (plan.QuotaControl)
            {
                var orgLimit = qe.Connection.QueryFirstOrDefault<AxResUserOrgLimit>(@"SELECT
                A.MealGroup,--گروه وعده غذایی
                A_MealGroup.Title as AMealGroupTitle,
                A.PercentageOuQuota-- درصد سهمیه واحد سازمانی کاربر
                   FROM
                Res_OrganizationMealQuota AS A
                    left outer join Res_MealGroup A_MealGroup on(A_MealGroup.id = A.MealGroup)
                left outer join Res_RestaurantUnitShare A_F1 on(A_F1.id= A.F1)
                left outer join OrganizationalStructure A_F1_Unit on(A_F1_Unit.id= A_F1.Unit)
                left outer join Personel A_F1_Unit_F16_R on(A_F1_Unit_F16_R.F16= A_F1_Unit.id)
                left outer join Res_PersonelRestaurantSetting A_F1_Unit_F16_R_Personnel_R on(A_F1_Unit_F16_R_Personnel_R.Personnel= A_F1_Unit_F16_R.id)
                WHERE
                (
                    A.MealGroup = @mealGroup AND
                A_F1_Unit_F16_R_Personnel_R.UserId = @userId)", new { mealGroup = plan.MealGroup, userId });


                if (orgLimit != null)
                {
                    var limit = orgLimit.PercentageOuQuota * plan.RemainingBookable;
                    if (limit < req.Num)
                        return new ApiResult(false, ApiResultStatusCode.NotFound, $"کاربر گرامی سهمیه واحد شما در این گروه غذایی به اتمام رسیده است");
                }
            }

            var reserve = qe.Connection.QueryFirstOrDefault<AxReserveRequest>("Select * from Res_PersonnelFoodReservation WHERE Personnel = @personnel and PersonnelDailyReservationDetails =@pid", new { personnel = personnelSettingId, pid = plan.Pid });
            if (reserve == null)
            {
                var nextId = qe.Connection.ExecuteScalar<long>("SELECT NEXT VALUE FOR [dbo].idseq_$1207113500000010123");
                var axReserveRequest = new AxReserveRequest
                {
                    Id = nextId,
                    Num = req.Num,
                    Personnel = personnelSettingId,
                    Progress = "APIV1",
                    DayOfWeek = plan.WeekDay,
                    Food = plan.Food,
                    Meal = plan.Meal,
                    PersonnelDailyReservationDetails = plan.Pid,
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
                qe.Connection.DeleteAsync(reserve);
                return new ApiResult(true, ApiResultStatusCode.NotFound, "رزرو با موفقیت حذف شد");
            }
            reserve.Num = req.Num;
            qe.Connection.UpdateAsync(reserve);
            return new ApiResult(true, ApiResultStatusCode.NotFound, "رزرو با موفقیت ویرایش شد");
        }
    }
}