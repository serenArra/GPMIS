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
    [AbpAuthorize(AppPermissions.Pages_GraduationRates)]
    public class GraduationRatesAppService : JobsAppServiceBase, IGraduationRatesAppService
    {
        private readonly IRepository<GraduationRate> _graduationRateRepository;
        private readonly IGraduationRatesExcelExporter _graduationRatesExcelExporter;

        public GraduationRatesAppService(IRepository<GraduationRate> graduationRateRepository, IGraduationRatesExcelExporter graduationRatesExcelExporter)
        {
            _graduationRateRepository = graduationRateRepository;
            _graduationRatesExcelExporter = graduationRatesExcelExporter;

        }

        public async Task<PagedResultDto<GetGraduationRateForViewDto>> GetAll(GetAllGraduationRatesInput input)
        {

            var filteredGraduationRates = _graduationRateRepository.GetAll()
                        .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false || e.NameAr.Contains(input.Filter) || e.NameEn.Contains(input.Filter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.NameArFilter), e => e.NameAr.Contains(input.NameArFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.NameEnFilter), e => e.NameEn.Contains(input.NameEnFilter))
                        .WhereIf(input.IsActiveFilter.HasValue && input.IsActiveFilter > -1, e => (input.IsActiveFilter == 1 && e.IsActive) || (input.IsActiveFilter == 0 && !e.IsActive));

            var pagedAndFilteredGraduationRates = filteredGraduationRates
                .OrderBy(input.Sorting ?? "id asc")
                .PageBy(input);

            var graduationRates = from o in pagedAndFilteredGraduationRates
                                  select new
                                  {

                                      o.NameAr,
                                      o.NameEn,
                                      o.IsActive,
                                      Id = o.Id
                                  };

            var totalCount = await filteredGraduationRates.CountAsync();

            var dbList = await graduationRates.ToListAsync();
            var results = new List<GetGraduationRateForViewDto>();

            foreach (var o in dbList)
            {
                var res = new GetGraduationRateForViewDto()
                {
                    GraduationRate = new GraduationRateDto
                    {

                        NameAr = o.NameAr,
                        NameEn = o.NameEn,
                        IsActive = o.IsActive,
                        Id = o.Id,
                    }
                };

                results.Add(res);
            }

            return new PagedResultDto<GetGraduationRateForViewDto>(
                totalCount,
                results
            );

        }

        public async Task<GetGraduationRateForViewDto> GetGraduationRateForView(int id)
        {
            var graduationRate = await _graduationRateRepository.GetAsync(id);

            var output = new GetGraduationRateForViewDto { GraduationRate = ObjectMapper.Map<GraduationRateDto>(graduationRate) };

            return output;
        }

        [AbpAuthorize(AppPermissions.Pages_GraduationRates_Edit)]
        public async Task<GetGraduationRateForEditOutput> GetGraduationRateForEdit(EntityDto input)
        {
            var graduationRate = await _graduationRateRepository.FirstOrDefaultAsync(input.Id);

            var output = new GetGraduationRateForEditOutput { GraduationRate = ObjectMapper.Map<CreateOrEditGraduationRateDto>(graduationRate) };

            return output;
        }

        public async Task CreateOrEdit(CreateOrEditGraduationRateDto input)
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

        [AbpAuthorize(AppPermissions.Pages_GraduationRates_Create)]
        protected virtual async Task Create(CreateOrEditGraduationRateDto input)
        {
            var graduationRate = ObjectMapper.Map<GraduationRate>(input);

            await _graduationRateRepository.InsertAsync(graduationRate);

        }

        [AbpAuthorize(AppPermissions.Pages_GraduationRates_Edit)]
        protected virtual async Task Update(CreateOrEditGraduationRateDto input)
        {
            var graduationRate = await _graduationRateRepository.FirstOrDefaultAsync((int)input.Id);
            ObjectMapper.Map(input, graduationRate);

        }

        [AbpAuthorize(AppPermissions.Pages_GraduationRates_Delete)]
        public async Task Delete(EntityDto input)
        {
            await _graduationRateRepository.DeleteAsync(input.Id);
        }

        public async Task<FileDto> GetGraduationRatesToExcel(GetAllGraduationRatesForExcelInput input)
        {

            var filteredGraduationRates = _graduationRateRepository.GetAll()
                        .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false || e.NameAr.Contains(input.Filter) || e.NameEn.Contains(input.Filter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.NameArFilter), e => e.NameAr.Contains(input.NameArFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.NameEnFilter), e => e.NameEn.Contains(input.NameEnFilter))
                        .WhereIf(input.IsActiveFilter.HasValue && input.IsActiveFilter > -1, e => (input.IsActiveFilter == 1 && e.IsActive) || (input.IsActiveFilter == 0 && !e.IsActive));

            var query = (from o in filteredGraduationRates
                         select new GetGraduationRateForViewDto()
                         {
                             GraduationRate = new GraduationRateDto
                             {
                                 NameAr = o.NameAr,
                                 NameEn = o.NameEn,
                                 IsActive = o.IsActive,
                                 Id = o.Id
                             }
                         });

            var graduationRateListDtos = await query.ToListAsync();

            return _graduationRatesExcelExporter.ExportToFile(graduationRateListDtos);
        }

    }
}