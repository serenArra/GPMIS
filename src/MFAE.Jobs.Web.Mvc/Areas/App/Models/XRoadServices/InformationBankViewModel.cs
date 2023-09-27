using MFAE.Jobs.XRoad.Dtos;

namespace MFAE.Jobs.Web.Areas.App.Models.XRoadServices
{
    public class InformationBankViewModel : InformationBankViewDto
    {
        public int IdentificationTypeId { set; get; }
        public string IdentificationDocumentNo { set; get; }
    }
}
