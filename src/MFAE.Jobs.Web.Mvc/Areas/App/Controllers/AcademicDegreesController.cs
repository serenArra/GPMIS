using System;
using System.Threading.Tasks;
using Abp.AspNetCore.Mvc.Authorization;
using Microsoft.AspNetCore.Mvc;
using MFAE.Jobs.Web.Areas.App.Models.AcademicDegrees;
using MFAE.Jobs.Web.Controllers;
using MFAE.Jobs.Authorization;
using MFAE.Jobs.ApplicationForm;
using MFAE.Jobs.ApplicationForm.Dtos;
using Abp.Application.Services.Dto;
using Abp.Extensions;

namespace MFAE.Jobs.Web.Areas.App.Controllers
{
    [Area("App")]
    [AbpMvcAuthorize(AppPermissions.Pages_AcademicDegrees)]
    public class AcademicDegreesController : JobsControllerBase
    {
        private readonly IAcademicDegreesAppService _academicDegreesAppService;

        public AcademicDegreesController(IAcademicDegreesAppService academicDegreesAppService)
        {
            _academicDegreesAppService = academicDegreesAppService;

        }

        public ActionResult Index()
        {
            var model = new AcademicDegreesViewModel
            {
                FilterText = ""
            };

            return View(model);
        }

        [AbpMvcAuthorize(AppPermissions.Pages_AcademicDegrees_Create, AppPermissions.Pages_AcademicDegrees_Edit)]
        public async Task<PartialViewResult> CreateOrEditModal(int? id)
        {
            GetAcademicDegreeForEditOutput getAcademicDegreeForEditOutput;

            if (id.HasValue)
            {
                getAcademicDegreeForEditOutput = await _academicDegreesAppService.GetAcademicDegreeForEdit(new EntityDto { Id = (int)id });
            }
            else
            {
                getAcademicDegreeForEditOutput = new GetAcademicDegreeForEditOutput
                {
                    AcademicDegree = new CreateOrEditAcademicDegreeDto()
                };
            }

            var viewModel = new CreateOrEditAcademicDegreeModalViewModel()
            {
                AcademicDegree = getAcademicDegreeForEditOutput.AcademicDegree,

            };

            return PartialView("_CreateOrEditModal", viewModel);
        }

        public async Task<PartialViewResult> ViewAcademicDegreeModal(int id)
        {
            var getAcademicDegreeForViewDto = await _academicDegreesAppService.GetAcademicDegreeForView(id);

            var model = new AcademicDegreeViewModel()
            {
                AcademicDegree = getAcademicDegreeForViewDto.AcademicDegree
            };

            return PartialView("_ViewAcademicDegreeModal", model);
        }

    }
}