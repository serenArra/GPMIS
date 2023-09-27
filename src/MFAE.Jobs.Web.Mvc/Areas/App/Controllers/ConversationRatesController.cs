using System;
using System.Threading.Tasks;
using Abp.AspNetCore.Mvc.Authorization;
using Microsoft.AspNetCore.Mvc;
using MFAE.Jobs.Web.Areas.App.Models.ConversationRates;
using MFAE.Jobs.Web.Controllers;
using MFAE.Jobs.Authorization;
using MFAE.Jobs.ApplicationForm;
using MFAE.Jobs.ApplicationForm.Dtos;
using Abp.Application.Services.Dto;
using Abp.Extensions;

namespace MFAE.Jobs.Web.Areas.App.Controllers
{
    [Area("App")]
    [AbpMvcAuthorize(AppPermissions.Pages_ConversationRates)]
    public class ConversationRatesController : JobsControllerBase
    {
        private readonly IConversationRatesAppService _conversationRatesAppService;

        public ConversationRatesController(IConversationRatesAppService conversationRatesAppService)
        {
            _conversationRatesAppService = conversationRatesAppService;

        }

        public ActionResult Index()
        {
            var model = new ConversationRatesViewModel
            {
                FilterText = ""
            };

            return View(model);
        }

        [AbpMvcAuthorize(AppPermissions.Pages_ConversationRates_Create, AppPermissions.Pages_ConversationRates_Edit)]
        public async Task<PartialViewResult> CreateOrEditModal(int? id)
        {
            GetConversationRateForEditOutput getConversationRateForEditOutput;

            if (id.HasValue)
            {
                getConversationRateForEditOutput = await _conversationRatesAppService.GetConversationRateForEdit(new EntityDto { Id = (int)id });
            }
            else
            {
                getConversationRateForEditOutput = new GetConversationRateForEditOutput
                {
                    ConversationRate = new CreateOrEditConversationRateDto()
                };
            }

            var viewModel = new CreateOrEditConversationRateModalViewModel()
            {
                ConversationRate = getConversationRateForEditOutput.ConversationRate,

            };

            return PartialView("_CreateOrEditModal", viewModel);
        }

        public async Task<PartialViewResult> ViewConversationRateModal(int id)
        {
            var getConversationRateForViewDto = await _conversationRatesAppService.GetConversationRateForView(id);

            var model = new ConversationRateViewModel()
            {
                ConversationRate = getConversationRateForViewDto.ConversationRate
            };

            return PartialView("_ViewConversationRateModal", model);
        }

    }
}