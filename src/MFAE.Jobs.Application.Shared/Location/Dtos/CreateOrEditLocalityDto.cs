using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace MFAE.Jobs.Location.Dtos
{
    public class CreateOrEditLocalityDto : EntityDto<int?>
    {

        [Required]
        [StringLength(LocalityConsts.MaxNameArLength, MinimumLength = LocalityConsts.MinNameArLength)]
        public string NameAr { get; set; }

        [Required]
        [StringLength(LocalityConsts.MaxNameEnLength, MinimumLength = LocalityConsts.MinNameEnLength)]
        public string NameEn { get; set; }

        [StringLength(LocalityConsts.MaxUniversalCodeLength, MinimumLength = LocalityConsts.MinUniversalCodeLength)]
        public string UniversalCode { get; set; }

        public int GovernorateId { get; set; }

    }
}