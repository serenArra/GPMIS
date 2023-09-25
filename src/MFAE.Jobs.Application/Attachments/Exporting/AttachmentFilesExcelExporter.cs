using System.Collections.Generic;
using Abp.Runtime.Session;
using Abp.Timing.Timezone;
using MFAE.Jobs.DataExporting.Excel.MiniExcel;
using MFAE.Jobs.Attachments.Dtos;
using MFAE.Jobs.Dto;
using MFAE.Jobs.Storage;

namespace MFAE.Jobs.Attachments.Exporting
{
    public class AttachmentFilesExcelExporter : MiniExcelExcelExporterBase, IAttachmentFilesExcelExporter
    {

        private readonly ITimeZoneConverter _timeZoneConverter;
        private readonly IAbpSession _abpSession;

        public AttachmentFilesExcelExporter(
            ITimeZoneConverter timeZoneConverter,
            IAbpSession abpSession,
            ITempFileCacheManager tempFileCacheManager) :
    base(tempFileCacheManager)
        {
            _timeZoneConverter = timeZoneConverter;
            _abpSession = abpSession;
        }

        public FileDto ExportToFile(List<GetAttachmentFileForViewDto> attachmentFiles)
        {
            var items = new List<Dictionary<string, object>>();

            foreach (var attachmentFile in attachmentFiles)
            {
                items.Add(new Dictionary<string, object>()
                {
                    {L("PhysicalName"), attachmentFile.AttachmentFile.PhysicalName},
                    {L("Description"), attachmentFile.AttachmentFile.Description},
                    {L("Size"), attachmentFile.AttachmentFile.Size},
                    {L("ObjectId"), attachmentFile.AttachmentFile.ObjectId},
                    {L("Path"), attachmentFile.AttachmentFile.Path},
                    {L("Version"), attachmentFile.AttachmentFile.Version},
                    {L("FileToken"), attachmentFile.AttachmentFile.FileToken},
                    {L("Tag"), attachmentFile.AttachmentFile.Tag},
                    {L("FilterKey"), attachmentFile.AttachmentFile.FilterKey},
                    {(L("AttachmentType")) + L("ArName"), attachmentFile.AttachmentTypeArName}
                });
            }

            return CreateExcelPackage("AttachmentFiles.xlsx", items);
        }
    }
}