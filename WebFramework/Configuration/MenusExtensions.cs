using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Common;
using Data;
using Data.Repositories;
using Entities.Framework;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.Extensions.DependencyInjection;
using WebFramework.Filters;

namespace WebFramework.Configuration
{
    public static class MenusExtensions
    {
        public static void UseAutomaticMenus(this IApplicationBuilder app, Assembly assembly)
        {
            using var serviceScope = app.ApplicationServices.GetService<IServiceScopeFactory>().CreateScope();
            var context = serviceScope.ServiceProvider.GetRequiredService<DataContext>();
            //context.Database.Migrate();

            var controllersActionList = assembly.GetTypes()
                .Where(type => typeof(ControllerBase).IsAssignableFrom(type))
                .SelectMany(type => type.GetMethods(BindingFlags.Instance | BindingFlags.DeclaredOnly | BindingFlags.Public))
                .Where(m => !m.GetCustomAttributes(typeof(System.Runtime.CompilerServices.CompilerGeneratedAttribute), true)
                    .Any())
                .Select(x => new AxModel
                {
                    Controller = x.DeclaringType?.Name,
                    Action = x.Name,
                    ReturnType = x.ReturnType.Name,
                    AxAuthorizeAttribute = x.GetCustomAttributes().Where(t => t.GetType().Name == "AxAuthorizeAttribute")
                        .Select(t => t as AxAuthorizeAttribute).FirstOrDefault(),
                    Attributes = x.GetCustomAttributes().Select(a => new
                    { Name = a.GetType().Name.Replace("Attribute", ""), Value = GetValue(a) })
                })
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
                    var lst = GetAxMenus(item.AxAuthorizeAttribute.AxOp/*, item.Url*/);
                    var key = item.AxAuthorizeAttribute.AxOp.GetAxKey();
                    int? parentId = null;
                    foreach (var menu in lst)
                    {
                        var dbMenu = menuRepository.GetFirst(x => x.Key == menu.Key);

                        if (menu.Key == key)
                            menu.ShowInMenu = item.AxAuthorizeAttribute.ShowInMenu;

                        if (dbMenu == null)
                        {
                            menu.OrderId = item.AxAuthorizeAttribute.Order;
                            menu.ParentId = parentId;
                            menuRepository.Add(menu);
                        }

                        parentId = dbMenu?.Id ?? menu.Id;
                    }
                }
            }
        }

        private static string GetSystemName(AxAuthorizeAttribute axAuthorizeAttribute, string controller, string action)
        {
            if (axAuthorizeAttribute == null)
                throw new Exception($"Deer programmer you forget set AxOpAttribute in {controller} in {action}");
            return axAuthorizeAttribute.AxOp.GetAxSystem();
        }

        private static List<Menu> GetAxMenus(AxOp axOp/*, string itemUrl*/)
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

        private static StateType GetValue(Attribute attribute)
        {
            if (attribute is AxAuthorizeAttribute authorizeAttribute)
                return authorizeAttribute.StateType;
            return StateType.Authorized;
        }


    }

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