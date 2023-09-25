using System;
using System.Threading.Tasks;
using Abp.AspNetCore.Mvc.Authorization;
using Microsoft.AspNetCore.Mvc;
using MFAE.Jobs.Web.Areas.App.Models.ApplicantLanguages;
using MFAE.Jobs.Web.Controllers;
using MFAE.Jobs.Authorization;
using MFAE.Jobs.ApplicationForm;
using MFAE.Jobs.ApplicationForm.Dtos;
using Abp.Application.Services.Dto;
using Abp.Extensions;

namespace MFAE.Jobs.Web.Areas.App.Controllers
{
    [Area("App")]
    [AbpMvcAuthorize(AppPermissions.Pages_ApplicantLanguages)]
    public class ApplicantLanguagesController : JobsControllerBase
    {
        private readonly IApplicantLanguagesAppService _applicantLanguagesAppService;

        public ApplicantLanguagesController(IApplicantLanguagesAppService applicantLanguagesAppService)
        {
            _applicantLanguagesAppService = applicantLanguagesAppService;

        }

        public ActionResult Index()
        {
            var model = new ApplicantLanguagesViewModel
            {
                FilterText = ""
            };

            return View(model);
        }

        [AbpMvcAuthorize(AppPermissions.Pages_ApplicantLanguages_Create, AppPermissions.Pages_ApplicantLanguages_Edit)]
        public async Task<PartialViewResult> CreateOrEditModal(long? id)
        {
            GetApplicantLanguageForEditOutput getApplicantLanguageForEditOutput;

            if (id.HasValue)
            {
                getApplicantLanguageForEditOutput = await _applicantLanguagesAppService.GetApplicantLanguageForEdit(new EntityDto<long> { Id = (long)id });
            }
            else
            {
                getApplicantLanguageForEditOutput = new GetApplicantLanguageForEditOutput
                {
                    ApplicantLanguage = new CreateOrEditApplicantLanguageDto()
                };
            }

            var viewModel = new CreateOrEditApplicantLanguageModalViewModel()
            {
                ApplicantLanguage = getApplicantLanguageForEditOutput.ApplicantLanguage,
                ApplicantFirstName = getApplicantLanguageForEditOutput.ApplicantFirstName,
                LanguageName = getApplicantLanguageForEditOutput.LanguageName,
                ConversationName = getApplicantLanguageForEditOutput.ConversationName,
                ConversationRateName = getApplicantLanguageForEditOutput.ConversationRateName,
                ApplicantLanguageApplicantList = await _applicantLanguagesAppService.GetAllApplicantForTableDropdown(),
                ApplicantLanguageLanguageList = await _applicantLanguagesAppService.GetAllLanguageForTableDropdown(),
                ApplicantLanguageConversationList = await _applicantLanguagesAppService.GetAllConversationForTableDropdown(),
                ApplicantLanguageConversationRateList = await _applicantLanguagesAppService.GetAllConversationRateForTableDropdown(),

            };

            return PartialView("_CreateOrEditModal", viewModel);
        }

        public async Task<PartialViewResult> ViewApplicantLanguageModal(long id)
        {
            var getApplicantLanguageForViewDto = await _applicantLanguagesAppService.GetApplicantLanguageForView(id);

            var model = new ApplicantLanguageViewModel()
            {
                ApplicantLanguage = getApplicantLanguageForViewDto.ApplicantLanguage
                ,
                ApplicantFirstName = getApplicantLanguageForViewDto.ApplicantFirstName

                ,
                LanguageName = getApplicantLanguageForViewDto.LanguageName

                ,
                ConversationName = getApplicantLanguageForViewDto.ConversationName

                ,
                ConversationRateName = getApplicantLanguageForViewDto.ConversationRateName

            };

            return PartialView("_ViewApplicantLanguageModal", model);
        }

    }
}