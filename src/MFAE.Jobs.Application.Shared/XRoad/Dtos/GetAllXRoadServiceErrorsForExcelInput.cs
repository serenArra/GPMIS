using Abp.Application.Services.Dto;
using System;

namespace MFAE.Jobs.XRoad.Dtos
{
    public class GetAllXRoadServiceErrorsForExcelInput
    {
        public string Filter { get; set; }

        public string ErrorCodeFilter { get; set; }

        public string ErrorMessageArFilter { get; set; }

        public string ErrorMessageEnFilter { get; set; }

        public string XRoadServiceNameFilter { get; set; }

    }
}