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
using Abp.Authorization;
using Microsoft.EntityFrameworkCore;
namespace MFAE.Jobs.Attachments
{
    [AbpAuthorize(AppPermissions.Pages_AttachmentFiles)]
    public class AttachmentFilesAppService : JobsAppServiceBase, IAttachmentFilesAppService
    {
        private readonly IRepository<AttachmentFile> _attachmentFileRepository;
        private readonly IAttachmentFilesExcelExporter _attachmentFilesExcelExporter;
        private readonly IRepository<AttachmentType, int> _lookup_attachmentTypeRepository;

        public AttachmentFilesAppService(IRepository<AttachmentFile> attachmentFileRepository, IAttachmentFilesExcelExporter attachmentFilesExcelExporter, IRepository<AttachmentType, int> lookup_attachmentTypeRepository)
        {
            _attachmentFileRepository = attachmentFileRepository;
            _attachmentFilesExcelExporter = attachmentFilesExcelExporter;
            _lookup_attachmentTypeRepository = lookup_attachmentTypeRepository;

        }

        public async Task<PagedResultDto<GetAttachmentFileForViewDto>> GetAll(GetAllAttachmentFilesInput input)
        {

            var filteredAttachmentFiles = _attachmentFileRepository.GetAll()
                        .Include(e => e.AttachmentTypeFk)
                        .Where(e => e.ObjectId == input.ApplicantIdFilter.ToString())
                        .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false || e.PhysicalName.Contains(input.Filter) || e.Description.Contains(input.Filter) || e.OriginalName.Contains(input.Filter) || e.ObjectId.Contains(input.Filter) || e.Path.Contains(input.Filter) || e.FileToken.Contains(input.Filter) || e.Tag.Contains(input.Filter) || e.FilterKey.Contains(input.Filter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.PhysicalNameFilter), e => e.PhysicalName.Contains(input.PhysicalNameFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.DescriptionFilter), e => e.Description.Contains(input.DescriptionFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.OriginalNameFilter), e => e.OriginalName.Contains(input.OriginalNameFilter))
                        .WhereIf(input.MinSizeFilter != null, e => e.Size >= input.MinSizeFilter)
                        .WhereIf(input.MaxSizeFilter != null, e => e.Size <= input.MaxSizeFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.ObjectIdFilter), e => e.ObjectId.Contains(input.ObjectIdFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.PathFilter), e => e.Path.Contains(input.PathFilter))
                        .WhereIf(input.MinVersionFilter != null, e => e.Version >= input.MinVersionFilter)
                        .WhereIf(input.MaxVersionFilter != null, e => e.Version <= input.MaxVersionFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.FileTokenFilter), e => e.FileToken.Contains(input.FileTokenFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.TagFilter), e => e.Tag.Contains(input.TagFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.FilterKeyFilter), e => e.FilterKey.Contains(input.FilterKeyFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.AttachmentTypeArNameFilter), e => e.AttachmentTypeFk != null && e.AttachmentTypeFk.ArName == input.AttachmentTypeArNameFilter);

            var pagedAndFilteredAttachmentFiles = filteredAttachmentFiles
                .OrderBy(input.Sorting ?? "id asc")
                .PageBy(input);

            var attachmentFiles = from o in pagedAndFilteredAttachmentFiles
                                  join o1 in _lookup_attachmentTypeRepository.GetAll() on o.AttachmentTypeId equals o1.Id into j1
                                  from s1 in j1.DefaultIfEmpty()

                                  select new
                                  {

                                      o.PhysicalName,
                                      o.Description,
                                      o.OriginalName,
                                      o.Size,
                                      o.ObjectId,
                                      o.Path,
                                      o.Version,
                                      o.FileToken,
                                      o.Tag,
                                      o.FilterKey,
                                      Id = o.Id,
                                      AttachmentTypeArName = s1 == null || s1.ArName == null ? "" : s1.ArName.ToString()
                                  };

            var totalCount = await filteredAttachmentFiles.CountAsync();

            var dbList = await attachmentFiles.ToListAsync();
            var results = new List<GetAttachmentFileForViewDto>();

            foreach (var o in dbList)
            {
                var res = new GetAttachmentFileForViewDto()
                {
                    AttachmentFile = new AttachmentFileDto
                    {

                        PhysicalName = o.PhysicalName,
                        Description = o.Description,
                        OriginalName = o.OriginalName,
                        Size = o.Size,
                        ObjectId = o.ObjectId,
                        Path = o.Path,
                        Version = o.Version,
                        FileToken = o.FileToken,
                        Tag = o.Tag,
                        FilterKey = o.FilterKey,
                        Id = o.Id,
                    },
                    AttachmentTypeArName = o.AttachmentTypeArName
                };

                results.Add(res);
            }

            return new PagedResultDto<GetAttachmentFileForViewDto>(
                totalCount,
                results
            );

        }

        public async Task<GetAttachmentFileForViewDto> GetAttachmentFileForView(int id)
        {
            var attachmentFile = await _attachmentFileRepository.GetAsync(id);

            var output = new GetAttachmentFileForViewDto { AttachmentFile = ObjectMapper.Map<AttachmentFileDto>(attachmentFile) };

            if (output.AttachmentFile.AttachmentTypeId != null)
            {
                var _lookupAttachmentType = await _lookup_attachmentTypeRepository.FirstOrDefaultAsync((int)output.AttachmentFile.AttachmentTypeId);
                output.AttachmentTypeArName = _lookupAttachmentType?.ArName?.ToString();
            }

            return output;
        }

        [AbpAuthorize(AppPermissions.Pages_AttachmentFiles_Edit)]
        public async Task<GetAttachmentFileForEditOutput> GetAttachmentFileForEdit(EntityDto input)
        {
            var attachmentFile = await _attachmentFileRepository.FirstOrDefaultAsync(input.Id);

            var output = new GetAttachmentFileForEditOutput { AttachmentFile = ObjectMapper.Map<CreateOrEditAttachmentFileDto>(attachmentFile) };

            if (output.AttachmentFile.AttachmentTypeId != null)
            {
                var _lookupAttachmentType = await _lookup_attachmentTypeRepository.FirstOrDefaultAsync((int)output.AttachmentFile.AttachmentTypeId);
                output.AttachmentTypeArName = _lookupAttachmentType?.ArName?.ToString();
            }

            return output;
        }

        public async Task CreateOrEdit(CreateOrEditAttachmentFileDto input)
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

        [AbpAuthorize(AppPermissions.Pages_AttachmentFiles_Create)]
        protected virtual async Task Create(CreateOrEditAttachmentFileDto input)
        {
            var attachmentFile = ObjectMapper.Map<AttachmentFile>(input);

            await _attachmentFileRepository.InsertAsync(attachmentFile);

        }

        [AbpAuthorize(AppPermissions.Pages_AttachmentFiles_Edit)]
        protected virtual async Task Update(CreateOrEditAttachmentFileDto input)
        {
            var attachmentFile = await _attachmentFileRepository.FirstOrDefaultAsync((int)input.Id);
            ObjectMapper.Map(input, attachmentFile);

        }

        [AbpAuthorize(AppPermissions.Pages_AttachmentFiles_Delete)]
        public async Task Delete(EntityDto input)
        {
            await _attachmentFileRepository.DeleteAsync(input.Id);
        }

        public async Task<FileDto> GetAttachmentFilesToExcel(GetAllAttachmentFilesForExcelInput input)
        {

            var filteredAttachmentFiles = _attachmentFileRepository.GetAll()
                        .Include(e => e.AttachmentTypeFk)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false || e.PhysicalName.Contains(input.Filter) || e.Description.Contains(input.Filter) || e.OriginalName.Contains(input.Filter) || e.ObjectId.Contains(input.Filter) || e.Path.Contains(input.Filter) || e.FileToken.Contains(input.Filter) || e.Tag.Contains(input.Filter) || e.FilterKey.Contains(input.Filter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.PhysicalNameFilter), e => e.PhysicalName.Contains(input.PhysicalNameFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.DescriptionFilter), e => e.Description.Contains(input.DescriptionFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.OriginalNameFilter), e => e.OriginalName.Contains(input.OriginalNameFilter))
                        .WhereIf(input.MinSizeFilter != null, e => e.Size >= input.MinSizeFilter)
                        .WhereIf(input.MaxSizeFilter != null, e => e.Size <= input.MaxSizeFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.ObjectIdFilter), e => e.ObjectId.Contains(input.ObjectIdFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.PathFilter), e => e.Path.Contains(input.PathFilter))
                        .WhereIf(input.MinVersionFilter != null, e => e.Version >= input.MinVersionFilter)
                        .WhereIf(input.MaxVersionFilter != null, e => e.Version <= input.MaxVersionFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.FileTokenFilter), e => e.FileToken.Contains(input.FileTokenFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.TagFilter), e => e.Tag.Contains(input.TagFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.FilterKeyFilter), e => e.FilterKey.Contains(input.FilterKeyFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.AttachmentTypeArNameFilter), e => e.AttachmentTypeFk != null && e.AttachmentTypeFk.ArName == input.AttachmentTypeArNameFilter);

            var query = (from o in filteredAttachmentFiles
                         join o1 in _lookup_attachmentTypeRepository.GetAll() on o.AttachmentTypeId equals o1.Id into j1
                         from s1 in j1.DefaultIfEmpty()

                         select new GetAttachmentFileForViewDto()
                         {
                             AttachmentFile = new AttachmentFileDto
                             {
                                 PhysicalName = o.PhysicalName,
                                 Description = o.Description,
                                 OriginalName = o.OriginalName,
                                 Size = o.Size,
                                 ObjectId = o.ObjectId,
                                 Path = o.Path,
                                 Version = o.Version,
                                 FileToken = o.FileToken,
                                 Tag = o.Tag,
                                 FilterKey = o.FilterKey,
                                 Id = o.Id
                             },
                             AttachmentTypeArName = s1 == null || s1.ArName == null ? "" : s1.ArName.ToString()
                         });

            var attachmentFileListDtos = await query.ToListAsync();

            return _attachmentFilesExcelExporter.ExportToFile(attachmentFileListDtos);
        }

        [AbpAuthorize(AppPermissions.Pages_AttachmentFiles)]
        public async Task<List<AttachmentFileAttachmentTypeLookupTableDto>> GetAllAttachmentTypeForTableDropdown()
        {
            return await _lookup_attachmentTypeRepository.GetAll()
                .Select(attachmentType => new AttachmentFileAttachmentTypeLookupTableDto
                {
                    Id = attachmentType.Id,
                    DisplayName = attachmentType == null || attachmentType.ArName == null ? "" : attachmentType.ArName.ToString()
                }).ToListAsync();
        }

    }
}