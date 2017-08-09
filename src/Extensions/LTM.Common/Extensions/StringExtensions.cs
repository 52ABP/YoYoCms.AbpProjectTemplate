using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using LTM.Common.Infrastructure;
using LTM.Common.Secutiry;
using Newtonsoft.Json;

namespace LTM.Common.Extensions
{
    /// <summary>
    ///     字符串<see cref="string" />类型的扩展辅助操作类
    /// </summary>
    public static class StringExtensions
    {
        #region 正则表达式

        /// <summary>
        ///     指示所指定的正则表达式在指定的输入字符串中是否找到了匹配项
        /// </summary>
        /// <param name="value">要搜索匹配项的字符串</param>
        /// <param name="pattern">要匹配的正则表达式模式</param>
        /// <returns>如果正则表达式找到匹配项，则为 true；否则，为 false</returns>
        public static bool IsMatch(this string value, string pattern)
        {
            if (value == null)
            {
                return false;
            }
            return Regex.IsMatch(value, pattern);
        }

        /// <summary>
        ///     在指定的输入字符串中搜索指定的正则表达式的第一个匹配项
        /// </summary>
        /// <param name="value">要搜索匹配项的字符串</param>
        /// <param name="pattern">要匹配的正则表达式模式</param>
        /// <returns>一个对象，包含有关匹配项的信息</returns>
        public static string Match(this string value, string pattern)
        {
            if (value == null)
            {
                return null;
            }
            return Regex.Match(value, pattern).Value;
        }

        /// <summary>
        ///     在指定的输入字符串中搜索指定的正则表达式的所有匹配项的字符串集合
        /// </summary>
        /// <param name="value"> 要搜索匹配项的字符串 </param>
        /// <param name="pattern"> 要匹配的正则表达式模式 </param>
        /// <returns> 一个集合，包含有关匹配项的字符串值 </returns>
        public static IEnumerable<string> Matches(this string value, string pattern)
        {
            if (value == null)
            {
                return new string[] {};
            }
            var matches = Regex.Matches(value, pattern);
            return from Match match in matches select match.Value;
        }

        /// <summary>
        ///     是否电子邮件
        /// </summary>
        public static bool IsEmail(this string value)
        {
            const string pattern = @"^[\w-]+(\.[\w-]+)*@[\w-]+(\.[\w-]+)+$";
            return value.IsMatch(pattern);
        }

        /// <summary>
        ///     是否是IP地址
        /// </summary>
        public static bool IsIpAddress(this string value)
        {
            const string pattern =
                @"^(\d(25[0-5]|2[0-4][0-9]|1?[0-9]?[0-9])\d\.){3}\d(25[0-5]|2[0-4][0-9]|1?[0-9]?[0-9])\d$";
            return value.IsMatch(pattern);
        }

        /// <summary>
        ///     是否是整数
        /// </summary>
        public static bool IsNumeric(this string value)
        {
            const string pattern = @"^\-?[0-9]+$";
            return value.IsMatch(pattern);
        }

        /// <summary>
        ///     是否是Unicode字符串
        /// </summary>
        public static bool IsUnicode(this string value)
        {
            const string pattern = @"^[\u4E00-\u9FA5\uE815-\uFA29]+$";
            return value.IsMatch(pattern);
        }

        /// <summary>
        ///     是否Url字符串
        /// </summary>
        public static bool IsUrl(this string value)
        {
            const string pattern =
                @"^(http|https|ftp|rtsp|mms):(\/\/|\\\\)[A-Za-z0-9%\-_@]+\.[A-Za-z0-9%\-_@]+[A-Za-z0-9\.\/=\?%\-&_~`@:\+!;]*$";
            return value.IsMatch(pattern);
        }

        /// <summary>
        ///     是否身份证号，验证如下3种情况：
        ///     1.身份证号码为15位数字；
        ///     2.身份证号码为18位数字；
        ///     3.身份证号码为17位数字+1个字母
        /// </summary>
        public static bool IsIdentityCard(this string value)
        {
            const string pattern = @"^(^\d{15}$|^\d{18}$|^\d{17}(\d|X|x))$";
            return value.IsMatch(pattern);
        }

