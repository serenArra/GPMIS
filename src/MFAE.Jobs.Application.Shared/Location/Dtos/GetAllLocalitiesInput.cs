using Abp.Application.Services.Dto;
using System;

namespace MFAE.Jobs.Location.Dtos
{
    public class GetAllLocalitiesInput : PagedAndSortedResultRequestDto
    {
        public string Filter { get; set; }

        public string NameArFilter { get; set; }

        public string NameEnFilter { get; set; }

        public string UniversalCodeFilter { get; set; }

        public string GovernorateNameFilter { get; set; }

    }
}