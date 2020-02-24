using Common.Utilities;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;

namespace WebFramework.Configuration
{
    public static class ApplicationBuilderExtensions
    {
        public static void UseHosts(this IApplicationBuilder app, IWebHostEnvironment env)
        {
            Assert.NotNull(app, nameof(app));
            Assert.NotNull(env, nameof(env));

            if (!env.IsDevelopment())
                app.UseHsts();
        }

        //public static void InitializeDatabase(this IApplicationBuilder app)
        //{
        //    using (var scope = app.ApplicationServices.GetRequiredService<IServiceScopeFactory>().CreateScope())
        //    {
        //        var dbContext = scope.ServiceProvider.GetService<DataContext>();
        //        dbContext.Database.Migrate();

        //        var dataInitializers = scope.ServiceProvider.GetServices<IDataInitializer>();
        //        foreach (var dataInitializer in dataInitializers)
        //            dataInitializer.InitializeData();
        //    }
        //}
    }
}