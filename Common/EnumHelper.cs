using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Common
{
    public static class EnumHelper
    {
        public static string GetDisplayName(this Enum enumValue)
        {
            return enumValue.GetType()
                .GetMember(enumValue.ToString())
                .First()
                .GetCustomAttribute<DisplayAttribute>()
                .GetName();
        }

        public static TAttribute GetAttribute<TAttribute>(this Enum value)
            where TAttribute : Attribute
        {
            var enumType = value.GetType();
            var name = Enum.GetName(enumType, value);
            return enumType.GetField(name).GetCustomAttributes(false).OfType<TAttribute>().SingleOrDefault();
        }


        public static string GetAxKey(this AxOp axOp)
        {
            var axDisplay = axOp.GetAttribute<AxDisplay>();
            var node = axDisplay;
            var sb = new StringBuilder();
            sb.Append(axDisplay.Key);
            while (node.Parent != AxOp.None)
            {
                node = node.Parent.GetAttribute<AxDisplay>();
                sb.Insert(0, node.Key);
            }
            var code = sb.ToString();
            return code;
        }

        public static string GetAxSystem(this AxOp axOp)
        {
            var axDisplay = axOp.GetAttribute<AxDisplay>();
            var node = axDisplay;
            var me = axOp;
            while (true)
            {
                if (node == null)
                    return "Basic";
                if (node.Parent == AxOp.None)
                    return me.ToString();
                me = node.Parent;
                node = node.Parent.GetAttribute<AxDisplay>();
            }
        }
    }
}
