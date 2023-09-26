using Abp.Application.Services.Dto;
using System;

namespace MFAE.Jobs.XRoad.Dtos
{
    public class GetAllXRoadServicesInput : PagedAndSortedResultRequestDto
    {
        public string Filter { get; set; }

        public string NameFilter { get; set; }

        public string ProviderCodeFilter { get; set; }

        public string ResultCodePathFilter { get; set; }

        public string ActionNameFilter { get; set; }

        public string SoapActionNameFilter { get; set; }

        public string VersionNoFilter { get; set; }

        public string ProducerCodeFilter { get; set; }

        public string DescriptionFilter { get; set; }

        public int? StatusFilter { get; set; }

        public string CodeFilter { get; set; }

    }
}