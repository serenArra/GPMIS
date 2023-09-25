using Abp.AutoMapper;
using MFAE.Jobs.Sessions.Dto;

namespace MFAE.Jobs.Models.Common
{
    [AutoMapFrom(typeof(GetCurrentLoginInformationsOutput)),
     AutoMapTo(typeof(GetCurrentLoginInformationsOutput))]
    public class CurrentLoginInformationPersistanceModel
    {
        public UserLoginInfoPersistanceModel User { get; set; }

        public TenantLoginInfoPersistanceModel Tenant { get; set; }

        public ApplicationInfoPersistanceModel Application { get; set; }
    }
}