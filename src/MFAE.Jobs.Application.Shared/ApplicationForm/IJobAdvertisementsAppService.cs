using System;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using MFAE.Jobs.ApplicationForm.Dtos;
using MFAE.Jobs.Dto;

namespace MFAE.Jobs.ApplicationForm
{
    public interface IJobAdvertisementsAppService : IApplicationService
    {
        Task<PagedResultDto<GetJobAdvertisementForViewDto>> GetAll(GetAllJobAdvertisementsInput input);

        Task<GetJobAdvertisementForViewDto> GetJobAdvertisementForView(int id);

        Task<GetJobAdvertisementForEditOutput> GetJobAdvertisementForEdit(EntityDto input);

        Task CreateOrEdit(CreateOrEditJobAdvertisementDto input);

        Task Delete(EntityDto input);

        Task<FileDto> GetJobAdvertisementsToExcel(GetAllJobAdvertisementsForExcelInput input);

    }
}