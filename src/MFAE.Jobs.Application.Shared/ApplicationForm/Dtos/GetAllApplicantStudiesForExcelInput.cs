using Abp.Application.Services.Dto;
using System;

namespace MFAE.Jobs.ApplicationForm.Dtos
{
    public class GetAllApplicantStudiesForExcelInput
    {
        public string Filter { get; set; }

        public string OtherSpecialtyFilter { get; set; }

        public string SecondSpecialtyFilter { get; set; }

        public string UniversityFilter { get; set; }

        public int? MaxGraduationYearFilter { get; set; }
        public int? MinGraduationYearFilter { get; set; }

        public string GraduationCountryFilter { get; set; }

        public string GraduationRateNameFilter { get; set; }

        public string AcademicDegreeNameFilter { get; set; }

        public string SpecialtiesNameFilter { get; set; }

        public string ApplicantFirstNameFilter { get; set; }

    }
}