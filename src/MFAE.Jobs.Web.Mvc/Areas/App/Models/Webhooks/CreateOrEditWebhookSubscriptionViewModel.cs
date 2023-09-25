using Abp.Application.Services.Dto;
using Abp.Webhooks;
using MFAE.Jobs.WebHooks.Dto;

namespace MFAE.Jobs.Web.Areas.App.Models.Webhooks
{
    public class CreateOrEditWebhookSubscriptionViewModel
    {
        public WebhookSubscription WebhookSubscription { get; set; }

        public ListResultDto<GetAllAvailableWebhooksOutput> AvailableWebhookEvents { get; set; }
    }
}