        /// <summary>
        ///     是否手机号码
        /// </summary>
        /// <param name="value"></param>
        /// <param name="isRestrict">是否按严格格式验证</param>
        public static bool IsMobileNumber(this string value, bool isRestrict = false)
        {
            var pattern = isRestrict ? @"^[1][3-8]\d{9}$" : @"^[1]\d{10}$";
            return value.IsMatch(pattern);
        }

        #endregion 正则表达式

        #region 其他操作

        /// <summary>
        ///     指示指定的字符串是 null 还是 System.String.Empty 字符串
        /// </summary>
        public static bool IsNullOrEmpty(this string value)
        {
            return string.IsNullOrEmpty(value);
        }

        /// <summary>
        ///     指示指定的字符串是 null、空还是仅由空白字符组成。
        /// </summary>
        public static bool IsNullOrWhiteSpace(this string value)
        {
            return string.IsNullOrWhiteSpace(value);
        }

        /// <summary>
        ///     为指定格式的字符串填充相应对象来生成字符串
        /// </summary>
        /// <param name="format">字符串格式，占位符以{n}表示</param>
        /// <param name="args">用于填充占位符的参数</param>
        /// <returns>格式化后的字符串</returns>
        public static string FormatWith(this string format, params object[] args)
        {
            format.CheckNotNull(nameof(format));
            return string.Format(CultureInfo.CurrentCulture, format, args);
        }

        /// <summary>
        ///     将字符串反转
        /// </summary>
        /// <param name="value">要反转的字符串</param>
        public static string ReverseString(this string value)
        {
            value.CheckNotNull(nameof(value));
            return new string(value.Reverse().ToArray());
        }

        /// <summary>
        ///     判断指定路径是否图片文件
        /// </summary>
        public static bool IsImageFile(this string filename)
        {
            if (!File.Exists(filename))
            {
                return false;
            }
            var filedata = File.ReadAllBytes(filename);
            if (filedata.Length == 0)
            {
                return false;
            }
            var code = BitConverter.ToUInt16(filedata, 0);
            switch (code)
            {
                case 0x4D42: //bmp
                case 0xD8FF: //jpg
                case 0x4947: //gif
                case 0x5089: //png
                    return true;

                default:
                    return false;
            }
        }

        /// <summary>
        ///     以指定字符串作为分隔符将指定字符串分隔成数组
        /// </summary>
        /// <param name="value">要分割的字符串</param>
        /// <param name="strSplit">字符串类型的分隔符</param>
        /// <param name="removeEmptyEntries">是否移除数据中元素为空字符串的项</param>
        /// <returns>分割后的数据</returns>
        public static string[] Split(this string value, string strSplit, bool removeEmptyEntries = false)
        {
            return value.Split(new[] {strSplit},
                removeEmptyEntries ? StringSplitOptions.RemoveEmptyEntries : StringSplitOptions.None);
        }

        /// <summary>
        ///     获取字符串的MD5 Hash值
        /// </summary>
        public static string ToMd5Hash(this string value)
        {
            return HashHelper.GetMd5(value);
        }

        /// <summary>
        ///     支持汉字的字符串长度，汉字长度计为2
        /// </summary>
        /// <param name="value">参数字符串</param>
        /// <returns>当前字符串的长度，汉字长度为2</returns>
        public static int TextLength(this string value)
        {
            var ascii = new ASCIIEncoding();
            var tempLen = 0;
            var bytes = ascii.GetBytes(value);
            foreach (var b in bytes)
            {
                if (b == 63)
                {
                    tempLen += 2;
                }
                else
                {
                    tempLen += 1;
                }
            }
            return tempLen;
        }

        /// <summary>
        ///     将JSON字符串还原为对象
        /// </summary>
        /// <typeparam name="T">要转换的目标类型</typeparam>
        /// <param name="json">JSON字符串 </param>
        /// <returns></returns>
        public static T FromJsonString<T>(this string json)
        {
            json.CheckNotNull(nameof(json));
            return JsonConvert.DeserializeObject<T>(json);
        }

        /// <summary>
        ///     将字符串转换为<see cref="byte" />[]数组，默认编码为<see cref="Encoding.UTF8" />
        /// </summary>
        public static byte[] ToBytes(this string value, Encoding encoding = null)
        {
            if (encoding == null)
            {
                encoding = Encoding.UTF8;
            }
            return encoding.GetBytes(value);
        }

