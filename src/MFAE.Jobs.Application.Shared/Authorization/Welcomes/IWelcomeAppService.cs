using Abp.Application.Services;
using MFAE.Jobs.ApplicationForm.Dtos;
using MFAE.Jobs.Authorization.Users.Dto;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MFAE.Jobs.Authorization.Welcomes
{
    public interface IWelcomeAppService: IApplicationService
    {
        Task<WelcomeUserDto> GetWelcomeUserForView();

    }
}
