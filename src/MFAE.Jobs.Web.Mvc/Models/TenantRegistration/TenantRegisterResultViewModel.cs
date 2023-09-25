using Abp.AutoMapper;
using MFAE.Jobs.MultiTenancy.Dto;

namespace MFAE.Jobs.Web.Models.TenantRegistration
{
    [AutoMapFrom(typeof(RegisterTenantOutput))]
    public class TenantRegisterResultViewModel : RegisterTenantOutput
    {
        public string TenantLoginAddress { get; set; }
    }
}