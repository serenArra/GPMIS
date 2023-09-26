using System;
using Abp.Application.Services.Dto;

namespace MFAE.Jobs.Location.Dtos
{
    public class LocalityDto : EntityDto
    {
        public string NameAr { get; set; }

        public string NameEn { get; set; }

        public string UniversalCode { get; set; }

        public int GovernorateId { get; set; }

    }
}