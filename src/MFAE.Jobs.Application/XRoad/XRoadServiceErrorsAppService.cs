using MFAE.Jobs.XRoad;

using System;
using System.Linq;
using System.Linq.Dynamic.Core;
using Abp.Linq.Extensions;
using System.Collections.Generic;
using System.Threading.Tasks;
using Abp.Domain.Repositories;
using MFAE.Jobs.XRoad.Exporting;
using MFAE.Jobs.XRoad.Dtos;
using MFAE.Jobs.Dto;
using Abp.Application.Services.Dto;
using MFAE.Jobs.Authorization;
using Abp.Extensions;
using Abp.Authorization;
using Microsoft.EntityFrameworkCore;
using Abp.UI;
using MFAE.Jobs.Storage;

namespace MFAE.Jobs.XRoad
{
    [AbpAuthorize(AppPermissions.Pages_XRoadServiceErrors)]
    public class XRoadServiceErrorsAppService : JobsAppServiceBase, IXRoadServiceErrorsAppService
    {
        private readonly IRepository<XRoadServiceError> _xRoadServiceErrorRepository;
        private readonly IXRoadServiceErrorsExcelExporter _xRoadServiceErrorsExcelExporter;
        private readonly IRepository<XRoadService, int> _lookup_xRoadServiceRepository;

        public XRoadServiceErrorsAppService(IRepository<XRoadServiceError> xRoadServiceErrorRepository, IXRoadServiceErrorsExcelExporter xRoadServiceErrorsExcelExporter, IRepository<XRoadService, int> lookup_xRoadServiceRepository)
        {
            _xRoadServiceErrorRepository = xRoadServiceErrorRepository;
            _xRoadServiceErrorsExcelExporter = xRoadServiceErrorsExcelExporter;
            _lookup_xRoadServiceRepository = lookup_xRoadServiceRepository;

        }

