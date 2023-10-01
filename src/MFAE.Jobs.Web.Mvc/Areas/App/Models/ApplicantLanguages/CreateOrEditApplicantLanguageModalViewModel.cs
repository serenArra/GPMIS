using MFAE.Jobs.ApplicationForm.Dtos;
using System.Collections.Generic;

namespace MFAE.Jobs.Web.Areas.App.Models.ApplicantLanguages
{
    public class CreateOrEditApplicantLanguageModalViewModel
    {
        public CreateOrEditApplicantLanguageDto ApplicantLanguage { get; set; }

        public string ApplicantFirstName { get; set; }

        public string LanguageName { get; set; }

        public string ConversationName { get; set; }

        public string ConversationRateName { get; set; }

        public List<ApplicantLanguageApplicantLookupTableDto> ApplicantLanguageApplicantList { get; set; }

        public List<ApplicantLanguageLanguageLookupTableDto> ApplicantLanguageLanguageList { get; set; }

        public List<ApplicantLanguageConversationLookupTableDto> ApplicantLanguageConversationList { get; set; }

        public List<ApplicantLanguageConversationRateLookupTableDto> ApplicantLanguageConversationRateList { get; set; }

        public bool IsEditMode => ApplicantLanguage.Id.HasValue;
    }
}