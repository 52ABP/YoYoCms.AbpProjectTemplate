using System;
using Abp.Domain.Entities.Auditing;

namespace YoYoCms.AbpProjectTemplate.SmsMessagelogs
{
    /// <summary>
    /// 短信日志记录表
    /// </summary>
    public class SmsMessagelog:AuditedEntity<long>
    {
        /// <summary>
        /// 发送电话
        /// </summary>
        public string PhoneNumber { get; set; }
        /// <summary>
        /// 发送内容
        /// </summary>
        public string Content { get; set; }

        /// <summary>
        /// 短信代码(验证码)
        /// </summary>
        public string SmsCode { get; set; }

        /// <summary>
        /// 失效时间
        /// </summary>
        public DateTime InvalidTime { get; set; }

        /// <summary>
        /// 是否发送成功
        /// </summary>
        public bool Sucess { get; set; }

        /// <summary>
        /// 短信提供商返回内容
        /// </summary>
        public string Result { get; set; }

        /// <summary>
        /// 是否已检测过
        /// </summary>
        public bool IsChecked { get; set; }

        /// <summary>
        /// 是否为通知短信
        /// </summary>
        public  bool IsNotification { get; set; }

    }
}