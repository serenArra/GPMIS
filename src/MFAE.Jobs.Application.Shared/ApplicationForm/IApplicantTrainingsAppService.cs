using System;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using MFAE.Jobs.ApplicationForm.Dtos;
using MFAE.Jobs.Dto;
using System.Collections.Generic;

namespace MFAE.Jobs.ApplicationForm
{
    public interface IApplicantTrainingsAppService : IApplicationService
    {
        Task<PagedResultDto<GetApplicantTrainingForViewDto>> GetAll(GetAllApplicantTrainingsInput input);

        Task<GetApplicantTrainingForViewDto> GetApplicantTrainingForView(long id);

        Task<GetApplicantTrainingForEditOutput> GetApplicantTrainingForEdit(EntityDto<long> input);

        Task CreateOrEdit(CreateOrEditApplicantTrainingDto input);

        Task Delete(EntityDto<long> input);

        Task<FileDto> GetApplicantTrainingsToExcel(GetAllApplicantTrainingsForExcelInput input);

        Task<List<ApplicantTrainingApplicantLookupTableDto>> GetAllApplicantForTableDropdown();

    }
}