using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace MFAE.Jobs.ApplicationForm.Dtos
{
    public class CreateOrEditAcademicDegreeDto : EntityDto<int?>
    {

        [Required]
        [StringLength(AcademicDegreeConsts.MaxNameArLength, MinimumLength = AcademicDegreeConsts.MinNameArLength)]
        public string NameAr { get; set; }

        [Required]
        [StringLength(AcademicDegreeConsts.MaxNameEnLength, MinimumLength = AcademicDegreeConsts.MinNameEnLength)]
        public string NameEn { get; set; }

        public bool IsActive { get; set; }

    }
}