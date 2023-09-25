using System.Threading.Tasks;
using MFAE.Jobs.Authorization.Users;

namespace MFAE.Jobs.WebHooks
{
    public interface IAppWebhookPublisher
    {
        Task PublishTestWebhook();
    }
}
