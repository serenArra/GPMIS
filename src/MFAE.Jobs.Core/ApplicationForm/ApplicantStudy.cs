using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities.Auditing;
using Abp.Auditing;

namespace MFAE.Jobs.ApplicationForm
{
    [Table("ApplicantStudies")]
    [Audited]
    public class ApplicantStudy : FullAuditedEntity<long>
    {

        [StringLength(ApplicantStudyConsts.MaxOtherSpecialtyLength, MinimumLength = ApplicantStudyConsts.MinOtherSpecialtyLength)]
        public virtual string OtherSpecialty { get; set; }

        [StringLength(ApplicantStudyConsts.MaxSecondSpecialtyLength, MinimumLength = ApplicantStudyConsts.MinSecondSpecialtyLength)]
        public virtual string SecondSpecialty { get; set; }

        [Required]
        [StringLength(ApplicantStudyConsts.MaxUniversityLength, MinimumLength = ApplicantStudyConsts.MinUniversityLength)]
        public virtual string University { get; set; }

        public virtual int GraduationYear { get; set; }

        [Required]
        [StringLength(ApplicantStudyConsts.MaxGraduationCountryLength, MinimumLength = ApplicantStudyConsts.MinGraduationCountryLength)]
        public virtual string GraduationCountry { get; set; }

        public virtual int GraduationRateId { get; set; }

        [ForeignKey("GraduationRateId")]
        public GraduationRate GraduationRateFk { get; set; }

        public virtual int AcademicDegreeId { get; set; }

        [ForeignKey("AcademicDegreeId")]
        public AcademicDegree AcademicDegreeFk { get; set; }

        public virtual int SpecialtiesId { get; set; }

        [ForeignKey("SpecialtiesId")]
        public Specialties SpecialtiesFk { get; set; }

        public virtual long ApplicantId { get; set; }

        [ForeignKey("ApplicantId")]
        public Applicant ApplicantFk { get; set; }

    }
}