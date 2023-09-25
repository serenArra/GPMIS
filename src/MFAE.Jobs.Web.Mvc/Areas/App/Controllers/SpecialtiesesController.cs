using System;
using System.Threading.Tasks;
using Abp.AspNetCore.Mvc.Authorization;
using Microsoft.AspNetCore.Mvc;
using MFAE.Jobs.Web.Areas.App.Models.Specialtieses;
using MFAE.Jobs.Web.Controllers;
using MFAE.Jobs.Authorization;
using MFAE.Jobs.ApplicationForm;
using MFAE.Jobs.ApplicationForm.Dtos;
using Abp.Application.Services.Dto;
using Abp.Extensions;

namespace MFAE.Jobs.Web.Areas.App.Controllers
{
    [Area("App")]
    [AbpMvcAuthorize(AppPermissions.Pages_Specialtieses)]
    public class SpecialtiesesController : JobsControllerBase
    {
        private readonly ISpecialtiesesAppService _specialtiesesAppService;

        public SpecialtiesesController(ISpecialtiesesAppService specialtiesesAppService)
        {
            _specialtiesesAppService = specialtiesesAppService;

        }

        public ActionResult Index()
        {
            var model = new SpecialtiesesViewModel
            {
                FilterText = ""
            };

            return View(model);
        }

        [AbpMvcAuthorize(AppPermissions.Pages_Specialtieses_Create, AppPermissions.Pages_Specialtieses_Edit)]
        public async Task<PartialViewResult> CreateOrEditModal(int? id)
        {
            GetSpecialtiesForEditOutput getSpecialtiesForEditOutput;

            if (id.HasValue)
            {
                getSpecialtiesForEditOutput = await _specialtiesesAppService.GetSpecialtiesForEdit(new EntityDto { Id = (int)id });
            }
            else
            {
                getSpecialtiesForEditOutput = new GetSpecialtiesForEditOutput
                {
                    Specialties = new CreateOrEditSpecialtiesDto()
                };
            }

            var viewModel = new CreateOrEditSpecialtiesModalViewModel()
            {
                Specialties = getSpecialtiesForEditOutput.Specialties,

            };

            return PartialView("_CreateOrEditModal", viewModel);
        }

        public async Task<PartialViewResult> ViewSpecialtiesModal(int id)
        {
            var getSpecialtiesForViewDto = await _specialtiesesAppService.GetSpecialtiesForView(id);

            var model = new SpecialtiesViewModel()
            {
                Specialties = getSpecialtiesForViewDto.Specialties
            };

            return PartialView("_ViewSpecialtiesModal", model);
        }

    }
}