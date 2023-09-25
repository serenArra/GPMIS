using System;
using System.Linq;
using System.Linq.Dynamic.Core;
using Abp.Linq.Extensions;
using System.Collections.Generic;
using System.Threading.Tasks;
using Abp.Domain.Repositories;
using MFAE.Jobs.ApplicationForm.Exporting;
using MFAE.Jobs.ApplicationForm.Dtos;
using MFAE.Jobs.Dto;
using Abp.Application.Services.Dto;
using MFAE.Jobs.Authorization;
using Abp.Extensions;
using Abp.Authorization;
using Microsoft.EntityFrameworkCore;
using Abp.UI;
using MFAE.Jobs.Storage;

namespace MFAE.Jobs.ApplicationForm
{
    [AbpAuthorize(AppPermissions.Pages_AcademicDegrees)]
    public class AcademicDegreesAppService : JobsAppServiceBase, IAcademicDegreesAppService
    {
        private readonly IRepository<AcademicDegree> _academicDegreeRepository;
        private readonly IAcademicDegreesExcelExporter _academicDegreesExcelExporter;

        public AcademicDegreesAppService(IRepository<AcademicDegree> academicDegreeRepository, IAcademicDegreesExcelExporter academicDegreesExcelExporter)
        {
            _academicDegreeRepository = academicDegreeRepository;
            _academicDegreesExcelExporter = academicDegreesExcelExporter;

        }

        public async Task<PagedResultDto<GetAcademicDegreeForViewDto>> GetAll(GetAllAcademicDegreesInput input)
        {

            var filteredAcademicDegrees = _academicDegreeRepository.GetAll()
                        .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false || e.NameAr.Contains(input.Filter) || e.NameEn.Contains(input.Filter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.NameArFilter), e => e.NameAr.Contains(input.NameArFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.NameEnFilter), e => e.NameEn.Contains(input.NameEnFilter))
                        .WhereIf(input.IsActiveFilter.HasValue && input.IsActiveFilter > -1, e => (input.IsActiveFilter == 1 && e.IsActive) || (input.IsActiveFilter == 0 && !e.IsActive));

            var pagedAndFilteredAcademicDegrees = filteredAcademicDegrees
                .OrderBy(input.Sorting ?? "id asc")
                .PageBy(input);

            var academicDegrees = from o in pagedAndFilteredAcademicDegrees
                                  select new
                                  {

                                      o.NameAr,
                                      o.NameEn,
                                      o.IsActive,
                                      Id = o.Id
                                  };

            var totalCount = await filteredAcademicDegrees.CountAsync();

            var dbList = await academicDegrees.ToListAsync();
            var results = new List<GetAcademicDegreeForViewDto>();

            foreach (var o in dbList)
            {
                var res = new GetAcademicDegreeForViewDto()
                {
                    AcademicDegree = new AcademicDegreeDto
                    {

                        NameAr = o.NameAr,
                        NameEn = o.NameEn,
                        IsActive = o.IsActive,
                        Id = o.Id,
                    }
                };

                results.Add(res);
            }

            return new PagedResultDto<GetAcademicDegreeForViewDto>(
                totalCount,
                results
            );

        }

        public async Task<GetAcademicDegreeForViewDto> GetAcademicDegreeForView(int id)
        {
            var academicDegree = await _academicDegreeRepository.GetAsync(id);

            var output = new GetAcademicDegreeForViewDto { AcademicDegree = ObjectMapper.Map<AcademicDegreeDto>(academicDegree) };

            return output;
        }

        [AbpAuthorize(AppPermissions.Pages_AcademicDegrees_Edit)]
        public async Task<GetAcademicDegreeForEditOutput> GetAcademicDegreeForEdit(EntityDto input)
        {
            var academicDegree = await _academicDegreeRepository.FirstOrDefaultAsync(input.Id);

            var output = new GetAcademicDegreeForEditOutput { AcademicDegree = ObjectMapper.Map<CreateOrEditAcademicDegreeDto>(academicDegree) };

            return output;
        }

        public async Task CreateOrEdit(CreateOrEditAcademicDegreeDto input)
        {
            if (input.Id == null)
            {
                await Create(input);
            }
            else
            {
                await Update(input);
            }
        }

        [AbpAuthorize(AppPermissions.Pages_AcademicDegrees_Create)]
        protected virtual async Task Create(CreateOrEditAcademicDegreeDto input)
        {
            var academicDegree = ObjectMapper.Map<AcademicDegree>(input);

            await _academicDegreeRepository.InsertAsync(academicDegree);

        }

        [AbpAuthorize(AppPermissions.Pages_AcademicDegrees_Edit)]
        protected virtual async Task Update(CreateOrEditAcademicDegreeDto input)
        {
            var academicDegree = await _academicDegreeRepository.FirstOrDefaultAsync((int)input.Id);
            ObjectMapper.Map(input, academicDegree);

        }

        [AbpAuthorize(AppPermissions.Pages_AcademicDegrees_Delete)]
        public async Task Delete(EntityDto input)
        {
            await _academicDegreeRepository.DeleteAsync(input.Id);
        }

        public async Task<FileDto> GetAcademicDegreesToExcel(GetAllAcademicDegreesForExcelInput input)
        {

            var filteredAcademicDegrees = _academicDegreeRepository.GetAll()
                        .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false || e.NameAr.Contains(input.Filter) || e.NameEn.Contains(input.Filter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.NameArFilter), e => e.NameAr.Contains(input.NameArFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.NameEnFilter), e => e.NameEn.Contains(input.NameEnFilter))
                        .WhereIf(input.IsActiveFilter.HasValue && input.IsActiveFilter > -1, e => (input.IsActiveFilter == 1 && e.IsActive) || (input.IsActiveFilter == 0 && !e.IsActive));

            var query = (from o in filteredAcademicDegrees
                         select new GetAcademicDegreeForViewDto()
                         {
                             AcademicDegree = new AcademicDegreeDto
                             {
                                 NameAr = o.NameAr,
                                 NameEn = o.NameEn,
                                 IsActive = o.IsActive,
                                 Id = o.Id
                             }
                         });

            var academicDegreeListDtos = await query.ToListAsync();

            return _academicDegreesExcelExporter.ExportToFile(academicDegreeListDtos);
        }

    }
}