        /// <summary>
        ///     将<see cref="byte" />[]数组转换为字符串，默认编码为<see cref="Encoding.UTF8" />
        /// </summary>
        public static string ToString(this byte[] bytes, Encoding encoding)
        {
            if (encoding == null)
            {
                encoding = Encoding.UTF8;
            }
            return encoding.GetString(bytes);
        }

        /// <summary>
        ///     将数字字符串转换为中文大写或人民币
        /// </summary>
        /// <param name="val">数字字符串</param>
        /// <param name="isRMB">是否为人民币格式</param>
        /// <returns>转换成功的字符串</returns>
        public static string NumberToCnChar(this string val, bool isRMB = false)
        {
            return DigitToChnText.Instance.Convert(val, isRMB);
        }

        #endregion 其他操作
    }

    /// <summary>
    ///     一般中文大写数字 和 人民币大写数字
    /// </summary>
    public class DigitToChnText : Singleton<DigitToChnText>
    {
        private readonly char[] chnGenDigit;
        private readonly char[] chnGenText;
        private readonly char[] chnRMBDigit;

        private readonly char[] chnRMBText;
        private readonly char[] chnRMBUnit;

        //
        // 构造函数
        //
        private DigitToChnText()
        {
            // 一般大写中文数字组
            chnGenText = new[] {'零', '一', '二', '三', '四', '五', '六', '七', '八', '九'};
            chnGenDigit = new[] {'十', '百', '千', '万', '亿'};

            // 人民币专用数字组
            chnRMBText = new[] {'零', '壹', '贰', '叁', '肆', '伍', '陆', '染', '捌', '玖'};
            chnRMBDigit = new[] {'拾', '佰', '仟', '萬', '億'};
            chnRMBUnit = new[] {'角', '分'};
        }

        //
        // 主转换函数
        // 参数
        // string strDigit - 待转换数字字符串
        // bool bToRMB - 是否转换成人民币
        // 返回
        // string    - 转换成的大写字符串
        //
        public string Convert(string strDigit, bool bToRMB)
        {
            // 检查输入数字有效性
            if (CheckDigit(ref strDigit, bToRMB))
            {
                // 定义结果字符串
                var strResult = new StringBuilder();

                // 提取符号部分
                ExtractSign(ref strResult, ref strDigit, bToRMB);

                // 提取并转换整数和小数部分
                ConvertNumber(ref strResult, ref strDigit, bToRMB);

                return strResult.ToString();
            }
            return "数据无效！";
        }

        //
        // 转换数字
        //
        protected void ConvertNumber(ref StringBuilder strResult, ref string strDigit, bool bToRMB)
        {
            int indexOfPoint;
            if (-1 == (indexOfPoint = strDigit.IndexOf('.'))) // 如果没有小数部分
            {
                strResult.Append(ConvertIntegral(strDigit, bToRMB));

                if (bToRMB) // 如果转换成人民币
                {
                    strResult.Append("圆整");
                }
            }
            else // 有小数部分
            {
                // 先转换整数部分
                if (0 == indexOfPoint) // 如果“.”是第一个字符
                {
                    if (!bToRMB) // 如果转换成一般中文大写
                    {
                        strResult.Append('零');
                    }
                }
                else // 如果“.”不是第一个字符
                {
                    strResult.Append(ConvertIntegral(strDigit.Substring(0, indexOfPoint), bToRMB));
                }

                // 再转换小数部分
                if (strDigit.Length - 1 != indexOfPoint) // 如果“.”不是最后一个字符
                {
                    if (bToRMB) // 如果转换成人民币
                    {
                        if (0 != indexOfPoint) // 如果“.”不是第一个字符
                        {
                            if (1 == strResult.Length && "零" == strResult.ToString()) // 如果整数部分只是'0'
                            {
                                strResult.Remove(0, 1); // 去掉“零”
                            }
                            else
                            {
                                strResult.Append('圆');
                            }
                        }
                    }
                    else
                    {
                        strResult.Append('点');
                    }

                    var strTmp = ConvertFractional(strDigit.Substring(indexOfPoint + 1), bToRMB);

                    if (0 != strTmp.Length) // 小数部分有返回值
                    {
                        if (bToRMB && // 如果转换为人民币
                            0 == strResult.Length && // 且没有整数部分
                            "零" == strTmp.Substring(0, 1)) // 且返回字串的第一个字符是“零”
                        {
                            strResult.Append(strTmp.Substring(1));
                        }
                        else
                        {
                            strResult.Append(strTmp);
                        }
                    }

                    if (bToRMB)
                    {
                        if (0 == strResult.Length) // 如果结果字符串什么也没有
                        {
                            strResult.Append("零圆整");
                        }
                        // 如果结果字符串最后以"圆"结尾
                        else if ("圆" == strResult.ToString().Substring(strResult.Length - 1, 1))
                        {
                            strResult.Append('整');
                        }
                    }
                }
                else if (bToRMB) // 如果"."是最后一个字符，且转换成人民币
                {
                    strResult.Append("圆整");
                }
            }
        }

