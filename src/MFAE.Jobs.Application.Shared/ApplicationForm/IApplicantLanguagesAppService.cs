using System;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using MFAE.Jobs.ApplicationForm.Dtos;
using MFAE.Jobs.Dto;
using System.Collections.Generic;
using System.Collections.Generic;
using System.Collections.Generic;
using System.Collections.Generic;

namespace MFAE.Jobs.ApplicationForm
{
    public interface IApplicantLanguagesAppService : IApplicationService
    {
        Task<PagedResultDto<GetApplicantLanguageForViewDto>> GetAll(GetAllApplicantLanguagesInput input);

        Task<GetApplicantLanguageForViewDto> GetApplicantLanguageForView(long id);

        Task<GetApplicantLanguageForEditOutput> GetApplicantLanguageForEdit(EntityDto<long> input);

        Task CreateOrEdit(CreateOrEditApplicantLanguageDto input);

        Task Delete(EntityDto<long> input);

        Task<FileDto> GetApplicantLanguagesToExcel(GetAllApplicantLanguagesForExcelInput input);

        Task<List<ApplicantLanguageApplicantLookupTableDto>> GetAllApplicantForTableDropdown();

        Task<List<ApplicantLanguageLanguageLookupTableDto>> GetAllLanguageForTableDropdown();

        Task<List<ApplicantLanguageConversationLookupTableDto>> GetAllConversationForTableDropdown();

        Task<List<ApplicantLanguageConversationRateLookupTableDto>> GetAllConversationRateForTableDropdown();

    }
}