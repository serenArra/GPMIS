using System;
using System.Threading.Tasks;
using Abp.AspNetCore.Mvc.Authorization;
using Microsoft.AspNetCore.Mvc;
using MFAE.Jobs.Web.Areas.App.Models.XRoadServiceAttributeMappings;
using MFAE.Jobs.Web.Controllers;
using MFAE.Jobs.Authorization;
using MFAE.Jobs.XRoad;
using MFAE.Jobs.XRoad.Dtos;
using Abp.Application.Services.Dto;
using Abp.Extensions;

namespace MFAE.Jobs.Web.Areas.App.Controllers
{
    [Area("App")]
    [AbpMvcAuthorize(AppPermissions.Pages_XRoadServiceAttributeMappings)]
    public class XRoadServiceAttributeMappingsController : JobsControllerBase
    {
        private readonly IXRoadServiceAttributeMappingsAppService _xRoadServiceAttributeMappingsAppService;

        public XRoadServiceAttributeMappingsController(IXRoadServiceAttributeMappingsAppService xRoadServiceAttributeMappingsAppService)
        {
            _xRoadServiceAttributeMappingsAppService = xRoadServiceAttributeMappingsAppService;

        }

        public ActionResult Index()
        {
            var model = new XRoadServiceAttributeMappingsViewModel
            {
                FilterText = ""
            };

            return View(model);
        }

        public async Task<PartialViewResult> IndexRequest(int attributeId)
        {
            var model = new XRoadServiceAttributeMappingsViewModel
            {
                FilterText = "",
                XRoadServiceAttributeId = attributeId
            };
            var serviceAttribute = await _xRoadServiceAttributeMappingsAppService.GetXRoadServiceAttribute(attributeId);
            model.AttributeName = serviceAttribute.Name;
            return PartialView("_IndexRequest", model);
        }

        public async Task<PartialViewResult> IndexResponse(int attributeId)
        {
            var model = new XRoadServiceAttributeMappingsViewModel
            {
                FilterText = "",
                XRoadServiceAttributeId = attributeId
            };
            var serviceAttribute = await _xRoadServiceAttributeMappingsAppService.GetXRoadServiceAttribute(attributeId);
            model.AttributeName = serviceAttribute.Name;
            return PartialView("_IndexResponse", model);
        }

        [AbpMvcAuthorize(AppPermissions.Pages_XRoadServiceAttributeMappings_Create, AppPermissions.Pages_XRoadServiceAttributeMappings_Edit)]
        public async Task<PartialViewResult> CreateOrEditModalRequest(int? id, int? attributeId)
        {
            GetXRoadServiceAttributeMappingForEditOutput getXRoadServiceAttributeMappingForEditOutput;

            if (id.HasValue)
            {
                getXRoadServiceAttributeMappingForEditOutput = await _xRoadServiceAttributeMappingsAppService.GetXRoadServiceAttributeMappingForEdit(new EntityDto { Id = (int)id });
            }
            else
            {
                getXRoadServiceAttributeMappingForEditOutput = await _xRoadServiceAttributeMappingsAppService.GetXRoadServiceAttributeMappingForCreate(attributeId.Value);
            }

            var viewModel = new CreateOrEditXRoadServiceAttributeMappingModalViewModel()
            {
                XRoadServiceAttributeMapping = getXRoadServiceAttributeMappingForEditOutput.XRoadServiceAttributeMapping,
                XRoadServiceAttributeName = getXRoadServiceAttributeMappingForEditOutput.XRoadServiceAttributeName,
            };

            return PartialView("_CreateOrEditModalRequest", viewModel);
        }
        [AbpMvcAuthorize(AppPermissions.Pages_XRoadServiceAttributeMappings_Create, AppPermissions.Pages_XRoadServiceAttributeMappings_Edit)]
        public async Task<PartialViewResult> CreateOrEditModalResponse(int? id, int? attributeId)
        {
            GetXRoadServiceAttributeMappingForEditOutput getXRoadServiceAttributeMappingForEditOutput;

            if (id.HasValue)
            {
                getXRoadServiceAttributeMappingForEditOutput = await _xRoadServiceAttributeMappingsAppService.GetXRoadServiceAttributeMappingForEdit(new EntityDto { Id = (int)id });
            }
            else
            {
                getXRoadServiceAttributeMappingForEditOutput = await _xRoadServiceAttributeMappingsAppService.GetXRoadServiceAttributeMappingForCreate(attributeId.Value);
            }

            var viewModel = new CreateOrEditXRoadServiceAttributeMappingModalViewModel()
            {
                XRoadServiceAttributeMapping = getXRoadServiceAttributeMappingForEditOutput.XRoadServiceAttributeMapping,
                XRoadServiceAttributeName = getXRoadServiceAttributeMappingForEditOutput.XRoadServiceAttributeName,
            };

            return PartialView("_CreateOrEditModalResponse", viewModel);
        }

        [AbpMvcAuthorize(AppPermissions.Pages_XRoadServiceAttributeMappings_Create, AppPermissions.Pages_XRoadServiceAttributeMappings_Edit)]
        public async Task<PartialViewResult> CreateOrEditModal(int? id)
        {
            GetXRoadServiceAttributeMappingForEditOutput getXRoadServiceAttributeMappingForEditOutput;

            if (id.HasValue)
            {
                getXRoadServiceAttributeMappingForEditOutput = await _xRoadServiceAttributeMappingsAppService.GetXRoadServiceAttributeMappingForEdit(new EntityDto { Id = (int)id });
            }
            else
            {
                getXRoadServiceAttributeMappingForEditOutput = new GetXRoadServiceAttributeMappingForEditOutput
                {
                    XRoadServiceAttributeMapping = new CreateOrEditXRoadServiceAttributeMappingDto()
                };
            }

            var viewModel = new CreateOrEditXRoadServiceAttributeMappingModalViewModel()
            {
                XRoadServiceAttributeMapping = getXRoadServiceAttributeMappingForEditOutput.XRoadServiceAttributeMapping,
                XRoadServiceAttributeName = getXRoadServiceAttributeMappingForEditOutput.XRoadServiceAttributeName,
            };

            return PartialView("_CreateOrEditModal", viewModel);
        }

        public async Task<PartialViewResult> ViewXRoadServiceAttributeMappingModal(int id)
        {
            var getXRoadServiceAttributeMappingForViewDto = await _xRoadServiceAttributeMappingsAppService.GetXRoadServiceAttributeMappingForView(id);

            var model = new XRoadServiceAttributeMappingViewModel()
            {
                XRoadServiceAttributeMapping = getXRoadServiceAttributeMappingForViewDto.XRoadServiceAttributeMapping
                ,
                XRoadServiceAttributeName = getXRoadServiceAttributeMappingForViewDto.XRoadServiceAttributeName

            };

            return PartialView("_ViewXRoadServiceAttributeMappingModal", model);
        }

        [AbpMvcAuthorize(AppPermissions.Pages_XRoadServiceAttributeMappings_Create, AppPermissions.Pages_XRoadServiceAttributeMappings_Edit)]
        public PartialViewResult XRoadServiceAttributeLookupTableModal(int? id, string displayName)
        {
            var viewModel = new XRoadServiceAttributeMappingXRoadServiceAttributeLookupTableViewModel()
            {
                Id = id,
                DisplayName = displayName,
                FilterText = ""
            };

            return PartialView("_XRoadServiceAttributeMappingXRoadServiceAttributeLookupTableModal", viewModel);
        }

    }
}