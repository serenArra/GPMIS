using System.Collections.Generic;
using Abp.Runtime.Session;
using Abp.Timing.Timezone;
using MFAE.Jobs.ApplicationForm.Dtos;
using MFAE.Jobs.Dto;
using MFAE.Jobs.Storage;
using MFAE.Jobs.DataExporting.Excel.MiniExcel;

namespace MFAE.Jobs.ApplicationForm.Exporting
{
    public class JobAdvertisementsExcelExporter : MiniExcelExcelExporterBase, IJobAdvertisementsExcelExporter
    {

        private readonly ITimeZoneConverter _timeZoneConverter;
        private readonly IAbpSession _abpSession;

        public JobAdvertisementsExcelExporter(
            ITimeZoneConverter timeZoneConverter,
            IAbpSession abpSession,
            ITempFileCacheManager tempFileCacheManager) :
    base(tempFileCacheManager)
        {
            _timeZoneConverter = timeZoneConverter;
            _abpSession = abpSession;
        }

        public FileDto ExportToFile(List<GetJobAdvertisementForViewDto> jobAdvertisements)
        {
            var items = new List<Dictionary<string, object>>();

            foreach (var jobAdvertisement in jobAdvertisements)
            {
                items.Add(new Dictionary<string, object>()
                {
                    {L("AdvertisementId"), jobAdvertisement.JobAdvertisement.AdvertisementId},
                    {L("AdvertisementDate"), jobAdvertisement.JobAdvertisement.AdvertisementDate},
                    {L("FromDate"), jobAdvertisement.JobAdvertisement.FromDate},
                    {L("ToDate"), jobAdvertisement.JobAdvertisement.ToDate},
                    {L("AllowedAge"), jobAdvertisement.JobAdvertisement.AllowedAge},
                    {L("Description"), jobAdvertisement.JobAdvertisement.Description},
                    {L("IsActive"), jobAdvertisement.JobAdvertisement.IsActive}
                    
                });
            }

            return CreateExcelPackage("JobAdvertisements.xlsx", items);
        }
    }
}