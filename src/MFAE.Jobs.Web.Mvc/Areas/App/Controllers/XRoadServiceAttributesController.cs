using System;
using System.Threading.Tasks;
using Abp.AspNetCore.Mvc.Authorization;
using Microsoft.AspNetCore.Mvc;
using MFAE.Jobs.Web.Areas.App.Models.XRoadServiceAttributes;
using MFAE.Jobs.Web.Controllers;
using MFAE.Jobs.Authorization;
using MFAE.Jobs.XRoad;
using MFAE.Jobs.XRoad.Dtos;
using Abp.Application.Services.Dto;
using Abp.Extensions;

namespace MFAE.Jobs.Web.Areas.App.Controllers
{
    [Area("App")]
    [AbpMvcAuthorize(AppPermissions.Pages_XRoadServiceAttributes)]
    public class XRoadServiceAttributesController : JobsControllerBase
    {
        private readonly IXRoadServiceAttributesAppService _xRoadServiceAttributesAppService;

        public XRoadServiceAttributesController(IXRoadServiceAttributesAppService xRoadServiceAttributesAppService)
        {
            _xRoadServiceAttributesAppService = xRoadServiceAttributesAppService;

        }

        public ActionResult Index()
        {
            var model = new XRoadServiceAttributesViewModel
            {
                FilterText = ""
            };

            return View(model);
        }

        public PartialViewResult IndexRequest(int serviceId)
        {
            var model = new XRoadServiceAttributesViewModel
            {
                FilterText = "",
                XRoadServiceId = serviceId
            };

            return PartialView("_IndexRequest", model);
        }
        public PartialViewResult IndexResponse(int serviceId)
        {
            var model = new XRoadServiceAttributesViewModel
            {
                FilterText = "",
                XRoadServiceId = serviceId
            };

            return PartialView("_IndexResponse", model);
        }

        [AbpMvcAuthorize(AppPermissions.Pages_XRoadServiceAttributes_Create, AppPermissions.Pages_XRoadServiceAttributes_Edit)]
        public async Task<PartialViewResult> CreateOrEditModal(int? id)
        {
            GetXRoadServiceAttributeForEditOutput getXRoadServiceAttributeForEditOutput;

            if (id.HasValue)
            {
                getXRoadServiceAttributeForEditOutput = await _xRoadServiceAttributesAppService.GetXRoadServiceAttributeForEdit(new EntityDto { Id = (int)id });
            }
            else
            {
                getXRoadServiceAttributeForEditOutput = new GetXRoadServiceAttributeForEditOutput
                {
                    XRoadServiceAttribute = new CreateOrEditXRoadServiceAttributeDto()
                };
            }

            var viewModel = new CreateOrEditXRoadServiceAttributeModalViewModel()
            {
                XRoadServiceAttribute = getXRoadServiceAttributeForEditOutput.XRoadServiceAttribute,
                XRoadServiceName = getXRoadServiceAttributeForEditOutput.XRoadServiceName,
            };

            return PartialView("_CreateOrEditModal", viewModel);
        }

        [AbpMvcAuthorize(AppPermissions.Pages_XRoadServiceAttributes_Create, AppPermissions.Pages_XRoadServiceAttributes_Edit)]
        public async Task<PartialViewResult> CreateOrEditModalRequest(int? id, int? xRoadServiceId)
        {
            GetXRoadServiceAttributeForEditOutput getXRoadServiceAttributeForEditOutput;

            if (id.HasValue)
            {
                getXRoadServiceAttributeForEditOutput = await _xRoadServiceAttributesAppService.GetXRoadServiceAttributeForEdit(new EntityDto { Id = (int)id });
            }
            else
            {
                getXRoadServiceAttributeForEditOutput = await _xRoadServiceAttributesAppService.GetXRoadServiceAttributeForCreate(xRoadServiceId.Value);
                getXRoadServiceAttributeForEditOutput.XRoadServiceAttribute.ServiceAttributeType = XRoadServiceAttributeTypeEnum.Request;
            }

            var viewModel = new CreateOrEditXRoadServiceAttributeModalViewModel()
            {
                XRoadServiceAttribute = getXRoadServiceAttributeForEditOutput.XRoadServiceAttribute,
                XRoadServiceName = getXRoadServiceAttributeForEditOutput.XRoadServiceName,
            };

            return PartialView("_CreateOrEditModalRequest", viewModel);
        }

        [AbpMvcAuthorize(AppPermissions.Pages_XRoadServiceAttributes_Create, AppPermissions.Pages_XRoadServiceAttributes_Edit)]
        public async Task<PartialViewResult> CreateOrEditModalResponse(int? id, int? xRoadServiceId)
        {
            GetXRoadServiceAttributeForEditOutput getXRoadServiceAttributeForEditOutput;

            if (id.HasValue)
            {
                getXRoadServiceAttributeForEditOutput = await _xRoadServiceAttributesAppService.GetXRoadServiceAttributeForEdit(new EntityDto { Id = (int)id });
            }
            else
            {
                getXRoadServiceAttributeForEditOutput = await _xRoadServiceAttributesAppService.GetXRoadServiceAttributeForCreate(xRoadServiceId.Value);
                getXRoadServiceAttributeForEditOutput.XRoadServiceAttribute.ServiceAttributeType = XRoadServiceAttributeTypeEnum.Response;
            }

            var viewModel = new CreateOrEditXRoadServiceAttributeModalViewModel()
            {
                XRoadServiceAttribute = getXRoadServiceAttributeForEditOutput.XRoadServiceAttribute,
                XRoadServiceName = getXRoadServiceAttributeForEditOutput.XRoadServiceName,
            };

            return PartialView("_CreateOrEditModalResponse", viewModel);
        }

        public async Task<PartialViewResult> ViewXRoadServiceAttributeModal(int id)
        {
            var getXRoadServiceAttributeForViewDto = await _xRoadServiceAttributesAppService.GetXRoadServiceAttributeForView(id);

            var model = new XRoadServiceAttributeViewModel()
            {
                XRoadServiceAttribute = getXRoadServiceAttributeForViewDto.XRoadServiceAttribute
                ,
                XRoadServiceName = getXRoadServiceAttributeForViewDto.XRoadServiceName

            };

            return PartialView("_ViewXRoadServiceAttributeModal", model);
        }

        [AbpMvcAuthorize(AppPermissions.Pages_XRoadServiceAttributes_Create, AppPermissions.Pages_XRoadServiceAttributes_Edit)]
        public PartialViewResult XRoadServiceLookupTableModal(int? id, string displayName)
        {
            var viewModel = new XRoadServiceAttributeXRoadServiceLookupTableViewModel()
            {
                Id = id,
                DisplayName = displayName,
                FilterText = ""
            };

            return PartialView("_XRoadServiceAttributeXRoadServiceLookupTableModal", viewModel);
        }

    }
}