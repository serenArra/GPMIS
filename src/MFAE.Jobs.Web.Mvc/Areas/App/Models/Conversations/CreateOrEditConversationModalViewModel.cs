using MFAE.Jobs.ApplicationForm.Dtos;

using Abp.Extensions;

namespace MFAE.Jobs.Web.Areas.App.Models.Conversations
{
    public class CreateOrEditConversationModalViewModel
    {
        public CreateOrEditConversationDto Conversation { get; set; }

        public bool IsEditMode => Conversation.Id.HasValue;
    }
}