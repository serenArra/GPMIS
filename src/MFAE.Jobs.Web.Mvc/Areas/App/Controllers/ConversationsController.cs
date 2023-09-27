using System;
using System.Threading.Tasks;
using Abp.AspNetCore.Mvc.Authorization;
using Microsoft.AspNetCore.Mvc;
using MFAE.Jobs.Web.Areas.App.Models.Conversations;
using MFAE.Jobs.Web.Controllers;
using MFAE.Jobs.Authorization;
using MFAE.Jobs.ApplicationForm;
using MFAE.Jobs.ApplicationForm.Dtos;
using Abp.Application.Services.Dto;
using Abp.Extensions;

namespace MFAE.Jobs.Web.Areas.App.Controllers
{
    [Area("App")]
    [AbpMvcAuthorize(AppPermissions.Pages_Conversations)]
    public class ConversationsController : JobsControllerBase
    {
        private readonly IConversationsAppService _conversationsAppService;

        public ConversationsController(IConversationsAppService conversationsAppService)
        {
            _conversationsAppService = conversationsAppService;

        }

        public ActionResult Index()
        {
            var model = new ConversationsViewModel
            {
                FilterText = ""
            };

            return View(model);
        }

        [AbpMvcAuthorize(AppPermissions.Pages_Conversations_Create, AppPermissions.Pages_Conversations_Edit)]
        public async Task<PartialViewResult> CreateOrEditModal(int? id)
        {
            GetConversationForEditOutput getConversationForEditOutput;

            if (id.HasValue)
            {
                getConversationForEditOutput = await _conversationsAppService.GetConversationForEdit(new EntityDto { Id = (int)id });
            }
            else
            {
                getConversationForEditOutput = new GetConversationForEditOutput
                {
                    Conversation = new CreateOrEditConversationDto()
                };
            }

            var viewModel = new CreateOrEditConversationModalViewModel()
            {
                Conversation = getConversationForEditOutput.Conversation,

            };

            return PartialView("_CreateOrEditModal", viewModel);
        }

        public async Task<PartialViewResult> ViewConversationModal(int id)
        {
            var getConversationForViewDto = await _conversationsAppService.GetConversationForView(id);

            var model = new ConversationViewModel()
            {
                Conversation = getConversationForViewDto.Conversation
            };

            return PartialView("_ViewConversationModal", model);
        }

    }
}