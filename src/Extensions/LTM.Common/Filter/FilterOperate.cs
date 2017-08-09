namespace LTM.Common.Filter
{
    /// <summary>
    ///     筛选操作方式
    /// </summary>
    public enum FilterOperate
    {
        /// <summary>
        ///     并且
        /// </summary>
        [OperateCode("and", "并且")] And = 1,

        /// <summary>
        ///     或者
        /// </summary>
        [OperateCode("or", "或者")] Or = 2,

        /// <summary>
        ///     等于
        /// </summary>
        [OperateCode("equal", "等于")] Equal = 3,

        /// <summary>
        ///     不等于
        /// </summary>
        [OperateCode("notequal", "不等于")] NotEqual = 4,

        /// <summary>
        ///     小于
        /// </summary>
        [OperateCode("less", "小于")] Less = 5,

        /// <summary>
        ///     小于或等于
        /// </summary>
        [OperateCode("lessorequal", "小于或等于")] LessOrEqual = 6,

        /// <summary>
        ///     大于
        /// </summary>
        [OperateCode("greater", "大于")] Greater = 7,

        /// <summary>
        ///     大于或等于
        /// </summary>
        [OperateCode("greaterorequal", "大于或等于")] GreaterOrEqual = 8,

        /// <summary>
        ///     以……开始
        /// </summary>
        [OperateCode("startswith", "前面包含")] StartsWith = 9,

        /// <summary>
        ///     以……结束
        /// </summary>
        [OperateCode("endswith", "结尾包含")] EndsWith = 10,

        /// <summary>
        ///     包含（相似）
        /// </summary>
        [OperateCode("contains", "包含")] Contains = 11

        ///// <summary>
        ///// 包括在
        ///// </summary>
        //[OperateCode("in")]
        //In = 12,

        ///// <summary>
        ///// 不包括在
        ///// </summary>
        //[OperateCode("notin")]
        //NotIn = 13
    }
}