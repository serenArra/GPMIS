using System;
using System.Threading.Tasks;
using Abp.AspNetCore.Mvc.Authorization;
using Microsoft.AspNetCore.Mvc;
using MFAE.Jobs.Web.Areas.App.Models.Localities;
using MFAE.Jobs.Web.Controllers;
using MFAE.Jobs.Authorization;
using MFAE.Jobs.Location;
using MFAE.Jobs.Location.Dtos;
using Abp.Application.Services.Dto;
using Abp.Extensions;

namespace MFAE.Jobs.Web.Areas.App.Controllers
{
    [Area("App")]
    [AbpMvcAuthorize(AppPermissions.Pages_Localities)]
    public class LocalitiesController : JobsControllerBase
    {
        private readonly ILocalitiesAppService _localitiesAppService;

        public LocalitiesController(ILocalitiesAppService localitiesAppService)
        {
            _localitiesAppService = localitiesAppService;

        }

        public ActionResult Index()
        {
            var model = new LocalitiesViewModel
            {
                FilterText = ""
            };

            return View(model);
        }

        [AbpMvcAuthorize(AppPermissions.Pages_Localities_Create, AppPermissions.Pages_Localities_Edit)]
        public async Task<PartialViewResult> CreateOrEditModal(int? id)
        {
            GetLocalityForEditOutput getLocalityForEditOutput;

            if (id.HasValue)
            {
                getLocalityForEditOutput = await _localitiesAppService.GetLocalityForEdit(new EntityDto { Id = (int)id });
            }
            else
            {
                getLocalityForEditOutput = new GetLocalityForEditOutput
                {
                    Locality = new CreateOrEditLocalityDto()
                };
            }

            var viewModel = new CreateOrEditLocalityModalViewModel()
            {
                Locality = getLocalityForEditOutput.Locality,
                GovernorateName = getLocalityForEditOutput.GovernorateName,
                LocalityGovernorateList = await _localitiesAppService.GetAllGovernorateForTableDropdown(),

            };

            return PartialView("_CreateOrEditModal", viewModel);
        }

        public async Task<PartialViewResult> ViewLocalityModal(int id)
        {
            var getLocalityForViewDto = await _localitiesAppService.GetLocalityForView(id);

            var model = new LocalityViewModel()
            {
                Locality = getLocalityForViewDto.Locality
                ,
                GovernorateName = getLocalityForViewDto.GovernorateName

            };

            return PartialView("_ViewLocalityModal", model);
        }

    }
}