using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;

namespace Common.Utilities
{
    public static class AttributeExtensions
    {
        public static Dictionary<string, string> GetCustomAttributesOfType(this Type type)
        {
            var dict = new Dictionary<string, string>();

            var props = type.GetProperties();
            foreach (var prop in props)
            {
                var attr = prop.GetCustomAttributes<DisplayAttribute>(false).FirstOrDefault();
                if (attr != null)
                {
                    var propName = prop.Name.ToLowerFirstChar();
                    var auth = attr.Name;
                    dict.Add(propName, auth);
                }
            }

            return dict;
        }
    }
}
