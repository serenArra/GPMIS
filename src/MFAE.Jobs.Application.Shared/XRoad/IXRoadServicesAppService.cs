using System;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using MFAE.Jobs.XRoad.Dtos;
using MFAE.Jobs.Dto;
using System.Collections.Generic;
using System.Dynamic;
using MFAE.Jobs.ApplicationForm.Dtos;

namespace MFAE.Jobs.XRoad
{
    public interface IXRoadServicesAppService : IApplicationService
    {
        Task<PagedResultDto<GetXRoadServiceForViewDto>> GetAll(GetAllXRoadServicesInput input);

        Task<GetXRoadServiceForViewDto> GetXRoadServiceForView(int id);
        Task<List<IdentificationTypeLookupTableDto>> GetAllIdentificationTypeForTableDropdown();
        Task<InformationBankViewDto> GetInformationBankForView(int identificationTypeId, string identificationDocumentNo);
        Task<CitizensListWithCodesDto> GetInformationBankForViewCitizensListWithCodes(int identificationTypeId, string identificationDocumentNo);

        Task<PassportInfoDto> GetInformationBankForViewPassportInfo(int identificationTypeId, string identificationDocumentNo);

        Task<CitizenPhotoDto> GetInformationCitizenPhoto(int identificationTypeId, string identificationDocumentNo);

        Task<GetXRoadServiceForEditOutput> GetXRoadServiceForEdit(EntityDto input);

        Task<CreateOrEditXRoadServiceDto> CreateOrEdit(CreateOrEditXRoadServiceDto input);

        Task Delete(EntityDto input);

        Task<FileDto> GetXRoadServicesToExcel(GetAllXRoadServicesForExcelInput input);

        Task<ExpandoObject> GetDynamic(string serviceCode, string requestXML);

    }
}