using MFAE.Jobs.ApplicationForm.Enums;

using System;
using Abp.Application.Services.Dto;

namespace MFAE.Jobs.ApplicationForm.Dtos
{
    public class ApplicantStatusDto : EntityDto<long>
    {
        public ApplicantStatusEnum Status { get; set; }

        public string Description { get; set; }

        public long ApplicantId { get; set; }

    }
}