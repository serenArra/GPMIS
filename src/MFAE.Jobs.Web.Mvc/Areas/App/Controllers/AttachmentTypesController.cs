using System;
using System.Threading.Tasks;
using Abp.AspNetCore.Mvc.Authorization;
using Microsoft.AspNetCore.Mvc;
using MFAE.Jobs.Web.Areas.App.Models.AttachmentTypes;
using MFAE.Jobs.Web.Controllers;
using MFAE.Jobs.Authorization;
using MFAE.Jobs.Attachments;
using MFAE.Jobs.Attachments.Dtos;
using Abp.Application.Services.Dto;
using Abp.Extensions;

namespace MFAE.Jobs.Web.Areas.App.Controllers
{
    [Area("App")]
    [AbpMvcAuthorize(AppPermissions.Pages_AttachmentTypes)]
    public class AttachmentTypesController : JobsControllerBase
    {
        private readonly IAttachmentTypesAppService _attachmentTypesAppService;

        public AttachmentTypesController(IAttachmentTypesAppService attachmentTypesAppService)
        {
            _attachmentTypesAppService = attachmentTypesAppService;

        }

        public ActionResult Index()
        {
            var model = new AttachmentTypesViewModel
            {
                FilterText = ""
            };

            return View(model);
        }

        [AbpMvcAuthorize(AppPermissions.Pages_AttachmentTypes_Create, AppPermissions.Pages_AttachmentTypes_Edit)]
        public async Task<PartialViewResult> CreateOrEditModal(int? id)
        {
            GetAttachmentTypeForEditOutput getAttachmentTypeForEditOutput;

            if (id.HasValue)
            {
                getAttachmentTypeForEditOutput = await _attachmentTypesAppService.GetAttachmentTypeForEdit(new EntityDto { Id = (int)id });
            }
            else
            {
                getAttachmentTypeForEditOutput = new GetAttachmentTypeForEditOutput
                {
                    AttachmentType = new CreateOrEditAttachmentTypeDto()
                };
            }

            var viewModel = new CreateOrEditAttachmentTypeModalViewModel()
            {
                AttachmentType = getAttachmentTypeForEditOutput.AttachmentType,
                AttachmentEntityTypeName = getAttachmentTypeForEditOutput.AttachmentEntityTypeName,
                AttachmentTypeGroupName = getAttachmentTypeForEditOutput.AttachmentTypeGroupName,
                AttachmentTypeAttachmentEntityTypeList = await _attachmentTypesAppService.GetAllAttachmentEntityTypeForTableDropdown(),
                AttachmentTypeAttachmentTypeGroupList = await _attachmentTypesAppService.GetAllAttachmentTypeGroupForTableDropdown(),

            };

            return PartialView("_CreateOrEditModal", viewModel);
        }

        public async Task<PartialViewResult> ViewAttachmentTypeModal(int id)
        {
            var getAttachmentTypeForViewDto = await _attachmentTypesAppService.GetAttachmentTypeForView(id);

            var model = new AttachmentTypeViewModel()
            {
                AttachmentType = getAttachmentTypeForViewDto.AttachmentType
                ,
                AttachmentEntityTypeName = getAttachmentTypeForViewDto.AttachmentEntityTypeName

                ,
                AttachmentTypeGroupName = getAttachmentTypeForViewDto.AttachmentTypeGroupName

            };

            return PartialView("_ViewAttachmentTypeModal", model);
        }

    }
}