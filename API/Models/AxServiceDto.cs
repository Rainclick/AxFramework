using System;
using Common.Utilities;

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
    }


}
