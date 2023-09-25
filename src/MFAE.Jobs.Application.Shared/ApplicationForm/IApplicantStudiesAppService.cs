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
    public interface IApplicantStudiesAppService : IApplicationService
    {
        Task<PagedResultDto<GetApplicantStudyForViewDto>> GetAll(GetAllApplicantStudiesInput input);

        Task<GetApplicantStudyForViewDto> GetApplicantStudyForView(long id);

        Task<GetApplicantStudyForEditOutput> GetApplicantStudyForEdit(EntityDto<long> input);

        Task CreateOrEdit(CreateOrEditApplicantStudyDto input);

        Task Delete(EntityDto<long> input);

        Task<FileDto> GetApplicantStudiesToExcel(GetAllApplicantStudiesForExcelInput input);

        Task<List<ApplicantStudyGraduationRateLookupTableDto>> GetAllGraduationRateForTableDropdown();

        Task<List<ApplicantStudyAcademicDegreeLookupTableDto>> GetAllAcademicDegreeForTableDropdown();

        Task<List<ApplicantStudySpecialtiesLookupTableDto>> GetAllSpecialtiesForTableDropdown();

        Task<List<ApplicantStudyApplicantLookupTableDto>> GetAllApplicantForTableDropdown();

    }
}