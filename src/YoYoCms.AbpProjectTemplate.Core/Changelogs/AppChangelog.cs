using System;
using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;

namespace YoYoCms.AbpProjectTemplate.Changelogs
{
    /// <summary>
    /// 系统更新日志
    /// </summary>
    public class AppChangelog:CreationAuditedEntity<long>
    {
        /// <summary>
        /// 标题
        /// </summary>
        public string Title { get; set; }
        /// <summary>
        /// 更新时间
        /// </summary>
        public DateTime ChangeTime { get; set; }

        /// <summary>
        /// 更新内容
        /// </summary>
        public string ChangeContent { get; set; }




    }
}