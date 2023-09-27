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
    [AbpAuthorize(AppPermissions.Pages_JobAdvertisements)]
    public class JobAdvertisementsAppService : JobsAppServiceBase, IJobAdvertisementsAppService
    {
        private readonly IRepository<JobAdvertisement> _jobAdvertisementRepository;
        private readonly IJobAdvertisementsExcelExporter _jobAdvertisementsExcelExporter;

        public JobAdvertisementsAppService(IRepository<JobAdvertisement> jobAdvertisementRepository, IJobAdvertisementsExcelExporter jobAdvertisementsExcelExporter)
        {
            _jobAdvertisementRepository = jobAdvertisementRepository;
            _jobAdvertisementsExcelExporter = jobAdvertisementsExcelExporter;

        }

        public async Task<PagedResultDto<GetJobAdvertisementForViewDto>> GetAll(GetAllJobAdvertisementsInput input)
        {

            var filteredJobAdvertisements = _jobAdvertisementRepository.GetAll()
                        .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false || e.Description.Contains(input.Filter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.DescriptionFilter), e => e.Description == input.DescriptionFilter);

            var pagedAndFilteredJobAdvertisements = filteredJobAdvertisements
                .OrderBy(input.Sorting ?? "id asc")
                .PageBy(input);

            var jobAdvertisements = from o in pagedAndFilteredJobAdvertisements
                                    select new
                                    {

                                        o.Description,
                                        Id = o.Id
                                    };

            var totalCount = await filteredJobAdvertisements.CountAsync();

            var dbList = await jobAdvertisements.ToListAsync();
            var results = new List<GetJobAdvertisementForViewDto>();

            foreach (var o in dbList)
            {
                var res = new GetJobAdvertisementForViewDto()
                {
                    JobAdvertisement = new JobAdvertisementDto
                    {

                        Description = o.Description,
                        Id = o.Id,
                    }
                };

                results.Add(res);
            }

            return new PagedResultDto<GetJobAdvertisementForViewDto>(
                totalCount,
                results
            );

        }

        public async Task<GetJobAdvertisementForViewDto> GetJobAdvertisementForView(int id)
        {
            var jobAdvertisement = await _jobAdvertisementRepository.GetAsync(id);

            var output = new GetJobAdvertisementForViewDto { JobAdvertisement = ObjectMapper.Map<JobAdvertisementDto>(jobAdvertisement) };

            return output;
        }

        [AbpAuthorize(AppPermissions.Pages_JobAdvertisements_Edit)]
        public async Task<GetJobAdvertisementForEditOutput> GetJobAdvertisementForEdit(EntityDto input)
        {
            var jobAdvertisement = await _jobAdvertisementRepository.FirstOrDefaultAsync(input.Id);

            var output = new GetJobAdvertisementForEditOutput { JobAdvertisement = ObjectMapper.Map<CreateOrEditJobAdvertisementDto>(jobAdvertisement) };

            return output;
        }

        public async Task CreateOrEdit(CreateOrEditJobAdvertisementDto input)
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

        [AbpAuthorize(AppPermissions.Pages_JobAdvertisements_Create)]
        protected virtual async Task Create(CreateOrEditJobAdvertisementDto input)
        {
            var jobAdvertisement = ObjectMapper.Map<JobAdvertisement>(input);

            await _jobAdvertisementRepository.InsertAsync(jobAdvertisement);

        }

        [AbpAuthorize(AppPermissions.Pages_JobAdvertisements_Edit)]
        protected virtual async Task Update(CreateOrEditJobAdvertisementDto input)
        {
            var jobAdvertisement = await _jobAdvertisementRepository.FirstOrDefaultAsync((int)input.Id);
            ObjectMapper.Map(input, jobAdvertisement);

        }

        [AbpAuthorize(AppPermissions.Pages_JobAdvertisements_Delete)]
        public async Task Delete(EntityDto input)
        {
            await _jobAdvertisementRepository.DeleteAsync(input.Id);
        }

        public async Task<FileDto> GetJobAdvertisementsToExcel(GetAllJobAdvertisementsForExcelInput input)
        {

            var filteredJobAdvertisements = _jobAdvertisementRepository.GetAll()
                        .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false || e.Description.Contains(input.Filter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.DescriptionFilter), e => e.Description == input.DescriptionFilter);

            var query = (from o in filteredJobAdvertisements
                         select new GetJobAdvertisementForViewDto()
                         {
                             JobAdvertisement = new JobAdvertisementDto
                             {
                                 Description = o.Description,
                                 Id = o.Id
                             }
                         });

            var jobAdvertisementListDtos = await query.ToListAsync();

            return _jobAdvertisementsExcelExporter.ExportToFile(jobAdvertisementListDtos);
        }

    }
}