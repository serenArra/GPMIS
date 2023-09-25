using MFAE.Jobs.Editions;
using MFAE.Jobs.Editions.Dto;
using MFAE.Jobs.MultiTenancy.Payments;
using MFAE.Jobs.Security;
using MFAE.Jobs.MultiTenancy.Payments.Dto;

namespace MFAE.Jobs.Web.Models.TenantRegistration
{
    public class TenantRegisterViewModel
    {
        public PasswordComplexitySetting PasswordComplexitySetting { get; set; }

        public int? EditionId { get; set; }

        public SubscriptionStartType? SubscriptionStartType { get; set; }

        public EditionSelectDto Edition { get; set; }

        public EditionPaymentType EditionPaymentType { get; set; }
    }
}
