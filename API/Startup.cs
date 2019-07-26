using System;
using API.Models;
using AutoMapper;
using Common;
using Entities.Framework;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using WebFramework.Configuration;
using WebFramework.Middlewares;

namespace API
{
    public class Startup
    {
        private readonly SiteSettings _siteSettings;
        public IConfiguration Configuration;
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;

            //AutoMapperConfiguration.InitializeAutoMapper();
            Mapper.Initialize(config =>
            {
                config.CreateMap<User, UserDto>().ReverseMap();
            });
            _siteSettings = configuration.GetSection(nameof(SiteSettings)).Get<SiteSettings>();
        }



        // This method gets called by the runtime. Use this method to add services to the container.
        public IServiceProvider ConfigureServices(IServiceCollection services)
        {
            services.Configure<SiteSettings>(Configuration.GetSection(nameof(SiteSettings)));
            services.AddDbContext(Configuration);
            services.AddMinimalMvc();
            services.AddJwtAuthentication(_siteSettings.JwtSettings);
            return services.BuildAutofacServiceProvider();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            app.UseCustomExceptionHandler();
            app.UseHsts(env);
            app.UseHttpsRedirection();
            app.UseAuthentication();
            app.UseMvc();
        }
    }
}
