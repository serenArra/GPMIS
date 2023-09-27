using System;
using System.Threading.Tasks;
using Abp.AspNetCore.Mvc.Authorization;
using Microsoft.AspNetCore.Mvc;
using MFAE.Jobs.Web.Areas.App.Models.GraduationRates;
using MFAE.Jobs.Web.Controllers;
using MFAE.Jobs.Authorization;
using MFAE.Jobs.ApplicationForm;
using MFAE.Jobs.ApplicationForm.Dtos;
using Abp.Application.Services.Dto;
using Abp.Extensions;

namespace MFAE.Jobs.Web.Areas.App.Controllers
{
    [Area("App")]
    [AbpMvcAuthorize(AppPermissions.Pages_GraduationRates)]
    public class GraduationRatesController : JobsControllerBase
    {
        private readonly IGraduationRatesAppService _graduationRatesAppService;

        public GraduationRatesController(IGraduationRatesAppService graduationRatesAppService)
        {
            _graduationRatesAppService = graduationRatesAppService;

        }

        public ActionResult Index()
        {
            var model = new GraduationRatesViewModel
            {
                FilterText = ""
            };

            return View(model);
        }

        [AbpMvcAuthorize(AppPermissions.Pages_GraduationRates_Create, AppPermissions.Pages_GraduationRates_Edit)]
        public async Task<PartialViewResult> CreateOrEditModal(int? id)
        {
            GetGraduationRateForEditOutput getGraduationRateForEditOutput;

            if (id.HasValue)
            {
                getGraduationRateForEditOutput = await _graduationRatesAppService.GetGraduationRateForEdit(new EntityDto { Id = (int)id });
            }
            else
            {
                getGraduationRateForEditOutput = new GetGraduationRateForEditOutput
                {
                    GraduationRate = new CreateOrEditGraduationRateDto()
                };
            }

            var viewModel = new CreateOrEditGraduationRateModalViewModel()
            {
                GraduationRate = getGraduationRateForEditOutput.GraduationRate,

            };

            return PartialView("_CreateOrEditModal", viewModel);
        }

        public async Task<PartialViewResult> ViewGraduationRateModal(int id)
        {
            var getGraduationRateForViewDto = await _graduationRatesAppService.GetGraduationRateForView(id);

            var model = new GraduationRateViewModel()
            {
                GraduationRate = getGraduationRateForViewDto.GraduationRate
            };

            return PartialView("_ViewGraduationRateModal", model);
        }

    }
}