using Abp.Localization;
using Microsoft.AspNetCore.Components;
using MFAE.Jobs.ApiClient;
using MFAE.Jobs.Authorization.Users.Dto;
using MFAE.Jobs.Authorization.Users.Profile;
using MFAE.Jobs.Core.Dependency;
using MFAE.Jobs.Core.Threading;
using MFAE.Jobs.Mobile.MAUI.Services.Account;
using MFAE.Jobs.Mobile.MAUI.Services.UI;
using MFAE.Jobs.Mobile.MAUI.Shared;


namespace MFAE.Jobs.Mobile.MAUI.Pages.MySettings
{
    public partial class LanguageSwitch : JobsComponentBase
    {
        protected LanguageService LanguageService { get; set; }

        private IApplicationContext _applicationContext;
        private readonly IProfileAppService _profileAppService;
        private List<LanguageInfo> _languages;
        private string _selectedLanguage;

        [Parameter] public EventCallback OnSave { get; set; }

        public LanguageSwitch()
        {
            _applicationContext = DependencyResolver.Resolve<IApplicationContext>();
            _profileAppService = DependencyResolver.Resolve<IProfileAppService>();
            LanguageService = DependencyResolver.Resolve<LanguageService>();

            _languages = _applicationContext.Configuration.Localization.Languages;
            _selectedLanguage = _languages.FirstOrDefault(l => l.Name == _applicationContext.CurrentLanguage.Name).Name;
        }

        public List<LanguageInfo> Languages
        {
            get => _languages;
            set => _languages = value;
        }

        public string SelectedLanguage
        {
            get => _selectedLanguage;
            set
            {
                _selectedLanguage = value;
                AsyncRunner.Run(ChangeLanguage());
            }
        }

        private async Task ChangeLanguage()
        {
            var selectedLanguage = _languages?.FirstOrDefault(l => l.Name == _selectedLanguage);
            _applicationContext.CurrentLanguage = selectedLanguage;

            await SetBusyAsync(async () =>
            {
                if (_applicationContext.LoginInfo is null)
                {
                    await UserConfigurationManager.GetAsync();
                    await OnSave.InvokeAsync();
                    LanguageService.ChangeLanguage();

                    return;
                }

                await WebRequestExecuter.Execute(async () =>
                {
                    await _profileAppService.ChangeLanguage(new ChangeUserLanguageDto
                    {
                        LanguageName = _selectedLanguage
                    });
                }, async () =>
                {
                    await UserConfigurationManager.GetAsync();
                    await OnSave.InvokeAsync();
                    LanguageService.ChangeLanguage();
                });
            });
        }
    }
}