using MFAE.Jobs.ApplicationForm.Dtos;
using System.Collections.Generic;

namespace MFAE.Jobs.Web.Areas.App.Models.XRoadServices
{
    public class ViewInformationBankPageViewModel
    {
        public string IdentificationDocumentNo { get; set; }
        public List<IdentificationTypeLookupTableDto> IdentificationTypeList { get; set; }
    }
}
