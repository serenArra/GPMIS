using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace MFAE.Jobs.Attachments.Dtos
{
    public class GetAttachmentTypeGroupForEditOutput
    {
        public CreateOrEditAttachmentTypeGroupDto AttachmentTypeGroup { get; set; }

    }
}