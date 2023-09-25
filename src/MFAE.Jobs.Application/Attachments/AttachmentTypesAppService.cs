using MFAE.Jobs.Attachments;

using System;
using System.Linq;
using System.Linq.Dynamic.Core;
using Abp.Linq.Extensions;
using System.Collections.Generic;
using System.Threading.Tasks;
using Abp.Domain.Repositories;
using MFAE.Jobs.Attachments.Exporting;
using MFAE.Jobs.Attachments.Dtos;
using MFAE.Jobs.Dto;
using Abp.Application.Services.Dto;
using MFAE.Jobs.Authorization;
using Abp.Extensions;
using Abp.Authorization;
using Microsoft.EntityFrameworkCore;
using Abp.UI;
using MFAE.Jobs.Storage;

namespace MFAE.Jobs.Attachments
{
    [AbpAuthorize(AppPermissions.Pages_AttachmentTypes)]
    public class AttachmentTypesAppService : JobsAppServiceBase, IAttachmentTypesAppService
    {
        private readonly IRepository<AttachmentType> _attachmentTypeRepository;
        private readonly IAttachmentTypesExcelExporter _attachmentTypesExcelExporter;
        private readonly IRepository<AttachmentEntityType, int> _lookup_attachmentEntityTypeRepository;
        private readonly IRepository<AttachmentTypeGroup, int> _lookup_attachmentTypeGroupRepository;

        public AttachmentTypesAppService(IRepository<AttachmentType> attachmentTypeRepository, IAttachmentTypesExcelExporter attachmentTypesExcelExporter, IRepository<AttachmentEntityType, int> lookup_attachmentEntityTypeRepository, IRepository<AttachmentTypeGroup, int> lookup_attachmentTypeGroupRepository)
        {
            _attachmentTypeRepository = attachmentTypeRepository;
            _attachmentTypesExcelExporter = attachmentTypesExcelExporter;
            _lookup_attachmentEntityTypeRepository = lookup_attachmentEntityTypeRepository;
            _lookup_attachmentTypeGroupRepository = lookup_attachmentTypeGroupRepository;

        }