        //
        // 检查输入数字有效性
        //
        private bool CheckDigit(ref string strDigit, bool bToRMB)
        {
            var isValidate = false;

            decimal dec = 0;
            try
            {
                dec = decimal.Parse(strDigit);
                isValidate = true;
            }
            catch (Exception ex)
            {
                isValidate = false;
                throw ex;
            }

            if (bToRMB) // 如果转换成人民币
            {
                if (dec >= 10000000000000000m)
                {
                    isValidate = false;
                    throw new Exception("输入数字太大，超出范围！");
                }
                if (dec < 0)
                {
                    isValidate = false;
                    throw new Exception("不允许人民币为负值！");
                }
            }
            else // 如果转换成中文大写
            {
                if (dec <= -10000000000000000m || dec >= 10000000000000000m)
                {
                    isValidate = false;
                    throw new Exception("输入数字太大或太小，超出范围！");
                }
                isValidate = true;
            }

            return isValidate;
        }

        //
        // 提取输入字符串的符号
        //
        protected void ExtractSign(ref StringBuilder strResult, ref string strDigit, bool bToRMB)
        {
            // '+'在最前
            if ("+" == strDigit.Substring(0, 1))
            {
                strDigit = strDigit.Substring(1);
            }
            // '-'在最前
            else if ("-" == strDigit.Substring(0, 1))
            {
                if (!bToRMB)
                {
                    strResult.Append('负');
                }
                strDigit = strDigit.Substring(1);
            }
            // '+'在最后
            else if ("+" == strDigit.Substring(strDigit.Length - 1, 1))
            {
                strDigit = strDigit.Substring(0, strDigit.Length - 1);
            }
            // '-'在最后
            else if ("-" == strDigit.Substring(strDigit.Length - 1, 1))
            {
                if (!bToRMB)
                {
                    strResult.Append('负');
                }
                strDigit = strDigit.Substring(0, strDigit.Length - 1);
            }
        }

