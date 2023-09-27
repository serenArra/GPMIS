using System;
using System.Collections.Generic;
using System.Text;

namespace MFAE.Jobs.XRoad.Dtos
{
    public class InformationBankViewDto
    {
        public CitizensListWithCodesDto CitizensListWithCodes { get; set; }
        public PassportInfoDto PassportInfo { get; set; }       

        public bool IsCitizensListWithCodesEnabled { get; set; }
        public bool IsCitizenPictureEnabled { get; set; }
        public bool IsPassportInfoEnabled { get; set; }
    }
}
