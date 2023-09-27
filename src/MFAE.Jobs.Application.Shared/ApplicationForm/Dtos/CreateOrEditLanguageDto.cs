using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace MFAE.Jobs.ApplicationForm.Dtos
{
    public class CreateOrEditAppLanguageDto : EntityDto<int?>
    {

        [Required]
        [StringLength(LanguageConsts.MaxNameArLength, MinimumLength = LanguageConsts.MinNameArLength)]
        public string NameAr { get; set; }

        [Required]
        [StringLength(LanguageConsts.MaxNameEnLength, MinimumLength = LanguageConsts.MinNameEnLength)]
        public string NameEn { get; set; }

        public bool IsActive { get; set; }

    }
}