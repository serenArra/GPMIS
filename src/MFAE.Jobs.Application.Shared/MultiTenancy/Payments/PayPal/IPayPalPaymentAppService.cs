using System.Threading.Tasks;
using Abp.Application.Services;
using MFAE.Jobs.MultiTenancy.Payments.PayPal.Dto;

namespace MFAE.Jobs.MultiTenancy.Payments.PayPal
{
    public interface IPayPalPaymentAppService : IApplicationService
    {
        Task ConfirmPayment(long paymentId, string paypalOrderId);

        PayPalConfigurationDto GetConfiguration();
    }
}
