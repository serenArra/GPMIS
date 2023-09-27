using System;
using System.Threading.Tasks;
using Abp.AspNetCore.Mvc.Authorization;
using Microsoft.AspNetCore.Mvc;
using MFAE.Jobs.Web.Areas.App.Models.MaritalStatuses;
using MFAE.Jobs.Web.Controllers;
using MFAE.Jobs.Authorization;
using MFAE.Jobs.ApplicationForm;
using MFAE.Jobs.ApplicationForm.Dtos;
using Abp.Application.Services.Dto;
using Abp.Extensions;

namespace MFAE.Jobs.Web.Areas.App.Controllers
{
    [Area("App")]
    [AbpMvcAuthorize(AppPermissions.Pages_MaritalStatuses)]
    public class MaritalStatusesController : JobsControllerBase
    {
        private readonly IMaritalStatusesAppService _maritalStatusesAppService;

        public MaritalStatusesController(IMaritalStatusesAppService maritalStatusesAppService)
        {
            _maritalStatusesAppService = maritalStatusesAppService;

        }

        public ActionResult Index()
        {
            var model = new MaritalStatusesViewModel
            {
                FilterText = ""
            };

            return View(model);
        }

        [AbpMvcAuthorize(AppPermissions.Pages_MaritalStatuses_Create, AppPermissions.Pages_MaritalStatuses_Edit)]
        public async Task<PartialViewResult> CreateOrEditModal(int? id)
        {
            GetMaritalStatusForEditOutput getMaritalStatusForEditOutput;

            if (id.HasValue)
            {
                getMaritalStatusForEditOutput = await _maritalStatusesAppService.GetMaritalStatusForEdit(new EntityDto { Id = (int)id });
            }
            else
            {
                getMaritalStatusForEditOutput = new GetMaritalStatusForEditOutput
                {
                    MaritalStatus = new CreateOrEditMaritalStatusDto()
                };
            }

            var viewModel = new CreateOrEditMaritalStatusModalViewModel()
            {
                MaritalStatus = getMaritalStatusForEditOutput.MaritalStatus,

            };

            return PartialView("_CreateOrEditModal", viewModel);
        }

        public async Task<PartialViewResult> ViewMaritalStatusModal(int id)
        {
            var getMaritalStatusForViewDto = await _maritalStatusesAppService.GetMaritalStatusForView(id);

            var model = new MaritalStatusViewModel()
            {
                MaritalStatus = getMaritalStatusForViewDto.MaritalStatus
            };

            return PartialView("_ViewMaritalStatusModal", model);
        }

    }
}