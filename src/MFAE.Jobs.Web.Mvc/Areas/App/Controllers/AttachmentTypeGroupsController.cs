using System;
using System.Threading.Tasks;
using Abp.AspNetCore.Mvc.Authorization;
using Microsoft.AspNetCore.Mvc;
using MFAE.Jobs.Web.Areas.App.Models.AttachmentTypeGroups;
using MFAE.Jobs.Web.Controllers;
using MFAE.Jobs.Authorization;
using MFAE.Jobs.Attachments;
using MFAE.Jobs.Attachments.Dtos;
using Abp.Application.Services.Dto;
using Abp.Extensions;

namespace MFAE.Jobs.Web.Areas.App.Controllers
{
    [Area("App")]
    [AbpMvcAuthorize(AppPermissions.Pages_AttachmentTypeGroups)]
    public class AttachmentTypeGroupsController : JobsControllerBase
    {
        private readonly IAttachmentTypeGroupsAppService _attachmentTypeGroupsAppService;

        public AttachmentTypeGroupsController(IAttachmentTypeGroupsAppService attachmentTypeGroupsAppService)
        {
            _attachmentTypeGroupsAppService = attachmentTypeGroupsAppService;

        }

        public ActionResult Index()
        {
            var model = new AttachmentTypeGroupsViewModel
            {
                FilterText = ""
            };

            return View(model);
        }

        [AbpMvcAuthorize(AppPermissions.Pages_AttachmentTypeGroups_Create, AppPermissions.Pages_AttachmentTypeGroups_Edit)]
        public async Task<PartialViewResult> CreateOrEditModal(int? id)
        {
            GetAttachmentTypeGroupForEditOutput getAttachmentTypeGroupForEditOutput;

            if (id.HasValue)
            {
                getAttachmentTypeGroupForEditOutput = await _attachmentTypeGroupsAppService.GetAttachmentTypeGroupForEdit(new EntityDto { Id = (int)id });
            }
            else
            {
                getAttachmentTypeGroupForEditOutput = new GetAttachmentTypeGroupForEditOutput
                {
                    AttachmentTypeGroup = new CreateOrEditAttachmentTypeGroupDto()
                };
            }

            var viewModel = new CreateOrEditAttachmentTypeGroupModalViewModel()
            {
                AttachmentTypeGroup = getAttachmentTypeGroupForEditOutput.AttachmentTypeGroup,

            };

            return PartialView("_CreateOrEditModal", viewModel);
        }

        public async Task<PartialViewResult> ViewAttachmentTypeGroupModal(int id)
        {
            var getAttachmentTypeGroupForViewDto = await _attachmentTypeGroupsAppService.GetAttachmentTypeGroupForView(id);

            var model = new AttachmentTypeGroupViewModel()
            {
                AttachmentTypeGroup = getAttachmentTypeGroupForViewDto.AttachmentTypeGroup
            };

            return PartialView("_ViewAttachmentTypeGroupModal", model);
        }

    }
}