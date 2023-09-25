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
    [AbpAuthorize(AppPermissions.Pages_AttachmentEntityTypes)]
    public class AttachmentEntityTypesAppService : JobsAppServiceBase, IAttachmentEntityTypesAppService
    {
        private readonly IRepository<AttachmentEntityType> _attachmentEntityTypeRepository;
        private readonly IAttachmentEntityTypesExcelExporter _attachmentEntityTypesExcelExporter;

        public AttachmentEntityTypesAppService(IRepository<AttachmentEntityType> attachmentEntityTypeRepository, IAttachmentEntityTypesExcelExporter attachmentEntityTypesExcelExporter)
        {
            _attachmentEntityTypeRepository = attachmentEntityTypeRepository;
            _attachmentEntityTypesExcelExporter = attachmentEntityTypesExcelExporter;

        }

        public async Task<PagedResultDto<GetAttachmentEntityTypeForViewDto>> GetAll(GetAllAttachmentEntityTypesInput input)
        {

            var filteredAttachmentEntityTypes = _attachmentEntityTypeRepository.GetAll()
                        .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false || e.ArName.Contains(input.Filter) || e.EnName.Contains(input.Filter) || e.Folder.Contains(input.Filter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.ArNameFilter), e => e.ArName.Contains(input.ArNameFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.EnNameFilter), e => e.EnName.Contains(input.EnNameFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.FolderFilter), e => e.Folder.Contains(input.FolderFilter))
                        .WhereIf(input.MinParentTypeIdFilter != null, e => e.ParentTypeId >= input.MinParentTypeIdFilter)
                        .WhereIf(input.MaxParentTypeIdFilter != null, e => e.ParentTypeId <= input.MaxParentTypeIdFilter);

            var pagedAndFilteredAttachmentEntityTypes = filteredAttachmentEntityTypes
                .OrderBy(input.Sorting ?? "id asc")
                .PageBy(input);

            var attachmentEntityTypes = from o in pagedAndFilteredAttachmentEntityTypes
                                        select new
                                        {

                                            o.ArName,
                                            o.EnName,
                                            o.Folder,
                                            o.ParentTypeId,
                                            Id = o.Id
                                        };

            var totalCount = await filteredAttachmentEntityTypes.CountAsync();

            var dbList = await attachmentEntityTypes.ToListAsync();
            var results = new List<GetAttachmentEntityTypeForViewDto>();

            foreach (var o in dbList)
            {
                var res = new GetAttachmentEntityTypeForViewDto()
                {
                    AttachmentEntityType = new AttachmentEntityTypeDto
                    {

                        ArName = o.ArName,
                        EnName = o.EnName,
                        Folder = o.Folder,
                        ParentTypeId = o.ParentTypeId,
                        Id = o.Id,
                    }
                };

                results.Add(res);
            }

            return new PagedResultDto<GetAttachmentEntityTypeForViewDto>(
                totalCount,
                results
            );

        }

        public async Task<GetAttachmentEntityTypeForViewDto> GetAttachmentEntityTypeForView(int id)
        {
            var attachmentEntityType = await _attachmentEntityTypeRepository.GetAsync(id);

            var output = new GetAttachmentEntityTypeForViewDto { AttachmentEntityType = ObjectMapper.Map<AttachmentEntityTypeDto>(attachmentEntityType) };

            return output;
        }

        [AbpAuthorize(AppPermissions.Pages_AttachmentEntityTypes_Edit)]
        public async Task<GetAttachmentEntityTypeForEditOutput> GetAttachmentEntityTypeForEdit(EntityDto input)
        {
            var attachmentEntityType = await _attachmentEntityTypeRepository.FirstOrDefaultAsync(input.Id);

            var output = new GetAttachmentEntityTypeForEditOutput { AttachmentEntityType = ObjectMapper.Map<CreateOrEditAttachmentEntityTypeDto>(attachmentEntityType) };

            return output;
        }

        public async Task CreateOrEdit(CreateOrEditAttachmentEntityTypeDto input)
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

        [AbpAuthorize(AppPermissions.Pages_AttachmentEntityTypes_Create)]
        protected virtual async Task Create(CreateOrEditAttachmentEntityTypeDto input)
        {
            var attachmentEntityType = ObjectMapper.Map<AttachmentEntityType>(input);

            await _attachmentEntityTypeRepository.InsertAsync(attachmentEntityType);

        }

        [AbpAuthorize(AppPermissions.Pages_AttachmentEntityTypes_Edit)]
        protected virtual async Task Update(CreateOrEditAttachmentEntityTypeDto input)
        {
            var attachmentEntityType = await _attachmentEntityTypeRepository.FirstOrDefaultAsync((int)input.Id);
            ObjectMapper.Map(input, attachmentEntityType);

        }

        [AbpAuthorize(AppPermissions.Pages_AttachmentEntityTypes_Delete)]
        public async Task Delete(EntityDto input)
        {
            await _attachmentEntityTypeRepository.DeleteAsync(input.Id);
        }

        public async Task<FileDto> GetAttachmentEntityTypesToExcel(GetAllAttachmentEntityTypesForExcelInput input)
        {

            var filteredAttachmentEntityTypes = _attachmentEntityTypeRepository.GetAll()
                        .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false || e.ArName.Contains(input.Filter) || e.EnName.Contains(input.Filter) || e.Folder.Contains(input.Filter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.ArNameFilter), e => e.ArName.Contains(input.ArNameFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.EnNameFilter), e => e.EnName.Contains(input.EnNameFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.FolderFilter), e => e.Folder.Contains(input.FolderFilter))
                        .WhereIf(input.MinParentTypeIdFilter != null, e => e.ParentTypeId >= input.MinParentTypeIdFilter)
                        .WhereIf(input.MaxParentTypeIdFilter != null, e => e.ParentTypeId <= input.MaxParentTypeIdFilter);

            var query = (from o in filteredAttachmentEntityTypes
                         select new GetAttachmentEntityTypeForViewDto()
                         {
                             AttachmentEntityType = new AttachmentEntityTypeDto
                             {
                                 ArName = o.ArName,
                                 EnName = o.EnName,
                                 Folder = o.Folder,
                                 ParentTypeId = o.ParentTypeId,
                                 Id = o.Id
                             }
                         });

            var attachmentEntityTypeListDtos = await query.ToListAsync();

            return _attachmentEntityTypesExcelExporter.ExportToFile(attachmentEntityTypeListDtos);
        }

    }
}