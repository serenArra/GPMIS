using System.Collections.Generic;
using Abp.Runtime.Session;
using Abp.Timing.Timezone;
using MFAE.Jobs.ApplicationForm.Dtos;
using MFAE.Jobs.Dto;
using MFAE.Jobs.Storage;
using MFAE.Jobs.DataExporting.Excel.MiniExcel;

namespace MFAE.Jobs.ApplicationForm.Exporting
{
    public class LanguagesExcelExporter : MiniExcelExcelExporterBase, ILanguagesExcelExporter
    {

        private readonly ITimeZoneConverter _timeZoneConverter;
        private readonly IAbpSession _abpSession;

        public LanguagesExcelExporter(
            ITimeZoneConverter timeZoneConverter,
            IAbpSession abpSession,
            ITempFileCacheManager tempFileCacheManager) :
            base(tempFileCacheManager)
        {
            _timeZoneConverter = timeZoneConverter;
            _abpSession = abpSession;
        }

        public FileDto ExportToFile(List<GetAppLanguageForViewDto> languages)
        {
            var items = new List<Dictionary<string, object>>();

            foreach (var language in languages)
            {
                items.Add(new Dictionary<string, object>()
                {
                    {L("NameAr"), language.Language.NameAr},
                    {L("NameEn"), language.Language.NameEn},
                    {L("IsActive"), language.Language.IsActive}
                });
            }

            return CreateExcelPackage("Languages.xlsx", items);
        }
    }
}