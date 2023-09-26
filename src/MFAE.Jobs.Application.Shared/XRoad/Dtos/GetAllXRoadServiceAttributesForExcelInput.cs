using Abp.Application.Services.Dto;
using System;

namespace MFAE.Jobs.XRoad.Dtos
{
    public class GetAllXRoadServiceAttributesForExcelInput
    {
        public string Filter { get; set; }

        public int? ServiceAttributeTypeFilter { get; set; }

        public string AttributeCodeFilter { get; set; }

        public string XMLPathFilter { get; set; }

        public string NameFilter { get; set; }

        public string DescriptionFilter { get; set; }

        public string FormatTransitionFilter { get; set; }

        public string XRoadServiceNameFilter { get; set; }

    }
}