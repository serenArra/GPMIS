using Abp.Application.Services.Dto;
using System;

namespace MFAE.Jobs.ApplicationForm.Dtos
{
    public class GetAllAcademicDegreesInput : PagedAndSortedResultRequestDto
    {
        public string Filter { get; set; }

        public string NameArFilter { get; set; }

        public string NameEnFilter { get; set; }

        public int? IsActiveFilter { get; set; }

    }
}