        public async Task<PagedResultDto<GetAttachmentTypeForViewDto>> GetAll(GetAllAttachmentTypesInput input)
        {
            var categoryFilter = input.CategoryFilter.HasValue
                        ? (AttachmentTypeCategories)input.CategoryFilter
                        : default;
            var privacyFlagFilter = input.PrivacyFlagFilter.HasValue
                ? (PrivacyFlag)input.PrivacyFlagFilter
                : default;

            var filteredAttachmentTypes = _attachmentTypeRepository.GetAll()
                        .Include(e => e.EntityTypeFk)
                        .Include(e => e.GroupFk)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false || e.ArName.Contains(input.Filter) || e.EnName.Contains(input.Filter) || e.AllowedExtensions.Contains(input.Filter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.ArNameFilter), e => e.ArName.Contains(input.ArNameFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.EnNameFilter), e => e.EnName.Contains(input.EnNameFilter))
                        .WhereIf(input.MinMaxSizeMBFilter != null, e => e.MaxSizeMB >= input.MinMaxSizeMBFilter)
                        .WhereIf(input.MaxMaxSizeMBFilter != null, e => e.MaxSizeMB <= input.MaxMaxSizeMBFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.AllowedExtensionsFilter), e => e.AllowedExtensions.Contains(input.AllowedExtensionsFilter))
                        .WhereIf(input.MinMaxAttachmentsFilter != null, e => e.MaxAttachments >= input.MinMaxAttachmentsFilter)
                        .WhereIf(input.MaxMaxAttachmentsFilter != null, e => e.MaxAttachments <= input.MaxMaxAttachmentsFilter)
                        .WhereIf(input.MinMinRequiredAttachmentsFilter != null, e => e.MinRequiredAttachments >= input.MinMinRequiredAttachmentsFilter)
                        .WhereIf(input.MaxMinRequiredAttachmentsFilter != null, e => e.MinRequiredAttachments <= input.MaxMinRequiredAttachmentsFilter)
                        .WhereIf(input.CategoryFilter.HasValue && input.CategoryFilter > -1, e => e.Category == categoryFilter)
                        .WhereIf(input.PrivacyFlagFilter.HasValue && input.PrivacyFlagFilter > -1, e => e.PrivacyFlag == privacyFlagFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.AttachmentEntityTypeNameFilter), e => e.EntityTypeFk != null && e.EntityTypeFk.Name == input.AttachmentEntityTypeNameFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.AttachmentTypeGroupNameFilter), e => e.GroupFk != null && e.GroupFk.Name == input.AttachmentTypeGroupNameFilter);

            var pagedAndFilteredAttachmentTypes = filteredAttachmentTypes
                .OrderBy(input.Sorting ?? "id asc")
                .PageBy(input);

            var attachmentTypes = from o in pagedAndFilteredAttachmentTypes
                                  join o1 in _lookup_attachmentEntityTypeRepository.GetAll() on o.EntityTypeId equals o1.Id into j1
                                  from s1 in j1.DefaultIfEmpty()

                                  join o2 in _lookup_attachmentTypeGroupRepository.GetAll() on o.GroupId equals o2.Id into j2
                                  from s2 in j2.DefaultIfEmpty()

                                  select new
                                  {

                                      o.ArName,
                                      o.EnName,
                                      o.MaxSizeMB,
                                      o.AllowedExtensions,
                                      o.MaxAttachments,
                                      o.MinRequiredAttachments,
                                      o.Category,
                                      o.PrivacyFlag,
                                      Id = o.Id,
                                      AttachmentEntityTypeName = s1 == null || s1.Name == null ? "" : s1.Name.ToString(),
                                      AttachmentTypeGroupName = s2 == null || s2.Name == null ? "" : s2.Name.ToString()
                                  };

            var totalCount = await filteredAttachmentTypes.CountAsync();

            var dbList = await attachmentTypes.ToListAsync();
            var results = new List<GetAttachmentTypeForViewDto>();

            foreach (var o in dbList)
            {
                var res = new GetAttachmentTypeForViewDto()
                {
                    AttachmentType = new AttachmentTypeDto
                    {

                        ArName = o.ArName,
                        EnName = o.EnName,
                        MaxSizeMB = o.MaxSizeMB,
                        AllowedExtensions = o.AllowedExtensions,
                        MaxAttachments = o.MaxAttachments,
                        MinRequiredAttachments = o.MinRequiredAttachments,
                        Category = o.Category,
                        PrivacyFlag = o.PrivacyFlag,
                        Id = o.Id,
                    },
                    AttachmentEntityTypeName = o.AttachmentEntityTypeName,
                    AttachmentTypeGroupName = o.AttachmentTypeGroupName
                };

                results.Add(res);
            }

            return new PagedResultDto<GetAttachmentTypeForViewDto>(
                totalCount,
                results
            );

        }

        public async Task<GetAttachmentTypeForViewDto> GetAttachmentTypeForView(int id)
        {
            var attachmentType = await _attachmentTypeRepository.GetAsync(id);

            var output = new GetAttachmentTypeForViewDto { AttachmentType = ObjectMapper.Map<AttachmentTypeDto>(attachmentType) };

            if (output.AttachmentType.EntityTypeId != null)
            {
                var _lookupAttachmentEntityType = await _lookup_attachmentEntityTypeRepository.FirstOrDefaultAsync((int)output.AttachmentType.EntityTypeId);
                output.AttachmentEntityTypeName = _lookupAttachmentEntityType?.Name?.ToString();
            }

            if (output.AttachmentType.GroupId != null)
            {
                var _lookupAttachmentTypeGroup = await _lookup_attachmentTypeGroupRepository.FirstOrDefaultAsync((int)output.AttachmentType.GroupId);
                output.AttachmentTypeGroupName = _lookupAttachmentTypeGroup?.Name?.ToString();
            }

            return output;
        }

        [AbpAuthorize(AppPermissions.Pages_AttachmentTypes_Edit)]
        public async Task<GetAttachmentTypeForEditOutput> GetAttachmentTypeForEdit(EntityDto input)
        {
            var attachmentType = await _attachmentTypeRepository.FirstOrDefaultAsync(input.Id);

            var output = new GetAttachmentTypeForEditOutput { AttachmentType = ObjectMapper.Map<CreateOrEditAttachmentTypeDto>(attachmentType) };

            if (output.AttachmentType.EntityTypeId != null)
            {
                var _lookupAttachmentEntityType = await _lookup_attachmentEntityTypeRepository.FirstOrDefaultAsync((int)output.AttachmentType.EntityTypeId);
                output.AttachmentEntityTypeName = _lookupAttachmentEntityType?.Name?.ToString();
            }

            if (output.AttachmentType.GroupId != null)
            {
                var _lookupAttachmentTypeGroup = await _lookup_attachmentTypeGroupRepository.FirstOrDefaultAsync((int)output.AttachmentType.GroupId);
                output.AttachmentTypeGroupName = _lookupAttachmentTypeGroup?.Name?.ToString();
            }

            return output;
        }

        public async Task CreateOrEdit(CreateOrEditAttachmentTypeDto input)
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

        [AbpAuthorize(AppPermissions.Pages_AttachmentTypes_Create)]
        protected virtual async Task Create(CreateOrEditAttachmentTypeDto input)
        {
            var attachmentType = ObjectMapper.Map<AttachmentType>(input);

            await _attachmentTypeRepository.InsertAsync(attachmentType);

        }

        [AbpAuthorize(AppPermissions.Pages_AttachmentTypes_Edit)]
        protected virtual async Task Update(CreateOrEditAttachmentTypeDto input)
        {
            var attachmentType = await _attachmentTypeRepository.FirstOrDefaultAsync((int)input.Id);
            ObjectMapper.Map(input, attachmentType);

        }

        [AbpAuthorize(AppPermissions.Pages_AttachmentTypes_Delete)]
        public async Task Delete(EntityDto input)
        {
            await _attachmentTypeRepository.DeleteAsync(input.Id);
        }

        public async Task<FileDto> GetAttachmentTypesToExcel(GetAllAttachmentTypesForExcelInput input)
        {
            var categoryFilter = input.CategoryFilter.HasValue
                        ? (AttachmentTypeCategories)input.CategoryFilter
                        : default;
            var privacyFlagFilter = input.PrivacyFlagFilter.HasValue
                ? (PrivacyFlag)input.PrivacyFlagFilter
                : default;

            var filteredAttachmentTypes = _attachmentTypeRepository.GetAll()
                        .Include(e => e.EntityTypeFk)
                        .Include(e => e.GroupFk)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false || e.ArName.Contains(input.Filter) || e.EnName.Contains(input.Filter) || e.AllowedExtensions.Contains(input.Filter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.ArNameFilter), e => e.ArName.Contains(input.ArNameFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.EnNameFilter), e => e.EnName.Contains(input.EnNameFilter))
                        .WhereIf(input.MinMaxSizeMBFilter != null, e => e.MaxSizeMB >= input.MinMaxSizeMBFilter)
                        .WhereIf(input.MaxMaxSizeMBFilter != null, e => e.MaxSizeMB <= input.MaxMaxSizeMBFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.AllowedExtensionsFilter), e => e.AllowedExtensions.Contains(input.AllowedExtensionsFilter))
                        .WhereIf(input.MinMaxAttachmentsFilter != null, e => e.MaxAttachments >= input.MinMaxAttachmentsFilter)
                        .WhereIf(input.MaxMaxAttachmentsFilter != null, e => e.MaxAttachments <= input.MaxMaxAttachmentsFilter)
                        .WhereIf(input.MinMinRequiredAttachmentsFilter != null, e => e.MinRequiredAttachments >= input.MinMinRequiredAttachmentsFilter)
                        .WhereIf(input.MaxMinRequiredAttachmentsFilter != null, e => e.MinRequiredAttachments <= input.MaxMinRequiredAttachmentsFilter)
                        .WhereIf(input.CategoryFilter.HasValue && input.CategoryFilter > -1, e => e.Category == categoryFilter)
                        .WhereIf(input.PrivacyFlagFilter.HasValue && input.PrivacyFlagFilter > -1, e => e.PrivacyFlag == privacyFlagFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.AttachmentEntityTypeNameFilter), e => e.EntityTypeFk != null && e.EntityTypeFk.Name == input.AttachmentEntityTypeNameFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.AttachmentTypeGroupNameFilter), e => e.GroupFk != null && e.GroupFk.Name == input.AttachmentTypeGroupNameFilter);

            var query = (from o in filteredAttachmentTypes
                         join o1 in _lookup_attachmentEntityTypeRepository.GetAll() on o.EntityTypeId equals o1.Id into j1
                         from s1 in j1.DefaultIfEmpty()

                         join o2 in _lookup_attachmentTypeGroupRepository.GetAll() on o.GroupId equals o2.Id into j2
                         from s2 in j2.DefaultIfEmpty()

                         select new GetAttachmentTypeForViewDto()
                         {
                             AttachmentType = new AttachmentTypeDto
                             {
                                 ArName = o.ArName,
                                 EnName = o.EnName,
                                 MaxSizeMB = o.MaxSizeMB,
                                 AllowedExtensions = o.AllowedExtensions,
                                 MaxAttachments = o.MaxAttachments,
                                 MinRequiredAttachments = o.MinRequiredAttachments,
                                 Category = o.Category,
                                 PrivacyFlag = o.PrivacyFlag,
                                 Id = o.Id
                             },
                             AttachmentEntityTypeName = s1 == null || s1.Name == null ? "" : s1.Name.ToString(),
                             AttachmentTypeGroupName = s2 == null || s2.Name == null ? "" : s2.Name.ToString()
                         });

            var attachmentTypeListDtos = await query.ToListAsync();

            return _attachmentTypesExcelExporter.ExportToFile(attachmentTypeListDtos);
        }

        [AbpAuthorize(AppPermissions.Pages_AttachmentTypes)]
        public async Task<List<AttachmentTypeAttachmentEntityTypeLookupTableDto>> GetAllAttachmentEntityTypeForTableDropdown()
        {
            return await _lookup_attachmentEntityTypeRepository.GetAll()
                .Select(attachmentEntityType => new AttachmentTypeAttachmentEntityTypeLookupTableDto
                {
                    Id = attachmentEntityType.Id,
                    DisplayName = attachmentEntityType == null || attachmentEntityType.Name == null ? "" : attachmentEntityType.Name.ToString()
                }).ToListAsync();
        }

        [AbpAuthorize(AppPermissions.Pages_AttachmentTypes)]
        public async Task<List<AttachmentTypeAttachmentTypeGroupLookupTableDto>> GetAllAttachmentTypeGroupForTableDropdown()
        {
            return await _lookup_attachmentTypeGroupRepository.GetAll()
                .Select(attachmentTypeGroup => new AttachmentTypeAttachmentTypeGroupLookupTableDto
                {
                    Id = attachmentTypeGroup.Id,
                    DisplayName = attachmentTypeGroup == null || attachmentTypeGroup.Name == null ? "" : attachmentTypeGroup.Name.ToString()
                }).ToListAsync();
        }

    }
}