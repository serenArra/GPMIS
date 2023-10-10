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
    [AbpAuthorize(AppPermissions.Pages_ApplicantLanguages)]
    public class ApplicantLanguagesAppService : JobsAppServiceBase, IApplicantLanguagesAppService
    {
        private readonly IRepository<ApplicantLanguage, long> _applicantLanguageRepository;
        private readonly IApplicantLanguagesExcelExporter _applicantLanguagesExcelExporter;
        private readonly IRepository<Applicant, long> _lookup_applicantRepository;
        private readonly IRepository<AppLanguage, int> _lookup_languageRepository;
        private readonly IRepository<Conversation, int> _lookup_conversationRepository;
        private readonly IRepository<ConversationRate, int> _lookup_conversationRateRepository;

        public ApplicantLanguagesAppService(IRepository<ApplicantLanguage, long> applicantLanguageRepository, IApplicantLanguagesExcelExporter applicantLanguagesExcelExporter, IRepository<Applicant, long> lookup_applicantRepository, IRepository<AppLanguage, int> lookup_languageRepository, IRepository<Conversation, int> lookup_conversationRepository, IRepository<ConversationRate, int> lookup_conversationRateRepository)
        {
            _applicantLanguageRepository = applicantLanguageRepository;
            _applicantLanguagesExcelExporter = applicantLanguagesExcelExporter;
            _lookup_applicantRepository = lookup_applicantRepository;
            _lookup_languageRepository = lookup_languageRepository;
            _lookup_conversationRepository = lookup_conversationRepository;
            _lookup_conversationRateRepository = lookup_conversationRateRepository;

        }

      



        public async Task<PagedResultDto<GetApplicantLanguageForViewDto>> GetAll(GetAllApplicantLanguagesInput input)
        {
            var aplicatlanguage =_applicantLanguageRepository.GetAll().Include(e => e.ApplicantFk)
                        .Include(e => e.LanguageFk)
                        .Include(e => e.ConversationFk)
                        .Include(e => e.ConversationRateFk).GroupBy(x => new { x.LanguageId })                        
                        .Select(x => new
                        {
                           applicstUser = x.Select(s=>s.ApplicantId).FirstOrDefault(),
                           language = x.Select(s=>s.LanguageFk.Name).FirstOrDefault(),
                           Read =x.Where(s=>s.ConversationId == 1).Select(s=>s.ConversationRateFk.Name).FirstOrDefault(),
                           Write = x.Where(s => s.ConversationId == 2).Select(s => s.ConversationRateFk.Name).FirstOrDefault(),
                           Speak = x.Where(s => s.ConversationId == 3).Select(s => s.ConversationRateFk.Name).FirstOrDefault(),
                        }).Where(e=>e.applicstUser == input.ApplicantIdFilter);


            var filteredApplicantLanguages = _applicantLanguageRepository.GetAll()
                        .Include(e => e.ApplicantFk)
                        .Include(e => e.LanguageFk)
                        .Include(e => e.ConversationFk)
                        .Include(e => e.ConversationRateFk)
                        .Where(e => e.ApplicantId == input.ApplicantIdFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false || e.Narrative.Contains(input.Filter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.NarrativeFilter), e => e.Narrative.Contains(input.NarrativeFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.ApplicantFirstNameFilter), e => e.ApplicantFk != null && e.ApplicantFk.FirstName == input.ApplicantFirstNameFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.LanguageNameFilter), e => e.LanguageFk != null && e.LanguageFk.Name == input.LanguageNameFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.ConversationNameFilter), e => e.ConversationFk != null && e.ConversationFk.Name == input.ConversationNameFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.ConversationRateNameFilter), e => e.ConversationRateFk != null && e.ConversationRateFk.Name == input.ConversationRateNameFilter);

            var pagedAndFilteredApplicantLanguages = filteredApplicantLanguages
                .OrderBy(input.Sorting ?? "id asc")
                .PageBy(input);

            var applicantLanguages = from o in pagedAndFilteredApplicantLanguages
                                    join o1 in _lookup_applicantRepository.GetAll() on o.ApplicantId equals o1.Id into j1
                                    from s1 in j1.DefaultIfEmpty()

                                     join o2 in _lookup_languageRepository.GetAll() on o.LanguageId equals o2.Id into j2
                                     from s2 in j2.DefaultIfEmpty()

                                     join o3 in _lookup_conversationRepository.GetAll() on o.ConversationId equals o3.Id into j3
                                     from s3 in j3.DefaultIfEmpty()

                                     join o4 in _lookup_conversationRateRepository.GetAll() on o.ConversationRateId equals o4.Id into j4
                                     from s4 in j4.DefaultIfEmpty()

                                     select new
                                     {
                                         o.ConversationId,
                                         o.LanguageId,
                                         o.Narrative,
                                         Id = o.Id,
                                         ApplicantFirstName = s1 == null || s1.FirstName == null ? "" : s1.FirstName.ToString(),
                                         LanguageName = s2 == null || s2.Name == null ? "" : s2.Name.ToString(),
                                         ConversationName = s3 == null || s3.Name == null ? "" : s3.Name.ToString(),
                                         ConversationRateName = s4 == null || s4.Name == null ? "" : s4.Name.ToString()
                                     };

            var totalCount = await filteredApplicantLanguages.CountAsync();

            var dbList = await applicantLanguages.GroupBy(x => new { x.LanguageId})
                        .Select(x => new
                        {
                            language = x.Select(s => s.LanguageName).FirstOrDefault(),
                            Read = x.Where(s => s.ConversationId == 1).Select(s => s.ConversationRateName).FirstOrDefault(),
                            Write = x.Where(s => s.ConversationId == 2).Select(s => s.ConversationRateName).FirstOrDefault(),
                            Speak = x.Where(s => s.ConversationId == 3).Select(s => s.ConversationRateName).FirstOrDefault(),
                        }).ToListAsync();
            var results = new List<GetApplicantLanguageForViewDto>();

            foreach (var o in dbList)
            {
                var res = new GetApplicantLanguageForViewDto()
                {
             
                    LanguageName = o.language,
                    ConversationReadRate =o.Read,
                    ConversationspeakRate = o.Speak,
                    ConversationWriteRate = o.Write,
                };

                results.Add(res);
            }

            return new PagedResultDto<GetApplicantLanguageForViewDto>(
                totalCount,
                results
            );

        }

        public async Task<GetApplicantLanguageForViewDto> GetApplicantLanguageForView(long id)
        {
            var applicantLanguage = await _applicantLanguageRepository.GetAsync(id);

            var output = new GetApplicantLanguageForViewDto { ApplicantLanguage = ObjectMapper.Map<ApplicantLanguageDto>(applicantLanguage) };

            if (output.ApplicantLanguage.ApplicantId != null)
            {
                var _lookupApplicant = await _lookup_applicantRepository.FirstOrDefaultAsync((long)output.ApplicantLanguage.ApplicantId);
                output.ApplicantFirstName = _lookupApplicant?.FirstName?.ToString();
            }

            if (output.ApplicantLanguage.LanguageId != null)
            {
                var _lookupLanguage = await _lookup_languageRepository.FirstOrDefaultAsync((int)output.ApplicantLanguage.LanguageId);
                output.LanguageName = _lookupLanguage?.Name?.ToString();
            }

            if (output.ApplicantLanguage.ConversationId != null)
            {
                var _lookupConversation = await _lookup_conversationRepository.FirstOrDefaultAsync((int)output.ApplicantLanguage.ConversationId);
                output.ConversationName = _lookupConversation?.Name?.ToString();
            }

            if (output.ApplicantLanguage.ConversationRateId != null)
            {
                var _lookupConversationRate = await _lookup_conversationRateRepository.FirstOrDefaultAsync((int)output.ApplicantLanguage.ConversationRateId);
                output.ConversationRateName = _lookupConversationRate?.Name?.ToString();
            }

            return output;
        }

        [AbpAuthorize(AppPermissions.Pages_ApplicantLanguages_Edit)]
        public async Task<GetApplicantLanguageForEditOutput> GetApplicantLanguageForEdit(EntityDto<long> input)
        {
            var applicantLanguage = await _applicantLanguageRepository.FirstOrDefaultAsync(input.Id);

            var output = new GetApplicantLanguageForEditOutput { ApplicantLanguage = ObjectMapper.Map<CreateOrEditApplicantLanguageDto>(applicantLanguage) };

            if (output.ApplicantLanguage.ApplicantId != null)
            {
                var _lookupApplicant = await _lookup_applicantRepository.FirstOrDefaultAsync((long)output.ApplicantLanguage.ApplicantId);
                output.ApplicantFirstName = _lookupApplicant?.FirstName?.ToString();
            }

            if (output.ApplicantLanguage.LanguageId != null)
            {
                var _lookupLanguage = await _lookup_languageRepository.FirstOrDefaultAsync((int)output.ApplicantLanguage.LanguageId);
                output.LanguageName = _lookupLanguage?.Name?.ToString();
            }

            if (output.ApplicantLanguage.ConversationId != null)
            {
                var _lookupConversation = await _lookup_conversationRepository.FirstOrDefaultAsync((int)output.ApplicantLanguage.ConversationId);
                output.ConversationName = _lookupConversation?.Name?.ToString();
            }

            if (output.ApplicantLanguage.ConversationRateId != null)
            {
                var _lookupConversationRate = await _lookup_conversationRateRepository.FirstOrDefaultAsync((int)output.ApplicantLanguage.ConversationRateId);
                output.ConversationRateName = _lookupConversationRate?.Name?.ToString();
            }

            return output;
        }

        public async Task CreateOrEdit(CreateOrEditApplicantLanguageDto input)
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

        [AbpAuthorize(AppPermissions.Pages_ApplicantLanguages_Create)]
        protected virtual async Task Create(CreateOrEditApplicantLanguageDto input)
        {
            var i = 0;
            foreach (var Conversation in input.ConversationIds)
            {
                var applicantLanguage = new ApplicantLanguage();
                applicantLanguage.LanguageId = input.LanguageId;
                applicantLanguage.ConversationId = Conversation;
                applicantLanguage.ConversationRateId = input.ConversationRateIds[i];
                applicantLanguage.ApplicantId = input.ApplicantId;
                await _applicantLanguageRepository.InsertAsync(applicantLanguage);
                i++;
            }
            
        }

        [AbpAuthorize(AppPermissions.Pages_ApplicantLanguages_Edit)]
        protected virtual async Task Update(CreateOrEditApplicantLanguageDto input)
        {
            var applicantLanguage = await _applicantLanguageRepository.FirstOrDefaultAsync((long)input.Id);
            ObjectMapper.Map(input, applicantLanguage);

        }

        [AbpAuthorize(AppPermissions.Pages_ApplicantLanguages_Delete)]
        public async Task Delete(EntityDto<long> input)
        {
            await _applicantLanguageRepository.DeleteAsync(input.Id);
        }

        public async Task<FileDto> GetApplicantLanguagesToExcel(GetAllApplicantLanguagesForExcelInput input)
        {

            var filteredApplicantLanguages = _applicantLanguageRepository.GetAll()
                        .Include(e => e.ApplicantFk)
                        .Include(e => e.LanguageFk)
                        .Include(e => e.ConversationFk)
                        .Include(e => e.ConversationRateFk)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false || e.Narrative.Contains(input.Filter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.NarrativeFilter), e => e.Narrative.Contains(input.NarrativeFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.ApplicantFirstNameFilter), e => e.ApplicantFk != null && e.ApplicantFk.FirstName == input.ApplicantFirstNameFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.LanguageNameFilter), e => e.LanguageFk != null && e.LanguageFk.Name == input.LanguageNameFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.ConversationNameFilter), e => e.ConversationFk != null && e.ConversationFk.Name == input.ConversationNameFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.ConversationRateNameFilter), e => e.ConversationRateFk != null && e.ConversationRateFk.Name == input.ConversationRateNameFilter);

            var query = (from o in filteredApplicantLanguages
                         join o1 in _lookup_applicantRepository.GetAll() on o.ApplicantId equals o1.Id into j1
                         from s1 in j1.DefaultIfEmpty()

                         join o2 in _lookup_languageRepository.GetAll() on o.LanguageId equals o2.Id into j2
                         from s2 in j2.DefaultIfEmpty()

                         join o3 in _lookup_conversationRepository.GetAll() on o.ConversationId equals o3.Id into j3
                         from s3 in j3.DefaultIfEmpty()

                         join o4 in _lookup_conversationRateRepository.GetAll() on o.ConversationRateId equals o4.Id into j4
                         from s4 in j4.DefaultIfEmpty()

                         select new GetApplicantLanguageForViewDto()
                         {
                             ApplicantLanguage = new ApplicantLanguageDto
                             {
                                 Narrative = o.Narrative,
                                 Id = o.Id
                             },
                             ApplicantFirstName = s1 == null || s1.FirstName == null ? "" : s1.FirstName.ToString(),
                             LanguageName = s2 == null || s2.Name == null ? "" : s2.Name.ToString(),
                             ConversationName = s3 == null || s3.Name == null ? "" : s3.Name.ToString(),
                             ConversationRateName = s4 == null || s4.Name == null ? "" : s4.Name.ToString()
                         });

            var applicantLanguageListDtos = await query.ToListAsync();

            return _applicantLanguagesExcelExporter.ExportToFile(applicantLanguageListDtos);
        }

        [AbpAuthorize(AppPermissions.Pages_ApplicantLanguages)]
        public async Task<List<ApplicantLanguageApplicantLookupTableDto>> GetAllApplicantForTableDropdown()
        {
            return await _lookup_applicantRepository.GetAll()
                .Select(applicant => new ApplicantLanguageApplicantLookupTableDto
                {
                    Id = applicant.Id,
                    DisplayName = applicant == null || applicant.FirstName == null ? "" : applicant.FirstName.ToString()
                }).ToListAsync();
        }

        [AbpAuthorize(AppPermissions.Pages_ApplicantLanguages)]
        public async Task<List<ApplicantLanguageLanguageLookupTableDto>> GetAllLanguageForTableDropdown()
        {
            return await _lookup_languageRepository.GetAll()
                .Select(language => new ApplicantLanguageLanguageLookupTableDto
                {
                    Id = language.Id,
                    DisplayName = language == null || language.Name == null ? "" : language.Name.ToString()
                }).ToListAsync();
        }

        [AbpAuthorize(AppPermissions.Pages_ApplicantLanguages)]
        public async Task<List<ApplicantLanguageConversationLookupTableDto>> GetAllConversationForTableDropdown()
        {
            return await _lookup_conversationRepository.GetAll()
                .Select(conversation => new ApplicantLanguageConversationLookupTableDto
                {
                    Id = conversation.Id,
                    DisplayName = conversation == null || conversation.Name == null ? "" : conversation.Name.ToString()
                }).ToListAsync();
        }

        [AbpAuthorize(AppPermissions.Pages_ApplicantLanguages)]
        public async Task<List<ApplicantLanguageConversationRateLookupTableDto>> GetAllConversationRateForTableDropdown()
        {
            return await _lookup_conversationRateRepository.GetAll()
                .Select(conversationRate => new ApplicantLanguageConversationRateLookupTableDto
                {
                    Id = conversationRate.Id,
                    DisplayName = conversationRate == null || conversationRate.Name == null ? "" : conversationRate.Name.ToString()
                }).ToListAsync();
        }

    }
}