using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Dynamic;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace LTM.Common.Extensions
{
    /// <summary>
    ///     基类型<see cref="object" />扩展辅助操作类
    /// </summary>
    public static class ObjectExtensions
    {
        #region 公共方法

        /// <summary>
        ///     把对象类型转换为指定类型
        /// </summary>
        /// <param name="value"></param>
        /// <param name="conversionType"></param>
        /// <returns></returns>
        public static object CastTo(this object value, Type conversionType)
        {
            if (value == null)
            {
                return null;
            }
            if (conversionType.IsNullableType())
            {
                conversionType = conversionType.GetUnNullableType();
            }
            if (conversionType.IsEnum)
            {
                return Enum.Parse(conversionType, value.ToString());
            }
            if (conversionType == typeof (Guid))
            {
                return Guid.Parse(value.ToString());
            }
            return Convert.ChangeType(value, conversionType);
        }

        /// <summary>
        ///     把对象类型转化为指定类型
        /// </summary>
        /// <typeparam name="T"> 动态类型 </typeparam>
        /// <param name="value"> 要转化的源对象 </param>
        /// <returns> 转化后的指定类型的对象，转化失败引发异常。 </returns>
        public static T CastTo<T>(this object value)
        {
            var result = CastTo(value, typeof (T));
            return (T) result;
        }

        /// <summary>
        ///     把对象类型转化为指定类型，转化失败时返回指定的默认值
        /// </summary>
        /// <typeparam name="T"> 动态类型 </typeparam>
        /// <param name="value"> 要转化的源对象 </param>
        /// <param name="defaultValue"> 转化失败返回的指定默认值 </param>
        /// <returns> 转化后的指定类型对象，转化失败时返回指定的默认值 </returns>
        public static T CastTo<T>(this object value, T defaultValue)
        {
            try
            {
                return CastTo<T>(value);
            }
            catch (Exception)
            {
                return defaultValue;
            }
        }

        /// <summary>
        ///     判断当前值是否介于指定范围内
        /// </summary>
        /// <typeparam name="T"> 动态类型 </typeparam>
        /// <param name="value"> 动态类型对象 </param>
        /// <param name="start"> 范围起点 </param>
        /// <param name="end"> 范围终点 </param>
        /// <param name="leftEqual"> 是否可等于上限（默认等于） </param>
        /// <param name="rightEqual"> 是否可等于下限（默认等于） </param>
        /// <returns> 是否介于 </returns>
        public static bool IsBetween<T>(this IComparable<T> value, T start, T end, bool leftEqual = false,
            bool rightEqual = false) where T : IComparable
        {
            var flag = leftEqual ? value.CompareTo(start) >= 0 : value.CompareTo(start) > 0;
            return flag && (rightEqual ? value.CompareTo(end) <= 0 : value.CompareTo(end) < 0);
        }

        /// <summary>
        ///     将对象序列化为JSON字符串，支持存在循环引用的对象
        /// </summary>
        /// <typeparam name="T">动态类型</typeparam>
        /// <param name="value">动态类型对象</param>
        /// <param name="tolower">是否将属性转为首字母小写形式</param>
        /// <returns>JSON字符串</returns>
        public static string ToJsonString<T>(this T value, bool tolower = false)
        {
            var settings = new JsonSerializerSettings
            {
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore
            };
            if (!tolower)
            {
                return JsonConvert.SerializeObject(value, settings);
            }
            settings.ContractResolver = new CamelCasePropertyNamesContractResolver();
            return JsonConvert.SerializeObject(value, settings);
        }

        /// <summary>
        ///     字符串转换为json对象
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="value"></param>
        /// <param name="tolower"></param>
        /// <returns></returns>
        public static T ToObject<T>(this string value, bool tolower = true)
        {
            var settings = new JsonSerializerSettings
            {
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore
            };
            if (!tolower)
            {
                return JsonConvert.DeserializeObject<T>(value, settings);
            }
            settings.ContractResolver = new CamelCasePropertyNamesContractResolver();
            return JsonConvert.DeserializeObject<T>(value, settings);
        }

        /// <summary>
        ///     将对象[主要是匿名对象]转换为dynamic
        /// </summary>
        public static dynamic ToDynamic(this object value)
        {
            IDictionary<string, object> expando = new ExpandoObject();
            var type = value.GetType();
            var properties = TypeDescriptor.GetProperties(type);
            foreach (PropertyDescriptor property in properties)
            {
                var val = property.GetValue(value);
                if (property.PropertyType.FullName.StartsWith("<>f__AnonymousType"))
                {
                    dynamic dval = val.ToDynamic();
                    expando.Add(property.Name, dval);
                }
                else
                {
                    expando.Add(property.Name, val);
                }
            }
            return expando as ExpandoObject;
        }

        /// <summary>
        ///     将对象转换为流
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static byte[] ToBytes(this object value)
        {
            using (var ms = new MemoryStream())
            {
                IFormatter formatter = new BinaryFormatter();
                formatter.Serialize(ms, value);
                return ms.GetBuffer();
            }
        }

        #endregion 公共方法
    }
}