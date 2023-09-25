using Abp.Application.Services;
using MFAE.Jobs.Dto;
using MFAE.Jobs.Logging.Dto;

namespace MFAE.Jobs.Logging
{
    public interface IWebLogAppService : IApplicationService
    {
        GetLatestWebLogsOutput GetLatestWebLogs();

        FileDto DownloadWebLogs();
    }
}
