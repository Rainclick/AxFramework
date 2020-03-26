using System;
using Common.Utilities;
using Dapper.Contrib.Extensions;

namespace API.Models
{
    public abstract class AxServiceDto
    {
        public long? Id { get; set; }
        public string FoodTitle { get; set; }
        public string RestaurantTitle { get; set; }
        public string MealTitle { get; set; }
        public string WeekDayTitle { get; set; }
        public int Num { get; set; }
    }

    public class AxServiceDtoHistory : AxServiceDto
    {
        public DateTime Date { get; set; }
        public string DateString => Date.ToPerDateString("MM/dd") + " " + WeekDayTitle;
        public string ServePlace { get; set; }
        public string StatusTxt { get; set; }
    }

    public class AxServiceDtoReserve : AxServiceDto
    {
        public long Day { get; set; }
        public DateTime DeliveryDate { get; set; }
        public string DeliveryDateString => DeliveryDate.ToPerDateString("MM/dd");
        public int Reservable { get; set; }
        public long Food { get; set; }
        public long Meal { get; set; }
        public long Restaurant { get; set; }
        public int WeekDay { get; set; }
    }


    [Table("Res_PersonnelFoodReservation")]
    public class AxReserveRequest
    {
        [ExplicitKey]
        public long Id { get; set; }
        public long PersonnelDailyReservationDetails { get; set; }
        public long Food { get; set; }
        public long ServeFoodPlace { get; set; }
        public long Personnel { get; set; }
        public long Meal { get; set; }
        public long Restaurant { get; set; }
        public int DayOfWeek { get; set; }
        public int Category { get; set; }
        public int Status { get; set; }
        public int Num { get; set; }
        public string StatusCaption { get; set; }
        public string CategoryCaption { get; set; }
        public string Progress { get; set; }
    }

}
