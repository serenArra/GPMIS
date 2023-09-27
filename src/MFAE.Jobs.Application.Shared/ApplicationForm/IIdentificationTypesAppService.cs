using System;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using MFAE.Jobs.ApplicationForm.Dtos;
using MFAE.Jobs.Dto;

namespace MFAE.Jobs.ApplicationForm
{
    public interface IIdentificationTypesAppService : IApplicationService
    {
        Task<PagedResultDto<GetIdentificationTypeForViewDto>> GetAll(GetAllIdentificationTypesInput input);

        Task<GetIdentificationTypeForViewDto> GetIdentificationTypeForView(int id);

        Task<GetIdentificationTypeForEditOutput> GetIdentificationTypeForEdit(EntityDto input);

        Task CreateOrEdit(CreateOrEditIdentificationTypeDto input);

        Task Delete(EntityDto input);

        Task<FileDto> GetIdentificationTypesToExcel(GetAllIdentificationTypesForExcelInput input);

    }
}