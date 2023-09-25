using System;
using System.Threading.Tasks;
using Abp.AspNetCore.Mvc.Authorization;
using Microsoft.AspNetCore.Mvc;
using MFAE.Jobs.Web.Areas.App.Models.Languages;
using MFAE.Jobs.Web.Controllers;
using MFAE.Jobs.Authorization;
using MFAE.Jobs.ApplicationForm;
using MFAE.Jobs.ApplicationForm.Dtos;
using Abp.Application.Services.Dto;
using Abp.Extensions;

namespace MFAE.Jobs.Web.Areas.App.Controllers
{
    [Area("App")]
    [AbpMvcAuthorize(AppPermissions.Pages_Languages)]
    public class LanguagesController : JobsControllerBase
    {
        private readonly IAppLanguagesAppService _languagesAppService;

        public LanguagesController(IAppLanguagesAppService languagesAppService)
        {
            _languagesAppService = languagesAppService;

        }

        public ActionResult Index()
        {
            var model = new LanguagesViewModel
            {
                FilterText = ""
            };

            return View(model);
        }

        [AbpMvcAuthorize(AppPermissions.Pages_Languages_Create, AppPermissions.Pages_Languages_Edit)]
        public async Task<PartialViewResult> CreateOrEditModal(int? id)
        {
            GetAppLanguageForEditOutput getLanguageForEditOutput;

            if (id.HasValue)
            {
                getLanguageForEditOutput = await _languagesAppService.GetLanguageForEdit(new EntityDto { Id = (int)id });
            }
            else
            {
                getLanguageForEditOutput = new GetAppLanguageForEditOutput
                {
                    Language = new CreateOrEditAppLanguageDto()
                };
            }

            var viewModel = new CreateOrEditLanguageModalViewModel()
            {
                Language = getLanguageForEditOutput.Language,

            };

            return PartialView("_CreateOrEditModal", viewModel);
        }

        public async Task<PartialViewResult> ViewLanguageModal(int id)
        {
            var getLanguageForViewDto = await _languagesAppService.GetLanguageForView(id);

            var model = new LanguageViewModel()
            {
                Language = getLanguageForViewDto.Language
            };

            return PartialView("_ViewLanguageModal", model);
        }

    }
}