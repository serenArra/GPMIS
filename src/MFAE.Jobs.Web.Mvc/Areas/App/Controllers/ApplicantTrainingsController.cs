using System;
using System.Threading.Tasks;
using Abp.AspNetCore.Mvc.Authorization;
using Microsoft.AspNetCore.Mvc;
using MFAE.Jobs.Web.Areas.App.Models.ApplicantTrainings;
using MFAE.Jobs.Web.Controllers;
using MFAE.Jobs.Authorization;
using MFAE.Jobs.ApplicationForm;
using MFAE.Jobs.ApplicationForm.Dtos;
using Abp.Application.Services.Dto;

namespace MFAE.Jobs.Web.Areas.App.Controllers
{
    [Area("App")]
    [AbpMvcAuthorize(AppPermissions.Pages_ApplicantTrainings)]
    public class ApplicantTrainingsController : JobsControllerBase
    {
        private readonly IApplicantTrainingsAppService _applicantTrainingsAppService;

        public ApplicantTrainingsController(IApplicantTrainingsAppService applicantTrainingsAppService)
        {
            _applicantTrainingsAppService = applicantTrainingsAppService;

        }

        public ActionResult Index()
        {
            var model = new ApplicantTrainingsViewModel
            {
                FilterText = ""
            };

            return View(model);
        }

        [AbpMvcAuthorize(AppPermissions.Pages_ApplicantTrainings_Create, AppPermissions.Pages_ApplicantTrainings_Edit)]
        public async Task<PartialViewResult> CreateOrEditModal(long? id, long applicantId)
        {
            GetApplicantTrainingForEditOutput getApplicantTrainingForEditOutput;

            if (id.HasValue)
            {
                getApplicantTrainingForEditOutput = await _applicantTrainingsAppService.GetApplicantTrainingForEdit(new EntityDto<long> { Id = (long)id });
            }
            else
            {
                getApplicantTrainingForEditOutput = new GetApplicantTrainingForEditOutput
                {
                    ApplicantTraining = new CreateOrEditApplicantTrainingDto()
                    {
                        ApplicantId = applicantId
                    }
                };
                getApplicantTrainingForEditOutput.ApplicantTraining.TrainingDate = DateTime.Now;
            }

            var viewModel = new CreateOrEditApplicantTrainingModalViewModel()
            {
                ApplicantTraining = getApplicantTrainingForEditOutput.ApplicantTraining,
                ApplicantFirstName = getApplicantTrainingForEditOutput.ApplicantFirstName,
                ApplicantTrainingApplicantList = await _applicantTrainingsAppService.GetAllApplicantForTableDropdown(),

            };

            return PartialView("_CreateOrEditModal", viewModel);
        }

        public async Task<PartialViewResult> ViewApplicantTrainingModal(long id)
        {
            var getApplicantTrainingForViewDto = await _applicantTrainingsAppService.GetApplicantTrainingForView(id);

            var model = new ApplicantTrainingViewModel()
            {
                ApplicantTraining = getApplicantTrainingForViewDto.ApplicantTraining
                ,
                ApplicantFirstName = getApplicantTrainingForViewDto.ApplicantFirstName

            };

            return PartialView("_ViewApplicantTrainingModal", model);
        }

    }
}