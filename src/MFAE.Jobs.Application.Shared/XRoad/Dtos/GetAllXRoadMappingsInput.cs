using Abp.Application.Services.Dto;
using System;

namespace MFAE.Jobs.XRoad.Dtos
{
    public class GetAllXRoadMappingsInput : PagedAndSortedResultRequestDto
    {
        public string Filter { get; set; }

        public int? LookupFilter { get; set; }

        public int? ServiceNameFilter { get; set; }

        public string CodeFilter { get; set; }

        public long? MaxSystemIdFilter { get; set; }
        public long? MinSystemIdFilter { get; set; }

    }
}