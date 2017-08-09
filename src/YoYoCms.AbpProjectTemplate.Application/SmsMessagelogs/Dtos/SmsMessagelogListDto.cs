
// 项目展示地址:"http://www.ddxc.org/"
 // 如果你有什么好的建议或者觉得可以加什么功能，请加QQ群：104390185大家交流沟通
// 项目展示地址:"http://www.yoyocms.com/"
//博客地址：http://www.cnblogs.com/wer-ltm/
//代码生成器帮助文档：http://www.cnblogs.com/wer-ltm/p/5777190.html
//<Author-作者>角落的白板笔</Author-作者>
// Copyright © YoYoCms@中国.2017-07-03T17:31:45. All Rights Reserved.
//<生成时间>2017-07-03T17:31:45</生成时间>

using System;
using System.ComponentModel;
using Abp.Application.Services.Dto;
using Abp.AutoMapper;

namespace YoYoCms.AbpProjectTemplate.SmsMessagelogs.Dtos
{
	/// <summary>
    /// 短信日志记录表列表Dto
    /// </summary>
    [AutoMapFrom(typeof(SmsMessagelog))]
    public class SmsMessagelogListDto : EntityDto<long>
    {
        /// <summary>
        /// 发送电话
        /// </summary>
        [DisplayName("发送电话")]
        public      string PhoneNumber { get; set; }
        /// <summary>
        /// 发送内容
        /// </summary>
        [DisplayName("发送内容")]
        public      string Content { get; set; }
        /// <summary>
        /// 失效时间
        /// </summary>
        [DisplayName("失效时间")]
        public      DateTime InvalidTime { get; set; }
        /// <summary>
        /// 是否发送成功
        /// </summary>
        [DisplayName("是否发送成功")]
        public      bool Sucess { get; set; }
        /// <summary>
        /// 是否已检测过
        /// </summary>
        [DisplayName("是否已检测过")]
        public      bool IsChecked { get; set; }
        /// <summary>
        /// 是否为通知短信
        /// </summary>
        [DisplayName("是否为通知短信")]
        public      bool IsNotification { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        [DisplayName("创建时间")]
        public      DateTime CreationTime { get; set; }
    }
}
