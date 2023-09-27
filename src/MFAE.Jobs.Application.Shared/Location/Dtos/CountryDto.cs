using System;
using Abp.Application.Services.Dto;

namespace MFAE.Jobs.Location.Dtos
{
    public class CountryDto : EntityDto
    {
        public string IsoNumeric { get; set; }

        public string IsoAlpha { get; set; }

        public string NameAr { get; set; }

        public string NameEn { get; set; }

        public string UniversalCode { get; set; }

    }
}