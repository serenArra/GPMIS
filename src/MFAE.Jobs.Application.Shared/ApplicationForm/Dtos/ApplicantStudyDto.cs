using System;
using Abp.Application.Services.Dto;

namespace MFAE.Jobs.ApplicationForm.Dtos
{
    public class ApplicantStudyDto : EntityDto<long>
    {
        public string OtherSpecialty { get; set; }

        public string SecondSpecialty { get; set; }

        public string University { get; set; }

        public int GraduationYear { get; set; }

        public string GraduationCountry { get; set; }

        public int GraduationRateId { get; set; }

        public int AcademicDegreeId { get; set; }

        public int SpecialtiesId { get; set; }

        public long ApplicantId { get; set; }

    }
}