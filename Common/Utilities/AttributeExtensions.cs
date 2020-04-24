using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;

namespace Common.Utilities
{
    public static class AttributeExtensions
    {
        public static Dictionary<string, string> GetCustomAttributesOfType(this Type type, List<string> ignoreList, bool ignoreOnlyGetter = false)
        {
            var dict = new Dictionary<string, string>();

            var props = type.GetProperties();
            foreach (var prop in props)
            {
                var propName = prop.Name.ToLowerFirstChar();
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
                    dict.Add(propName, name);
                }
            }

            return dict;
        }
    }
}
