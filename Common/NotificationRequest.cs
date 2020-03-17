using System.Collections.Generic;

namespace Common
{
    public class NotificationRequest
    {
        public List<string> ids { get; set; }
        public string title { get; set; }
        public string body { get; set; }
        public string url { get; set; }
        public string imageUrl { get; set; }
        public int? catId { get; set; }
        public int? tag { get; set; }
        public int? trId { get; set; }
        public string activityName { get; set; }
        public string fragmentName { get; set; }
        public int sType { get; set; }
    }

    public enum ResponseStatusType
    {
        GetGeoData = 1,
        SendMsg = 2,
        Offer = 3,
        ForAdmin = 4,
        UserRegister = 5,
        SecurityAlert = 6,
        SetGeoData = 7
    }
}