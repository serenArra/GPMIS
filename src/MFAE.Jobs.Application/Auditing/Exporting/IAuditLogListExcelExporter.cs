using System.Collections.Generic;
using MFAE.Jobs.Auditing.Dto;
using MFAE.Jobs.Dto;

namespace MFAE.Jobs.Auditing.Exporting
{
    public interface IAuditLogListExcelExporter
    {
        FileDto ExportToFile(List<AuditLogListDto> auditLogListDtos);

        FileDto ExportToFile(List<EntityChangeListDto> entityChangeListDtos);
    }
}
