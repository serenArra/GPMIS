using System;
using Abp.Application.Services.Dto;

namespace MFAE.Jobs.ApplicationForm.Dtos
{
    public class ApplicantLanguageDto : EntityDto<long>
    {
        public string Narrative { get; set; }

        public long ApplicantId { get; set; }

        public int LanguageId { get; set; }

        public int ConversationId { get; set; }

        public int ConversationRateId { get; set; }

    }
}