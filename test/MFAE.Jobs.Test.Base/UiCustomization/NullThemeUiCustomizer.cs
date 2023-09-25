using System.Threading.Tasks;
using Abp;
using MFAE.Jobs.Configuration.Dto;
using MFAE.Jobs.UiCustomization;
using MFAE.Jobs.UiCustomization.Dto;

namespace MFAE.Jobs.Test.Base.UiCustomization
{
    public class NullThemeUiCustomizer : IUiCustomizer
    {
        public Task<UiCustomizationSettingsDto> GetUiSettings()
        {
            return Task.FromResult(new UiCustomizationSettingsDto());
        }

        public Task UpdateUserUiManagementSettingsAsync(UserIdentifier user, ThemeSettingsDto settings)
        {
            throw new System.NotImplementedException();
        }

        public Task UpdateTenantUiManagementSettingsAsync(int tenantId, ThemeSettingsDto settings, UserIdentifier changerUser)
        {
            throw new System.NotImplementedException();
        }

        public Task UpdateApplicationUiManagementSettingsAsync(ThemeSettingsDto settings, UserIdentifier changerUser)
        {
            throw new System.NotImplementedException();
        }

        public Task<ThemeSettingsDto> GetHostUiManagementSettings()
        {
            throw new System.NotImplementedException();
        }

        public Task<ThemeSettingsDto> GetTenantUiCustomizationSettings(int tenantId)
        {
            throw new System.NotImplementedException();
        }

        public Task UpdateDarkModeSettingsAsync(UserIdentifier user, bool isDarkModeEnabled)
        {
            throw new System.NotImplementedException();
        }

        public Task<string> GetBodyClass()
        {
            throw new System.NotImplementedException();
        }

        public Task<string> GetBodyStyle()
        {
            throw new System.NotImplementedException();
        }
    }
}