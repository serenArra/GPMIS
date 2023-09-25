using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace MFAE.Jobs.Attachments.Dtos
{
    public class GetAttachmentEntityTypeForEditOutput
    {
        public CreateOrEditAttachmentEntityTypeDto AttachmentEntityType { get; set; }

    }
}