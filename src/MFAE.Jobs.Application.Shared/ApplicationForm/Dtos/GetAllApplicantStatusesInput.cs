using Abp.Application.Services.Dto;
using System;

namespace MFAE.Jobs.ApplicationForm.Dtos
{
    public class GetAllApplicantStatusesInput : PagedAndSortedResultRequestDto
    {
        public string Filter { get; set; }

        public int? StatusFilter { get; set; }

        public string DescriptionFilter { get; set; }

        public string ApplicantFullNameFilter { get; set; }

    }
}