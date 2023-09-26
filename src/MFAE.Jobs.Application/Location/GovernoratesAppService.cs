using MFAE.Jobs.Location;

using System;
using System.Linq;
using System.Linq.Dynamic.Core;
using Abp.Linq.Extensions;
using System.Collections.Generic;
using System.Threading.Tasks;
using Abp.Domain.Repositories;
using MFAE.Jobs.Location.Exporting;
using MFAE.Jobs.Location.Dtos;
using MFAE.Jobs.Dto;
using Abp.Application.Services.Dto;
using MFAE.Jobs.Authorization;
using Abp.Extensions;
using Abp.Authorization;
using Microsoft.EntityFrameworkCore;
using Abp.UI;
using MFAE.Jobs.Storage;

namespace MFAE.Jobs.Location
{
    [AbpAuthorize(AppPermissions.Pages_Governorates)]
    public class GovernoratesAppService : JobsAppServiceBase, IGovernoratesAppService
    {
        private readonly IRepository<Governorate> _governorateRepository;
        private readonly IGovernoratesExcelExporter _governoratesExcelExporter;
        private readonly IRepository<Country, int> _lookup_countryRepository;

        public GovernoratesAppService(IRepository<Governorate> governorateRepository, IGovernoratesExcelExporter governoratesExcelExporter, IRepository<Country, int> lookup_countryRepository)
        {
            _governorateRepository = governorateRepository;
            _governoratesExcelExporter = governoratesExcelExporter;
            _lookup_countryRepository = lookup_countryRepository;

        }

        public async Task<PagedResultDto<GetGovernorateForViewDto>> GetAll(GetAllGovernoratesInput input)
        {

            var filteredGovernorates = _governorateRepository.GetAll()
                        .Include(e => e.CountryFk)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false || e.NameAr.Contains(input.Filter) || e.NameEn.Contains(input.Filter) || e.UniversalCode.Contains(input.Filter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.NameArFilter), e => e.NameAr.Contains(input.NameArFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.NameEnFilter), e => e.NameEn.Contains(input.NameEnFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.UniversalCodeFilter), e => e.UniversalCode.Contains(input.UniversalCodeFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.CountryNameFilter), e => e.CountryFk != null && e.CountryFk.Name == input.CountryNameFilter);

            var pagedAndFilteredGovernorates = filteredGovernorates
                .OrderBy(input.Sorting ?? "id asc")
                .PageBy(input);

            var governorates = from o in pagedAndFilteredGovernorates
                               join o1 in _lookup_countryRepository.GetAll() on o.CountryId equals o1.Id into j1
                               from s1 in j1.DefaultIfEmpty()

                               select new
                               {

                                   o.NameAr,
                                   o.NameEn,
                                   o.UniversalCode,
                                   Id = o.Id,
                                   CountryName = s1 == null || s1.Name == null ? "" : s1.Name.ToString()
                               };

            var totalCount = await filteredGovernorates.CountAsync();

            var dbList = await governorates.ToListAsync();
            var results = new List<GetGovernorateForViewDto>();

            foreach (var o in dbList)
            {
                var res = new GetGovernorateForViewDto()
                {
                    Governorate = new GovernorateDto
                    {

                        NameAr = o.NameAr,
                        NameEn = o.NameEn,
                        UniversalCode = o.UniversalCode,
                        Id = o.Id,
                    },
                    CountryName = o.CountryName
                };

                results.Add(res);
            }

            return new PagedResultDto<GetGovernorateForViewDto>(
                totalCount,
                results
            );

        }

        public async Task<GetGovernorateForViewDto> GetGovernorateForView(int id)
        {
            var governorate = await _governorateRepository.GetAsync(id);

            var output = new GetGovernorateForViewDto { Governorate = ObjectMapper.Map<GovernorateDto>(governorate) };

            if (output.Governorate.CountryId != null)
            {
                var _lookupCountry = await _lookup_countryRepository.FirstOrDefaultAsync((int)output.Governorate.CountryId);
                output.CountryName = _lookupCountry?.Name?.ToString();
            }

            return output;
        }

        [AbpAuthorize(AppPermissions.Pages_Governorates_Edit)]
        public async Task<GetGovernorateForEditOutput> GetGovernorateForEdit(EntityDto input)
        {
            var governorate = await _governorateRepository.FirstOrDefaultAsync(input.Id);

            var output = new GetGovernorateForEditOutput { Governorate = ObjectMapper.Map<CreateOrEditGovernorateDto>(governorate) };

            if (output.Governorate.CountryId != null)
            {
                var _lookupCountry = await _lookup_countryRepository.FirstOrDefaultAsync((int)output.Governorate.CountryId);
                output.CountryName = _lookupCountry?.Name?.ToString();
            }

            return output;
        }

        public async Task CreateOrEdit(CreateOrEditGovernorateDto input)
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

        [AbpAuthorize(AppPermissions.Pages_Governorates_Create)]
        protected virtual async Task Create(CreateOrEditGovernorateDto input)
        {
            var governorate = ObjectMapper.Map<Governorate>(input);

            await _governorateRepository.InsertAsync(governorate);

        }

        [AbpAuthorize(AppPermissions.Pages_Governorates_Edit)]
        protected virtual async Task Update(CreateOrEditGovernorateDto input)
        {
            var governorate = await _governorateRepository.FirstOrDefaultAsync((int)input.Id);
            ObjectMapper.Map(input, governorate);

        }

        [AbpAuthorize(AppPermissions.Pages_Governorates_Delete)]
        public async Task Delete(EntityDto input)
        {
            await _governorateRepository.DeleteAsync(input.Id);
        }

        public async Task<FileDto> GetGovernoratesToExcel(GetAllGovernoratesForExcelInput input)
        {

            var filteredGovernorates = _governorateRepository.GetAll()
                        .Include(e => e.CountryFk)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false || e.NameAr.Contains(input.Filter) || e.NameEn.Contains(input.Filter) || e.UniversalCode.Contains(input.Filter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.NameArFilter), e => e.NameAr.Contains(input.NameArFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.NameEnFilter), e => e.NameEn.Contains(input.NameEnFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.UniversalCodeFilter), e => e.UniversalCode.Contains(input.UniversalCodeFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.CountryNameFilter), e => e.CountryFk != null && e.CountryFk.Name == input.CountryNameFilter);

            var query = (from o in filteredGovernorates
                         join o1 in _lookup_countryRepository.GetAll() on o.CountryId equals o1.Id into j1
                         from s1 in j1.DefaultIfEmpty()

                         select new GetGovernorateForViewDto()
                         {
                             Governorate = new GovernorateDto
                             {
                                 NameAr = o.NameAr,
                                 NameEn = o.NameEn,
                                 UniversalCode = o.UniversalCode,
                                 Id = o.Id
                             },
                             CountryName = s1 == null || s1.Name == null ? "" : s1.Name.ToString()
                         });

            var governorateListDtos = await query.ToListAsync();

            return _governoratesExcelExporter.ExportToFile(governorateListDtos);
        }

        [AbpAuthorize(AppPermissions.Pages_Governorates)]
        public async Task<List<GovernorateCountryLookupTableDto>> GetAllCountryForTableDropdown()
        {
            return await _lookup_countryRepository.GetAll()
                .Select(country => new GovernorateCountryLookupTableDto
                {
                    Id = country.Id,
                    DisplayName = country == null || country.Name == null ? "" : country.Name.ToString()
                }).ToListAsync();
        }

    }
}