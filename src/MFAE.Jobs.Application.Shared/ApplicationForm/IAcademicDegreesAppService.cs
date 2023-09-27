using System;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using MFAE.Jobs.ApplicationForm.Dtos;
using MFAE.Jobs.Dto;

namespace MFAE.Jobs.ApplicationForm
{
    public interface IAcademicDegreesAppService : IApplicationService
    {
        Task<PagedResultDto<GetAcademicDegreeForViewDto>> GetAll(GetAllAcademicDegreesInput input);

        Task<GetAcademicDegreeForViewDto> GetAcademicDegreeForView(int id);

        Task<GetAcademicDegreeForEditOutput> GetAcademicDegreeForEdit(EntityDto input);

        Task CreateOrEdit(CreateOrEditAcademicDegreeDto input);

        Task Delete(EntityDto input);

        Task<FileDto> GetAcademicDegreesToExcel(GetAllAcademicDegreesForExcelInput input);

    }
}