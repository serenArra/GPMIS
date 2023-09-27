using Abp.AspNetCore.Mvc.Authorization;
using Microsoft.AspNetCore.Mvc;
using MFAE.Jobs.Web.Controllers;
using MFAE.Jobs.Web.Areas.App.Models.Welcomes;
using System.Threading.Tasks;
using MFAE.Jobs.Authorization.Welcomes;
using MFAE.Jobs.Web.Areas.App.Models.ApplicantLanguages;
using MFAE.Jobs.Web.Areas.App.Models.ApplicantStudies;
using MFAE.Jobs.Web.Areas.App.Models.ApplicantTrainings;
using MFAE.Jobs.Web.Areas.App.Models.AttachmentFiles;
using MFAE.Jobs.ApplicationForm.Dtos;
using System.Collections.Generic;

namespace MFAE.Jobs.Web.Areas.App.Controllers
{
    [Area("App")]
    [AbpMvcAuthorize]
    public class WelcomeController : JobsControllerBase
    {
        private readonly IWelcomeAppService _welcomeAppService;
         
        public WelcomeController(IWelcomeAppService welcomeAppService)
        {
            _welcomeAppService = welcomeAppService;
        }

        public async Task<ActionResult> Index()
        {
            var getWelcomeUserForView = await _welcomeAppService.GetWelcomeUserForView();
            var model = new WelcomesViewModel()
            {
                User = getWelcomeUserForView.User,
                JobAdvertisement = getWelcomeUserForView.JobAdvertisement
            };

            return View(model);
        }

        public async Task<ActionResult> ApplicationForms()
        {
            var model = new CreateOrEditApplicationModel()
            {
               Applicant = new CreateOrEditApplicantDto(),
               CreateOrEditApplicantLanguageModal = new CreateOrEditApplicantLanguageModalViewModel(),
               CreateOrEditApplicantStudyModal = new CreateOrEditApplicantStudyModalViewModel(),
               CreateOrEditApplicantTrainingModel = new CreateOrEditApplicantTrainingModalViewModel(),
               CreateOrEditAttachmentFileModel = new CreateOrEditAttachmentFileModalViewModel(),
               ApplicantIdentificationTypeList = new List<ApplicantIdentificationTypeLookupTableDto>(),
               ApplicantMaritalStatusList = new List<ApplicantMaritalStatusLookupTableDto>(),
               ApplicantUserList = new List<ApplicantUserLookupTableDto>(),
               ApplicantApplicantStatusList = new List<ApplicantApplicantStatusLookupTableDto>(),
               ApplicantCountryList = new List<ApplicantCountryLookupTableDto>(),
               ApplicantGovernorateList = new List<ApplicantGovernorateLookupTableDto>(),
               ApplicantLocalityList = new List<ApplicantLocalityLookupTableDto>(),
            };
            return View(model);
        }


    }
}