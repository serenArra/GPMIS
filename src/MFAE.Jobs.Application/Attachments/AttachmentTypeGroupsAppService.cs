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
    [AbpAuthorize(AppPermissions.Pages_AttachmentTypeGroups)]
    public class AttachmentTypeGroupsAppService : JobsAppServiceBase, IAttachmentTypeGroupsAppService
    {
        private readonly IRepository<AttachmentTypeGroup> _attachmentTypeGroupRepository;
        private readonly IAttachmentTypeGroupsExcelExporter _attachmentTypeGroupsExcelExporter;

        public AttachmentTypeGroupsAppService(IRepository<AttachmentTypeGroup> attachmentTypeGroupRepository, IAttachmentTypeGroupsExcelExporter attachmentTypeGroupsExcelExporter)
        {
            _attachmentTypeGroupRepository = attachmentTypeGroupRepository;
            _attachmentTypeGroupsExcelExporter = attachmentTypeGroupsExcelExporter;

        }

        public async Task<PagedResultDto<GetAttachmentTypeGroupForViewDto>> GetAll(GetAllAttachmentTypeGroupsInput input)
        {

            var filteredAttachmentTypeGroups = _attachmentTypeGroupRepository.GetAll()
                        .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false || e.ArName.Contains(input.Filter) || e.EnName.Contains(input.Filter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.ArNameFilter), e => e.ArName.Contains(input.ArNameFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.EnNameFilter), e => e.EnName.Contains(input.EnNameFilter));

            var pagedAndFilteredAttachmentTypeGroups = filteredAttachmentTypeGroups
                .OrderBy(input.Sorting ?? "id asc")
                .PageBy(input);

            var attachmentTypeGroups = from o in pagedAndFilteredAttachmentTypeGroups
                                       select new
                                       {

                                           o.ArName,
                                           o.EnName,
                                           Id = o.Id
                                       };

            var totalCount = await filteredAttachmentTypeGroups.CountAsync();

            var dbList = await attachmentTypeGroups.ToListAsync();
            var results = new List<GetAttachmentTypeGroupForViewDto>();

            foreach (var o in dbList)
            {
                var res = new GetAttachmentTypeGroupForViewDto()
                {
                    AttachmentTypeGroup = new AttachmentTypeGroupDto
                    {

                        ArName = o.ArName,
                        EnName = o.EnName,
                        Id = o.Id,
                    }
                };

                results.Add(res);
            }

            return new PagedResultDto<GetAttachmentTypeGroupForViewDto>(
                totalCount,
                results
            );

        }

        public async Task<GetAttachmentTypeGroupForViewDto> GetAttachmentTypeGroupForView(int id)
        {
            var attachmentTypeGroup = await _attachmentTypeGroupRepository.GetAsync(id);

            var output = new GetAttachmentTypeGroupForViewDto { AttachmentTypeGroup = ObjectMapper.Map<AttachmentTypeGroupDto>(attachmentTypeGroup) };

            return output;
        }

        [AbpAuthorize(AppPermissions.Pages_AttachmentTypeGroups_Edit)]
        public async Task<GetAttachmentTypeGroupForEditOutput> GetAttachmentTypeGroupForEdit(EntityDto input)
        {
            var attachmentTypeGroup = await _attachmentTypeGroupRepository.FirstOrDefaultAsync(input.Id);

            var output = new GetAttachmentTypeGroupForEditOutput { AttachmentTypeGroup = ObjectMapper.Map<CreateOrEditAttachmentTypeGroupDto>(attachmentTypeGroup) };

            return output;
        }

        public async Task CreateOrEdit(CreateOrEditAttachmentTypeGroupDto input)
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

        [AbpAuthorize(AppPermissions.Pages_AttachmentTypeGroups_Create)]
        protected virtual async Task Create(CreateOrEditAttachmentTypeGroupDto input)
        {
            var attachmentTypeGroup = ObjectMapper.Map<AttachmentTypeGroup>(input);

            await _attachmentTypeGroupRepository.InsertAsync(attachmentTypeGroup);

        }

        [AbpAuthorize(AppPermissions.Pages_AttachmentTypeGroups_Edit)]
        protected virtual async Task Update(CreateOrEditAttachmentTypeGroupDto input)
        {
            var attachmentTypeGroup = await _attachmentTypeGroupRepository.FirstOrDefaultAsync((int)input.Id);
            ObjectMapper.Map(input, attachmentTypeGroup);

        }

        [AbpAuthorize(AppPermissions.Pages_AttachmentTypeGroups_Delete)]
        public async Task Delete(EntityDto input)
        {
            await _attachmentTypeGroupRepository.DeleteAsync(input.Id);
        }

        public async Task<FileDto> GetAttachmentTypeGroupsToExcel(GetAllAttachmentTypeGroupsForExcelInput input)
        {

            var filteredAttachmentTypeGroups = _attachmentTypeGroupRepository.GetAll()
                        .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false || e.ArName.Contains(input.Filter) || e.EnName.Contains(input.Filter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.ArNameFilter), e => e.ArName.Contains(input.ArNameFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.EnNameFilter), e => e.EnName.Contains(input.EnNameFilter));

            var query = (from o in filteredAttachmentTypeGroups
                         select new GetAttachmentTypeGroupForViewDto()
                         {
                             AttachmentTypeGroup = new AttachmentTypeGroupDto
                             {
                                 ArName = o.ArName,
                                 EnName = o.EnName,
                                 Id = o.Id
                             }
                         });

            var attachmentTypeGroupListDtos = await query.ToListAsync();

            return _attachmentTypeGroupsExcelExporter.ExportToFile(attachmentTypeGroupListDtos);
        }

    }
}