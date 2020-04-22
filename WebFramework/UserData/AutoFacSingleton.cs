using Autofac;

namespace WebFramework.UserData
{
    public class AutoFacSingleton
    {
        private AutoFacSingleton() { }

        public static ILifetimeScope Instance { get; set; }

        public string Value { get; set; }
        public string Name { get; set; }
    }
}
