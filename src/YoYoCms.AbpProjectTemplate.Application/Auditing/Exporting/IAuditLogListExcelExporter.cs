using System.Collections.Generic;
using YoYoCms.AbpProjectTemplate.Auditing.Dto;
using YoYoCms.AbpProjectTemplate.Dto;

namespace YoYoCms.AbpProjectTemplate.Auditing.Exporting
{
    public interface IAuditLogListExcelExporter
    {
        FileDto ExportToFile(List<AuditLogListDto> auditLogListDtos);
    }
}
