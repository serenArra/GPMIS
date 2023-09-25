using System.Collections.Generic;
using MFAE.Jobs.Editions.Dto;
using MFAE.Jobs.MultiTenancy.Payments;

namespace MFAE.Jobs.Web.Models.Payment
{
    public class ExtendEditionViewModel
    {
        public EditionSelectDto Edition { get; set; }

        public List<PaymentGatewayModel> PaymentGateways { get; set; }
    }
}