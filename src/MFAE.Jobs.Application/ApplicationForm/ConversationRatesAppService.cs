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
    [AbpAuthorize(AppPermissions.Pages_ConversationRates)]
    public class ConversationRatesAppService : JobsAppServiceBase, IConversationRatesAppService
    {
        private readonly IRepository<ConversationRate> _conversationRateRepository;
        private readonly IConversationRatesExcelExporter _conversationRatesExcelExporter;

        public ConversationRatesAppService(IRepository<ConversationRate> conversationRateRepository, IConversationRatesExcelExporter conversationRatesExcelExporter)
        {
            _conversationRateRepository = conversationRateRepository;
            _conversationRatesExcelExporter = conversationRatesExcelExporter;

        }

        public async Task<PagedResultDto<GetConversationRateForViewDto>> GetAll(GetAllConversationRatesInput input)
        {

            var filteredConversationRates = _conversationRateRepository.GetAll()
                        .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false || e.NameAr.Contains(input.Filter) || e.NameEn.Contains(input.Filter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.NameArFilter), e => e.NameAr.Contains(input.NameArFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.NameEnFilter), e => e.NameEn.Contains(input.NameEnFilter))
                        .WhereIf(input.IsActiveFilter.HasValue && input.IsActiveFilter > -1, e => (input.IsActiveFilter == 1 && e.IsActive) || (input.IsActiveFilter == 0 && !e.IsActive));

            var pagedAndFilteredConversationRates = filteredConversationRates
                .OrderBy(input.Sorting ?? "id asc")
                .PageBy(input);

            var conversationRates = from o in pagedAndFilteredConversationRates
                                    select new
                                    {

                                        o.NameAr,
                                        o.NameEn,
                                        o.IsActive,
                                        Id = o.Id
                                    };

            var totalCount = await filteredConversationRates.CountAsync();

            var dbList = await conversationRates.ToListAsync();
            var results = new List<GetConversationRateForViewDto>();

            foreach (var o in dbList)
            {
                var res = new GetConversationRateForViewDto()
                {
                    ConversationRate = new ConversationRateDto
                    {

                        NameAr = o.NameAr,
                        NameEn = o.NameEn,
                        IsActive = o.IsActive,
                        Id = o.Id,
                    }
                };

                results.Add(res);
            }

            return new PagedResultDto<GetConversationRateForViewDto>(
                totalCount,
                results
            );

        }

        public async Task<GetConversationRateForViewDto> GetConversationRateForView(int id)
        {
            var conversationRate = await _conversationRateRepository.GetAsync(id);

            var output = new GetConversationRateForViewDto { ConversationRate = ObjectMapper.Map<ConversationRateDto>(conversationRate) };

            return output;
        }

        [AbpAuthorize(AppPermissions.Pages_ConversationRates_Edit)]
        public async Task<GetConversationRateForEditOutput> GetConversationRateForEdit(EntityDto input)
        {
            var conversationRate = await _conversationRateRepository.FirstOrDefaultAsync(input.Id);

            var output = new GetConversationRateForEditOutput { ConversationRate = ObjectMapper.Map<CreateOrEditConversationRateDto>(conversationRate) };

            return output;
        }

        public async Task CreateOrEdit(CreateOrEditConversationRateDto input)
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

        [AbpAuthorize(AppPermissions.Pages_ConversationRates_Create)]
        protected virtual async Task Create(CreateOrEditConversationRateDto input)
        {
            var conversationRate = ObjectMapper.Map<ConversationRate>(input);

            await _conversationRateRepository.InsertAsync(conversationRate);

        }

        [AbpAuthorize(AppPermissions.Pages_ConversationRates_Edit)]
        protected virtual async Task Update(CreateOrEditConversationRateDto input)
        {
            var conversationRate = await _conversationRateRepository.FirstOrDefaultAsync((int)input.Id);
            ObjectMapper.Map(input, conversationRate);

        }

        [AbpAuthorize(AppPermissions.Pages_ConversationRates_Delete)]
        public async Task Delete(EntityDto input)
        {
            await _conversationRateRepository.DeleteAsync(input.Id);
        }

        public async Task<FileDto> GetConversationRatesToExcel(GetAllConversationRatesForExcelInput input)
        {

            var filteredConversationRates = _conversationRateRepository.GetAll()
                        .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false || e.NameAr.Contains(input.Filter) || e.NameEn.Contains(input.Filter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.NameArFilter), e => e.NameAr.Contains(input.NameArFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.NameEnFilter), e => e.NameEn.Contains(input.NameEnFilter))
                        .WhereIf(input.IsActiveFilter.HasValue && input.IsActiveFilter > -1, e => (input.IsActiveFilter == 1 && e.IsActive) || (input.IsActiveFilter == 0 && !e.IsActive));

            var query = (from o in filteredConversationRates
                         select new GetConversationRateForViewDto()
                         {
                             ConversationRate = new ConversationRateDto
                             {
                                 NameAr = o.NameAr,
                                 NameEn = o.NameEn,
                                 IsActive = o.IsActive,
                                 Id = o.Id
                             }
                         });

            var conversationRateListDtos = await query.ToListAsync();

            return _conversationRatesExcelExporter.ExportToFile(conversationRateListDtos);
        }

    }
}