        //
        // 转换整数部分
        //
        protected string ConvertIntegral(string strIntegral, bool bToRMB)
        {
            // 去掉数字前面所有的'0'
            // 并把数字分割到字符数组中
            var integral = long.Parse(strIntegral).ToString().ToCharArray();

            // 定义结果字符串
            var strInt = new StringBuilder();

            int digit;
            digit = integral.Length - 1;

            // 使用正确的引用
            var chnText = bToRMB ? chnRMBText : chnGenText;
            var chnDigit = bToRMB ? chnRMBDigit : chnGenDigit;

            // 变成中文数字并添加中文数位
            // 处理最高位到十位的所有数字
            int i;
            for (i = 0; i < integral.Length - 1; i++)
            {
                // 添加数字
                strInt.Append(chnText[integral[i] - '0']);

                // 添加数位
                if (0 == digit%4) // '万' 或 '亿'
                {
                    if (4 == digit || 12 == digit)
                    {
                        strInt.Append(chnDigit[3]); // '万'
                    }
                    else if (8 == digit)
                    {
                        strInt.Append(chnDigit[4]); // '亿'
                    }
                }
                else // '十'，'百'或'千'
                {
                    strInt.Append(chnDigit[digit%4 - 1]);
                }

                digit--;
            }

            // 如果个位数不是'0'
            // 或者只有一位数
            // 则添加相应的中文数字
            if ('0' != integral[integral.Length - 1] || 1 == integral.Length)
            {
                strInt.Append(chnText[integral[i] - '0']);
            }

            // 遍历整个字符串
            i = 0;
            string strTemp; // 临时存储字符串
            int j; // 查找“零x”结构时用
            bool bDoSomething; // 找到“零万”或“零亿”时为真

            while (i < strInt.Length)
            {
                j = i;

                bDoSomething = false;

                // 查找所有相连的“零x”结构
                while (j < strInt.Length - 1 && "零" == strInt.ToString().Substring(j, 1))
                {
                    strTemp = strInt.ToString().Substring(j + 1, 1);

                    // 如果是“零万”或者“零亿”则停止查找
                    if (chnDigit[3].ToString() /* 万或萬 */== strTemp ||
                        chnDigit[4].ToString() /* 亿或億 */== strTemp)
                    {
                        bDoSomething = true;
                        break;
                    }

                    j += 2;
                }

                if (j != i) // 如果找到非“零万”或者“零亿”的“零x”结构，则全部删除
                {
                    strInt = strInt.Remove(i, j - i);

                    // 除了在最尾处，或后面不是"零万"或"零亿"的情况下,
                    // 其他处均补入一个“零”
                    if (i <= strInt.Length - 1 && !bDoSomething)
                    {
                        strInt = strInt.Insert(i, '零');
                        i++;
                    }
                }

                if (bDoSomething) // 如果找到"零万"或"零亿"结构
                {
                    strInt = strInt.Remove(i, 1); // 去掉'零'
                    i++;
                    continue;
                }

                // 指针每次可移动2位
                i += 2;
            }

            // 遇到“亿万”变成“亿零”或"亿"
            strTemp = chnDigit[4] + chnDigit[3].ToString(); // 定义字符串“亿万”或“億萬”
            var index = strInt.ToString().IndexOf(strTemp);
            if (-1 != index)
            {
                if (strInt.Length - 2 != index && index + 2 < strInt.Length &&
                    "零" != strInt.ToString().Substring(index + 2, 1)) // 并且其后没有"零"
                {
                    strInt = strInt.Replace(strTemp, chnDigit[4].ToString(), index, 2); // 变“亿万”为“亿零”
                    strInt = strInt.Insert(index + 1, "零");
                }
                else // 如果“亿万”在末尾或是其后已经有“零”
                {
                    strInt = strInt.Replace(strTemp, chnDigit[4].ToString(), index, 2); // 变“亿万”为“亿”
                }
            }

            if (!bToRMB) // 如果转换为一般中文大写
            {
                // 开头为“一十”改为“十”
                if (strInt.Length > 1 && "一十" == strInt.ToString().Substring(0, 2))
                {
                    strInt = strInt.Remove(0, 1);
                }
            }

            return strInt.ToString();
        }

        //
        // 转换小数部分
        //
        protected string ConvertFractional(string strFractional, bool bToRMB)
        {
            var fractional = strFractional.ToCharArray();

            var strFrac = new StringBuilder();

            // 变成中文数字
            int i;
            if (bToRMB) // 如果转换为人民币
            {
                for (i = 0; i < Math.Min(2, fractional.Length); i++)
                {
                    strFrac.Append(chnRMBText[fractional[i] - '0']);
                    strFrac.Append(chnRMBUnit[i]);
                }

                // 如果最后两位是“零分”则删除
                if ("零分" == strFrac.ToString().Substring(strFrac.Length - 2, 2))
                {
                    strFrac.Remove(strFrac.Length - 2, 2);
                }

                // 如果以“零角”开头
                if ("零角" == strFrac.ToString().Substring(0, 2))
                {
                    // 如果只剩下“零角”
                    if (2 == strFrac.Length)
                    {
                        strFrac.Remove(0, 2);
                    }
                    else // 如果还有“x分”，则删除“角”
                    {
                        strFrac.Remove(1, 1);
                    }
                }
            }
            else // 如果转换为一般中文大写
            {
                for (i = 0; i < fractional.Length; i++)
                {
                    strFrac.Append(chnGenText[fractional[i] - '0']);
                }
            }

            return strFrac.ToString();
        }
    }
}