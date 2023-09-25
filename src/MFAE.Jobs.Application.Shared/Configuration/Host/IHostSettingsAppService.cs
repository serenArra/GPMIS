﻿using System.Threading.Tasks;
using Abp.Application.Services;
using MFAE.Jobs.Configuration.Host.Dto;

namespace MFAE.Jobs.Configuration.Host
{
    public interface IHostSettingsAppService : IApplicationService
    {
        Task<HostSettingsEditDto> GetAllSettings();

        Task UpdateAllSettings(HostSettingsEditDto input);

        Task SendTestEmail(SendTestEmailInput input);
    }
}
