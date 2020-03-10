using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.Models;
using Common;
using Dapper;
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

        [Route("GetConfig/{vc:int}")]
        [HttpGet]
        [AllowAnonymous]
        public ApiResult<ConfigDto> GetConfig(int vc)
        {
            using var qe = new QueryExecutor();
            var config = qe.RunFirstOrDefault<ConfigDto>($"select * from AxConfig where VersionCode > {vc} order by datetime desc");
            var apiResult = new ApiResult<ConfigDto>(true, ApiResultStatusCode.Success, config);
            return apiResult;
        }


        [HttpGet("[action]")]
        public async Task<IEnumerable<AxServiceDto>> GetData()
        {
            using var qe = new QueryExecutor();
            var data = await qe.Connection.QueryAsync<AxServiceDto>(@"
                /*ReportName='_ريز برنامه غذايي جهت سرويس موبايل', ReportId='1204113530000020189'*/
SELECT
A.Id,
A.DeliveryDate,
A_Food.Title as FoodTitle,
A_MealGroup.Title as A_MealGroup_Title,
A_Restaurant.Title as RestaurantTitle,
A_Meal.Title as A_Meal_Title,
A.Planned,
A.Reservable,
A.Reserved,
A.Gusets,
A.Used,
A.TotalUsed,
A.WeekDay,A.Food,
A.Meal,
A.Restaurant,
A.MealGroup,
A.WeekDayTitle,
A_DailyFoodPlanDetails.DailyFoodPlan as Day,
Res_PersonnelFoodReservation.Personnel,
Res_PersonnelFoodReservation.Num, 
Res_PersonnelFoodReservation.Category, 
Res_PersonnelFoodReservation.Status
FROM
Res_FoodPlanDetail AS A 
left outer join Res_Food A_Food on (A_Food.id=A.Food) 
left outer join Res_MealGroup A_MealGroup on (A_MealGroup.id=A.MealGroup) 
left outer join Res_Restaurant A_Restaurant on (A_Restaurant.id=A.Restaurant) 
left outer join Res_Meal A_Meal on (A_Meal.id=A.Meal) 
left outer join Res_DailyFoodPlanDetails A_DailyFoodPlanDetails on (A_DailyFoodPlanDetails.id=A.DailyFoodPlanDetails)
FULL OUTER JOIN Res_PersonnelFoodReservation ON A.Food = Res_PersonnelFoodReservation.Food AND
A.DeliveryDate = Res_PersonnelFoodReservation.Date AND A.Meal = Res_PersonnelFoodReservation.Meal
");
            var a = data.Where(x => x.Id != null && x.Day > 0);
            return a;
        }
    }
}