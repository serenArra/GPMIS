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
    [AbpAuthorize(AppPermissions.Pages_Specialtieses)]
    public class SpecialtiesesAppService : JobsAppServiceBase, ISpecialtiesesAppService
    {
        private readonly IRepository<Specialties> _specialtiesRepository;
        private readonly ISpecialtiesesExcelExporter _specialtiesesExcelExporter;

        public SpecialtiesesAppService(IRepository<Specialties> specialtiesRepository, ISpecialtiesesExcelExporter specialtiesesExcelExporter)
        {
            _specialtiesRepository = specialtiesRepository;
            _specialtiesesExcelExporter = specialtiesesExcelExporter;

        }

        public async Task<PagedResultDto<GetSpecialtiesForViewDto>> GetAll(GetAllSpecialtiesesInput input)
        {

            var filteredSpecialtieses = _specialtiesRepository.GetAll()
                        .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false || e.NameAr.Contains(input.Filter) || e.NameEn.Contains(input.Filter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.NameArFilter), e => e.NameAr.Contains(input.NameArFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.NameEnFilter), e => e.NameEn.Contains(input.NameEnFilter))
                        .WhereIf(input.IsActiveFilter.HasValue && input.IsActiveFilter > -1, e => (input.IsActiveFilter == 1 && e.IsActive) || (input.IsActiveFilter == 0 && !e.IsActive));

            var pagedAndFilteredSpecialtieses = filteredSpecialtieses
                .OrderBy(input.Sorting ?? "id asc")
                .PageBy(input);

            var specialtieses = from o in pagedAndFilteredSpecialtieses
                                select new
                                {

                                    o.NameAr,
                                    o.NameEn,
                                    o.IsActive,
                                    Id = o.Id
                                };

            var totalCount = await filteredSpecialtieses.CountAsync();

            var dbList = await specialtieses.ToListAsync();
            var results = new List<GetSpecialtiesForViewDto>();

            foreach (var o in dbList)
            {
                var res = new GetSpecialtiesForViewDto()
                {
                    Specialties = new SpecialtiesDto
                    {

                        NameAr = o.NameAr,
                        NameEn = o.NameEn,
                        IsActive = o.IsActive,
                        Id = o.Id,
                    }
                };

                results.Add(res);
            }

            return new PagedResultDto<GetSpecialtiesForViewDto>(
                totalCount,
                results
            );

        }

        public async Task<GetSpecialtiesForViewDto> GetSpecialtiesForView(int id)
        {
            var specialties = await _specialtiesRepository.GetAsync(id);

            var output = new GetSpecialtiesForViewDto { Specialties = ObjectMapper.Map<SpecialtiesDto>(specialties) };

            return output;
        }

        [AbpAuthorize(AppPermissions.Pages_Specialtieses_Edit)]
        public async Task<GetSpecialtiesForEditOutput> GetSpecialtiesForEdit(EntityDto input)
        {
            var specialties = await _specialtiesRepository.FirstOrDefaultAsync(input.Id);

            var output = new GetSpecialtiesForEditOutput { Specialties = ObjectMapper.Map<CreateOrEditSpecialtiesDto>(specialties) };

            return output;
        }

        public async Task CreateOrEdit(CreateOrEditSpecialtiesDto input)
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

        [AbpAuthorize(AppPermissions.Pages_Specialtieses_Create)]
        protected virtual async Task Create(CreateOrEditSpecialtiesDto input)
        {
            var specialties = ObjectMapper.Map<Specialties>(input);

            await _specialtiesRepository.InsertAsync(specialties);

        }

        [AbpAuthorize(AppPermissions.Pages_Specialtieses_Edit)]
        protected virtual async Task Update(CreateOrEditSpecialtiesDto input)
        {
            var specialties = await _specialtiesRepository.FirstOrDefaultAsync((int)input.Id);
            ObjectMapper.Map(input, specialties);

        }

        [AbpAuthorize(AppPermissions.Pages_Specialtieses_Delete)]
        public async Task Delete(EntityDto input)
        {
            await _specialtiesRepository.DeleteAsync(input.Id);
        }

        public async Task<FileDto> GetSpecialtiesesToExcel(GetAllSpecialtiesesForExcelInput input)
        {

            var filteredSpecialtieses = _specialtiesRepository.GetAll()
                        .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false || e.NameAr.Contains(input.Filter) || e.NameEn.Contains(input.Filter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.NameArFilter), e => e.NameAr.Contains(input.NameArFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.NameEnFilter), e => e.NameEn.Contains(input.NameEnFilter))
                        .WhereIf(input.IsActiveFilter.HasValue && input.IsActiveFilter > -1, e => (input.IsActiveFilter == 1 && e.IsActive) || (input.IsActiveFilter == 0 && !e.IsActive));

            var query = (from o in filteredSpecialtieses
                         select new GetSpecialtiesForViewDto()
                         {
                             Specialties = new SpecialtiesDto
                             {
                                 NameAr = o.NameAr,
                                 NameEn = o.NameEn,
                                 IsActive = o.IsActive,
                                 Id = o.Id
                             }
                         });

            var specialtiesListDtos = await query.ToListAsync();

            return _specialtiesesExcelExporter.ExportToFile(specialtiesListDtos);
        }

    }
}