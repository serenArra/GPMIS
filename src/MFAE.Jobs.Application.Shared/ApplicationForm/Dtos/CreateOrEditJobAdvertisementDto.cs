using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace MFAE.Jobs.ApplicationForm.Dtos
{
    public class CreateOrEditJobAdvertisementDto : EntityDto<int?>
    {

        public string Description { get; set; }

        [Required]
        [StringLength(JobAdvertisementConsts.MaxAdvertisementIdLength, MinimumLength = JobAdvertisementConsts.MinAdvertisementIdLength)]
        public string AdvertisementId { get; set; }

        public DateTime AdvertisementDate { get; set; }

        public DateTime FromDate { get; set; }

        public DateTime ToDate { get; set; }

        public double AllowedAge { get; set; }

        public bool IsActive { get; set; }

    }
}