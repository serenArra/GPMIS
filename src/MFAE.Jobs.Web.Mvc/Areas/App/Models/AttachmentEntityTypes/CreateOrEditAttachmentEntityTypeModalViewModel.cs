using MFAE.Jobs.Attachments.Dtos;

using Abp.Extensions;

namespace MFAE.Jobs.Web.Areas.App.Models.AttachmentEntityTypes
{
    public class CreateOrEditAttachmentEntityTypeModalViewModel
    {
        public CreateOrEditAttachmentEntityTypeDto AttachmentEntityType { get; set; }

        public bool IsEditMode => AttachmentEntityType.Id.HasValue;
    }
}