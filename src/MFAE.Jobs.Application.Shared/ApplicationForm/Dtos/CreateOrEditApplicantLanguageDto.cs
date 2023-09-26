﻿using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace MFAE.Jobs.ApplicationForm.Dtos
{
    public class CreateOrEditApplicantLanguageDto : EntityDto<long?>
    {

        [StringLength(ApplicantLanguageConsts.MaxNarrativeLength, MinimumLength = ApplicantLanguageConsts.MinNarrativeLength)]
        public string Narrative { get; set; }

        public long ApplicantId { get; set; }

        public int LanguageId { get; set; }

        public int ConversationId { get; set; }

        public int ConversationRateId { get; set; }

    }
}