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
    [AbpAuthorize(AppPermissions.Pages_Languages)]
    public class LanguagesAppService : JobsAppServiceBase, IAppLanguagesAppService
    {
        private readonly IRepository<AppLanguage> _languageRepository;
        private readonly ILanguagesExcelExporter _languagesExcelExporter;

        public LanguagesAppService(IRepository<AppLanguage> languageRepository, ILanguagesExcelExporter languagesExcelExporter)
        {
            _languageRepository = languageRepository;
            _languagesExcelExporter = languagesExcelExporter;

        }

        public async Task<PagedResultDto<GetAppLanguageForViewDto>> GetAll(GetAllAppLanguagesInput input)
        {

            var filteredLanguages = _languageRepository.GetAll()
                        .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false || e.NameAr.Contains(input.Filter) || e.NameEn.Contains(input.Filter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.NameArFilter), e => e.NameAr.Contains(input.NameArFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.NameEnFilter), e => e.NameEn.Contains(input.NameEnFilter))
                        .WhereIf(input.IsActiveFilter.HasValue && input.IsActiveFilter > -1, e => (input.IsActiveFilter == 1 && e.IsActive) || (input.IsActiveFilter == 0 && !e.IsActive));

            var pagedAndFilteredLanguages = filteredLanguages
                .OrderBy(input.Sorting ?? "id asc")
                .PageBy(input);

            var languages = from o in pagedAndFilteredLanguages
                            select new
                            {

                                o.NameAr,
                                o.NameEn,
                                o.IsActive,
                                Id = o.Id
                            };

            var totalCount = await filteredLanguages.CountAsync();

            var dbList = await languages.ToListAsync();
            var results = new List<GetAppLanguageForViewDto>();

            foreach (var o in dbList)
            {
                var res = new GetAppLanguageForViewDto()
                {
                    Language = new AppLanguageDto
                    {

                        NameAr = o.NameAr,
                        NameEn = o.NameEn,
                        IsActive = o.IsActive,
                        Id = o.Id,
                    }
                };

                results.Add(res);
            }

            return new PagedResultDto<GetAppLanguageForViewDto>(
                totalCount,
                results
            );

        }

        public async Task<GetAppLanguageForViewDto> GetLanguageForView(int id)
        {
            var language = await _languageRepository.GetAsync(id);

            var output = new GetAppLanguageForViewDto { Language = ObjectMapper.Map<AppLanguageDto>(language) };

            return output;
        }

        [AbpAuthorize(AppPermissions.Pages_Languages_Edit)]
        public async Task<GetAppLanguageForEditOutput> GetLanguageForEdit(EntityDto input)
        {
            var language = await _languageRepository.FirstOrDefaultAsync(input.Id);

            var output = new GetAppLanguageForEditOutput { Language = ObjectMapper.Map<CreateOrEditAppLanguageDto>(language) };

            return output;
        }

        public async Task CreateOrEdit(CreateOrEditAppLanguageDto input)
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

        [AbpAuthorize(AppPermissions.Pages_Languages_Create)]
        protected virtual async Task Create(CreateOrEditAppLanguageDto input)
        {
            var language = ObjectMapper.Map<AppLanguage>(input);

            await _languageRepository.InsertAsync(language);

        }

        [AbpAuthorize(AppPermissions.Pages_Languages_Edit)]
        protected virtual async Task Update(CreateOrEditAppLanguageDto input)
        {
            var language = await _languageRepository.FirstOrDefaultAsync((int)input.Id);
            ObjectMapper.Map(input, language);

        }

        [AbpAuthorize(AppPermissions.Pages_Languages_Delete)]
        public async Task Delete(EntityDto input)
        {
            await _languageRepository.DeleteAsync(input.Id);
        }

        public async Task<FileDto> GetLanguagesToExcel(GetAllLanguagesForExcelInput input)
        {

            var filteredLanguages = _languageRepository.GetAll()
                        .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false || e.NameAr.Contains(input.Filter) || e.NameEn.Contains(input.Filter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.NameArFilter), e => e.NameAr.Contains(input.NameArFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.NameEnFilter), e => e.NameEn.Contains(input.NameEnFilter))
                        .WhereIf(input.IsActiveFilter.HasValue && input.IsActiveFilter > -1, e => (input.IsActiveFilter == 1 && e.IsActive) || (input.IsActiveFilter == 0 && !e.IsActive));

            var query = (from o in filteredLanguages
                         select new GetAppLanguageForViewDto()
                         {
                             Language = new AppLanguageDto
                             {
                                 NameAr = o.NameAr,
                                 NameEn = o.NameEn,
                                 IsActive = o.IsActive,
                                 Id = o.Id
                             }
                         });

            var languageListDtos = await query.ToListAsync();

            return _languagesExcelExporter.ExportToFile(languageListDtos);
        }

    }
}