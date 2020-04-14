using System.Reflection;
using API.Hubs;
using Autofac;
using Autofac.Extensions.DependencyInjection;
using Common;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using NLog;
using WebFramework.Configuration;
using WebFramework.CustomMapping;
using WebFramework.Middlewares;
using WebFramework.Swagger;

namespace API
{
    public class Startup
    {
        private readonly SiteSettings _siteSettings;
        public IConfiguration Configuration;
        public ILifetimeScope AutofacContainer { get; private set; }
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;

            AutoMapperConfiguration.InitializeAutoMapper();

            _siteSettings = configuration.GetSection(nameof(SiteSettings)).Get<SiteSettings>();
        }



        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.Configure<SiteSettings>(Configuration.GetSection(nameof(SiteSettings)));
            services.AddDbContext(Configuration);
            services.AddCors(o => o.AddPolicy("MyPolicy", builder =>
            {
                builder.AllowAnyOrigin()
                    .AllowAnyMethod()
                    .AllowAnyHeader();
            }));
            services.AddMvc(options =>
            {
                options.EnableEndpointRouting = false;
            }).AddNewtonsoftJson();
            services.AddJwtAuthentication(_siteSettings.JwtSettings);
            services.AddCustomApiVersioning();
            services.AddSwagger();
            services.AddSignalR();

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        /// <summary>
        /// 
        /// </summary>
        /// <param name="app"></param>
        /// <param name="env"></param>
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseCustomExceptionHandler();
            app.UseHosts(env);
            app.UseHttpsRedirection();
            app.UseAuthentication();
            app.UseCors("MyPolicy");
            app.UseMvc();
            app.UseSwaggerAndUi();
            app.UseRouting();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                endpoints.MapHub<ChartHub>("/chart");
            });

            var configurationVariable = Configuration.GetConnectionString("SqlServer");
            LogManager.Configuration.Variables["ConnectionString"] = configurationVariable;

            this.AutofacContainer = app.ApplicationServices.GetAutofacRoot();

            var assembly = Assembly.GetAssembly(typeof(Startup));
            app.UseAutomaticMenus(assembly);
        }


        public void ConfigureContainer(ContainerBuilder builder)
        {
            //builder.Populate(services);

            //Register Services to Autofac ContainerBuilder
            builder.AddServices();

        }

    }
}
