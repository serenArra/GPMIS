using System;
using System.Threading.Tasks;
using Abp.AspNetCore.Mvc.Authorization;
using Microsoft.AspNetCore.Mvc;
using MFAE.Jobs.Web.Areas.App.Models.ApplicantStudies;
using MFAE.Jobs.Web.Controllers;
using MFAE.Jobs.Authorization;
using MFAE.Jobs.ApplicationForm;
using MFAE.Jobs.ApplicationForm.Dtos;
using Abp.Application.Services.Dto;
using Abp.Extensions;

namespace MFAE.Jobs.Web.Areas.App.Controllers
{
    [Area("App")]
    [AbpMvcAuthorize(AppPermissions.Pages_ApplicantStudies)]
    public class ApplicantStudiesController : JobsControllerBase
    {
        private readonly IApplicantStudiesAppService _applicantStudiesAppService;

        public ApplicantStudiesController(IApplicantStudiesAppService applicantStudiesAppService)
        {
            _applicantStudiesAppService = applicantStudiesAppService;

        }

        public ActionResult Index()
        {
            var model = new ApplicantStudiesViewModel
            {
                FilterText = ""
            };

            return View(model);
        }

        [AbpMvcAuthorize(AppPermissions.Pages_ApplicantStudies_Create, AppPermissions.Pages_ApplicantStudies_Edit)]
        public async Task<PartialViewResult> CreateOrEditModal(long? id)
        {
            GetApplicantStudyForEditOutput getApplicantStudyForEditOutput;

            if (id.HasValue)
            {
                getApplicantStudyForEditOutput = await _applicantStudiesAppService.GetApplicantStudyForEdit(new EntityDto<long> { Id = (long)id });
            }
            else
            {
                getApplicantStudyForEditOutput = new GetApplicantStudyForEditOutput
                {
                    ApplicantStudy = new CreateOrEditApplicantStudyDto()
                };
            }

            var viewModel = new CreateOrEditApplicantStudyModalViewModel()
            {
                ApplicantStudy = getApplicantStudyForEditOutput.ApplicantStudy,
                GraduationRateName = getApplicantStudyForEditOutput.GraduationRateName,
                AcademicDegreeName = getApplicantStudyForEditOutput.AcademicDegreeName,
                SpecialtiesName = getApplicantStudyForEditOutput.SpecialtiesName,
                ApplicantFirstName = getApplicantStudyForEditOutput.ApplicantFirstName,
                ApplicantStudyGraduationRateList = await _applicantStudiesAppService.GetAllGraduationRateForTableDropdown(),
                ApplicantStudyAcademicDegreeList = await _applicantStudiesAppService.GetAllAcademicDegreeForTableDropdown(),
                ApplicantStudySpecialtiesList = await _applicantStudiesAppService.GetAllSpecialtiesForTableDropdown(),
                ApplicantStudyApplicantList = await _applicantStudiesAppService.GetAllApplicantForTableDropdown(),

            };

            return PartialView("_CreateOrEditModal", viewModel);
        }

        public async Task<PartialViewResult> ViewApplicantStudyModal(long id)
        {
            var getApplicantStudyForViewDto = await _applicantStudiesAppService.GetApplicantStudyForView(id);

            var model = new ApplicantStudyViewModel()
            {
                ApplicantStudy = getApplicantStudyForViewDto.ApplicantStudy
                ,
                GraduationRateName = getApplicantStudyForViewDto.GraduationRateName

                ,
                AcademicDegreeName = getApplicantStudyForViewDto.AcademicDegreeName

                ,
                SpecialtiesName = getApplicantStudyForViewDto.SpecialtiesName

                ,
                ApplicantFirstName = getApplicantStudyForViewDto.ApplicantFirstName

            };

            return PartialView("_ViewApplicantStudyModal", model);
        }

    }
}