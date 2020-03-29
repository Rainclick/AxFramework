using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace Common.Utilities
{
    public static class OrderByUtility
    {
        //makes expression for specific prop
        public static Expression<Func<TSource, object>> GetExpression<TSource>(string propertyName)
        {
            var param = Expression.Parameter(typeof(TSource), "x");
            Expression conversion = Expression.Convert(Expression.Property
                (param, propertyName), typeof(object));   //important to use the Expression.Convert
            return Expression.Lambda<Func<TSource, object>>(conversion, param);
        }

        //makes deleget for specific prop
        public static Func<TSource, object> GetFunc<TSource>(string propertyName)
        {
            return GetExpression<TSource>(propertyName).Compile();  //only need compiled expression
        }

        //OrderBy overload
        public static IOrderedEnumerable<TSource>
            OrderBy<TSource>(this IEnumerable<TSource> source, string propertyName)
        {
            return source.OrderBy(GetFunc<TSource>(propertyName));
        }

        //OrderBy overload
        public static IQueryable<TSource> OrderBy<TSource>(this IQueryable<TSource> source, string propertyName, SortType sortType = SortType.Asc)
        {
            if (!string.IsNullOrWhiteSpace(propertyName))
            {
                if (sortType == SortType.Asc)
                    return source.OrderBy(GetExpression<TSource>(propertyName));
                return source.OrderByDescending(GetExpression<TSource>(propertyName));
            }
            return source;
        }
    }
}
