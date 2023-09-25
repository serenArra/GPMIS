using System.Threading.Tasks;
using Abp.Application.Services;
using MFAE.Jobs.MultiTenancy.Payments.Dto;
using MFAE.Jobs.MultiTenancy.Payments.Stripe.Dto;

namespace MFAE.Jobs.MultiTenancy.Payments.Stripe
{
    public interface IStripePaymentAppService : IApplicationService
    {
        Task ConfirmPayment(StripeConfirmPaymentInput input);

        StripeConfigurationDto GetConfiguration();

        Task<SubscriptionPaymentDto> GetPaymentAsync(StripeGetPaymentInput input);

        Task<string> CreatePaymentSession(StripeCreatePaymentSessionInput input);
    }
}