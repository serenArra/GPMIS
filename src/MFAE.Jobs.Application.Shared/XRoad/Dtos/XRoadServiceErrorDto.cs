using System;
using Abp.Application.Services.Dto;

namespace MFAE.Jobs.XRoad.Dtos
{
    public class XRoadServiceErrorDto : EntityDto
    {
        public string ErrorCode { get; set; }

        public string ErrorMessageAr { get; set; }

        public string ErrorMessageEn { get; set; }

        public int XRoadServiceId { get; set; }

    }
}