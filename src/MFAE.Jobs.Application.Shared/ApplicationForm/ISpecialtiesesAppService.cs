using System;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using MFAE.Jobs.ApplicationForm.Dtos;
using MFAE.Jobs.Dto;

namespace MFAE.Jobs.ApplicationForm
{
    public interface ISpecialtiesesAppService : IApplicationService
    {
        Task<PagedResultDto<GetSpecialtiesForViewDto>> GetAll(GetAllSpecialtiesesInput input);

        Task<GetSpecialtiesForViewDto> GetSpecialtiesForView(int id);

        Task<GetSpecialtiesForEditOutput> GetSpecialtiesForEdit(EntityDto input);

        Task CreateOrEdit(CreateOrEditSpecialtiesDto input);

        Task Delete(EntityDto input);

        Task<FileDto> GetSpecialtiesesToExcel(GetAllSpecialtiesesForExcelInput input);

    }
}