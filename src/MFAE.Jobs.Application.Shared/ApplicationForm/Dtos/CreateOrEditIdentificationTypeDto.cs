using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace MFAE.Jobs.ApplicationForm.Dtos
{
    public class CreateOrEditIdentificationTypeDto : EntityDto<int?>
    {

        [Required]
        [StringLength(IdentificationTypeConsts.MaxNameArLength, MinimumLength = IdentificationTypeConsts.MinNameArLength)]
        public string NameAr { get; set; }

        [Required]
        [StringLength(IdentificationTypeConsts.MaxNameEnLength, MinimumLength = IdentificationTypeConsts.MinNameEnLength)]
        public string NameEn { get; set; }

        public bool IsActive { get; set; }

        public bool IsDefault { get; set; }

    }
}