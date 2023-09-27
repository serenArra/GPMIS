using System;
using System.Threading.Tasks;
using Abp.AspNetCore.Mvc.Authorization;
using Microsoft.AspNetCore.Mvc;
using MFAE.Jobs.Web.Areas.App.Models.XRoadServiceErrors;
using MFAE.Jobs.Web.Controllers;
using MFAE.Jobs.Authorization;
using MFAE.Jobs.XRoad;
using MFAE.Jobs.XRoad.Dtos;
using Abp.Application.Services.Dto;
using Abp.Extensions;

namespace MFAE.Jobs.Web.Areas.App.Controllers
{
    [Area("App")]
    [AbpMvcAuthorize(AppPermissions.Pages_XRoadServiceErrors)]
    public class XRoadServiceErrorsController : JobsControllerBase
    {
        private readonly IXRoadServiceErrorsAppService _xRoadServiceErrorsAppService;

        public XRoadServiceErrorsController(IXRoadServiceErrorsAppService xRoadServiceErrorsAppService)
        {
            _xRoadServiceErrorsAppService = xRoadServiceErrorsAppService;

        }

        public PartialViewResult GetPartial(int serviceId)
        {
            var model = new XRoadServiceErrorsViewModel
            {
                FilterText = "",
                XRoadServiceId = serviceId
            };

            //return View(model);
            return PartialView("_Index", model);
        }

        public ActionResult Index()
        {
            var model = new XRoadServiceErrorsViewModel
            {
                FilterText = ""
            };

            return View(model);
        }

        [AbpMvcAuthorize(AppPermissions.Pages_XRoadServiceErrors_Create, AppPermissions.Pages_XRoadServiceErrors_Edit)]
        public async Task<PartialViewResult> CreateOrEditModal(int? id, int? xRoadServiceId)
        {
            GetXRoadServiceErrorForEditOutput getXRoadServiceErrorForEditOutput;

            if (id.HasValue)
            {
                getXRoadServiceErrorForEditOutput = await _xRoadServiceErrorsAppService.GetXRoadServiceErrorForEdit(new EntityDto { Id = (int)id });
            }
            else
            {

                //getXRoadServiceErrorForEditOutput = new GetXRoadServiceErrorForEditOutput{
                //	XRoadServiceError = new CreateOrEditXRoadServiceErrorDto()
                //};
                getXRoadServiceErrorForEditOutput = await _xRoadServiceErrorsAppService.GetXRoadServiceErrorForCreate(xRoadServiceId.Value);

            }

            var viewModel = new CreateOrEditXRoadServiceErrorModalViewModel()
            {
                XRoadServiceError = getXRoadServiceErrorForEditOutput.XRoadServiceError,
                XRoadServiceName = getXRoadServiceErrorForEditOutput.XRoadServiceName,
            };

            return PartialView("_CreateOrEditModal", viewModel);
        }


        public async Task<PartialViewResult> ViewXRoadServiceErrorModal(int id)
        {
            var getXRoadServiceErrorForViewDto = await _xRoadServiceErrorsAppService.GetXRoadServiceErrorForView(id);

            var model = new XRoadServiceErrorViewModel()
            {
                XRoadServiceError = getXRoadServiceErrorForViewDto.XRoadServiceError
                ,
                XRoadServiceName = getXRoadServiceErrorForViewDto.XRoadServiceName

            };

            return PartialView("_ViewXRoadServiceErrorModal", model);
        }

        [AbpMvcAuthorize(AppPermissions.Pages_XRoadServiceErrors_Create, AppPermissions.Pages_XRoadServiceErrors_Edit)]
        public PartialViewResult XRoadServiceLookupTableModal(int? id, string displayName)
        {
            var viewModel = new XRoadServiceErrorXRoadServiceLookupTableViewModel()
            {
                Id = id,
                DisplayName = displayName,
                FilterText = ""
            };

            return PartialView("_XRoadServiceErrorXRoadServiceLookupTableModal", viewModel);
        }

    }
}