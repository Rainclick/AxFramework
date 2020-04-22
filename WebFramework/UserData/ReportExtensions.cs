using System;
using System.Linq;
using Autofac;
using AutoMapper.QueryableExtensions;
using Common.Utilities;
using Data.Repositories;
using Entities.Framework;
using Entities.Framework.Reports;
using ExpressionBuilder.Generics;

namespace WebFramework.UserData
{
    public static class ReportExtensions
    {
        public static object Execute0<T>(this Report report) where T : BaseEntity
        {
            var repository = AutoFacSingleton.Instance.Resolve<IBaseRepository<T>>();
            object data;
            Filter<T> predicate = null;
            if (report.Filters != null && report.Filters.Any())
            {         
                var request = new DataRequest();
                request.Filters.AddRange(report.Filters);
                predicate = request.GetFilter<T>();
            }

            switch (report.ExecuteType)
            {
                case ReportType.All:
                    data = !string.IsNullOrWhiteSpace(report.Sort) ? repository.GetAll(predicate).OrderBy(report.Sort, report.SortType).Take(report.TakeSize > 0 ? report.TakeSize : int.MaxValue) : repository.GetAll(predicate).Take(report.TakeSize > 0 ? report.TakeSize : int.MaxValue);
                    break;
                case ReportType.First:
                    data = !string.IsNullOrWhiteSpace(report.Sort) ? repository.GetAll(predicate).OrderBy(report.Sort, report.SortType).Take(1) : repository.GetAll(predicate).Take(1);
                    break;
                case ReportType.Count:
                    data = repository.Count(predicate);
                    break;
                default:
                    throw new ArgumentOutOfRangeException($"report '{report.Title}' executeType is not valid");
            }
            return data;
        }

        public static object Execute(this Report report)
        {
            var assembly = typeof(BaseEntity).Assembly;
            var type = assembly.GetType(report.TypeName);
            var data = typeof(ReportExtensions).GetMethod("Execute0")?.MakeGenericMethod(type).Invoke(null, new object[] { report });
            return data;
        }

        public static IQueryable<T> AxProjectTo<T>(this IQueryable queryable)
        {
            return queryable.ProjectTo<T>();
        }

    }
}
