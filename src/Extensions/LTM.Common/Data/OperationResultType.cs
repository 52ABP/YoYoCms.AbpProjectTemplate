using System.ComponentModel;

namespace LTM.Common.Data
{
    /// <summary>
    ///     表示业务操作结果的枚举
    /// </summary>
    public enum OperationResultType
    {
        /// <summary>
        ///     输入信息验证失败
        /// </summary>
        [Description("输入信息验证失败。")] ValidError,

        /// <summary>
        ///     指定参数的数据不存在
        /// </summary>
        [Description("指定参数的数据不存在。")] QueryNull,

        /// <summary>
        ///     操作取消或操作没引发任何变化
        /// </summary>
        [Description("操作没有引发任何变化，提交取消。")] NoChanged,

        /// <summary>
        ///     操作成功
        /// </summary>
        [Description("操作成功。")] Success,

        /// <summary>
        ///     操作引发错误
        /// </summary>
        [Description("操作引发错误。")] Error
    }
}