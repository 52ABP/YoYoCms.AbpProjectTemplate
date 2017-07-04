
// 项目展示地址:"http://www.ddxc.org/"
 // 如果你有什么好的建议或者觉得可以加什么功能，请加QQ群：104390185大家交流沟通
// 项目展示地址:"http://www.yoyocms.com/"
//博客地址：http://www.cnblogs.com/wer-ltm/
//代码生成器帮助文档：http://www.cnblogs.com/wer-ltm/p/5777190.html
// <Author-作者>角落的白板笔</Author-作者>
// Copyright © YoYoCms@中国.2017-07-03T17:31:44. All Rights Reserved.
//<生成时间>2017-07-03T17:31:44</生成时间>

using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Abp.AutoMapper;

namespace YoYoCms.AbpProjectTemplate.SmsMessagelogs.Dtos
{
    /// <summary>
    /// 短信日志记录表编辑用Dto
    /// </summary>
    [AutoMap(typeof(SmsMessagelog))]
    public class SmsMessagelogEditDto 
    {

	/// <summary>
    ///   主键Id
    /// </summary>
    [DisplayName("主键Id")]
	public long? Id{get;set;}

        /// <summary>
        /// 发送电话
        /// </summary>
        [DisplayName("发送电话")]
        [Required]
        public   string  PhoneNumber { get; set; }

        /// <summary>
        /// 发送内容
        /// </summary>
        [DisplayName("发送内容")]
        public   string  Content { get; set; }

        /// <summary>
        /// 短信代码(验证码)
        /// </summary>
        [DisplayName("短信代码(验证码)")]
        public   string  SmsCode { get; set; }

        /// <summary>
        /// 失效时间
        /// </summary>
        [DisplayName("失效时间")]
        public   DateTime  InvalidTime { get; set; }

        /// <summary>
        /// 是否发送成功
        /// </summary>
        [DisplayName("是否发送成功")]
        public   bool  Sucess { get; set; }

        /// <summary>
        /// 短信提供商返回内容
        /// </summary>
        [DisplayName("短信提供商返回内容")]
        public   string  Result { get; set; }

        /// <summary>
        /// 是否已检测过
        /// </summary>
        [DisplayName("是否已检测过")]
        public   bool  IsChecked { get; set; }

        /// <summary>
        /// 是否为通知短信
        /// </summary>
        [DisplayName("是否为通知短信")]
        public   bool  IsNotification { get; set; }

    }
}
