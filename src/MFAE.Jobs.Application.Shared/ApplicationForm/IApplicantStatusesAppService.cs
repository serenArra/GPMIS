using System;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using MFAE.Jobs.ApplicationForm.Dtos;
using MFAE.Jobs.Dto;
using System.Collections.Generic;

namespace MFAE.Jobs.ApplicationForm
{
    public interface IApplicantStatusesAppService : IApplicationService
    {
        Task<PagedResultDto<GetApplicantStatusForViewDto>> GetAll(GetAllApplicantStatusesInput input);

        Task<GetApplicantStatusForViewDto> GetApplicantStatusForView(long id);

        Task<GetApplicantStatusForEditOutput> GetApplicantStatusForEdit(EntityDto<long> input);

        Task CreateOrEdit(CreateOrEditApplicantStatusDto input);

        Task Delete(EntityDto<long> input);

        Task<FileDto> GetApplicantStatusesToExcel(GetAllApplicantStatusesForExcelInput input);

        Task<List<ApplicantStatusApplicantLookupTableDto>> GetAllApplicantForTableDropdown();

    }
}