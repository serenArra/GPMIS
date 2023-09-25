using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace MFAE.Jobs.Attachments.Dtos
{
    public class CreateOrEditAttachmentFileDto : EntityDto<int?>
    {

        [Required]
        [StringLength(AttachmentFileConsts.MaxPhysicalNameLength, MinimumLength = AttachmentFileConsts.MinPhysicalNameLength)]
        public string PhysicalName { get; set; }

        [StringLength(AttachmentFileConsts.MaxDescriptionLength, MinimumLength = AttachmentFileConsts.MinDescriptionLength)]
        public string Description { get; set; }

        [Required]
        [StringLength(AttachmentFileConsts.MaxOriginalNameLength, MinimumLength = AttachmentFileConsts.MinOriginalNameLength)]
        public string OriginalName { get; set; }

        public long Size { get; set; }

        [Required]
        public string ObjectId { get; set; }

        public string Path { get; set; }

        public int Version { get; set; }

        [Required]
        public string FileToken { get; set; }

        public string Tag { get; set; }

        [StringLength(AttachmentFileConsts.MaxFilterKeyLength, MinimumLength = AttachmentFileConsts.MinFilterKeyLength)]
        public string FilterKey { get; set; }

        public int AttachmentTypeId { get; set; }

    }
}