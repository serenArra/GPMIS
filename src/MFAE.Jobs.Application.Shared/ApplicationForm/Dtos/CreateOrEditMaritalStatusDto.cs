using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace MFAE.Jobs.ApplicationForm.Dtos
{
    public class CreateOrEditMaritalStatusDto : EntityDto<int?>
    {

        [Required]
        [StringLength(MaritalStatusConsts.MaxNameArLength, MinimumLength = MaritalStatusConsts.MinNameArLength)]
        public string NameAr { get; set; }

        [Required]
        [StringLength(MaritalStatusConsts.MaxNameEnLength, MinimumLength = MaritalStatusConsts.MinNameEnLength)]
        public string NameEn { get; set; }

        public string IsActive { get; set; }

    }
}