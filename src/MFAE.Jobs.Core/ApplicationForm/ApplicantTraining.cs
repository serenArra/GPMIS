using MFAE.Jobs.ApplicationForm.Enums;
using MFAE.Jobs.ApplicationForm;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities.Auditing;
using Abp.Domain.Entities;
using Abp.Auditing;

namespace MFAE.Jobs.ApplicationForm
{
    [Table("ApplicantTrainings")]
    [Audited]
    public class ApplicantTraining : FullAuditedEntity<long>
    {

        [Required]
        [StringLength(ApplicantTrainingConsts.MaxSubjectLength, MinimumLength = ApplicantTrainingConsts.MinSubjectLength)]
        public virtual string Subject { get; set; }

        [Required]
        [StringLength(ApplicantTrainingConsts.MaxLocationLength, MinimumLength = ApplicantTrainingConsts.MinLocationLength)]
        public virtual string Location { get; set; }

        public virtual DateTime TrainingDate { get; set; }

        public virtual int Duration { get; set; }

        public virtual DurationType DurationType { get; set; }

        public virtual long ApplicantId { get; set; }

        [ForeignKey("ApplicantId")]
        public Applicant ApplicantFk { get; set; }

    }
}