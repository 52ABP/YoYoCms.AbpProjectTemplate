using System.Collections.Generic;
using LTM.Common.Exceptions;
using LTM.Common.Properties;

namespace LTM.Common.Filter
{
    /// <summary>
    ///     筛选条件组
    /// </summary>
    public class FilterGroup
    {
        private FilterOperate _operate;

        /// <summary>
        ///     初始化一个<see cref="FilterGroup" />的新实例
        /// </summary>
        public FilterGroup()
        {
            Operate = FilterOperate.And;
            Rules = new List<FilterRule>();
            Groups = new List<FilterGroup>();
        }

        /// <summary>
        ///     使用操作方式初始化一个<see cref="FilterGroup" />的新实例
        /// </summary>
        /// <param name="operate">条件间操作方式</param>
        public FilterGroup(FilterOperate operate)
            : this()
        {
            Operate = operate;
        }

        /// <summary>
        ///     初始化一个<see cref="FilterGroup" />类型的新实例
        /// </summary>
        /// <param name="operateCode">条件间操作方式的前台码</param>
        public FilterGroup(string operateCode)
            : this(FilterHelper.GetFilterOperate(operateCode))
        {
        }

        /// <summary>
        ///     获取或设置 条件集合
        /// </summary>
        public ICollection<FilterRule> Rules { get; set; }

        /// <summary>
        ///     获取或设置 条件组集合
        /// </summary>
        public ICollection<FilterGroup> Groups { get; set; }

        /// <summary>
        ///     获取或设置 条件间操作方式，仅限And, Or
        /// </summary>
        public FilterOperate Operate
        {
            get { return _operate; }
            set
            {
                if (value != FilterOperate.And && value != FilterOperate.Or)
                {
                    throw new KingsSharpException(Resources.Filter_GroupOperateError);
                }
                _operate = value;
            }
        }
    }
}