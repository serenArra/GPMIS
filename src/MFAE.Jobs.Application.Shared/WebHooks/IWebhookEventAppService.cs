using System.Threading.Tasks;
using Abp.Webhooks;

namespace MFAE.Jobs.WebHooks
{
    public interface IWebhookEventAppService
    {
        Task<WebhookEvent> Get(string id);
    }
}
