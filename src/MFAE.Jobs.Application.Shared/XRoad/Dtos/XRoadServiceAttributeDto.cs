using MFAE.Jobs.XRoad;

using System;
using Abp.Application.Services.Dto;

namespace MFAE.Jobs.XRoad.Dtos
{
    public class XRoadServiceAttributeDto : EntityDto
    {
        public XRoadServiceAttributeTypeEnum ServiceAttributeType { get; set; }

        public string AttributeCode { get; set; }

        public string XMLPath { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public string FormatTransition { get; set; }

        public int? XRoadServiceID { get; set; }

    }
}