using System;
using System.Threading.Tasks;
using Abp.AspNetCore.Mvc.Authorization;
using Microsoft.AspNetCore.Mvc;
using MFAE.Jobs.Web.Areas.App.Models.Governorates;
using MFAE.Jobs.Web.Controllers;
using MFAE.Jobs.Authorization;
using MFAE.Jobs.Location;
using MFAE.Jobs.Location.Dtos;
using Abp.Application.Services.Dto;
using Abp.Extensions;

namespace MFAE.Jobs.Web.Areas.App.Controllers
{
    [Area("App")]
    [AbpMvcAuthorize(AppPermissions.Pages_Governorates)]
    public class GovernoratesController : JobsControllerBase
    {
        private readonly IGovernoratesAppService _governoratesAppService;

        public GovernoratesController(IGovernoratesAppService governoratesAppService)
        {
            _governoratesAppService = governoratesAppService;

        }

        public ActionResult Index()
        {
            var model = new GovernoratesViewModel
            {
                FilterText = ""
            };

            return View(model);
        }

        [AbpMvcAuthorize(AppPermissions.Pages_Governorates_Create, AppPermissions.Pages_Governorates_Edit)]
        public async Task<PartialViewResult> CreateOrEditModal(int? id)
        {
            GetGovernorateForEditOutput getGovernorateForEditOutput;

            if (id.HasValue)
            {
                getGovernorateForEditOutput = await _governoratesAppService.GetGovernorateForEdit(new EntityDto { Id = (int)id });
            }
            else
            {
                getGovernorateForEditOutput = new GetGovernorateForEditOutput
                {
                    Governorate = new CreateOrEditGovernorateDto()
                };
            }

            var viewModel = new CreateOrEditGovernorateModalViewModel()
            {
                Governorate = getGovernorateForEditOutput.Governorate,
                CountryName = getGovernorateForEditOutput.CountryName,
                GovernorateCountryList = await _governoratesAppService.GetAllCountryForTableDropdown(),

            };

            return PartialView("_CreateOrEditModal", viewModel);
        }

        public async Task<PartialViewResult> ViewGovernorateModal(int id)
        {
            var getGovernorateForViewDto = await _governoratesAppService.GetGovernorateForView(id);

            var model = new GovernorateViewModel()
            {
                Governorate = getGovernorateForViewDto.Governorate
                ,
                CountryName = getGovernorateForViewDto.CountryName

            };

            return PartialView("_ViewGovernorateModal", model);
        }

    }
}