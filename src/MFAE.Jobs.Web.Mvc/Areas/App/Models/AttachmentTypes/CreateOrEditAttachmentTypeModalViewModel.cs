using MFAE.Jobs.Attachments.Dtos;
using System.Collections.Generic;
using System.Collections.Generic;

using Abp.Extensions;

namespace MFAE.Jobs.Web.Areas.App.Models.AttachmentTypes
{
    public class CreateOrEditAttachmentTypeModalViewModel
    {
        public CreateOrEditAttachmentTypeDto AttachmentType { get; set; }

        public string AttachmentEntityTypeName { get; set; }

        public string AttachmentTypeGroupName { get; set; }

        public List<AttachmentTypeAttachmentEntityTypeLookupTableDto> AttachmentTypeAttachmentEntityTypeList { get; set; }

        public List<AttachmentTypeAttachmentTypeGroupLookupTableDto> AttachmentTypeAttachmentTypeGroupList { get; set; }

        public bool IsEditMode => AttachmentType.Id.HasValue;
    }
}