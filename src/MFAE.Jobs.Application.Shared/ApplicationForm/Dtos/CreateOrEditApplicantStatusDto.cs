using MFAE.Jobs.ApplicationForm.Enums;

using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace MFAE.Jobs.ApplicationForm.Dtos
{
    public class CreateOrEditApplicantStatusDto : EntityDto<long?>
    {

        public ApplicantStatusEnum Status { get; set; }

        [StringLength(ApplicantStatusConsts.MaxDescriptionLength, MinimumLength = ApplicantStatusConsts.MinDescriptionLength)]
        public string Description { get; set; }

        public long ApplicantId { get; set; }

    }
}