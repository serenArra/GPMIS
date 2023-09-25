using Abp.Dependency;
using MFAE.Jobs.ApiClient;
using MFAE.Jobs.ApiClient.Models;
using MFAE.Jobs.Core.Threading;
using MFAE.Jobs.Localization;
using MFAE.Jobs.Mobile.MAUI.Services.Account;
using MFAE.Jobs.Mobile.MAUI.Services.UI;
using MFAE.Jobs.Services.Navigation;
using MFAE.Jobs.Services.Storage;
using MFAE.Jobs.Sessions;
using MFAE.Jobs.Sessions.Dto;

namespace MFAE.Jobs.Services.Account
{
    public class AccountService : IAccountService, ISingletonDependency
    {
        private readonly IApplicationContext _applicationContext;
        private readonly ISessionAppService _sessionAppService;
        private readonly IAccessTokenManager _accessTokenManager;
        private readonly IDataStorageService _dataStorageService;
        private readonly INavigationService _navigationService;

        public AccountService(
            IApplicationContext applicationContext,
            ISessionAppService sessionAppService,
            IAccessTokenManager accessTokenManager,
            AbpAuthenticateModel abpAuthenticateModel,
            IDataStorageService dataStorageService,
            INavigationService navigationService
            )
        {
            _applicationContext = applicationContext;
            _sessionAppService = sessionAppService;
            _accessTokenManager = accessTokenManager;
            _dataStorageService = dataStorageService;
            AbpAuthenticateModel = abpAuthenticateModel;
            _navigationService = navigationService;
        }

        public AbpAuthenticateModel AbpAuthenticateModel { get; set; }
        public AbpAuthenticateResultModel AuthenticateResultModel { get; set; }

        public async Task LoginUserAsync()
        {
            await WebRequestExecuter.Execute(_accessTokenManager.LoginAsync, AuthenticateSucceed, ex => Task.CompletedTask);
        }

        public Task LogoutAsync()
        {
            _accessTokenManager.Logout();
            _applicationContext.ClearLoginInfo();
            _dataStorageService.ClearSessionPersistance();
            return Task.CompletedTask;
        }

        private async Task AuthenticateSucceed(AbpAuthenticateResultModel result)
        {
            AuthenticateResultModel = result;

            if (AuthenticateResultModel.ShouldResetPassword)
            {
                await UserDialogsService.Instance.AlertError(L.Localize("LoginFailed") + " " + L.Localize("ChangePasswordToLogin"));
                return;
            }

            if (AuthenticateResultModel.RequiresTwoFactorVerification)
            {
                _navigationService.NavigateTo(NavigationUrlConsts.SendTwoFactorCode);
                return;
            }

            await _dataStorageService.StoreAuthenticateResultAsync(AuthenticateResultModel);

            AbpAuthenticateModel.Password = null;
            await SetCurrentUserInfoAsync();
            await UserConfigurationManager.GetAsync();
            _navigationService.NavigateTo(NavigationUrlConsts.Tenants);
        }

        private async Task SetCurrentUserInfoAsync()
        {
            await WebRequestExecuter.Execute(async () =>
                await _sessionAppService.GetCurrentLoginInformations(), GetCurrentUserInfoExecuted);
        }

        private async Task GetCurrentUserInfoExecuted(GetCurrentLoginInformationsOutput result)
        {
            _applicationContext.SetLoginInfo(result);

            await _dataStorageService.StoreLoginInformationAsync(_applicationContext.LoginInfo);
        }
    }
}