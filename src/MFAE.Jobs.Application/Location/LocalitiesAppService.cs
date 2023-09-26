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
    [AbpAuthorize(AppPermissions.Pages_Localities)]
    public class LocalitiesAppService : JobsAppServiceBase, ILocalitiesAppService
    {
        private readonly IRepository<Locality> _localityRepository;
        private readonly ILocalitiesExcelExporter _localitiesExcelExporter;
        private readonly IRepository<Governorate, int> _lookup_governorateRepository;

        public LocalitiesAppService(IRepository<Locality> localityRepository, ILocalitiesExcelExporter localitiesExcelExporter, IRepository<Governorate, int> lookup_governorateRepository)
        {
            _localityRepository = localityRepository;
            _localitiesExcelExporter = localitiesExcelExporter;
            _lookup_governorateRepository = lookup_governorateRepository;

        }

        public async Task<PagedResultDto<GetLocalityForViewDto>> GetAll(GetAllLocalitiesInput input)
        {

            var filteredLocalities = _localityRepository.GetAll()
                        .Include(e => e.GovernorateFk)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false || e.NameAr.Contains(input.Filter) || e.NameEn.Contains(input.Filter) || e.UniversalCode.Contains(input.Filter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.NameArFilter), e => e.NameAr.Contains(input.NameArFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.NameEnFilter), e => e.NameEn.Contains(input.NameEnFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.UniversalCodeFilter), e => e.UniversalCode.Contains(input.UniversalCodeFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.GovernorateNameFilter), e => e.GovernorateFk != null && e.GovernorateFk.Name == input.GovernorateNameFilter);

            var pagedAndFilteredLocalities = filteredLocalities
                .OrderBy(input.Sorting ?? "id asc")
                .PageBy(input);

            var localities = from o in pagedAndFilteredLocalities
                             join o1 in _lookup_governorateRepository.GetAll() on o.GovernorateId equals o1.Id into j1
                             from s1 in j1.DefaultIfEmpty()

                             select new
                             {

                                 o.NameAr,
                                 o.NameEn,
                                 o.UniversalCode,
                                 Id = o.Id,
                                 GovernorateName = s1 == null || s1.Name == null ? "" : s1.Name.ToString()
                             };

            var totalCount = await filteredLocalities.CountAsync();

            var dbList = await localities.ToListAsync();
            var results = new List<GetLocalityForViewDto>();

            foreach (var o in dbList)
            {
                var res = new GetLocalityForViewDto()
                {
                    Locality = new LocalityDto
                    {

                        NameAr = o.NameAr,
                        NameEn = o.NameEn,
                        UniversalCode = o.UniversalCode,
                        Id = o.Id,
                    },
                    GovernorateName = o.GovernorateName
                };

                results.Add(res);
            }

            return new PagedResultDto<GetLocalityForViewDto>(
                totalCount,
                results
            );

        }

        public async Task<GetLocalityForViewDto> GetLocalityForView(int id)
        {
            var locality = await _localityRepository.GetAsync(id);

            var output = new GetLocalityForViewDto { Locality = ObjectMapper.Map<LocalityDto>(locality) };

            if (output.Locality.GovernorateId != null)
            {
                var _lookupGovernorate = await _lookup_governorateRepository.FirstOrDefaultAsync((int)output.Locality.GovernorateId);
                output.GovernorateName = _lookupGovernorate?.Name?.ToString();
            }

            return output;
        }

        [AbpAuthorize(AppPermissions.Pages_Localities_Edit)]
        public async Task<GetLocalityForEditOutput> GetLocalityForEdit(EntityDto input)
        {
            var locality = await _localityRepository.FirstOrDefaultAsync(input.Id);

            var output = new GetLocalityForEditOutput { Locality = ObjectMapper.Map<CreateOrEditLocalityDto>(locality) };

            if (output.Locality.GovernorateId != null)
            {
                var _lookupGovernorate = await _lookup_governorateRepository.FirstOrDefaultAsync((int)output.Locality.GovernorateId);
                output.GovernorateName = _lookupGovernorate?.Name?.ToString();
            }

            return output;
        }

        public async Task CreateOrEdit(CreateOrEditLocalityDto input)
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

        [AbpAuthorize(AppPermissions.Pages_Localities_Create)]
        protected virtual async Task Create(CreateOrEditLocalityDto input)
        {
            var locality = ObjectMapper.Map<Locality>(input);

            await _localityRepository.InsertAsync(locality);

        }

        [AbpAuthorize(AppPermissions.Pages_Localities_Edit)]
        protected virtual async Task Update(CreateOrEditLocalityDto input)
        {
            var locality = await _localityRepository.FirstOrDefaultAsync((int)input.Id);
            ObjectMapper.Map(input, locality);

        }

        [AbpAuthorize(AppPermissions.Pages_Localities_Delete)]
        public async Task Delete(EntityDto input)
        {
            await _localityRepository.DeleteAsync(input.Id);
        }

        public async Task<FileDto> GetLocalitiesToExcel(GetAllLocalitiesForExcelInput input)
        {

            var filteredLocalities = _localityRepository.GetAll()
                        .Include(e => e.GovernorateFk)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false || e.NameAr.Contains(input.Filter) || e.NameEn.Contains(input.Filter) || e.UniversalCode.Contains(input.Filter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.NameArFilter), e => e.NameAr.Contains(input.NameArFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.NameEnFilter), e => e.NameEn.Contains(input.NameEnFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.UniversalCodeFilter), e => e.UniversalCode.Contains(input.UniversalCodeFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.GovernorateNameFilter), e => e.GovernorateFk != null && e.GovernorateFk.Name == input.GovernorateNameFilter);

            var query = (from o in filteredLocalities
                         join o1 in _lookup_governorateRepository.GetAll() on o.GovernorateId equals o1.Id into j1
                         from s1 in j1.DefaultIfEmpty()

                         select new GetLocalityForViewDto()
                         {
                             Locality = new LocalityDto
                             {
                                 NameAr = o.NameAr,
                                 NameEn = o.NameEn,
                                 UniversalCode = o.UniversalCode,
                                 Id = o.Id
                             },
                             GovernorateName = s1 == null || s1.Name == null ? "" : s1.Name.ToString()
                         });

            var localityListDtos = await query.ToListAsync();

            return _localitiesExcelExporter.ExportToFile(localityListDtos);
        }

        [AbpAuthorize(AppPermissions.Pages_Localities)]
        public async Task<List<LocalityGovernorateLookupTableDto>> GetAllGovernorateForTableDropdown()
        {
            return await _lookup_governorateRepository.GetAll()
                .Select(governorate => new LocalityGovernorateLookupTableDto
                {
                    Id = governorate.Id,
                    DisplayName = governorate == null || governorate.Name == null ? "" : governorate.Name.ToString()
                }).ToListAsync();
        }

    }
}