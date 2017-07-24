
// 项目展示地址:"http://www.ddxc.org/"
 // 如果你有什么好的建议或者觉得可以加什么功能，请加QQ群：104390185大家交流沟通
// 项目展示地址:"http://www.yoyocms.com/"
//博客地址：http://www.cnblogs.com/wer-ltm/
//代码生成器帮助文档：http://www.cnblogs.com/wer-ltm/p/5777190.html
// <Author-作者>角落的白板笔</Author-作者>
// Copyright © YoYoCms@中国.2017-07-03T17:31:57. All Rights Reserved.
//<生成时间>2017-07-03T17:31:57</生成时间>

using System.Collections.Generic;
using YoYoCms.AbpProjectTemplate.Dto;
using YoYoCms.AbpProjectTemplate.SmsMessagelogs.Dtos;

namespace YoYoCms.AbpProjectTemplate.SmsMessagelogs.Exporting
{
	/// <summary>
    /// 短信日志记录表的数据导出功能 
    /// </summary>
    public interface ISmsMessagelogListExcelExporter
    {
        
//## 可以将下面的这个实体类，作为filedto来进行接收 


    //public class FileDto
    //{
    //    [Required]
    //    public string FileName { get; set; }

    //    [Required]
    //    public string FileType { get; set; }

    //    [Required]
    //    public string FileToken { get; set; }

    //    public FileDto()
    //    {
            
    //    }

    //    public FileDto(string fileName, string fileType)
    //    {
    //        FileName = fileName;
    //        FileType = fileType;
    //        FileToken = Guid.NewGuid().ToString("N");
    //    }
    //}

        /// <summary>
        /// 导出短信日志记录表到EXCEL文件
        /// <param name="smsMessagelogListDtos">导出信息的DTO</param>
        /// </summary>
        FileDto ExportSmsMessagelogToFile(List<SmsMessagelogListDto> smsMessagelogListDtos);



    }
}
