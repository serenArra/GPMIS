using Abp.Application.Services;
using MFAE.Jobs.Authorization.Users.Dto;
using System.Threading.Tasks;

namespace MFAE.Jobs.Authorization.Welcomes
{
    public interface IWelcomeAppService: IApplicationService
    {
        Task<WelcomeUserDto> GetWelcomeUserForView();

    }
}
