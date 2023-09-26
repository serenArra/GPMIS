using MFAE.Jobs.XRoad;

using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace MFAE.Jobs.XRoad.Dtos
{
    public class CreateOrEditXRoadMappingDto : EntityDto<int?>
    {

        public XRoadLookupEnum Lookup { get; set; }

        public XRoadServicesEnum ServiceName { get; set; }

        [Required]
        [StringLength(XRoadMappingConsts.MaxCodeLength, MinimumLength = XRoadMappingConsts.MinCodeLength)]
        public string Code { get; set; }

        public long SystemId { get; set; }

    }
}