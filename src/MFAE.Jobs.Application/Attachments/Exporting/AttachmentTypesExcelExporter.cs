using System.Collections.Generic;
using Abp.Runtime.Session;
using Abp.Timing.Timezone;
using MFAE.Jobs.DataExporting.Excel.MiniExcel;
using MFAE.Jobs.Attachments.Dtos;
using MFAE.Jobs.Dto;
using MFAE.Jobs.Storage;

namespace MFAE.Jobs.Attachments.Exporting
{
    public class AttachmentTypesExcelExporter : MiniExcelExcelExporterBase, IAttachmentTypesExcelExporter
    {

        private readonly ITimeZoneConverter _timeZoneConverter;
        private readonly IAbpSession _abpSession;

        public AttachmentTypesExcelExporter(
            ITimeZoneConverter timeZoneConverter,
            IAbpSession abpSession,
            ITempFileCacheManager tempFileCacheManager) :
    base(tempFileCacheManager)
        {
            _timeZoneConverter = timeZoneConverter;
            _abpSession = abpSession;
        }

        public FileDto ExportToFile(List<GetAttachmentTypeForViewDto> attachmentTypes)
        {
            var items = new List<Dictionary<string, object>>();

            foreach (var attachmentType in attachmentTypes)
            {
                items.Add(new Dictionary<string, object>()
                {
                    {L("ArName"), attachmentType.AttachmentType.ArName},
                    {L("EnName"), attachmentType.AttachmentType.EnName},
                    {L("MaxSizeMB"), attachmentType.AttachmentType.MaxSizeMB},
                    {L("AllowedExtensions"), attachmentType.AttachmentType.AllowedExtensions},
                    {L("MaxAttachments"), attachmentType.AttachmentType.MaxAttachments},
                    {L("MinRequiredAttachments"), attachmentType.AttachmentType.MinRequiredAttachments},
                    {L("Category"), attachmentType.AttachmentType.Category},
                    {L("PrivacyFlag"), attachmentType.AttachmentType.PrivacyFlag},
                    {(L("AttachmentEntityType")) + L("Name"), attachmentType.AttachmentEntityTypeName},
                    {(L("AttachmentTypeGroup")) + L("Name"), attachmentType.AttachmentTypeGroupName}
                });
            }

            return CreateExcelPackage("AttachmentTypes.xlsx", items);
        }
    }
}