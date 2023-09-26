using System;
using System.Threading.Tasks;
using Abp.AspNetCore.Mvc.Authorization;
using Microsoft.AspNetCore.Mvc;
using MFAE.Jobs.Web.Areas.App.Models.XRoadMappings;
using MFAE.Jobs.Web.Controllers;
using MFAE.Jobs.Authorization;
using MFAE.Jobs.XRoad;
using MFAE.Jobs.XRoad.Dtos;
using Abp.Application.Services.Dto;
using Abp.Extensions;

namespace MFAE.Jobs.Web.Areas.App.Controllers
{
    [Area("App")]
    [AbpMvcAuthorize(AppPermissions.Pages_XRoadMappings)]
    public class XRoadMappingsController : JobsControllerBase
    {
        private readonly IXRoadMappingsAppService _xRoadMappingsAppService;

        public XRoadMappingsController(IXRoadMappingsAppService xRoadMappingsAppService)
        {
            _xRoadMappingsAppService = xRoadMappingsAppService;

        }

        public ActionResult Index()
        {
            var model = new XRoadMappingsViewModel
            {
                FilterText = ""
            };

            return View(model);
        }

        [AbpMvcAuthorize(AppPermissions.Pages_XRoadMappings_Create, AppPermissions.Pages_XRoadMappings_Edit)]
        public async Task<PartialViewResult> CreateOrEditModal(int? id)
        {
            GetXRoadMappingForEditOutput getXRoadMappingForEditOutput;

            if (id.HasValue)
            {
                getXRoadMappingForEditOutput = await _xRoadMappingsAppService.GetXRoadMappingForEdit(new EntityDto { Id = (int)id });
            }
            else
            {
                getXRoadMappingForEditOutput = new GetXRoadMappingForEditOutput
                {
                    XRoadMapping = new CreateOrEditXRoadMappingDto()
                };
            }

            var viewModel = new CreateOrEditXRoadMappingModalViewModel()
            {
                XRoadMapping = getXRoadMappingForEditOutput.XRoadMapping,

            };

            return PartialView("_CreateOrEditModal", viewModel);
        }

        public async Task<PartialViewResult> ViewXRoadMappingModal(int id)
        {
            var getXRoadMappingForViewDto = await _xRoadMappingsAppService.GetXRoadMappingForView(id);

            var model = new XRoadMappingViewModel()
            {
                XRoadMapping = getXRoadMappingForViewDto.XRoadMapping
            };

            return PartialView("_ViewXRoadMappingModal", model);
        }

    }
}