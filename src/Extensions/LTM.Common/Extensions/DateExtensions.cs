using System;

namespace LTM.Common.Extensions
{
    public static class DateExtensions
    {
        /// <summary>
        ///     将时间转换为常用字符串
        /// </summary>
        /// <param name="dt"></param>
        /// <returns>用于理解的字符</returns>
        public static string ToSimple(this DateTime dt)
        {
            var span = DateTime.Now - dt;
            if (span.TotalDays > 720)
            {
                return "很久以前";
            }
            if (span.TotalDays > 360)
            {
                return "一年以前";
            }
            if (span.TotalDays > 180)
            {
                return "半年前";
            }
            if (span.TotalDays > 60)
            {
                return "2个月前";
            }
            if (span.TotalDays > 30)
            {
                return "1个月前";
            }
            if (span.TotalDays > 14)
            {
                return "2周前";
            }
            if (span.TotalDays > 7)
            {
                return "1周前";
            }
            if (span.TotalDays > 1)
            {
                return string.Format("{0}天前", (int) Math.Floor(span.TotalDays));
            }
            if (span.TotalHours > 1)
            {
                return string.Format("{0}小时前", (int) Math.Floor(span.TotalHours));
            }
            if (span.TotalMinutes > 1)
            {
                return string.Format("{0}分钟前", (int) Math.Floor(span.TotalMinutes));
            }
            if (span.TotalSeconds >= 1)
            {
                return string.Format("{0}秒前", (int) Math.Floor(span.TotalSeconds));
            }
            return "1秒前";
        }

        /// <summary>
        ///     将时间转换为常用字符串
        /// </summary>
        /// <param name="dt"></param>
        /// <returns>用于理解的字符</returns>
        public static string ToSimple(this DateTime? dt)
        {
            if (dt.Value != null)
            {
                return dt.Value.ToSimple();
            }
            return "";
        }

        /// <summary>
        ///     将时间转换为yyyy-MM-dd格式
        /// </summary>
        /// <param name="dt"></param>
        /// <returns>yyyy-MM-dd字符串</returns>
        public static string ToShortDate(this DateTime dt)
        {
            return dt.ToString("yyyy-MM-dd");
        }

        /// <summary>
        ///     转换为yyyy-MM-dd HH:mm:ss格式
        /// </summary>
        /// <param name="dt"></param>
        /// <returns>yyyy-MM-dd HH:mm:ss字符串</returns>
        public static string ToLongDate(this DateTime dt)
        {
            return dt.ToString("yyyy-MM-dd HH:mm:ss");
        }
    }
}