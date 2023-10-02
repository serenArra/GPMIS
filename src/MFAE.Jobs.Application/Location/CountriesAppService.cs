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
    [AbpAuthorize(AppPermissions.Pages_Countries)]
    public class CountriesAppService : JobsAppServiceBase, ICountriesAppService
    {
        private readonly IRepository<Country> _countryRepository;
        private readonly ICountriesExcelExporter _countriesExcelExporter;

        public CountriesAppService(IRepository<Country> countryRepository, ICountriesExcelExporter countriesExcelExporter)
        {
            _countryRepository = countryRepository;
            _countriesExcelExporter = countriesExcelExporter;

        }

        public async Task<PagedResultDto<GetCountryForViewDto>> GetAll(GetAllCountriesInput input)
        {

            var filteredCountries = _countryRepository.GetAll()
                        .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false || e.IsoNumeric.Contains(input.Filter) || e.IsoAlpha.Contains(input.Filter) || e.NameAr.Contains(input.Filter) || e.NameEn.Contains(input.Filter) || e.UniversalCode.Contains(input.Filter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.IsoNumericFilter), e => e.IsoNumeric.Contains(input.IsoNumericFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.IsoAlphaFilter), e => e.IsoAlpha.Contains(input.IsoAlphaFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.NameArFilter), e => e.NameAr.Contains(input.NameArFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.NameEnFilter), e => e.NameEn.Contains(input.NameEnFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.UniversalCodeFilter), e => e.UniversalCode.Contains(input.UniversalCodeFilter));

            var pagedAndFilteredCountries = filteredCountries
                .OrderBy(input.Sorting ?? "id asc")
                .PageBy(input);

            var countries = from o in pagedAndFilteredCountries
                            select new
                            {

                                o.IsoNumeric,
                                o.IsoAlpha,
                                o.NameAr,
                                o.NameEn,
                                o.Name,
                                o.UniversalCode,
                                Id = o.Id
                            };

            var totalCount = await filteredCountries.CountAsync();

            var dbList = await countries.ToListAsync();
            var results = new List<GetCountryForViewDto>();

            foreach (var o in dbList)
            {
                var res = new GetCountryForViewDto()
                {
                    Country = new CountryDto
                    {

                        IsoNumeric = o.IsoNumeric,
                        IsoAlpha = o.IsoAlpha,
                        NameAr = o.NameAr,
                        NameEn = o.NameEn,
                        Name = o.Name,
                        UniversalCode = o.UniversalCode,
                        Id = o.Id,
                    }
                };

                results.Add(res);
            }

            return new PagedResultDto<GetCountryForViewDto>(
                totalCount,
                results
            );

        }

        public async Task<GetCountryForViewDto> GetCountryForView(int id)
        {
            var country = await _countryRepository.GetAsync(id);

            var output = new GetCountryForViewDto { Country = ObjectMapper.Map<CountryDto>(country) };

            return output;
        }

        [AbpAuthorize(AppPermissions.Pages_Countries_Edit)]
        public async Task<GetCountryForEditOutput> GetCountryForEdit(EntityDto input)
        {
            var country = await _countryRepository.FirstOrDefaultAsync(input.Id);

            var output = new GetCountryForEditOutput { Country = ObjectMapper.Map<CreateOrEditCountryDto>(country) };

            return output;
        }

        public async Task CreateOrEdit(CreateOrEditCountryDto input)
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

        [AbpAuthorize(AppPermissions.Pages_Countries_Create)]
        protected virtual async Task Create(CreateOrEditCountryDto input)
        {
            var country = ObjectMapper.Map<Country>(input);

            await _countryRepository.InsertAsync(country);

        }

        [AbpAuthorize(AppPermissions.Pages_Countries_Edit)]
        protected virtual async Task Update(CreateOrEditCountryDto input)
        {
            var country = await _countryRepository.FirstOrDefaultAsync((int)input.Id);
            ObjectMapper.Map(input, country);

        }

        [AbpAuthorize(AppPermissions.Pages_Countries_Delete)]
        public async Task Delete(EntityDto input)
        {
            await _countryRepository.DeleteAsync(input.Id);
        }

        public async Task<FileDto> GetCountriesToExcel(GetAllCountriesForExcelInput input)
        {

            var filteredCountries = _countryRepository.GetAll()
                        .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false || e.IsoNumeric.Contains(input.Filter) || e.IsoAlpha.Contains(input.Filter) || e.NameAr.Contains(input.Filter) || e.NameEn.Contains(input.Filter) || e.UniversalCode.Contains(input.Filter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.IsoNumericFilter), e => e.IsoNumeric.Contains(input.IsoNumericFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.IsoAlphaFilter), e => e.IsoAlpha.Contains(input.IsoAlphaFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.NameArFilter), e => e.NameAr.Contains(input.NameArFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.NameEnFilter), e => e.NameEn.Contains(input.NameEnFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.UniversalCodeFilter), e => e.UniversalCode.Contains(input.UniversalCodeFilter));

            var query = (from o in filteredCountries
                         select new GetCountryForViewDto()
                         {
                             Country = new CountryDto
                             {
                                 IsoNumeric = o.IsoNumeric,
                                 IsoAlpha = o.IsoAlpha,
                                 NameAr = o.NameAr,
                                 NameEn = o.NameEn,
                                 UniversalCode = o.UniversalCode,
                                 Id = o.Id
                             }
                         });

            var countryListDtos = await query.ToListAsync();

            return _countriesExcelExporter.ExportToFile(countryListDtos);
        }

    }
}