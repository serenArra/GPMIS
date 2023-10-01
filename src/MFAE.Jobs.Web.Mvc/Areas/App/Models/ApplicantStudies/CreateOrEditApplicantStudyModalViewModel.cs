using MFAE.Jobs.ApplicationForm.Dtos;
using System.Collections.Generic;

namespace MFAE.Jobs.Web.Areas.App.Models.ApplicantStudies
{
    public class CreateOrEditApplicantStudyModalViewModel
    {
        public CreateOrEditApplicantStudyDto ApplicantStudy { get; set; }

        public string GraduationRateName { get; set; }

        public string AcademicDegreeName { get; set; }

        public string SpecialtiesName { get; set; }

        public string ApplicantFirstName { get; set; }

        public List<ApplicantStudyGraduationRateLookupTableDto> ApplicantStudyGraduationRateList { get; set; }

        public List<ApplicantStudyAcademicDegreeLookupTableDto> ApplicantStudyAcademicDegreeList { get; set; }

        public List<ApplicantStudySpecialtiesLookupTableDto> ApplicantStudySpecialtiesList { get; set; }

        public List<ApplicantStudyApplicantLookupTableDto> ApplicantStudyApplicantList { get; set; }

        public bool IsEditMode => ApplicantStudy.Id.HasValue;
    }
}