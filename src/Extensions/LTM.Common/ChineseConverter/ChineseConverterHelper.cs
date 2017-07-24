#region 版权信息

// ------------------------------------------------------------------------------
// Copyright © 积微物联有限公司 版权所有。 
// 项目名：LTM.Common 
// 文件名：Helper.cs
// 创建标识：梅军章  2017-03-07 13:18
// 创建描述：
// 
// 修改标识：
// 修改描述：
//  ------------------------------------------------------------------------------

#endregion

using System.Text;
using Microsoft.International.Converters.PinYinConverter;

namespace LTM.Common.ChineseConverter
{
    /// <summary>
    /// 中英文转换帮助类
    /// </summary>
    public static class ChineseConverterHelper
    {

        /// <summary>
        /// 中文转拼音
        /// </summary>
        /// <param name="chainessStr"></param>
        /// <param name="isUppper">是否大写</param>
        /// <returns></returns>
        public static string ChineseConverterToSpell(this string chainessStr,bool isUppper=false)
        {
            //英文
            var returnSpell = new StringBuilder();

            foreach (var obj in chainessStr)
            {
                try
                {
                    var chineseChar = new ChineseChar(obj);
                    var returnSpellChar = chineseChar.Pinyins[0]; //TODO 这里获取第一个拼音，但有可能是多音字的情况
                    var item = returnSpellChar.Substring(0, returnSpellChar.Length - 1);
                    if (!isUppper)
                    {
                        returnSpell.Append(item.Substring(0, 1).ToUpper() + item.Substring(1).ToLower());
                    }
                    else
                    {
                        returnSpell.Append(item);
                    }
                }
                catch
                {
                    returnSpell.Append(obj.ToString());
                }
            }
            return returnSpell.ToString();
        }
    }
}