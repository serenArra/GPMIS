﻿using System;
using System.Threading.Tasks;
using MFAE.Jobs.ApiClient.Models;

namespace MFAE.Jobs.ApiClient
{
    public interface IAccessTokenManager
    {
        string GetAccessToken();

        string GetEncryptedAccessToken();

        Task<AbpAuthenticateResultModel> LoginAsync();

        Task<(string accessToken, string encryptedAccessToken)> RefreshTokenAsync();

        void Logout();

        bool IsUserLoggedIn { get; }

        bool IsRefreshTokenExpired { get; }

        AbpAuthenticateResultModel AuthenticateResult { get; set; }

        DateTime AccessTokenRetrieveTime { get; set; }
    }
}