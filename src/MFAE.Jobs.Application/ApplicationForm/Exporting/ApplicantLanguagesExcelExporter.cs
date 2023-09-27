using System.Collections.Generic;
using Abp.Runtime.Session;
using Abp.Timing.Timezone;
using MFAE.Jobs.ApplicationForm.Dtos;
using MFAE.Jobs.Dto;
using MFAE.Jobs.Storage;
using MFAE.Jobs.DataExporting.Excel.MiniExcel;

namespace MFAE.Jobs.ApplicationForm.Exporting
{
    public class ApplicantLanguagesExcelExporter : MiniExcelExcelExporterBase, IApplicantLanguagesExcelExporter
    {

        private readonly ITimeZoneConverter _timeZoneConverter;
        private readonly IAbpSession _abpSession;

        public ApplicantLanguagesExcelExporter(
            ITimeZoneConverter timeZoneConverter,
            IAbpSession abpSession,
            ITempFileCacheManager tempFileCacheManager) :
    base(tempFileCacheManager)
        {
            _timeZoneConverter = timeZoneConverter;
            _abpSession = abpSession;
        }

        public FileDto ExportToFile(List<GetApplicantLanguageForViewDto> applicantLanguages)
        {
            var items = new List<Dictionary<string, object>>();

            foreach (var applicantLanguage in applicantLanguages)
            {
                items.Add(new Dictionary<string, object>()
                {
                    {L("Narrative"), applicantLanguage.ApplicantLanguage.Narrative},
                    {L("Name"), applicantLanguage.ApplicantFirstName},
                    {L("Language"), applicantLanguage.LanguageName},
                    {L("Conversation"), applicantLanguage.ConversationName},
                    {L("ConversationRate"), applicantLanguage.ConversationRateName},
                });
            }

            return CreateExcelPackage("ApplicantLanguages.xlsx", items);
        }
    }
}