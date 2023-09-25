using Abp.Application.Services.Dto;
using System;

namespace MFAE.Jobs.ApplicationForm.Dtos
{
    public class GetAllApplicantLanguagesForExcelInput
    {
        public string Filter { get; set; }

        public string NarrativeFilter { get; set; }

        public string ApplicantFirstNameFilter { get; set; }

        public string LanguageNameFilter { get; set; }

        public string ConversationNameFilter { get; set; }

        public string ConversationRateNameFilter { get; set; }

    }
}