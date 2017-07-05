// ------------------------------------------------------------------------------
// Copyright  成都积微物联电子商务有限公司 版权所有。 
// 项目名：Jwell.CloudProcurement.Application 
// 文件名：ContentType.cs
// 创建标识：吴来伟 2017-03-07
// 创建描述：
// 
// 修改标识：
// 修改描述：
//  ------------------------------------------------------------------------------

using System.ComponentModel;

namespace LTM.Common.Enums
{
    /// <summary>
    /// 基础表类型
    /// </summary>
    public enum ContentType
    {
        /// <summary>
        /// 无
        /// </summary>
        [Description("无")]
        Wu = 0,

        /// <summary>
        /// 新闻
        /// </summary>
        [Description("新闻")]
        News =1,

        /// <summary>
        /// 产品
        /// </summary>
        [Description("产品")]
        Product =2
    }
}