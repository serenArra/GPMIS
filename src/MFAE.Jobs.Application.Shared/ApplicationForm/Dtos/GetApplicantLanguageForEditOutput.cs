using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace MFAE.Jobs.ApplicationForm.Dtos
{
    public class GetApplicantLanguageForEditOutput
    {
        public CreateOrEditApplicantLanguageDto ApplicantLanguage { get; set; }

        public string ApplicantFirstName { get; set; }

        public string LanguageName { get; set; }

        public string ConversationName { get; set; }

        public string ConversationRateName { get; set; }

    }
}