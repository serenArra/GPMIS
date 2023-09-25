using System.Threading.Tasks;
using Abp.Application.Services;
using MFAE.Jobs.Sessions.Dto;

namespace MFAE.Jobs.Sessions
{
    public interface ISessionAppService : IApplicationService
    {
        Task<GetCurrentLoginInformationsOutput> GetCurrentLoginInformations();

        Task<UpdateUserSignInTokenOutput> UpdateUserSignInToken();
    }
}
