using System;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using MFAE.Jobs.ApplicationForm.Dtos;
using MFAE.Jobs.Dto;

namespace MFAE.Jobs.ApplicationForm
{
    public interface IMaritalStatusesAppService : IApplicationService
    {
        Task<PagedResultDto<GetMaritalStatusForViewDto>> GetAll(GetAllMaritalStatusesInput input);

        Task<GetMaritalStatusForViewDto> GetMaritalStatusForView(int id);

        Task<GetMaritalStatusForEditOutput> GetMaritalStatusForEdit(EntityDto input);

        Task CreateOrEdit(CreateOrEditMaritalStatusDto input);

        Task Delete(EntityDto input);

        Task<FileDto> GetMaritalStatusesToExcel(GetAllMaritalStatusesForExcelInput input);

    }
}