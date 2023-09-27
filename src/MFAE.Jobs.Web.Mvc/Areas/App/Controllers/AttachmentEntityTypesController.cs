using System;
using System.Threading.Tasks;
using Abp.AspNetCore.Mvc.Authorization;
using Microsoft.AspNetCore.Mvc;
using MFAE.Jobs.Web.Areas.App.Models.AttachmentEntityTypes;
using MFAE.Jobs.Web.Controllers;
using MFAE.Jobs.Authorization;
using MFAE.Jobs.Attachments;
using MFAE.Jobs.Attachments.Dtos;
using Abp.Application.Services.Dto;
using Abp.Extensions;

namespace MFAE.Jobs.Web.Areas.App.Controllers
{
    [Area("App")]
    [AbpMvcAuthorize(AppPermissions.Pages_AttachmentEntityTypes)]
    public class AttachmentEntityTypesController : JobsControllerBase
    {
        private readonly IAttachmentEntityTypesAppService _attachmentEntityTypesAppService;

        public AttachmentEntityTypesController(IAttachmentEntityTypesAppService attachmentEntityTypesAppService)
        {
            _attachmentEntityTypesAppService = attachmentEntityTypesAppService;

        }

        public ActionResult Index()
        {
            var model = new AttachmentEntityTypesViewModel
            {
                FilterText = ""
            };

            return View(model);
        }

        [AbpMvcAuthorize(AppPermissions.Pages_AttachmentEntityTypes_Create, AppPermissions.Pages_AttachmentEntityTypes_Edit)]
        public async Task<PartialViewResult> CreateOrEditModal(int? id)
        {
            GetAttachmentEntityTypeForEditOutput getAttachmentEntityTypeForEditOutput;

            if (id.HasValue)
            {
                getAttachmentEntityTypeForEditOutput = await _attachmentEntityTypesAppService.GetAttachmentEntityTypeForEdit(new EntityDto { Id = (int)id });
            }
            else
            {
                getAttachmentEntityTypeForEditOutput = new GetAttachmentEntityTypeForEditOutput
                {
                    AttachmentEntityType = new CreateOrEditAttachmentEntityTypeDto()
                };
            }

            var viewModel = new CreateOrEditAttachmentEntityTypeModalViewModel()
            {
                AttachmentEntityType = getAttachmentEntityTypeForEditOutput.AttachmentEntityType,

            };

            return PartialView("_CreateOrEditModal", viewModel);
        }

        public async Task<PartialViewResult> ViewAttachmentEntityTypeModal(int id)
        {
            var getAttachmentEntityTypeForViewDto = await _attachmentEntityTypesAppService.GetAttachmentEntityTypeForView(id);

            var model = new AttachmentEntityTypeViewModel()
            {
                AttachmentEntityType = getAttachmentEntityTypeForViewDto.AttachmentEntityType
            };

            return PartialView("_ViewAttachmentEntityTypeModal", model);
        }

    }
}