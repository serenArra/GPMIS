using System;
using System.Threading.Tasks;
using Abp.AspNetCore.Mvc.Authorization;
using Microsoft.AspNetCore.Mvc;
using MFAE.Jobs.Web.Areas.App.Models.IdentificationTypes;
using MFAE.Jobs.Web.Controllers;
using MFAE.Jobs.Authorization;
using MFAE.Jobs.ApplicationForm;
using MFAE.Jobs.ApplicationForm.Dtos;
using Abp.Application.Services.Dto;
using Abp.Extensions;

namespace MFAE.Jobs.Web.Areas.App.Controllers
{
    [Area("App")]
    [AbpMvcAuthorize(AppPermissions.Pages_IdentificationTypes)]
    public class IdentificationTypesController : JobsControllerBase
    {
        private readonly IIdentificationTypesAppService _identificationTypesAppService;

        public IdentificationTypesController(IIdentificationTypesAppService identificationTypesAppService)
        {
            _identificationTypesAppService = identificationTypesAppService;

        }

        public ActionResult Index()
        {
            var model = new IdentificationTypesViewModel
            {
                FilterText = ""
            };

            return View(model);
        }

        [AbpMvcAuthorize(AppPermissions.Pages_IdentificationTypes_Create, AppPermissions.Pages_IdentificationTypes_Edit)]
        public async Task<PartialViewResult> CreateOrEditModal(int? id)
        {
            GetIdentificationTypeForEditOutput getIdentificationTypeForEditOutput;

            if (id.HasValue)
            {
                getIdentificationTypeForEditOutput = await _identificationTypesAppService.GetIdentificationTypeForEdit(new EntityDto { Id = (int)id });
            }
            else
            {
                getIdentificationTypeForEditOutput = new GetIdentificationTypeForEditOutput
                {
                    IdentificationType = new CreateOrEditIdentificationTypeDto()
                };
            }

            var viewModel = new CreateOrEditIdentificationTypeModalViewModel()
            {
                IdentificationType = getIdentificationTypeForEditOutput.IdentificationType,

            };

            return PartialView("_CreateOrEditModal", viewModel);
        }

        public async Task<PartialViewResult> ViewIdentificationTypeModal(int id)
        {
            var getIdentificationTypeForViewDto = await _identificationTypesAppService.GetIdentificationTypeForView(id);

            var model = new IdentificationTypeViewModel()
            {
                IdentificationType = getIdentificationTypeForViewDto.IdentificationType
            };

            return PartialView("_ViewIdentificationTypeModal", model);
        }

    }
}