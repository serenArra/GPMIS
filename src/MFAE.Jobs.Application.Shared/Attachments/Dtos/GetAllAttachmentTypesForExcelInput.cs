using Abp.Application.Services.Dto;
using System;

namespace MFAE.Jobs.Attachments.Dtos
{
    public class GetAllAttachmentTypesForExcelInput
    {
        public string Filter { get; set; }

        public string ArNameFilter { get; set; }

        public string EnNameFilter { get; set; }

        public int? MaxMaxSizeMBFilter { get; set; }
        public int? MinMaxSizeMBFilter { get; set; }

        public string AllowedExtensionsFilter { get; set; }

        public int? MaxMaxAttachmentsFilter { get; set; }
        public int? MinMaxAttachmentsFilter { get; set; }

        public int? MaxMinRequiredAttachmentsFilter { get; set; }
        public int? MinMinRequiredAttachmentsFilter { get; set; }

        public int? CategoryFilter { get; set; }

        public int? PrivacyFlagFilter { get; set; }

        public string AttachmentEntityTypeNameFilter { get; set; }

        public string AttachmentTypeGroupNameFilter { get; set; }

    }
}