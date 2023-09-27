using Abp.Application.Services.Dto;
using System;

namespace MFAE.Jobs.XRoad.Dtos
{
    public class GetAllXRoadServiceAttributeMappingsInput : PagedAndSortedResultRequestDto
    {
        public string Filter { get; set; }

        public int? ServiceAttributeTypeFilter { get; set; }

        public string SourceValueFilter { get; set; }

        public string DestinationValueFilter { get; set; }

        public string XRoadServiceAttributeNameFilter { get; set; }

        public int? AttributeId { get; set; }

    }
}