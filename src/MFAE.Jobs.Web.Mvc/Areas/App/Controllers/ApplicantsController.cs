using System;
using System.Threading.Tasks;
using Abp.AspNetCore.Mvc.Authorization;
using Microsoft.AspNetCore.Mvc;
using MFAE.Jobs.Web.Areas.App.Models.Applicants;
using MFAE.Jobs.Web.Controllers;
using MFAE.Jobs.Authorization;
using MFAE.Jobs.ApplicationForm;
using MFAE.Jobs.ApplicationForm.Dtos;
using Abp.Application.Services.Dto;
using Abp.Extensions;

namespace MFAE.Jobs.Web.Areas.App.Controllers
{
    [Area("App")]
    [AbpMvcAuthorize(AppPermissions.Pages_Applicants)]
    public class ApplicantsController : JobsControllerBase
    {
        private readonly IApplicantsAppService _applicantsAppService;

        public ApplicantsController(IApplicantsAppService applicantsAppService)
        {
            _applicantsAppService = applicantsAppService;

        }

        public ActionResult Index()
        {
            var model = new ApplicantsViewModel
            {
                FilterText = ""
            };

            return View(model);
        }

        [AbpMvcAuthorize(AppPermissions.Pages_Applicants_Create, AppPermissions.Pages_Applicants_Edit)]
        public async Task<PartialViewResult> CreateOrEditModal(long? id)
        {
            GetApplicantForEditOutput getApplicantForEditOutput;

            if (id.HasValue)
            {
                getApplicantForEditOutput = await _applicantsAppService.GetApplicantForEdit(new EntityDto<long> { Id = (long)id });
            }
            else
            {
                getApplicantForEditOutput = new GetApplicantForEditOutput
                {
                    Applicant = new CreateOrEditApplicantDto()
                };
                getApplicantForEditOutput.Applicant.BirthDate = DateTime.Now;
            }

            var viewModel = new CreateOrEditApplicantModalViewModel()
            {
                Applicant = getApplicantForEditOutput.Applicant,
                IdentificationTypeName = getApplicantForEditOutput.IdentificationTypeName,
                MaritalStatusName = getApplicantForEditOutput.MaritalStatusName,
                UserName = getApplicantForEditOutput.UserName,
                ApplicantStatusDescription = getApplicantForEditOutput.ApplicantStatusDescription,
                ApplicantIdentificationTypeList = await _applicantsAppService.GetAllIdentificationTypeForTableDropdown(),
                ApplicantMaritalStatusList = await _applicantsAppService.GetAllMaritalStatusForTableDropdown(),
                ApplicantUserList = await _applicantsAppService.GetAllUserForTableDropdown(),
                ApplicantApplicantStatusList = await _applicantsAppService.GetAllApplicantStatusForTableDropdown(),

            };

            return PartialView("_CreateOrEditModal", viewModel);
        }

        public async Task<PartialViewResult> ViewApplicantModal(long id)
        {
            var getApplicantForViewDto = await _applicantsAppService.GetApplicantForView(id);

            var model = new ApplicantViewModel()
            {
                Applicant = getApplicantForViewDto.Applicant
                ,
                IdentificationTypeName = getApplicantForViewDto.IdentificationTypeName

                ,
                MaritalStatusName = getApplicantForViewDto.MaritalStatusName

                ,
                UserName = getApplicantForViewDto.UserName

                ,
                ApplicantStatusDescription = getApplicantForViewDto.ApplicantStatusDescription

            };

            return PartialView("_ViewApplicantModal", model);
        }

    }
}