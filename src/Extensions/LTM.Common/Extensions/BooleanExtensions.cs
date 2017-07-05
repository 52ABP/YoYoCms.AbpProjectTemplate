namespace LTM.Common.Extensions
{
    /// <summary>
    ///     布尔值<see cref="bool" />类型的扩展辅助操作类
    /// </summary>
    public static class BooleanExtensions
    {
        /// <summary>
        ///     把布尔值转换为小写字符串
        /// </summary>
        public static string ToLower(this bool value)
        {
            return value.ToString().ToLower();
        }
    }
}