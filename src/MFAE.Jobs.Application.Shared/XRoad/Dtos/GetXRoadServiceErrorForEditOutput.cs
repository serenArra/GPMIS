using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace MFAE.Jobs.XRoad.Dtos
{
    public class GetXRoadServiceErrorForEditOutput
    {
        public CreateOrEditXRoadServiceErrorDto XRoadServiceError { get; set; }

        public string XRoadServiceName { get; set; }

    }
}