

// 项目展示地址:"http://www.ddxc.org/"
 // 如果你有什么好的建议或者觉得可以加什么功能，请加QQ群：104390185大家交流沟通
// 项目展示地址:"http://www.yoyocms.com/"
//博客地址：http://www.cnblogs.com/wer-ltm/
//代码生成器帮助文档：http://www.cnblogs.com/wer-ltm/p/5777190.html
// <Author-作者>角落的白板笔</Author-作者>
// Copyright © YoYoCms@中国.2017-07-03T17:31:49. All Rights Reserved.
//<生成时间>2017-07-03T17:31:49</生成时间>

using System.Collections.Generic;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using YoYoCms.AbpProjectTemplate.Dto;
using YoYoCms.AbpProjectTemplate.SmsMessagelogs.Dtos;

namespace YoYoCms.AbpProjectTemplate.SmsMessagelogs
{
	/// <summary>
    /// 短信日志记录表服务接口
    /// </summary>
    public interface ISmsMessagelogAppService : IApplicationService
    {
        #region 短信日志记录表管理

        /// <summary>
        /// 根据查询条件获取短信日志记录表分页列表
        /// </summary>
        Task<PagedResultDto<SmsMessagelogListDto>> GetPagedSmsMessagelogsAsync(GetSmsMessagelogInput input);

        /// <summary>
        /// 通过Id获取短信日志记录表信息进行编辑或修改 
        /// </summary>
        Task<GetSmsMessagelogForEditOutput> GetSmsMessagelogForEditAsync(NullableIdDto<long> input);

		  /// <summary>
        /// 通过指定id获取短信日志记录表ListDto信息
        /// </summary>
		Task<SmsMessagelogListDto> GetSmsMessagelogByIdAsync(EntityDto<long> input);



        /// <summary>
        /// 新增或更改短信日志记录表
        /// </summary>
        Task CreateOrUpdateSmsMessagelogAsync(CreateOrUpdateSmsMessagelogInput input);





        /// <summary>
        /// 新增短信日志记录表
        /// </summary>
        Task<SmsMessagelogEditDto> CreateSmsMessagelogAsync(SmsMessagelogEditDto input);

        /// <summary>
        /// 更新短信日志记录表
        /// </summary>
        Task UpdateSmsMessagelogAsync(SmsMessagelogEditDto input);

        /// <summary>
        /// 删除短信日志记录表
        /// </summary>
        Task DeleteSmsMessagelogAsync(EntityDto<long> input);

        /// <summary>
        /// 批量删除短信日志记录表
        /// </summary>
        Task BatchDeleteSmsMessagelogAsync(List<long> input);

        #endregion

#region Excel导出功能



         /// <summary>
        /// 获取短信日志记录表信息转换为Excel
        /// </summary>
        /// <returns></returns>
        Task<FileDto> GetSmsMessagelogToExcel();

#endregion





    }
}
