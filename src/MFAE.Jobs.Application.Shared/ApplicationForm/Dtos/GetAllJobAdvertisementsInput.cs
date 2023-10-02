using Abp.Application.Services.Dto;
using System;

namespace MFAE.Jobs.ApplicationForm.Dtos
{
    public class GetAllJobAdvertisementsInput : PagedAndSortedResultRequestDto
    {
        public string Filter { get; set; }

        public string DescriptionFilter { get; set; }

    }
}