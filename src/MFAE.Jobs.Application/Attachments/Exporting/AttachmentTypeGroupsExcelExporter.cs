using System.Collections.Generic;
using Abp.Runtime.Session;
using Abp.Timing.Timezone;
using MFAE.Jobs.DataExporting.Excel.MiniExcel;
using MFAE.Jobs.Attachments.Dtos;
using MFAE.Jobs.Dto;
using MFAE.Jobs.Storage;

namespace MFAE.Jobs.Attachments.Exporting
{
    public class AttachmentTypeGroupsExcelExporter : MiniExcelExcelExporterBase, IAttachmentTypeGroupsExcelExporter
    {

        private readonly ITimeZoneConverter _timeZoneConverter;
        private readonly IAbpSession _abpSession;

        public AttachmentTypeGroupsExcelExporter(
            ITimeZoneConverter timeZoneConverter,
            IAbpSession abpSession,
            ITempFileCacheManager tempFileCacheManager) :
    base(tempFileCacheManager)
        {
            _timeZoneConverter = timeZoneConverter;
            _abpSession = abpSession;
        }

        public FileDto ExportToFile(List<GetAttachmentTypeGroupForViewDto> attachmentTypeGroups)
        {
            var items = new List<Dictionary<string, object>>();

            foreach (var attachmentTypeGroup in attachmentTypeGroups)
            {
                items.Add(new Dictionary<string, object>()
                {
                    {L("ArName"), attachmentTypeGroup.AttachmentTypeGroup.ArName},
                    {L("EnName"), attachmentTypeGroup.AttachmentTypeGroup.EnName},
                   
                });
            }

            return CreateExcelPackage("AttachmentTypeGroups.xlsx", items);
        }
    }
}