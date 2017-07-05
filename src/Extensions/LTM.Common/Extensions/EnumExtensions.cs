using System;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Reflection;

namespace LTM.Common.Extensions
{
    /// <summary>
    ///     枚举<see cref="Enum" />的扩展辅助操作方法
    /// </summary>
    public static class EnumExtensions
    {
        /// <summary>
        ///     获取枚举项上的<see cref="DescriptionAttribute" />特性的文字描述
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string ToDescription(this Enum value)
        {
            var type = value.GetType();
            var member = type.GetMember(value.ToString()).FirstOrDefault();
            return member != null ? member.ToDescription() : value.ToString();
        }

        /// <summary>
        ///     枚举遍历，返回枚举的名称、值、特性
        /// </summary>
        /// <param name="enumType">枚举类型</param>
        /// <param name="action">回调函数</param>
        public static void Each(this Type enumType, Action<string, string, string> action)
        {
            if (enumType.BaseType != typeof (Enum))
            {
                return;
            }
            var arr = Enum.GetValues(enumType);
            foreach (var name in arr)
            {
                var value = (int) Enum.Parse(enumType, name.ToString());
                var fieldInfo = enumType.GetField(name.ToString());
                var description = "";
                if (fieldInfo != null)
                {
                    var attr = Attribute.GetCustomAttribute(fieldInfo,
                        typeof (DescriptionAttribute), false) as DescriptionAttribute;
                    if (attr != null)
                    {
                        description = attr.Description;
                    }
                }
                action(name.ToString(), value.ToString(), description);
            }
        }

        /// <summary>
        ///     根据枚举类型值返回枚举定义Description属性
        /// </summary>
        /// <param name="value"></param>
        /// <param name="enumType"></param>
        /// <returns></returns>
        public static string ToEnumDescriptionString(this short value, Type enumType)
        {
            var nvc = new NameValueCollection();
            var typeDescription = typeof (DescriptionAttribute);
            var fields = enumType.GetFields();
            var strText = string.Empty;
            var strValue = string.Empty;
            foreach (var field in fields)
            {
                if (field.FieldType.IsEnum)
                {
                    strValue =
                        ((int) enumType.InvokeMember(field.Name, BindingFlags.GetField, null, null, null)).ToString();
                    var arr = field.GetCustomAttributes(typeDescription, true);
                    if (arr.Length > 0)
                    {
                        var aa = (DescriptionAttribute) arr[0];
                        strText = aa.Description;
                    }
                    else
                    {
                        strText = "";
                    }
                    nvc.Add(strValue, strText);
                }
            }
            return nvc[value.ToString()];
        }
    }
}