using MFAE.Jobs.ApplicationForm.Enums;

using System;
using Abp.Application.Services.Dto;

namespace MFAE.Jobs.ApplicationForm.Dtos
{
    public class ApplicantTrainingDto : EntityDto<long>
    {
        public string Subject { get; set; }

        public string Location { get; set; }

        public DateTime TrainingDate { get; set; }

        public int Duration { get; set; }

        public DurationType DurationType { get; set; }

        public long ApplicantId { get; set; }

    }
}