using MFAE.Jobs.ApplicationForm.Dtos;

using Abp.Extensions;

namespace MFAE.Jobs.Web.Areas.App.Models.ConversationRates
{
    public class CreateOrEditConversationRateModalViewModel
    {
        public CreateOrEditConversationRateDto ConversationRate { get; set; }

        public bool IsEditMode => ConversationRate.Id.HasValue;
    }
}