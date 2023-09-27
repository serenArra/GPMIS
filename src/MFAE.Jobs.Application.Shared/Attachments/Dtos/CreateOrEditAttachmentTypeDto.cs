using MFAE.Jobs.Attachments;

using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace MFAE.Jobs.Attachments.Dtos
{
    public class CreateOrEditAttachmentTypeDto : EntityDto<int?>
    {

        [Required]
        [StringLength(AttachmentTypeConsts.MaxArNameLength, MinimumLength = AttachmentTypeConsts.MinArNameLength)]
        public string ArName { get; set; }

        [Required]
        [StringLength(AttachmentTypeConsts.MaxEnNameLength, MinimumLength = AttachmentTypeConsts.MinEnNameLength)]
        public string EnName { get; set; }

        [Range(AttachmentTypeConsts.MinMaxSizeMBValue, AttachmentTypeConsts.MaxMaxSizeMBValue)]
        public int MaxSizeMB { get; set; }

        public string AllowedExtensions { get; set; }

        [Range(AttachmentTypeConsts.MinMaxAttachmentsValue, AttachmentTypeConsts.MaxMaxAttachmentsValue)]
        public int MaxAttachments { get; set; }

        [Range(AttachmentTypeConsts.MinMinRequiredAttachmentsValue, AttachmentTypeConsts.MaxMinRequiredAttachmentsValue)]
        public int MinRequiredAttachments { get; set; }

        public AttachmentTypeCategories Category { get; set; }

        public PrivacyFlag PrivacyFlag { get; set; }

        public int? EntityTypeId { get; set; }

        public int? GroupId { get; set; }

    }
}