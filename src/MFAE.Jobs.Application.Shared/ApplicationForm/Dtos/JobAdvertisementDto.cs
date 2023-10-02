using System;
using Abp.Application.Services.Dto;

namespace MFAE.Jobs.ApplicationForm.Dtos
{
    public class JobAdvertisementDto : EntityDto
    {
        public string Description { get; set; }

        public string AdvertisementId { get; set; }

        public DateTime AdvertisementDate { get; set; }

        public DateTime FromDate { get; set; }

        public DateTime ToDate { get; set; }

        public double AllowedAge { get; set; }

        public bool IsActive { get; set; }

    }
}