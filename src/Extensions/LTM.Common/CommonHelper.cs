using System;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;

namespace LTM.Common
{
    /// <summary>
    /// 用于平时拓展的一些功能
    /// </summary>
    public static class CommonHelper
    {


        /// <summary>
        /// string转SHA1加密
        /// </summary>
        /// <param name="orgStr"></param>
        /// <param name="encode"></param>
        /// <returns></returns>
        public static string Sha1(string orgStr, string encode = "UTF-8")
        {
            var sha1 = new SHA1Managed();
            var sha1Bytes = System.Text.Encoding.GetEncoding(encode).GetBytes(orgStr);
            byte[] resultHash = sha1.ComputeHash(sha1Bytes);
            string sha1String = BitConverter.ToString(resultHash).ToLower();
            sha1String = sha1String.Replace("-", "");
            return sha1String;
        }

        /// <summary>
        /// 字母大小写数组
        /// </summary>
        private static readonly string[] Strs = {
                                  "a","b","c","d","e","f","g","h","i","j","k","l","m","n","o","p","q","r","s","t","u","v","w","x","y","z",
                                  "A","B","C","D","E","F","G","H","I","J","K","L","M","N","O","P","Q","R","S","T","U","V","W","X","Y","Z"
                                 };
        /// <summary>
        /// 创建随机字符串
        /// </summary>
        /// <returns></returns>
        public static string CreateRandom_str()
        {
            var r = new Random();
            var sb = new StringBuilder();
            var length = Strs.Length;
            for (int i = 0; i < 15; i++)
            {
                sb.Append(Strs[r.Next(length - 1)]);
            }
            return sb.ToString();
        }







        /// <summary>
        ///     秒转换小时
        /// </summary>
        /// <param name="time"></param>
        /// <returns></returns>
        public static string SecondToHour(double time)
        {

     
            var hour = 0;
            var minute = 0;
            var second = 0;
            second = Convert.ToInt32(time);

            if (second > 60)
            {
                minute = second/60;
            }
            if (minute > 60)
            {
                hour = minute/60;
                minute = minute%60;
            }
            return hour + "小时" + minute + "分钟";
        }


        /// <summary>
        ///     字节转换大小方法  (单位换算)
        /// </summary>
        /// <param name="size">bypes字节值</param>
        /// <returns></returns>
        public static string HumanReadableFilesize(double size)
        {
            var units = new[] {"B", "KB", "MB", "GB", "TB", "PB"};
            var mod = 1024.0;
            var i = 0;
            while (size >= mod)
            {
                size /= mod;
                i++;
            }
            return Math.Round(size) + units[i];
        }

        /// <summary>
        ///     重量单位换算(含 单位)
        /// </summary>
        /// <param name="weight"></param>
        /// <returns></returns>
        public static string ConvertWeight(decimal weight)
        {
            var units = new[] {"", "万", "亿", "兆"};

            var mod = 10000m;
            var i = 0;
            while (weight >= mod)
            {
                weight /= mod;
                i++;
            }
            return Math.Round(weight, 2) + units[i];
        }

        /// <summary>
        ///    重量单位换算为万
        /// </summary>
        /// <param name="weight"></param>
        /// <returns></returns>
        public static string ConvertToWanWeight(decimal weight)
        { 
            var mod = 10000m; 
                weight /= mod;
                
                return Math.Round(weight, 2).ToString();
 
        }


        /// <summary>
        /// 将数组转换为人民币大写
        /// </summary>
        /// <param name="number"></param>
        /// <returns></returns>
        public static string ConvertToChinese(decimal number)
        {
            var s = number.ToString("#L#E#D#C#K#E#D#C#J#E#D#C#I#E#D#C#H#E#D#C#G#E#D#C#F#E#D#C#.0B0A");
            var d = Regex.Replace(s,
                @"((?<=-|^)[^1-9]*)|((?'z'0)[0A-E]*((?=[1-9])|(?'-z'(?=[F-L\.]|$))))|((?'b'[F-L])(?'z'0)[0A-L]*((?=[1-9])|(?'-z'(?=[\.]|$))))",
                "${b}${z}");
            var r = Regex.Replace(d, ".", m => "负元空零壹贰叁肆伍陆柒捌玖空空空空空空空分角拾佰仟万亿兆京垓秭穰"[m.Value[0] - '-'].ToString());
            if (r.EndsWith("元"))
                r += "整";

          


            return r;
        }



        /// <summary>  
        /// 创建时间戳Timestamp  
        /// </summary>  
        /// <returns></returns>  
        public static int GetCreatetime()
        {
            var dateStart = new DateTime(1970, 1, 1, 8, 0, 0);
            return Convert.ToInt32((DateTime.Now - dateStart).TotalSeconds);
        }

        /// <summary>
        /// 时间戳转为C#格式时间
        /// </summary>
        /// <param name="timeStamp"></param>
        /// <returns></returns>
        public static DateTime GetTime(string timeStamp)
        {
            //1448496000000

        
            var dtStart = TimeZone.CurrentTimeZone.ToLocalTime(new DateTime(1970, 1, 1));
            var lTime = long.Parse(timeStamp+ "0000");
            var toNow = new TimeSpan(lTime);

            return dtStart.Add(toNow);
        }

        /// <summary>
        /// 验证邮箱
        /// </summary>
        /// <param name="email"></param>
        public static bool IsEmail(string email)
        {
            var regex = new Regex("^\\s*([A-Za-z0-9_-]+(\\.\\w+)*@(\\w+\\.)+\\w{2,5})\\s*$");
            if (regex.IsMatch(email))
            {
                return true;
            }
            return false;
        }



    }
}