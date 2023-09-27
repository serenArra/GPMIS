using MFAE.Jobs.XRoad;

using System;
using Abp.Application.Services.Dto;

namespace MFAE.Jobs.XRoad.Dtos
{
    public class XRoadMappingDto : EntityDto
    {
        public XRoadLookupEnum Lookup { get; set; }

        public XRoadServicesEnum ServiceName { get; set; }

        public string Code { get; set; }

        public long SystemId { get; set; }

    }
}