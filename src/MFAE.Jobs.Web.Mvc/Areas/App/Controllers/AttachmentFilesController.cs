using System;
using System.Threading.Tasks;
using Abp.AspNetCore.Mvc.Authorization;
using Microsoft.AspNetCore.Mvc;
using MFAE.Jobs.Web.Areas.App.Models.AttachmentFiles;
using MFAE.Jobs.Web.Controllers;
using MFAE.Jobs.Authorization;
using MFAE.Jobs.Attachments;
using MFAE.Jobs.Attachments.Dtos;
using Abp.Application.Services.Dto;
using Abp.Extensions;

namespace MFAE.Jobs.Web.Areas.App.Controllers
{
    [Area("App")]
    [AbpMvcAuthorize(AppPermissions.Pages_AttachmentFiles)]
    public class AttachmentFilesController : JobsControllerBase
    {
        private readonly IAttachmentFilesAppService _attachmentFilesAppService;

        public AttachmentFilesController(IAttachmentFilesAppService attachmentFilesAppService)
        {
            _attachmentFilesAppService = attachmentFilesAppService;

        }

        public ActionResult Index()
        {
            var model = new AttachmentFilesViewModel
            {
                FilterText = ""
            };

            return View(model);
        }

        [AbpMvcAuthorize(AppPermissions.Pages_AttachmentFiles_Create, AppPermissions.Pages_AttachmentFiles_Edit)]
        public async Task<PartialViewResult> CreateOrEditModal(int? id)
        {
            GetAttachmentFileForEditOutput getAttachmentFileForEditOutput;

            if (id.HasValue)
            {
                getAttachmentFileForEditOutput = await _attachmentFilesAppService.GetAttachmentFileForEdit(new EntityDto { Id = (int)id });
            }
            else
            {
                getAttachmentFileForEditOutput = new GetAttachmentFileForEditOutput
                {
                    AttachmentFile = new CreateOrEditAttachmentFileDto()
                };
            }

            var viewModel = new CreateOrEditAttachmentFileModalViewModel()
            {
                AttachmentFile = getAttachmentFileForEditOutput.AttachmentFile,
                AttachmentTypeArName = getAttachmentFileForEditOutput.AttachmentTypeArName,
                AttachmentFileAttachmentTypeList = await _attachmentFilesAppService.GetAllAttachmentTypeForTableDropdown(),

            };

            return PartialView("_CreateOrEditModal", viewModel);
        }

        public async Task<PartialViewResult> ViewAttachmentFileModal(int id)
        {
            var getAttachmentFileForViewDto = await _attachmentFilesAppService.GetAttachmentFileForView(id);

            var model = new AttachmentFileViewModel()
            {
                AttachmentFile = getAttachmentFileForViewDto.AttachmentFile
                ,
                AttachmentTypeArName = getAttachmentFileForViewDto.AttachmentTypeArName

            };

            return PartialView("_ViewAttachmentFileModal", model);
        }

    }
}