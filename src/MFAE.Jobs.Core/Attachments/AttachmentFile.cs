using MFAE.Jobs.Attachments;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities.Auditing;
using Abp.Domain.Entities;
using Abp.Auditing;

namespace MFAE.Jobs.Attachments
{
    [Table("AttachmentFiles")]
    [Audited]
    public class AttachmentFile : FullAuditedEntity
    {

        [Required]
        [StringLength(AttachmentFileConsts.MaxPhysicalNameLength, MinimumLength = AttachmentFileConsts.MinPhysicalNameLength)]
        public virtual string PhysicalName { get; set; }

        [StringLength(AttachmentFileConsts.MaxDescriptionLength, MinimumLength = AttachmentFileConsts.MinDescriptionLength)]
        public virtual string Description { get; set; }

        [Required]
        [StringLength(AttachmentFileConsts.MaxOriginalNameLength, MinimumLength = AttachmentFileConsts.MinOriginalNameLength)]
        public virtual string OriginalName { get; set; }

        public virtual long Size { get; set; }

        [Required]
        public virtual string ObjectId { get; set; }

        public virtual string Path { get; set; }

        public virtual int Version { get; set; }

        [Required]
        public virtual string FileToken { get; set; }

        public virtual string Tag { get; set; }

        [StringLength(AttachmentFileConsts.MaxFilterKeyLength, MinimumLength = AttachmentFileConsts.MinFilterKeyLength)]
        public virtual string FilterKey { get; set; }

        public virtual int AttachmentTypeId { get; set; }

        [ForeignKey("AttachmentTypeId")]
        public AttachmentType AttachmentTypeFk { get; set; }

    }
}