namespace Entities.Framework
{
    public class UserToken : BaseEntity<int>
    {
        public string Token { get; set; }
        public bool Active { get; set; }
        public string DeviceName { get; set; }
        public string Browser { get; set; }
        public string UserAgent { get; set; }
        public string ClientId { get; set; }
    }
}
