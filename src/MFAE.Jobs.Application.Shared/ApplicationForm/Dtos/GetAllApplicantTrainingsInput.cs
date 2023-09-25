using Abp.Application.Services.Dto;
using System;

namespace MFAE.Jobs.ApplicationForm.Dtos
{
    public class GetAllApplicantTrainingsInput : PagedAndSortedResultRequestDto
    {
        public string Filter { get; set; }

        public string SubjectFilter { get; set; }

        public string LocationFilter { get; set; }

        public DateTime? MaxTrainingDateFilter { get; set; }
        public DateTime? MinTrainingDateFilter { get; set; }

        public int? MaxDurationFilter { get; set; }
        public int? MinDurationFilter { get; set; }

        public int? DurationTypeFilter { get; set; }

        public string ApplicantFirstNameFilter { get; set; }

    }
}