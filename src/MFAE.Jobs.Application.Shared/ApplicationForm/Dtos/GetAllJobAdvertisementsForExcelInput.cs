using Abp.Application.Services.Dto;
using System;

namespace MFAE.Jobs.ApplicationForm.Dtos
{
    public class GetAllJobAdvertisementsForExcelInput
    {
        public string Filter { get; set; }

        public string DescriptionFilter { get; set; }

        public string AdvertisementIdFilter { get; set; }

        public DateTime? MaxAdvertisementDateFilter { get; set; }
        public DateTime? MinAdvertisementDateFilter { get; set; }

        public DateTime? MaxFromDateFilter { get; set; }
        public DateTime? MinFromDateFilter { get; set; }

        public DateTime? MaxToDateFilter { get; set; }
        public DateTime? MinToDateFilter { get; set; }

        public double? MaxAllowedAgeFilter { get; set; }
        public double? MinAllowedAgeFilter { get; set; }

        public int? IsActiveFilter { get; set; }

    }
}