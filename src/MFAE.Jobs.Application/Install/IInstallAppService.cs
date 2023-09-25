using System.Threading.Tasks;
using Abp.Application.Services;
using MFAE.Jobs.Install.Dto;

namespace MFAE.Jobs.Install
{
    public interface IInstallAppService : IApplicationService
    {
        Task Setup(InstallDto input);

        AppSettingsJsonDto GetAppSettingsJson();

        CheckDatabaseOutput CheckDatabase();
    }
}