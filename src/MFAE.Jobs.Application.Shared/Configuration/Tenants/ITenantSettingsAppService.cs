using System.Threading.Tasks;
using Abp.Application.Services;
using MFAE.Jobs.Configuration.Tenants.Dto;

namespace MFAE.Jobs.Configuration.Tenants
{
    public interface ITenantSettingsAppService : IApplicationService
    {
        Task<TenantSettingsEditDto> GetAllSettings();

        Task UpdateAllSettings(TenantSettingsEditDto input);

        Task ClearDarkLogo();
        
        Task ClearLightLogo();

        Task ClearCustomCss();
    }
}
