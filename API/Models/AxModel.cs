using WebFramework.Filters;

namespace API.Models
{
    public class AxModel
    {
        public AxAuthorizeAttribute AxAuthorizeAttribute { get; set; }
        public string Controller { get; set; }
        public string Action { get; set; }
        public object Attributes { get; set; }
        public string ReturnType { get; set; }
        public string Url { get; set; }
    }
}
