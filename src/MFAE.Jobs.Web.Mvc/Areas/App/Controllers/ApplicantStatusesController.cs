using System;
using System.Threading.Tasks;
using Abp.AspNetCore.Mvc.Authorization;
using Microsoft.AspNetCore.Mvc;
using MFAE.Jobs.Web.Areas.App.Models.ApplicantStatuses;
using MFAE.Jobs.Web.Controllers;
using MFAE.Jobs.Authorization;
using MFAE.Jobs.ApplicationForm;
using MFAE.Jobs.ApplicationForm.Dtos;
using Abp.Application.Services.Dto;
using Abp.Extensions;

namespace MFAE.Jobs.Web.Areas.App.Controllers
{
    [Area("App")]
    [AbpMvcAuthorize(AppPermissions.Pages_ApplicantStatuses)]
    public class ApplicantStatusesController : JobsControllerBase
    {
        private readonly IApplicantStatusesAppService _applicantStatusesAppService;

        public ApplicantStatusesController(IApplicantStatusesAppService applicantStatusesAppService)
        {
            _applicantStatusesAppService = applicantStatusesAppService;

        }

        public ActionResult Index()
        {
            var model = new ApplicantStatusesViewModel
            {
                FilterText = ""
            };

            return View(model);
        }

        [AbpMvcAuthorize(AppPermissions.Pages_ApplicantStatuses_Create, AppPermissions.Pages_ApplicantStatuses_Edit)]
        public async Task<PartialViewResult> CreateOrEditModal(long? id)
        {
            GetApplicantStatusForEditOutput getApplicantStatusForEditOutput;

            if (id.HasValue)
            {
                getApplicantStatusForEditOutput = await _applicantStatusesAppService.GetApplicantStatusForEdit(new EntityDto<long> { Id = (long)id });
            }
            else
            {
                getApplicantStatusForEditOutput = new GetApplicantStatusForEditOutput
                {
                    ApplicantStatus = new CreateOrEditApplicantStatusDto()
                };
            }

            var viewModel = new CreateOrEditApplicantStatusModalViewModel()
            {
                ApplicantStatus = getApplicantStatusForEditOutput.ApplicantStatus,
                ApplicantFullName = getApplicantStatusForEditOutput.ApplicantFullName,
                ApplicantStatusApplicantList = await _applicantStatusesAppService.GetAllApplicantForTableDropdown(),

            };

            return PartialView("_CreateOrEditModal", viewModel);
        }

        public async Task<PartialViewResult> ViewApplicantStatusModal(long id)
        {
            var getApplicantStatusForViewDto = await _applicantStatusesAppService.GetApplicantStatusForView(id);

            var model = new ApplicantStatusViewModel()
            {
                ApplicantStatus = getApplicantStatusForViewDto.ApplicantStatus
                ,
                ApplicantFullName = getApplicantStatusForViewDto.ApplicantFullName

            };

            return PartialView("_ViewApplicantStatusModal", model);
        }

    }
}