                                                                            

        
// 项目展示地址:"http://www.ddxc.org/"
 // 如果你有什么好的建议或者觉得可以加什么功能，请加QQ群：104390185大家交流沟通
// 项目展示地址:"http://www.yoyocms.com/"
//博客地址：http://www.cnblogs.com/wer-ltm/
//代码生成器帮助文档：http://www.cnblogs.com/wer-ltm/p/5777190.html
// <Author-作者>角落的白板笔</Author-作者>
// Copyright © YoYoCms@中国.2017-07-03T17:31:58. All Rights Reserved.
//<生成时间>2017-07-03T17:31:58</生成时间>

using System.Collections.Generic;
using Abp.Runtime.Session;
using Abp.Timing.Timezone;
using YoYoCms.AbpProjectTemplate.DataExporting.Excel.EpPlus;
using YoYoCms.AbpProjectTemplate.Dto;
using YoYoCms.AbpProjectTemplate.SmsMessagelogs.Dtos;

namespace YoYoCms.AbpProjectTemplate.SmsMessagelogs.Exporting
{
    /// <summary>
    /// 短信日志记录表的导出EXCEL功能的实现
    /// </summary>
    public class SmsMessagelogListExcelExporter : EpPlusExcelExporterBase, ISmsMessagelogListExcelExporter
    {
     
        private readonly ITimeZoneConverter _timeZoneConverter;
        private readonly IAbpSession _abpSession;


        /// <summary>
        /// 构造方法
        /// </summary>
        public SmsMessagelogListExcelExporter(ITimeZoneConverter timeZoneConverter,     IAbpSession abpSession)
        {
                       _timeZoneConverter = timeZoneConverter;    
                     _abpSession = abpSession;
        }

    

         /// <summary>
        /// 导出短信日志记录表到EXCEL文件
        /// <param name="smsMessagelogListDtos">导出信息的DTO</param>
        /// </summary>
    public    FileDto ExportSmsMessagelogToFile(List<SmsMessagelogListDto> smsMessagelogListDtos){


var file=CreateExcelPackage("smsMessagelogList.xlsx",excelPackage=>{

var sheet=excelPackage.Workbook.Worksheets.Add(L("SmsMessagelog"));
                    sheet.OutLineApplyStyle = true;

                    AddHeader(
                        sheet,
                          L("PhoneNumber"),  
     L("Content"),  
     L("InvalidTime"),  
     L("Sucess"),  
     L("IsChecked"),  
     L("IsNotification"),  
     L("CreationTime")
                        );
         AddObjects(sheet,2,smsMessagelogListDtos,
            
      _ => _.PhoneNumber,   
       
      _ => _.Content,   
       
 _ =>_timeZoneConverter.Convert( _.InvalidTime,_abpSession.TenantId, _abpSession.GetUserId()),          
      _ => _.Sucess,   
       
      _ => _.IsChecked,   
       
      _ => _.IsNotification,   
       
 _ =>_timeZoneConverter.Convert( _.CreationTime,_abpSession.TenantId, _abpSession.GetUserId())   
);
                    //写个时间转换的吧
          //var creationTimeColumn = sheet.Column(10);
                    //creationTimeColumn.Style.Numberformat.Format = "yyyy-mm-dd";

        for (var i = 1; i <= 7; i++)
                    {
                        sheet.Column(i).AutoFit();
                    }       

});
   return file;

}


 

 
  

    }
    }
