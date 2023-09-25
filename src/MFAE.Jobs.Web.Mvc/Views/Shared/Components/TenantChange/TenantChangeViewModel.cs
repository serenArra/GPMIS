using Abp.AutoMapper;
using MFAE.Jobs.Sessions.Dto;

namespace MFAE.Jobs.Web.Views.Shared.Components.TenantChange
{
    [AutoMapFrom(typeof(GetCurrentLoginInformationsOutput))]
    public class TenantChangeViewModel
    {
        public TenantLoginInfoDto Tenant { get; set; }
    }
}