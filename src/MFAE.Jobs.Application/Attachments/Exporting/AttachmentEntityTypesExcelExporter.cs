using System.Collections.Generic;
using Abp.Runtime.Session;
using Abp.Timing.Timezone;
using MFAE.Jobs.DataExporting.Excel.MiniExcel;
using MFAE.Jobs.Attachments.Dtos;
using MFAE.Jobs.Dto;
using MFAE.Jobs.Storage;

namespace MFAE.Jobs.Attachments.Exporting
{
    public class AttachmentEntityTypesExcelExporter : MiniExcelExcelExporterBase, IAttachmentEntityTypesExcelExporter
    {

        private readonly ITimeZoneConverter _timeZoneConverter;
        private readonly IAbpSession _abpSession;

        public AttachmentEntityTypesExcelExporter(
            ITimeZoneConverter timeZoneConverter,
            IAbpSession abpSession,
            ITempFileCacheManager tempFileCacheManager) :
    base(tempFileCacheManager)
        {
            _timeZoneConverter = timeZoneConverter;
            _abpSession = abpSession;
        }

        public FileDto ExportToFile(List<GetAttachmentEntityTypeForViewDto> attachmentEntityTypes)
        {
            var items = new List<Dictionary<string, object>>();

            foreach (var attachmentEntityType in attachmentEntityTypes)
            {
                items.Add(new Dictionary<string, object>()
                {
                    {L("ArName"), attachmentEntityType.AttachmentEntityType.ArName},
                    {L("EnName"), attachmentEntityType.AttachmentEntityType.EnName},
                    {L("Folder"), attachmentEntityType.AttachmentEntityType.Folder},
                    {L("ParentTypeId"), attachmentEntityType.AttachmentEntityType.ParentTypeId}                    
                });
            }

            return CreateExcelPackage("AttachmentEntityTypes.xlsx", items);
        }
    }
}