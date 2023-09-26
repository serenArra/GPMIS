using System;
using System.Threading.Tasks;
using Abp.AspNetCore.Mvc.Authorization;
using Microsoft.AspNetCore.Mvc;
using MFAE.Jobs.Web.Areas.App.Models.XRoadServices;
using MFAE.Jobs.Web.Controllers;
using MFAE.Jobs.Authorization;
using MFAE.Jobs.XRoad;
using MFAE.Jobs.XRoad.Dtos;
using Abp.Application.Services.Dto;
using Abp.Extensions;

namespace MFAE.Jobs.Web.Areas.App.Controllers
{
    [Area("App")]
    [AbpMvcAuthorize(AppPermissions.Pages_XRoadServices)]
    public class XRoadServicesController : JobsControllerBase
    {
        private readonly IXRoadServicesAppService _xRoadServicesAppService;

        public XRoadServicesController(IXRoadServicesAppService xRoadServicesAppService)
        {
            _xRoadServicesAppService = xRoadServicesAppService;

        }

        public ActionResult Index()
        {
            var model = new XRoadServicesViewModel
            {
                FilterText = ""
            };

            return View(model);
        }

        [AbpMvcAuthorize(AppPermissions.Pages_XRoadServices_Create, AppPermissions.Pages_XRoadServices_Edit)]
        public async Task<ActionResult> CreateOrEdit(int? id)
        {
            GetXRoadServiceForEditOutput getXRoadServiceForEditOutput;

            if (id.HasValue)
            {
                getXRoadServiceForEditOutput = await _xRoadServicesAppService.GetXRoadServiceForEdit(new EntityDto { Id = (int)id });
            }
            else
            {
                getXRoadServiceForEditOutput = new GetXRoadServiceForEditOutput
                {
                    XRoadService = new CreateOrEditXRoadServiceDto()
                };
                getXRoadServiceForEditOutput.XRoadService.Status = XRoadServiceStatusEnum.Active;
            }

            var viewModel = new CreateOrEditXRoadServiceModalViewModel()
            {
                XRoadService = getXRoadServiceForEditOutput.XRoadService,
            };

            return View("CreateOrEdit", viewModel);
        }

        [AbpMvcAuthorize(AppPermissions.Pages_XRoadServices_Create, AppPermissions.Pages_XRoadServices_Edit)]
        public async Task<PartialViewResult> CreateOrEditModal(int? id)
        {
            GetXRoadServiceForEditOutput getXRoadServiceForEditOutput;

            if (id.HasValue)
            {
                getXRoadServiceForEditOutput = await _xRoadServicesAppService.GetXRoadServiceForEdit(new EntityDto { Id = (int)id });
            }
            else
            {
                getXRoadServiceForEditOutput = new GetXRoadServiceForEditOutput
                {
                    XRoadService = new CreateOrEditXRoadServiceDto()
                };
            }

            var viewModel = new CreateOrEditXRoadServiceModalViewModel()
            {
                XRoadService = getXRoadServiceForEditOutput.XRoadService,
            };

            return PartialView("_CreateOrEditModal", viewModel);
        }

        public async Task<PartialViewResult> ViewXRoadServiceModal(int id)
        {
            var getXRoadServiceForViewDto = await _xRoadServicesAppService.GetXRoadServiceForView(id);

            var model = new XRoadServiceViewModel()
            {
                XRoadService = getXRoadServiceForViewDto.XRoadService
            };

            return PartialView("_ViewXRoadServiceModal", model);
        }

        public async Task<ActionResult> ViewInformationBankPage()
        {

            var model = new ViewInformationBankPageViewModel()
            {
                IdentificationTypeList = await _xRoadServicesAppService.GetAllIdentificationTypeForTableDropdown()
            };

            return View("ViewInformationBankPage", model);
        }

        public async Task<ActionResult> CitizenshipVerification()
        {

            var model = new ViewInformationBankPageViewModel()
            {
                IdentificationTypeList = await _xRoadServicesAppService.GetAllIdentificationTypeForTableDropdown()
            };

            return View("CitizenshipVerification", model);
        }

        public async Task<ActionResult> ViewInformationBank(int identificationTypeId, string identificationDocumentNo)
        {
            var informationBankViewDto = await _xRoadServicesAppService.GetInformationBankForView(identificationTypeId, identificationDocumentNo);

            var model = new InformationBankViewModel()
            {
                IdentificationDocumentNo = identificationDocumentNo,
                IdentificationTypeId = identificationTypeId,
                CitizensListWithCodes = informationBankViewDto.CitizensListWithCodes,
                PassportInfo = informationBankViewDto.PassportInfo,
                IsCitizensListWithCodesEnabled = informationBankViewDto.IsCitizensListWithCodesEnabled,
                IsPassportInfoEnabled = informationBankViewDto.IsPassportInfoEnabled,
                IsCitizenPictureEnabled = informationBankViewDto.IsCitizenPictureEnabled
            };

            return PartialView("_ViewInformationBank", model);
        }
        public async Task<ActionResult> ViewInformationBankCitizensListWithCodes(int identificationTypeId, string identificationDocumentNo)
        {
            var model = await _xRoadServicesAppService.GetInformationBankForViewCitizensListWithCodes(identificationTypeId, identificationDocumentNo);

            return PartialView("_CitizensListWithCodes", model);
        }

        public async Task<ActionResult> ViewInformationBankPassportInfo(int identificationTypeId, string identificationDocumentNo)
        {
            var model = await _xRoadServicesAppService.GetInformationBankForViewPassportInfo(identificationTypeId, identificationDocumentNo);

            return PartialView("_PassportInfo", model);
        }
        public async Task<ActionResult> ViewInformationCitizenPicture(int identificationTypeId, string identificationDocumentNo)
        {
            var model = await _xRoadServicesAppService.GetInformationCitizenPhoto(identificationTypeId, identificationDocumentNo);

            return PartialView("_CitizenPicture", model);
        }
    }
}