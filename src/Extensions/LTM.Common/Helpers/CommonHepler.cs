using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LTM.Common.Helpers
{
    public class CommonHepler
    {
        /// <summary>
        /// 拆分数组
        /// </summary>
        /// <param name="str"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        public static List<long> BreakUpStr(string str, char key)
        {
            if(string.IsNullOrEmpty(str))
                return new List<long>();
            var strArray = str.Split(new[] { key }, StringSplitOptions.RemoveEmptyEntries);
            return Array.ConvertAll(strArray, long.Parse).ToList();
        }

        /// <summary>
        ///   金额单位换算为万
        /// </summary>
        /// <param name="weight"></param>
        /// <returns></returns>
        public static string ConvertToWanWeight(decimal weight)
        {
            var mod = 10000m;
            weight /= mod;

            return Math.Round(weight, 2).ToString();

        }
    }
}
