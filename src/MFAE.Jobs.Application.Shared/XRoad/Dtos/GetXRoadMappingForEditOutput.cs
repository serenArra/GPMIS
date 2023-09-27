using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace MFAE.Jobs.XRoad.Dtos
{
    public class GetXRoadMappingForEditOutput
    {
        public CreateOrEditXRoadMappingDto XRoadMapping { get; set; }

    }
}