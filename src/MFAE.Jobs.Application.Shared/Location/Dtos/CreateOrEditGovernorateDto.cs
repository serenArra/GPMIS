using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace MFAE.Jobs.Location.Dtos
{
    public class CreateOrEditGovernorateDto : EntityDto<int?>
    {

        [Required]
        [StringLength(GovernorateConsts.MaxNameArLength, MinimumLength = GovernorateConsts.MinNameArLength)]
        public string NameAr { get; set; }

        [StringLength(GovernorateConsts.MaxNameEnLength, MinimumLength = GovernorateConsts.MinNameEnLength)]
        public string NameEn { get; set; }

        [StringLength(GovernorateConsts.MaxUniversalCodeLength, MinimumLength = GovernorateConsts.MinUniversalCodeLength)]
        public string UniversalCode { get; set; }

        public int CountryId { get; set; }

    }
}