using MFAE.Jobs.ApplicationForm.Enums;

using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace MFAE.Jobs.ApplicationForm.Dtos
{
    public class CreateOrEditApplicantTrainingDto : EntityDto<long?>
    {

        [Required]
        [StringLength(ApplicantTrainingConsts.MaxSubjectLength, MinimumLength = ApplicantTrainingConsts.MinSubjectLength)]
        public string Subject { get; set; }

        [Required]
        [StringLength(ApplicantTrainingConsts.MaxLocationLength, MinimumLength = ApplicantTrainingConsts.MinLocationLength)]
        public string Location { get; set; }

        public DateTime TrainingDate { get; set; }

        public int Duration { get; set; }

        public DurationType DurationType { get; set; }

        public long ApplicantId { get; set; }

    }
}