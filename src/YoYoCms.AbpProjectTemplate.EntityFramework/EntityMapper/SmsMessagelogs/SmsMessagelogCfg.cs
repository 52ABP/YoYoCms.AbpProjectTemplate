 // 项目展示地址:"http://www.ddxc.org/"
 // 如果你有什么好的建议或者觉得可以加什么功能，请加QQ群：104390185大家交流沟通
// 项目展示地址:"http://www.yoyocms.com/"
//博客地址：http://www.cnblogs.com/wer-ltm/
//代码生成器帮助文档：http://www.cnblogs.com/wer-ltm/p/5777190.html
// <Author-作者>角落的白板笔</Author-作者>
// Copyright © YoYoCms@中国.2017-07-03T17:31:56. All Rights Reserved.
//<生成时间>2017-07-03T17:31:56</生成时间>

using System.Data.Entity.ModelConfiguration;
using YoYoCms.AbpProjectTemplate.EntityFramework;
using YoYoCms.AbpProjectTemplate.SmsMessagelogs;

namespace YoYoCms.AbpProjectTemplate.EntityMapper.SmsMessagelogs
{

	/// <summary>
    /// 短信日志记录表的数据配置文件
    /// </summary>
    public class SmsMessagelogCfg : EntityTypeConfiguration<SmsMessagelog>
    {
		/// <summary>
        ///  构造方法[默认链接字符串< see cref = "AbpProjectTemplateDbContext" /> ]
        /// </summary>
		public SmsMessagelogCfg ()
		{
		            ToTable("SmsMessagelog", AbpProjectTemplateConsts.SchemaName.ABP);
 





		    // 发送电话
			Property(a => a.PhoneNumber).HasMaxLength(4000);
		    // 发送内容
			Property(a => a.Content).HasMaxLength(4000);
		    // 短信代码(验证码)
			Property(a => a.SmsCode).HasMaxLength(4000);
		    // 短信提供商返回内容
			Property(a => a.Result).HasMaxLength(4000);



		}
    }
}