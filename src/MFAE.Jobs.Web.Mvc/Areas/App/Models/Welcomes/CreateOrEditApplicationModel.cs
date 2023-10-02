using MFAE.Jobs.ApplicationForm.Dtos;
using MFAE.Jobs.Authorization.Users.Dto;
using MFAE.Jobs.Web.Areas.App.Models.ApplicantLanguages;
using MFAE.Jobs.Web.Areas.App.Models.ApplicantStudies;
using MFAE.Jobs.Web.Areas.App.Models.ApplicantTrainings;
using MFAE.Jobs.Web.Areas.App.Models.AttachmentFiles;
using System.Collections.Generic;

namespace MFAE.Jobs.Web.Areas.App.Models.Welcomes
{
    public class CreateOrEditApplicationModel
    {
        public CreateOrEditApplicantDto Applicant { get; set; }

        public string CountryName { get; set; }

        public string GovernorateName { get; set; }

        public string LocalityName { get; set; }

        public CreateOrEditApplicantStudyModalViewModel CreateOrEditApplicantStudyModal { get; set; }

        public CreateOrEditApplicantLanguageModalViewModel CreateOrEditApplicantLanguageModal { get; set; }

        public CreateOrEditApplicantTrainingModalViewModel CreateOrEditApplicantTrainingModel { get; set; }

        public CreateOrEditAttachmentFileModalViewModel CreateOrEditAttachmentFileModel { get; set; }

        public List<ApplicantIdentificationTypeLookupTableDto> ApplicantIdentificationTypeList { get; set; }

        public List<ApplicantMaritalStatusLookupTableDto> ApplicantMaritalStatusList { get; set; }

        public List<ApplicantUserLookupTableDto> ApplicantUserList { get; set; }

        public List<ApplicantApplicantStatusLookupTableDto> ApplicantApplicantStatusList { get; set; }

        public List<ApplicantCountryLookupTableDto> ApplicantCountryList { get; set; }

        public List<ApplicantGovernorateLookupTableDto> ApplicantGovernorateList { get; set; }

        public List<ApplicantLocalityLookupTableDto> ApplicantLocalityList { get; set; }

        public bool IsEditMode => Applicant.Id.HasValue;

    }
}
