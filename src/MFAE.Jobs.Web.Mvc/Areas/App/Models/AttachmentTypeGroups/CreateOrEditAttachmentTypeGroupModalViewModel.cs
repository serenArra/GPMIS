using MFAE.Jobs.Attachments.Dtos;

using Abp.Extensions;

namespace MFAE.Jobs.Web.Areas.App.Models.AttachmentTypeGroups
{
    public class CreateOrEditAttachmentTypeGroupModalViewModel
    {
        public CreateOrEditAttachmentTypeGroupDto AttachmentTypeGroup { get; set; }

        public bool IsEditMode => AttachmentTypeGroup.Id.HasValue;
    }
}