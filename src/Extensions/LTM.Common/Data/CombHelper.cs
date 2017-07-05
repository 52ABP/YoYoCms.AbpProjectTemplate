using System;

namespace LTM.Common.Data
{
    /// <summary>
    ///     COMB（<see cref="Guid" />与<see cref="DateTime" />混合构成的可排序<see cref="Guid" />）类型操作类
    /// </summary>
    public static class CombHelper
    {
        /// <summary>
        ///     返回Guid用于数据库操作，特定的时间代码可以提高检索效率
        /// </summary>
        /// <returns>COMB类型 Guid 数据</returns>
        public static Guid NewComb()
        {
            var guidArray = Guid.NewGuid().ToByteArray();
            var dtBase = new DateTime(1900, 1, 1);
            var dtNow = DateTime.Now;
            //获取用于生成byte字符串的天数与毫秒数
            var days = new TimeSpan(dtNow.Ticks - dtBase.Ticks);
            var msecs = new TimeSpan(dtNow.Ticks - new DateTime(dtNow.Year, dtNow.Month, dtNow.Day).Ticks);
            //转换成byte数组
            //注意SqlServer的时间计数只能精确到1/300秒
            var daysArray = BitConverter.GetBytes(days.Days);
            var msecsArray = BitConverter.GetBytes((long) (msecs.TotalMilliseconds/3.333333));

            //反转字节以符合SqlServer的排序
            Array.Reverse(daysArray);
            Array.Reverse(msecsArray);

            //把字节复制到Guid中
            Array.Copy(daysArray, daysArray.Length - 2, guidArray, guidArray.Length - 6, 2);
            Array.Copy(msecsArray, msecsArray.Length - 4, guidArray, guidArray.Length - 4, 4);
            return new Guid(guidArray);
        }

        /// <summary>
        ///     从SQL Server 返回的Guid中生成时间信息
        /// </summary>
        public static DateTime GetDateFromComb(Guid id)
        {
            var baseDate = new DateTime(1900, 1, 1);
            var daysArray = new byte[4];
            var msecsArray = new byte[4];
            var guidArray = id.ToByteArray();

            // Copy the date parts of the guid to the respective byte arrays.
            Array.Copy(guidArray, guidArray.Length - 6, daysArray, 2, 2);
            Array.Copy(guidArray, guidArray.Length - 4, msecsArray, 0, 4);

            // Reverse the arrays to put them into the appropriate order
            Array.Reverse(daysArray);
            Array.Reverse(msecsArray);

            // Convert the bytes to ints
            var days = BitConverter.ToInt32(daysArray, 0);
            var msecs = BitConverter.ToInt32(msecsArray, 0);

            var date = baseDate.AddDays(days);
            date = date.AddMilliseconds(msecs*3.333333);

            return date;
        }
    }
}