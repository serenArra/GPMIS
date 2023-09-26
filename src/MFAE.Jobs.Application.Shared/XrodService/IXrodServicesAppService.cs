using Abp.Application.Services;
using MFAE.Jobs.XrodService.Dto;
using System.Threading.Tasks;

namespace MFAE.Jobs.XrodService
{
    public interface IXrodServicesAppService : IApplicationService
    {
        Task<CitizenInformation> GetCitizenInformation(string CardNo);
    }
}
