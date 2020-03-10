using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Globalization;
using Common.Utilities;

namespace API.Models
{
    public class AxServiceDto
    {
        public long? Id { get; set; }
        public string FoodTitle { get; set; }
        public string RestaurantTitle { get; set; }
        public string MealTitle { get; set; }
        public long Reservable { get; set; }
        public string WeekDayTitle { get; set; }
        public long Day { get; set; }
        public DateTime DeliveryDate { get; set; }
        public string DeliveryDateString => DeliveryDate.ToPerDateString("MM/dd");
    }
}