        public async Task<PagedResultDto<GetXRoadServiceErrorForViewDto>> GetAll(GetAllXRoadServiceErrorsInput input)
        {

            var filteredXRoadServiceErrors = _xRoadServiceErrorRepository.GetAll()
                        .Include(e => e.XRoadServiceFk)
                        .WhereIf((input.XRoadServiceId != null), e => e.XRoadServiceFk != null && e.XRoadServiceFk.Id == input.XRoadServiceId)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false || e.ErrorCode.Contains(input.Filter) || e.ErrorMessageAr.Contains(input.Filter) || e.ErrorMessageEn.Contains(input.Filter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.ErrorCodeFilter), e => e.ErrorCode == input.ErrorCodeFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.ErrorMessageArFilter), e => e.ErrorMessageAr == input.ErrorMessageArFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.ErrorMessageEnFilter), e => e.ErrorMessageEn == input.ErrorMessageEnFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.XRoadServiceNameFilter), e => e.XRoadServiceFk != null && e.XRoadServiceFk.Name == input.XRoadServiceNameFilter);

            var pagedAndFilteredXRoadServiceErrors = filteredXRoadServiceErrors
                .OrderBy(input.Sorting ?? "id asc")
                .PageBy(input);

            var xRoadServiceErrors = from o in pagedAndFilteredXRoadServiceErrors
                                     join o1 in _lookup_xRoadServiceRepository.GetAll() on o.XRoadServiceId equals o1.Id into j1
                                     from s1 in j1.DefaultIfEmpty()

                                     select new GetXRoadServiceErrorForViewDto()
                                     {
                                         XRoadServiceError = new XRoadServiceErrorDto
                                         {
                                             ErrorCode = o.ErrorCode,
                                             ErrorMessageAr = o.ErrorMessageAr,
                                             ErrorMessageEn = o.ErrorMessageEn,
                                             Id = o.Id
                                         },
                                         XRoadServiceName = s1 == null || s1.Name == null ? "" : s1.Name.ToString()
                                     };

            var totalCount = await filteredXRoadServiceErrors.CountAsync();

            return new PagedResultDto<GetXRoadServiceErrorForViewDto>(
                totalCount,
                await xRoadServiceErrors.ToListAsync()
            );

        }

        public async Task<GetXRoadServiceErrorForViewDto> GetXRoadServiceErrorForView(int id)
        {
            var xRoadServiceError = await _xRoadServiceErrorRepository.GetAsync(id);

            var output = new GetXRoadServiceErrorForViewDto { XRoadServiceError = ObjectMapper.Map<XRoadServiceErrorDto>(xRoadServiceError) };

            if (output.XRoadServiceError.XRoadServiceId != null)
            {
                var _lookupXRoadService = await _lookup_xRoadServiceRepository.FirstOrDefaultAsync((int)output.XRoadServiceError.XRoadServiceId);
                output.XRoadServiceName = _lookupXRoadService?.Name?.ToString();
            }

            return output;
        }

        [AbpAuthorize(AppPermissions.Pages_XRoadServiceErrors_Edit)]
        public async Task<GetXRoadServiceErrorForEditOutput> GetXRoadServiceErrorForEdit(EntityDto input)
        {
            var xRoadServiceError = await _xRoadServiceErrorRepository.FirstOrDefaultAsync(input.Id);

            var output = new GetXRoadServiceErrorForEditOutput { XRoadServiceError = ObjectMapper.Map<CreateOrEditXRoadServiceErrorDto>(xRoadServiceError) };

            if (output.XRoadServiceError.XRoadServiceId != null)
            {
                var _lookupXRoadService = await _lookup_xRoadServiceRepository.FirstOrDefaultAsync((int)output.XRoadServiceError.XRoadServiceId);
                output.XRoadServiceName = _lookupXRoadService?.Name?.ToString();
            }

            return output;
        }

        [AbpAuthorize(AppPermissions.Pages_XRoadServiceErrors_Create)]
        public async Task<GetXRoadServiceErrorForEditOutput> GetXRoadServiceErrorForCreate(int serviceId)
        {
            var output = new GetXRoadServiceErrorForEditOutput
            {
                XRoadServiceError = new CreateOrEditXRoadServiceErrorDto()
            };

            output.XRoadServiceError.XRoadServiceId = serviceId;
            var _lookupXRoadService = await _lookup_xRoadServiceRepository.FirstOrDefaultAsync(serviceId);
            output.XRoadServiceName = _lookupXRoadService?.Name?.ToString();

            return output;
        }

        public async Task CreateOrEdit(CreateOrEditXRoadServiceErrorDto input)
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

        [AbpAuthorize(AppPermissions.Pages_XRoadServiceErrors_Create)]
        protected virtual async Task Create(CreateOrEditXRoadServiceErrorDto input)
        {
            var xRoadServiceError = ObjectMapper.Map<XRoadServiceError>(input);

            await _xRoadServiceErrorRepository.InsertAsync(xRoadServiceError);

        }

        [AbpAuthorize(AppPermissions.Pages_XRoadServiceErrors_Edit)]
        protected virtual async Task Update(CreateOrEditXRoadServiceErrorDto input)
        {
            var xRoadServiceError = await _xRoadServiceErrorRepository.FirstOrDefaultAsync((int)input.Id);
            ObjectMapper.Map(input, xRoadServiceError);

        }

        [AbpAuthorize(AppPermissions.Pages_XRoadServiceErrors_Delete)]
        public async Task Delete(EntityDto input)
        {
            await _xRoadServiceErrorRepository.DeleteAsync(input.Id);
        }

        public async Task<FileDto> GetXRoadServiceErrorsToExcel(GetAllXRoadServiceErrorsForExcelInput input)
        {

            var filteredXRoadServiceErrors = _xRoadServiceErrorRepository.GetAll()
                        .Include(e => e.XRoadServiceFk)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false || e.ErrorCode.Contains(input.Filter) || e.ErrorMessageAr.Contains(input.Filter) || e.ErrorMessageEn.Contains(input.Filter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.ErrorCodeFilter), e => e.ErrorCode.Contains(input.ErrorCodeFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.ErrorMessageArFilter), e => e.ErrorMessageAr.Contains(input.ErrorMessageArFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.ErrorMessageEnFilter), e => e.ErrorMessageEn.Contains(input.ErrorMessageEnFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.XRoadServiceNameFilter), e => e.XRoadServiceFk != null && e.XRoadServiceFk.Name == input.XRoadServiceNameFilter);

            var query = (from o in filteredXRoadServiceErrors
                         join o1 in _lookup_xRoadServiceRepository.GetAll() on o.XRoadServiceId equals o1.Id into j1
                         from s1 in j1.DefaultIfEmpty()

                         select new GetXRoadServiceErrorForViewDto()
                         {
                             XRoadServiceError = new XRoadServiceErrorDto
                             {
                                 ErrorCode = o.ErrorCode,
                                 ErrorMessageAr = o.ErrorMessageAr,
                                 ErrorMessageEn = o.ErrorMessageEn,
                                 Id = o.Id
                             },
                             XRoadServiceName = s1 == null || s1.Name == null ? "" : s1.Name.ToString()
                         });

            var xRoadServiceErrorListDtos = await query.ToListAsync();

            return _xRoadServiceErrorsExcelExporter.ExportToFile(xRoadServiceErrorListDtos);
        }

        [AbpAuthorize(AppPermissions.Pages_XRoadServiceErrors)]
        public async Task<PagedResultDto<XRoadServiceErrorXRoadServiceLookupTableDto>> GetAllXRoadServiceForLookupTable(GetAllForLookupTableInput input)
        {
            var query = _lookup_xRoadServiceRepository.GetAll().WhereIf(
                   !string.IsNullOrWhiteSpace(input.Filter),
                  e => e.Name != null && e.Name.Contains(input.Filter)
               );

            var totalCount = await query.CountAsync();

            var xRoadServiceList = await query
                .PageBy(input)
                .ToListAsync();

            var lookupTableDtoList = new List<XRoadServiceErrorXRoadServiceLookupTableDto>();
            foreach (var xRoadService in xRoadServiceList)
            {
                lookupTableDtoList.Add(new XRoadServiceErrorXRoadServiceLookupTableDto
                {
                    Id = xRoadService.Id,
                    DisplayName = xRoadService.Name?.ToString()
                });
            }

            return new PagedResultDto<XRoadServiceErrorXRoadServiceLookupTableDto>(
                totalCount,
                lookupTableDtoList
            );
        }
    }
}