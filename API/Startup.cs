using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using API.Models;
using Autofac;
using Autofac.Extensions.DependencyInjection;
using Common;
using Data;
using Data.Repositories;
using Entities.Framework;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using NLog;
using WebFramework.Configuration;
using WebFramework.CustomMapping;
using WebFramework.Filters;
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

            var configurationVariable = Configuration.GetConnectionString("SqlServer");
            LogManager.Configuration.Variables["ConnectionString"] = configurationVariable;

            this.AutofacContainer = app.ApplicationServices.GetAutofacRoot();
            using var serviceScope = app.ApplicationServices.GetService<IServiceScopeFactory>().CreateScope();
            var context = serviceScope.ServiceProvider.GetRequiredService<DataContext>();
            //context.Database.Migrate();


            var asm = Assembly.GetAssembly(typeof(Startup));

            var controllersActionList = asm.GetTypes()
                .Where(type => typeof(ControllerBase).IsAssignableFrom(type))
                .SelectMany(type => type.GetMethods(BindingFlags.Instance | BindingFlags.DeclaredOnly | BindingFlags.Public))
                .Where(m => !m.GetCustomAttributes(typeof(System.Runtime.CompilerServices.CompilerGeneratedAttribute), true).Any())
                .Select(x => new AxModel { Controller = x.DeclaringType?.Name, Action = x.Name, ReturnType = x.ReturnType.Name, AxAuthorizeAttribute = x.GetCustomAttributes().Where(t => t.GetType().Name == "AxAuthorizeAttribute").Select(t => t as AxAuthorizeAttribute).FirstOrDefault(), Attributes = x.GetCustomAttributes().Select(a => new { Name = a.GetType().Name.Replace("Attribute", ""), Value = GetValue(a) }) })
                .OrderBy(x => x.Controller).ThenBy(x => x.Action).ToList();

            var menuRepository = context.GetService<IBaseRepository<Menu>>();
            var axModels = controllersActionList.Where(x => !x.Controller.Contains("BaseController")).ToList();
            axModels.ForEach(x =>
                x.Url = "/" + GetSystemName(x.AxAuthorizeAttribute, x.Controller, x.Action) + "/"
                        + x.Controller.Replace("Controller", "") + "/"
                        + x.Action);

            foreach (var item in axModels)
            {

                if (item.AxAuthorizeAttribute.StateType == StateType.Authorized)
                {
                    var lst = GetAxMenus(item.AxAuthorizeAttribute.AxOp, item.Url);
                    var key = item.AxAuthorizeAttribute.AxOp.GetAxKey();
                    int? parentId = null;
                    foreach (var menu in lst)
                    {
                        var dbMenu = menuRepository.GetFirst(x => x.Key == menu.Key);

                        if (menu.Key == key)
                            menu.ShowInMenu = item.AxAuthorizeAttribute.ShowInMenu;

                        if (dbMenu == null)
                        {
                            menu.ParentId = parentId;
                            menuRepository.Add(menu);
                        }

                        parentId = dbMenu?.Id ?? menu.Id;
                    }
                }
            }
        }

        private string GetSystemName(AxAuthorizeAttribute axAuthorizeAttribute, string controller, string action)
        {
            if (axAuthorizeAttribute == null)
                throw new Exception($"Deer programmer you forget set AxOpAttribute in {controller} in {action}");
            return axAuthorizeAttribute.AxOp.GetAxSystem();
        }

        private List<Menu> GetAxMenus(AxOp axOp, string itemUrl)
        {
            var axDisplay = axOp.GetAttribute<AxDisplay>();
            var node = axDisplay;
            var me = axOp;
            var lst = new List<Menu>();
            while (true)
            {
                var menu = new Menu
                {
                    Active = true,
                    CreatorUserId = 1,
                    InsertDateTime = DateTime.Now,
                    AxOp = me,
                    OrderId = node.Order,
                    Title = node.Title,
                    Key = me.GetAxKey()
                };
                lst.Insert(0, menu);

                if (node.Parent == AxOp.None)
                {
                    //lst[^1].Url = itemUrl;
                    return lst;
                }

                me = node.Parent;
                node = node.Parent.GetAttribute<AxDisplay>();
            }
        }

        private StateType GetValue(Attribute attribute)
        {
            if (attribute is AxAuthorizeAttribute authorizeAttribute)
                return authorizeAttribute.StateType;
            return StateType.Authorized;
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="builder"></param>
        public void ConfigureContainer(ContainerBuilder builder)
        {
            //builder.Populate(services);

            //Register Services to Autofac ContainerBuilder
            builder.AddServices();

        }

    }
}
