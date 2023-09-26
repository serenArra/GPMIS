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
using Abp.Authorization;
using Microsoft.EntityFrameworkCore;


namespace MFAE.Jobs.ApplicationForm
{
    [AbpAuthorize(AppPermissions.Pages_IdentificationTypes)]
    public class IdentificationTypesAppService : JobsAppServiceBase, IIdentificationTypesAppService
    {
        private readonly IRepository<IdentificationType> _identificationTypeRepository;
        private readonly IIdentificationTypesExcelExporter _identificationTypesExcelExporter;

        public IdentificationTypesAppService(IRepository<IdentificationType> identificationTypeRepository, IIdentificationTypesExcelExporter identificationTypesExcelExporter)
        {
            _identificationTypeRepository = identificationTypeRepository;
            _identificationTypesExcelExporter = identificationTypesExcelExporter;

        }

        public async Task<PagedResultDto<GetIdentificationTypeForViewDto>> GetAll(GetAllIdentificationTypesInput input)
        {

            var filteredIdentificationTypes = _identificationTypeRepository.GetAll()
                        .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false || e.NameAr.Contains(input.Filter) || e.NameEn.Contains(input.Filter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.NameArFilter), e => e.NameAr.Contains(input.NameArFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.NameEnFilter), e => e.NameEn.Contains(input.NameEnFilter))
                        .WhereIf(input.IsActiveFilter.HasValue && input.IsActiveFilter > -1, e => (input.IsActiveFilter == 1 && e.IsActive) || (input.IsActiveFilter == 0 && !e.IsActive))
                        .WhereIf(input.IsDefaultFilter.HasValue && input.IsDefaultFilter > -1, e => (input.IsDefaultFilter == 1 && e.IsDefault) || (input.IsDefaultFilter == 0 && !e.IsDefault));

            var pagedAndFilteredIdentificationTypes = filteredIdentificationTypes
                .OrderBy(input.Sorting ?? "id asc")
                .PageBy(input);

            var identificationTypes = from o in pagedAndFilteredIdentificationTypes
                                      select new
                                      {

                                          o.NameAr,
                                          o.NameEn,
                                          o.IsActive,
                                          o.IsDefault,
                                          Id = o.Id
                                      };

            var totalCount = await filteredIdentificationTypes.CountAsync();

            var dbList = await identificationTypes.ToListAsync();
            var results = new List<GetIdentificationTypeForViewDto>();

            foreach (var o in dbList)
            {
                var res = new GetIdentificationTypeForViewDto()
                {
                    IdentificationType = new IdentificationTypeDto
                    {

                        NameAr = o.NameAr,
                        NameEn = o.NameEn,
                        IsActive = o.IsActive,
                        IsDefault = o.IsDefault,
                        Id = o.Id,
                    }
                };

                results.Add(res);
            }

            return new PagedResultDto<GetIdentificationTypeForViewDto>(
                totalCount,
                results
            );

        }

        public async Task<GetIdentificationTypeForViewDto> GetIdentificationTypeForView(int id)
        {
            var identificationType = await _identificationTypeRepository.GetAsync(id);

            var output = new GetIdentificationTypeForViewDto { IdentificationType = ObjectMapper.Map<IdentificationTypeDto>(identificationType) };

            return output;
        }

        [AbpAuthorize(AppPermissions.Pages_IdentificationTypes_Edit)]
        public async Task<GetIdentificationTypeForEditOutput> GetIdentificationTypeForEdit(EntityDto input)
        {
            var identificationType = await _identificationTypeRepository.FirstOrDefaultAsync(input.Id);

            var output = new GetIdentificationTypeForEditOutput { IdentificationType = ObjectMapper.Map<CreateOrEditIdentificationTypeDto>(identificationType) };

            return output;
        }

        public async Task CreateOrEdit(CreateOrEditIdentificationTypeDto input)
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

        [AbpAuthorize(AppPermissions.Pages_IdentificationTypes_Create)]
        protected virtual async Task Create(CreateOrEditIdentificationTypeDto input)
        {
            var identificationType = ObjectMapper.Map<IdentificationType>(input);

            await _identificationTypeRepository.InsertAsync(identificationType);

        }

        [AbpAuthorize(AppPermissions.Pages_IdentificationTypes_Edit)]
        protected virtual async Task Update(CreateOrEditIdentificationTypeDto input)
        {
            var identificationType = await _identificationTypeRepository.FirstOrDefaultAsync((int)input.Id);
            ObjectMapper.Map(input, identificationType);

        }

        [AbpAuthorize(AppPermissions.Pages_IdentificationTypes_Delete)]
        public async Task Delete(EntityDto input)
        {
            await _identificationTypeRepository.DeleteAsync(input.Id);
        }

        public async Task<FileDto> GetIdentificationTypesToExcel(GetAllIdentificationTypesForExcelInput input)
        {

            var filteredIdentificationTypes = _identificationTypeRepository.GetAll()
                        .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false || e.NameAr.Contains(input.Filter) || e.NameEn.Contains(input.Filter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.NameArFilter), e => e.NameAr.Contains(input.NameArFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.NameEnFilter), e => e.NameEn.Contains(input.NameEnFilter))
                        .WhereIf(input.IsActiveFilter.HasValue && input.IsActiveFilter > -1, e => (input.IsActiveFilter == 1 && e.IsActive) || (input.IsActiveFilter == 0 && !e.IsActive))
                        .WhereIf(input.IsDefaultFilter.HasValue && input.IsDefaultFilter > -1, e => (input.IsDefaultFilter == 1 && e.IsDefault) || (input.IsDefaultFilter == 0 && !e.IsDefault));

            var query = (from o in filteredIdentificationTypes
                         select new GetIdentificationTypeForViewDto()
                         {
                             IdentificationType = new IdentificationTypeDto
                             {
                                 NameAr = o.NameAr,
                                 NameEn = o.NameEn,
                                 IsActive = o.IsActive,
                                 IsDefault = o.IsDefault,
                                 Id = o.Id
                             }
                         });

            var identificationTypeListDtos = await query.ToListAsync();

            return _identificationTypesExcelExporter.ExportToFile(identificationTypeListDtos);
        }

    }
}