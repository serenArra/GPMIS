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
    [Table("ApplicantStatuses")]
    [Audited]
    public class ApplicantStatus : FullAuditedEntity<long>
    {

        public virtual ApplicantStatusEnum Status { get; set; }

        [StringLength(ApplicantStatusConsts.MaxDescriptionLength, MinimumLength = ApplicantStatusConsts.MinDescriptionLength)]
        public virtual string Description { get; set; }

        public virtual long ApplicantId { get; set; }

        [ForeignKey("ApplicantId")]
        public Applicant ApplicantFk { get; set; }

    }
}