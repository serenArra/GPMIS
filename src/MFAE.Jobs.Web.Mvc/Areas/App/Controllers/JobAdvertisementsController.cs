using System;
using System.Threading.Tasks;
using Abp.AspNetCore.Mvc.Authorization;
using Microsoft.AspNetCore.Mvc;
using MFAE.Jobs.Web.Areas.App.Models.JobAdvertisements;
using MFAE.Jobs.Web.Controllers;
using MFAE.Jobs.Authorization;
using MFAE.Jobs.ApplicationForm;
using MFAE.Jobs.ApplicationForm.Dtos;
using Abp.Application.Services.Dto;
using Abp.Extensions;

namespace MFAE.Jobs.Web.Areas.App.Controllers
{
    [Area("App")]
    [AbpMvcAuthorize(AppPermissions.Pages_JobAdvertisements)]
    public class JobAdvertisementsController : JobsControllerBase
    {
        private readonly IJobAdvertisementsAppService _jobAdvertisementsAppService;

        public JobAdvertisementsController(IJobAdvertisementsAppService jobAdvertisementsAppService)
        {
            _jobAdvertisementsAppService = jobAdvertisementsAppService;

        }

        public ActionResult Index()
        {
            var model = new JobAdvertisementsViewModel
            {
                FilterText = ""
            };

            return View(model);
        }

        [AbpMvcAuthorize(AppPermissions.Pages_JobAdvertisements_Create, AppPermissions.Pages_JobAdvertisements_Edit)]
        public async Task<PartialViewResult> CreateOrEditModal(int? id)
        {
            GetJobAdvertisementForEditOutput getJobAdvertisementForEditOutput;

            if (id.HasValue)
            {
                getJobAdvertisementForEditOutput = await _jobAdvertisementsAppService.GetJobAdvertisementForEdit(new EntityDto { Id = (int)id });
            }
            else
            {
                getJobAdvertisementForEditOutput = new GetJobAdvertisementForEditOutput
                {
                    JobAdvertisement = new CreateOrEditJobAdvertisementDto()
                };
                getJobAdvertisementForEditOutput.JobAdvertisement.AdvertisementDate = DateTime.Now;
                getJobAdvertisementForEditOutput.JobAdvertisement.FromDate = DateTime.Now;
                getJobAdvertisementForEditOutput.JobAdvertisement.ToDate = DateTime.Now;
            }

            var viewModel = new CreateOrEditJobAdvertisementViewModel()
            {
                JobAdvertisement = getJobAdvertisementForEditOutput.JobAdvertisement,

            };

            return PartialView("_CreateOrEditModal", viewModel);
        }

        public async Task<PartialViewResult> ViewJobAdvertisementModal(int id)
        {
            var getJobAdvertisementForViewDto = await _jobAdvertisementsAppService.GetJobAdvertisementForView(id);

            var model = new JobAdvertisementViewModel()
            {
                JobAdvertisement = getJobAdvertisementForViewDto.JobAdvertisement
            };

            return PartialView("_ViewJobAdvertisementModal", model);
        }

    }
}