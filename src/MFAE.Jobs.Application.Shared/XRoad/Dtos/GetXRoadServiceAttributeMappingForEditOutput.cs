using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace MFAE.Jobs.XRoad.Dtos
{
    public class GetXRoadServiceAttributeMappingForEditOutput
    {
        public CreateOrEditXRoadServiceAttributeMappingDto XRoadServiceAttributeMapping { get; set; }

        public string XRoadServiceAttributeName { get; set; }

    }
}