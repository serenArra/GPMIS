using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace MFAE.Jobs.ApplicationForm.Dtos
{
    public class CreateOrEditSpecialtiesDto : EntityDto<int?>
    {

        [Required]
        [StringLength(SpecialtiesConsts.MaxNameArLength, MinimumLength = SpecialtiesConsts.MinNameArLength)]
        public string NameAr { get; set; }

        [Required]
        [StringLength(SpecialtiesConsts.MaxNameEnLength, MinimumLength = SpecialtiesConsts.MinNameEnLength)]
        public string NameEn { get; set; }

        public bool IsActive { get; set; }

    }
}