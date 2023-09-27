using System;
using Abp.Application.Services.Dto;

namespace MFAE.Jobs.Location.Dtos
{
    public class GovernorateDto : EntityDto
    {
        public string NameAr { get; set; }

        public string NameEn { get; set; }

        public string UniversalCode { get; set; }

        public int CountryId { get; set; }

    }
}