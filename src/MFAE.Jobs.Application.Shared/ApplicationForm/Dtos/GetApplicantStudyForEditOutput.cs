using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace MFAE.Jobs.ApplicationForm.Dtos
{
    public class GetApplicantStudyForEditOutput
    {
        public CreateOrEditApplicantStudyDto ApplicantStudy { get; set; }

        public string GraduationRateName { get; set; }

        public string AcademicDegreeName { get; set; }

        public string SpecialtiesName { get; set; }

        public string ApplicantFirstName { get; set; }

    }
}