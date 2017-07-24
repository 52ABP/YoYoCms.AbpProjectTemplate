using System;

namespace LTM.Common.Filter
{
    /// <summary>
    ///     表示查询操作的前台操作码
    /// </summary>
    public class OperateCodeAttribute : Attribute
    {
        /// <summary>
        ///     初始化一个<see cref="OperateCodeAttribute" />类型的新实例
        /// </summary>
        public OperateCodeAttribute(string code)
        {
            Code = code;
        }

        /// <summary>
        ///     初始化一个<see cref="OperateCodeAttribute" />类型的新实例
        /// </summary>
        /// <param name="code"></param>
        /// <param name="name"></param>
        public OperateCodeAttribute(string code, string name)
        {
            Code = code;
            Name = name;
        }

        /// <summary>
        ///     获取 属性名称
        /// </summary>
        public string Code { get; private set; }

        /// <summary>
        ///     前台名称
        /// </summary>
        public string Name { get; private set; }
    }
}