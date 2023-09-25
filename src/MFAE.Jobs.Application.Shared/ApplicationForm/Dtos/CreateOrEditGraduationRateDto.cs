using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace MFAE.Jobs.ApplicationForm.Dtos
{
    public class CreateOrEditGraduationRateDto : EntityDto<int?>
    {

        [Required]
        [StringLength(GraduationRateConsts.MaxNameArLength, MinimumLength = GraduationRateConsts.MinNameArLength)]
        public string NameAr { get; set; }

        [Required]
        [StringLength(GraduationRateConsts.MaxNameEnLength, MinimumLength = GraduationRateConsts.MinNameEnLength)]
        public string NameEn { get; set; }

        public bool IsActive { get; set; }

    }
}