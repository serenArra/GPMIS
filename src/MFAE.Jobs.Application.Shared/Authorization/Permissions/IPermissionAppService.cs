using Abp.Application.Services;
using Abp.Application.Services.Dto;
using MFAE.Jobs.Authorization.Permissions.Dto;

namespace MFAE.Jobs.Authorization.Permissions
{
    public interface IPermissionAppService : IApplicationService
    {
        ListResultDto<FlatPermissionWithLevelDto> GetAllPermissions();
    }
}
