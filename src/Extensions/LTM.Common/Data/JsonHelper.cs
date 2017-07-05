using System;
using System.Text.RegularExpressions;
using Newtonsoft.Json;

namespace LTM.Common.Data
{
    /// <summary>
    ///     JSON辅助操作类
    /// </summary>
    public static class JsonHelper
    {
        /// <summary>
        ///     处理Json的时间格式为正常格式
        /// </summary>
        public static string JsonDateTimeFormat(string json)
        {
            json = Regex.Replace(json,
                @"\\/Date\((\d+)\)\\/",
                match =>
                {
                    var dt = new DateTime(1970, 1, 1);
                    dt = dt.AddMilliseconds(long.Parse(match.Groups[1].Value));
                    dt = dt.ToLocalTime();
                    return dt.ToString("yyyy-MM-dd HH:mm:ss.fff");
                });
            return json;
        }

        /// <summary>
        ///     把对象序列化成Json字符串格式
        /// </summary>
        /// <param name="object"></param>
        /// <returns></returns>
        public static string ToJson(object @object)
        {
            var json = JsonConvert.SerializeObject(@object);
            return JsonDateTimeFormat(json);
        }

        /// <summary>
        ///     把Json字符串转换为强类型对象
        /// </summary>
        public static T FromJson<T>(string json)
        {
            json = JsonDateTimeFormat(json);
            return JsonConvert.DeserializeObject<T>(json);
        }
    }
}