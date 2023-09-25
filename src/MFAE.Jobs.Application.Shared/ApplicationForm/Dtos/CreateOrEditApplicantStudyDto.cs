using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace MFAE.Jobs.ApplicationForm.Dtos
{
    public class CreateOrEditApplicantStudyDto : EntityDto<long?>
    {

        [StringLength(ApplicantStudyConsts.MaxOtherSpecialtyLength, MinimumLength = ApplicantStudyConsts.MinOtherSpecialtyLength)]
        public string OtherSpecialty { get; set; }

        [StringLength(ApplicantStudyConsts.MaxSecondSpecialtyLength, MinimumLength = ApplicantStudyConsts.MinSecondSpecialtyLength)]
        public string SecondSpecialty { get; set; }

        [Required]
        [StringLength(ApplicantStudyConsts.MaxUniversityLength, MinimumLength = ApplicantStudyConsts.MinUniversityLength)]
        public string University { get; set; }

        public int GraduationYear { get; set; }

        [Required]
        [StringLength(ApplicantStudyConsts.MaxGraduationCountryLength, MinimumLength = ApplicantStudyConsts.MinGraduationCountryLength)]
        public string GraduationCountry { get; set; }

        public int GraduationRateId { get; set; }

        public int AcademicDegreeId { get; set; }

        public int SpecialtiesId { get; set; }

        public long ApplicantId { get; set; }

    }
}