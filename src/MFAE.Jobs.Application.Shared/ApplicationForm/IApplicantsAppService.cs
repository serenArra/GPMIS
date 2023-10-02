using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using MFAE.Jobs.ApplicationForm.Dtos;
using MFAE.Jobs.Dto;
using System.Collections.Generic;

namespace MFAE.Jobs.ApplicationForm
{
    public interface IApplicantsAppService : IApplicationService
    {
        Task<PagedResultDto<GetApplicantForViewDto>> GetAll(GetAllApplicantsInput input);

        Task<GetApplicantForViewDto> GetApplicantForView(long id);

        Task<GetApplicantForEditOutput> GetApplicantForEdit(EntityDto<long> input);

        Task<long> CreateOrEdit(CreateOrEditApplicantDto input);

        Task Delete(EntityDto<long> input);

        Task<FileDto> GetApplicantsToExcel(GetAllApplicantsForExcelInput input);

        Task<List<ApplicantIdentificationTypeLookupTableDto>> GetAllIdentificationTypeForTableDropdown();

        Task<List<ApplicantMaritalStatusLookupTableDto>> GetAllMaritalStatusForTableDropdown();

        Task<List<ApplicantUserLookupTableDto>> GetAllUserForTableDropdown();

        Task<List<ApplicantApplicantStatusLookupTableDto>> GetAllApplicantStatusForTableDropdown();

        Task<List<ApplicantCountryLookupTableDto>> GetAllCountryForTableDropdown();

        Task<List<ApplicantGovernorateLookupTableDto>> GetAllGovernorateForTableDropdown();

        Task<List<ApplicantLocalityLookupTableDto>> GetAllLocalityForTableDropdown();

    }
}