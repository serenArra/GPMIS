using System.Collections.Generic;
using MFAE.Jobs.Editions;
using MFAE.Jobs.Editions.Dto;
using MFAE.Jobs.MultiTenancy.Payments;
using MFAE.Jobs.MultiTenancy.Payments.Dto;

namespace MFAE.Jobs.Web.Models.Payment
{
    public class BuyEditionViewModel
    {
        public SubscriptionStartType? SubscriptionStartType { get; set; }

        public EditionSelectDto Edition { get; set; }

        public decimal? AdditionalPrice { get; set; }

        public EditionPaymentType EditionPaymentType { get; set; }

        public List<PaymentGatewayModel> PaymentGateways { get; set; }
    }
}
