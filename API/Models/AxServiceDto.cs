using System.ComponentModel.DataAnnotations.Schema;

namespace API.Models
{
    public class AxServiceDto
    {
        public long? Id { get; set; }
        public string FoodTitle { get; set; }
        public string RestaurantTitle { get; set; }
        public long Reservable { get; set; }
        public string WeekDayTitle { get; set; }
        public long Day { get; set; }
    }
}
