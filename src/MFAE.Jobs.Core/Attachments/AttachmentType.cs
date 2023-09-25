using MFAE.Jobs.Attachments;
using MFAE.Jobs.Attachments;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities.Auditing;
using Abp.Domain.Entities;
using Abp.Auditing;

namespace MFAE.Jobs.Attachments
{
    [Table("AttachmentTypes")]
    [Audited]
    public class AttachmentType : FullAuditedEntity
    {

        [Required]
        [StringLength(AttachmentTypeConsts.MaxArNameLength, MinimumLength = AttachmentTypeConsts.MinArNameLength)]
        public virtual string ArName { get; set; }

        [Required]
        [StringLength(AttachmentTypeConsts.MaxEnNameLength, MinimumLength = AttachmentTypeConsts.MinEnNameLength)]
        public virtual string EnName { get; set; }

        [Range(AttachmentTypeConsts.MinMaxSizeMBValue, AttachmentTypeConsts.MaxMaxSizeMBValue)]
        public virtual int MaxSizeMB { get; set; }

        public virtual string AllowedExtensions { get; set; }

        [Range(AttachmentTypeConsts.MinMaxAttachmentsValue, AttachmentTypeConsts.MaxMaxAttachmentsValue)]
        public virtual int MaxAttachments { get; set; }

        [Range(AttachmentTypeConsts.MinMinRequiredAttachmentsValue, AttachmentTypeConsts.MaxMinRequiredAttachmentsValue)]
        public virtual int MinRequiredAttachments { get; set; }

        public virtual AttachmentTypeCategories Category { get; set; }

        public virtual PrivacyFlag PrivacyFlag { get; set; }

        public virtual int? EntityTypeId { get; set; }

        [ForeignKey("EntityTypeId")]
        public AttachmentEntityType EntityTypeFk { get; set; }

        public virtual int? GroupId { get; set; }

        [ForeignKey("GroupId")]
        public AttachmentTypeGroup GroupFk { get; set; }

    }
}