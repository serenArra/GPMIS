using System.Threading.Tasks;
using Abp.Application.Services;

namespace MFAE.Jobs.MultiTenancy
{
    public interface ISubscriptionAppService : IApplicationService
    {
        Task DisableRecurringPayments();

        Task EnableRecurringPayments();
    }
}
