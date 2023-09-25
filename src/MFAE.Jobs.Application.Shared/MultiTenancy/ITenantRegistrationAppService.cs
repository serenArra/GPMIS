using System.Threading.Tasks;
using Abp.Application.Services;
using MFAE.Jobs.Editions.Dto;
using MFAE.Jobs.MultiTenancy.Dto;

namespace MFAE.Jobs.MultiTenancy
{
    public interface ITenantRegistrationAppService: IApplicationService
    {
        Task<RegisterTenantOutput> RegisterTenant(RegisterTenantInput input);

        Task<EditionsSelectOutput> GetEditionsForSelect();

        Task<EditionSelectDto> GetEdition(int editionId);
    }
}