using MFAE.Jobs.Attachments.Dtos;
using System.Collections.Generic;

using Abp.Extensions;

namespace MFAE.Jobs.Web.Areas.App.Models.AttachmentFiles
{
    public class CreateOrEditAttachmentFileModalViewModel
    {
        public CreateOrEditAttachmentFileDto AttachmentFile { get; set; }

        public string AttachmentTypeArName { get; set; }

        public List<AttachmentFileAttachmentTypeLookupTableDto> AttachmentFileAttachmentTypeList { get; set; }

        public bool IsEditMode => AttachmentFile.Id.HasValue;
    }
}