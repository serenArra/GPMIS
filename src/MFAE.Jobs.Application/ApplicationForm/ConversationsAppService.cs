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
    [AbpAuthorize(AppPermissions.Pages_Conversations)]
    public class ConversationsAppService : JobsAppServiceBase, IConversationsAppService
    {
        private readonly IRepository<Conversation> _conversationRepository;
        private readonly IConversationsExcelExporter _conversationsExcelExporter;

        public ConversationsAppService(IRepository<Conversation> conversationRepository, IConversationsExcelExporter conversationsExcelExporter)
        {
            _conversationRepository = conversationRepository;
            _conversationsExcelExporter = conversationsExcelExporter;

        }

        public async Task<PagedResultDto<GetConversationForViewDto>> GetAll(GetAllConversationsInput input)
        {

            var filteredConversations = _conversationRepository.GetAll()
                        .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false || e.NameAr.Contains(input.Filter) || e.NameEn.Contains(input.Filter) || e.IsActive.Contains(input.Filter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.NameArFilter), e => e.NameAr.Contains(input.NameArFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.NameEnFilter), e => e.NameEn.Contains(input.NameEnFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.IsActiveFilter), e => e.IsActive.Contains(input.IsActiveFilter));

            var pagedAndFilteredConversations = filteredConversations
                .OrderBy(input.Sorting ?? "id asc")
                .PageBy(input);

            var conversations = from o in pagedAndFilteredConversations
                                select new
                                {

                                    o.NameAr,
                                    o.NameEn,
                                    o.IsActive,
                                    Id = o.Id
                                };

            var totalCount = await filteredConversations.CountAsync();

            var dbList = await conversations.ToListAsync();
            var results = new List<GetConversationForViewDto>();

            foreach (var o in dbList)
            {
                var res = new GetConversationForViewDto()
                {
                    Conversation = new ConversationDto
                    {

                        NameAr = o.NameAr,
                        NameEn = o.NameEn,
                        IsActive = o.IsActive,
                        Id = o.Id,
                    }
                };

                results.Add(res);
            }

            return new PagedResultDto<GetConversationForViewDto>(
                totalCount,
                results
            );

        }

        public async Task<GetConversationForViewDto> GetConversationForView(int id)
        {
            var conversation = await _conversationRepository.GetAsync(id);

            var output = new GetConversationForViewDto { Conversation = ObjectMapper.Map<ConversationDto>(conversation) };

            return output;
        }

        [AbpAuthorize(AppPermissions.Pages_Conversations_Edit)]
        public async Task<GetConversationForEditOutput> GetConversationForEdit(EntityDto input)
        {
            var conversation = await _conversationRepository.FirstOrDefaultAsync(input.Id);

            var output = new GetConversationForEditOutput { Conversation = ObjectMapper.Map<CreateOrEditConversationDto>(conversation) };

            return output;
        }

        public async Task CreateOrEdit(CreateOrEditConversationDto input)
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

        [AbpAuthorize(AppPermissions.Pages_Conversations_Create)]
        protected virtual async Task Create(CreateOrEditConversationDto input)
        {
            var conversation = ObjectMapper.Map<Conversation>(input);

            await _conversationRepository.InsertAsync(conversation);

        }

        [AbpAuthorize(AppPermissions.Pages_Conversations_Edit)]
        protected virtual async Task Update(CreateOrEditConversationDto input)
        {
            var conversation = await _conversationRepository.FirstOrDefaultAsync((int)input.Id);
            ObjectMapper.Map(input, conversation);

        }

        [AbpAuthorize(AppPermissions.Pages_Conversations_Delete)]
        public async Task Delete(EntityDto input)
        {
            await _conversationRepository.DeleteAsync(input.Id);
        }

        public async Task<FileDto> GetConversationsToExcel(GetAllConversationsForExcelInput input)
        {

            var filteredConversations = _conversationRepository.GetAll()
                        .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false || e.NameAr.Contains(input.Filter) || e.NameEn.Contains(input.Filter) || e.IsActive.Contains(input.Filter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.NameArFilter), e => e.NameAr.Contains(input.NameArFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.NameEnFilter), e => e.NameEn.Contains(input.NameEnFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.IsActiveFilter), e => e.IsActive.Contains(input.IsActiveFilter));

            var query = (from o in filteredConversations
                         select new GetConversationForViewDto()
                         {
                             Conversation = new ConversationDto
                             {
                                 NameAr = o.NameAr,
                                 NameEn = o.NameEn,
                                 IsActive = o.IsActive,
                                 Id = o.Id
                             }
                         });

            var conversationListDtos = await query.ToListAsync();

            return _conversationsExcelExporter.ExportToFile(conversationListDtos);
        }

    }
}