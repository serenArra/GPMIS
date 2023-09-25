using MFAE.Jobs.Editions.Dto;

namespace MFAE.Jobs.MultiTenancy.Payments.Dto
{
    public class PaymentInfoDto
    {
        public EditionSelectDto Edition { get; set; }

        public decimal AdditionalPrice { get; set; }

        public bool IsLessThanMinimumUpgradePaymentAmount()
        {
            return AdditionalPrice < JobsConsts.MinimumUpgradePaymentAmount;
        }
    }
}
