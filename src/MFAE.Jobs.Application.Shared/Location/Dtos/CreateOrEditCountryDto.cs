using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace MFAE.Jobs.Location.Dtos
{
    public class CreateOrEditCountryDto : EntityDto<int?>
    {

        [Required]
        [StringLength(CountryConsts.MaxIsoNumericLength, MinimumLength = CountryConsts.MinIsoNumericLength)]
        public string IsoNumeric { get; set; }

        [Required]
        [StringLength(CountryConsts.MaxIsoAlphaLength, MinimumLength = CountryConsts.MinIsoAlphaLength)]
        public string IsoAlpha { get; set; }

        [Required]
        [StringLength(CountryConsts.MaxNameArLength, MinimumLength = CountryConsts.MinNameArLength)]
        public string NameAr { get; set; }

        [Required]
        [StringLength(CountryConsts.MaxNameEnLength, MinimumLength = CountryConsts.MinNameEnLength)]
        public string NameEn { get; set; }

        [StringLength(CountryConsts.MaxUniversalCodeLength, MinimumLength = CountryConsts.MinUniversalCodeLength)]
        public string UniversalCode { get; set; }

    }
}