using MFAE.Jobs.ApplicationForm;
using MFAE.Jobs.ApplicationForm;
using MFAE.Jobs.ApplicationForm;
using MFAE.Jobs.ApplicationForm;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities.Auditing;
using Abp.Domain.Entities;
using Abp.Auditing;

namespace MFAE.Jobs.ApplicationForm
{
    [Table("ApplicantLanguages")]
    [Audited]
    public class ApplicantLanguage : FullAuditedEntity<long>
    {

        [StringLength(ApplicantLanguageConsts.MaxNarrativeLength, MinimumLength = ApplicantLanguageConsts.MinNarrativeLength)]
        public virtual string Narrative { get; set; }

        public virtual long ApplicantId { get; set; }

        [ForeignKey("ApplicantId")]
        public Applicant ApplicantFk { get; set; }

        public virtual int LanguageId { get; set; }

        [ForeignKey("LanguageId")]
        public AppLanguage LanguageFk { get; set; }

        public virtual int ConversationId { get; set; }

        [ForeignKey("ConversationId")]
        public Conversation ConversationFk { get; set; }

        public virtual int ConversationRateId { get; set; }

        [ForeignKey("ConversationRateId")]
        public ConversationRate ConversationRateFk { get; set; }

    }
}