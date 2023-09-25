using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace MFAE.Jobs.Attachments.Dtos
{
    public class CreateOrEditAttachmentEntityTypeDto : EntityDto<int?>
    {

        [Required]
        [StringLength(AttachmentEntityTypeConsts.MaxArNameLength, MinimumLength = AttachmentEntityTypeConsts.MinArNameLength)]
        public string ArName { get; set; }

        [Required]
        [StringLength(AttachmentEntityTypeConsts.MaxEnNameLength, MinimumLength = AttachmentEntityTypeConsts.MinEnNameLength)]
        public string EnName { get; set; }

        [Required]
        [StringLength(AttachmentEntityTypeConsts.MaxFolderLength, MinimumLength = AttachmentEntityTypeConsts.MinFolderLength)]
        public string Folder { get; set; }

        public int? ParentTypeId { get; set; }

    }
}