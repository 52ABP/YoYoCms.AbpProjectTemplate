// 项目展示地址:"http://www.ddxc.org/"
 // 如果你有什么好的建议或者觉得可以加什么功能，请加QQ群：104390185大家交流沟通
// 项目展示地址:"http://www.yoyocms.com/"
//博客地址：http://www.cnblogs.com/wer-ltm/
//代码生成器帮助文档：http://www.cnblogs.com/wer-ltm/p/5777190.html
// <Author-作者>角落的白板笔</Author-作者>
// Copyright © YoYoCms@中国.2017-07-03T17:31:53. All Rights Reserved.
//<生成时间>2017-07-03T17:31:53</生成时间>

using Abp.Domain.Repositories;
using Abp.Domain.Services;

namespace YoYoCms.AbpProjectTemplate.SmsMessagelogs
{
    /// <summary>
    /// 短信日志记录表业务管理
    /// </summary>
    public class SmsMessagelogManage : IDomainService
    {
        private readonly IRepository<SmsMessagelog,long> _smsMessagelogRepository;

         /// <summary>
        /// 构造方法
        /// </summary>
        public SmsMessagelogManage(IRepository<SmsMessagelog,long> smsMessagelogRepository  )
        {
            _smsMessagelogRepository = smsMessagelogRepository;
        }

		//TODO:编写领域业务代码


		/// <summary>
        ///     初始化
        /// </summary>
        private void Init()
        {


 

        }


		}

    
	
}
