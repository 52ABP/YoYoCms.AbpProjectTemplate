                                                                    
     
        
// 项目展示地址:"http://www.ddxc.org/"
 // 如果你有什么好的建议或者觉得可以加什么功能，请加QQ群：104390185大家交流沟通
// 项目展示地址:"http://www.yoyocms.com/"
//博客地址：http://www.cnblogs.com/wer-ltm/
//代码生成器帮助文档：http://www.cnblogs.com/wer-ltm/p/5777190.html
// <Author-作者>角落的白板笔</Author-作者>
// Copyright © YoYoCms@中国.2017-07-03T17:31:50. All Rights Reserved.
//<生成时间>2017-07-03T17:31:50</生成时间>

using System.Collections.Generic;
using System.Data.Entity;
using System.Linq.Dynamic;
using System.Threading.Tasks;
using Abp.Application.Services.Dto;
using Abp.Authorization;
using Abp.AutoMapper;
using Abp.Domain.Repositories;
using Abp.Linq.Extensions;
using YoYoCms.AbpProjectTemplate.Dto;
using YoYoCms.AbpProjectTemplate.SmsMessagelogs.Authorization;
using YoYoCms.AbpProjectTemplate.SmsMessagelogs.Dtos;
using YoYoCms.AbpProjectTemplate.SmsMessagelogs.Exporting;

namespace YoYoCms.AbpProjectTemplate.SmsMessagelogs
{
    /// <summary>
    /// 短信日志记录表服务实现
    /// </summary>
    [AbpAuthorize(SmsMessagelogAppPermissions.SmsMessagelog)]
    public class SmsMessagelogAppService : AbpProjectTemplateAppServiceBase, ISmsMessagelogAppService
    {
        private readonly IRepository<SmsMessagelog,long> _smsMessagelogRepository;
		           private readonly ISmsMessagelogListExcelExporter _smsMessagelogListExcelExporter;
           

		private readonly SmsMessagelogManage _smsMessagelogManage;
        /// <summary>
        /// 构造方法
        /// </summary>
        public SmsMessagelogAppService( IRepository<SmsMessagelog,long> smsMessagelogRepository	,
SmsMessagelogManage smsMessagelogManage
	  ,            ISmsMessagelogListExcelExporter smsMessagelogListExcelExporter  
  )
        {
            _smsMessagelogRepository = smsMessagelogRepository;
			 _smsMessagelogManage = smsMessagelogManage;
			             _smsMessagelogListExcelExporter = smsMessagelogListExcelExporter;  
        }

    #region 短信日志记录表管理

    /// <summary>
    /// 根据查询条件获取短信日志记录表分页列表
    /// </summary>
    public async Task<PagedResultDto<SmsMessagelogListDto>> GetPagedSmsMessagelogsAsync(GetSmsMessagelogInput input)
{
			
    var query = _smsMessagelogRepository.GetAll().WhereIf(string.IsNullOrEmpty(input.FilterText),a=>a.PhoneNumber==input.FilterText);
    //TODO:根据传入的参数添加过滤条件

    var smsMessagelogCount = await query.CountAsync();

    var smsMessagelogs = await query
    .OrderBy(input.Sorting)
    .PageBy(input)
    .ToListAsync();

    var smsMessagelogListDtos = smsMessagelogs.MapTo<List<SmsMessagelogListDto>>();
    return new PagedResultDto<SmsMessagelogListDto>(
    smsMessagelogCount,
    smsMessagelogListDtos
    );
    }

        /// <summary>
    /// 通过Id获取短信日志记录表信息进行编辑或修改 
    /// </summary>
    public async Task<GetSmsMessagelogForEditOutput> GetSmsMessagelogForEditAsync(NullableIdDto<long> input)
{
    var output=new GetSmsMessagelogForEditOutput();

    SmsMessagelogEditDto smsMessagelogEditDto;

    if(input.Id.HasValue)
	{
    var entity=await _smsMessagelogRepository.GetAsync(input.Id.Value);
    smsMessagelogEditDto=entity.MapTo<SmsMessagelogEditDto>();
    }
	else 
	{
	smsMessagelogEditDto=new SmsMessagelogEditDto();	
	}

	output.SmsMessagelog=smsMessagelogEditDto;
	return output;
    }


    /// <summary>
    /// 通过指定id获取短信日志记录表ListDto信息
    /// </summary>
    public async Task<SmsMessagelogListDto> GetSmsMessagelogByIdAsync(EntityDto<long> input)
{
    var entity = await _smsMessagelogRepository.GetAsync(input.Id);

    return entity.MapTo<SmsMessagelogListDto>();
    }







    /// <summary>
    /// 新增或更改短信日志记录表
    /// </summary>
    public async Task CreateOrUpdateSmsMessagelogAsync(CreateOrUpdateSmsMessagelogInput input)
{
    if (input.SmsMessagelogEditDto.Id.HasValue)
{
    await UpdateSmsMessagelogAsync(input.SmsMessagelogEditDto);
    }
    else
{
    await CreateSmsMessagelogAsync(input.SmsMessagelogEditDto);
    }
    }

    /// <summary>
    /// 新增短信日志记录表
    /// </summary>
    [AbpAuthorize(SmsMessagelogAppPermissions.SmsMessagelog_CreateSmsMessagelog)]
    public virtual async Task<SmsMessagelogEditDto> CreateSmsMessagelogAsync(SmsMessagelogEditDto input)
{
    //TODO:新增前的逻辑判断，是否允许新增

	var entity = input.MapTo<SmsMessagelog>();
	
    entity = await _smsMessagelogRepository.InsertAsync(entity);
    return entity.MapTo<SmsMessagelogEditDto>();
    }

    /// <summary>
    /// 编辑短信日志记录表
    /// </summary>
    [AbpAuthorize(SmsMessagelogAppPermissions.SmsMessagelog_EditSmsMessagelog)]
    public virtual async Task UpdateSmsMessagelogAsync(SmsMessagelogEditDto input)
{
    //TODO:更新前的逻辑判断，是否允许更新

    var entity = await _smsMessagelogRepository.GetAsync(input.Id.Value);
	input.MapTo(entity);

    await _smsMessagelogRepository.UpdateAsync(entity);
    }

    /// <summary>
    /// 删除短信日志记录表
    /// </summary>
    [AbpAuthorize(SmsMessagelogAppPermissions.SmsMessagelog_DeleteSmsMessagelog)]
    public async Task DeleteSmsMessagelogAsync(EntityDto<long> input)
{
    //TODO:删除前的逻辑判断，是否允许删除
    await _smsMessagelogRepository.DeleteAsync(input.Id);
    }

        /// <summary>
    /// 批量删除短信日志记录表
    /// </summary>
    [AbpAuthorize(SmsMessagelogAppPermissions.SmsMessagelog_DeleteSmsMessagelog)]
    public async Task BatchDeleteSmsMessagelogAsync(List<long> input)
{
    //TODO:批量删除前的逻辑判断，是否允许删除
    await _smsMessagelogRepository.DeleteAsync(s=>input.Contains(s.Id));
    }

            #endregion


  #region 短信日志记录表的Excel导出功能


        public async Task<FileDto> GetSmsMessagelogToExcel()
        {
                var entities = await    _smsMessagelogRepository.GetAll().ToListAsync();     

var dtos=             entities.MapTo<List<SmsMessagelogListDto>>();

var fileDto=_smsMessagelogListExcelExporter.ExportSmsMessagelogToFile(dtos);
 
           

            return fileDto;
        }


#endregion
  









    }
    }
