using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;

namespace Common.Utilities
{
    public static class AttributeExtensions
    {
        public static List<Column> GetCustomAttributesOfType(this Type type, List<string> ignoreList, bool ignoreOnlyGetter = false)
        {
            var columns = new List<Column>();

            var props = type.GetProperties();
            foreach (var prop in props)
            {
                var propName = prop.Name.ToLowerFirstChar();
                var propType = prop.PropertyType.Name;
                if (ignoreOnlyGetter)
                {
                    var setter = prop.GetSetMethod(true);
                    if (setter == null)
                        continue;
                }

                if (ignoreList != null && ignoreList.Any())
                {

                    var exist = ignoreList.Any(x => x.ToLowerFirstChar() == propName);
                    if (exist)
                        continue;
                }

                var attr = prop.GetCustomAttributes<DisplayAttribute>(false).FirstOrDefault();
                if (attr != null)
                {
                    var name = attr.Name;
                    var attrType= attr.Description;
                    if (!string.IsNullOrWhiteSpace(attrType))
                        propType = attrType;
                    columns.Add(new Column { Name = propName, Type = propType, Title = name });
                }
            }

            return columns;
        }
    }

    public class Column
    {
        public string Name { get; set; }
        public string Title { get; set; }
        public string Type { get; set; }
    }
}
