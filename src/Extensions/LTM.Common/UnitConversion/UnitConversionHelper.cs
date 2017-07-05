using System;

namespace LTM.Common.UnitConversion
{
    /// <summary>
    /// 单位换算帮助类
    /// </summary>
    public static class UnitConversionHelper
    {
        /// <summary>  
        /// 字节转换大小方法  (单位换算)
        /// </summary>  
        /// <param name="size">bypes字节值</param>  
        /// <returns></returns>  
        public static string StorageUnitConversion(double size)
        {
            var units = new String[] { "B", "KB", "MB", "GB", "TB", "PB" };
            var mod = 1024.0;
            int i = 0;
            while (size >= mod)
            {
                size /= mod;
                i++;
            }
            return Math.Round(size) + units[i];
        }
    }
}
