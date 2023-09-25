using Abp.AutoMapper;
using MFAE.Jobs.MultiTenancy.Dto;

namespace MFAE.Jobs.Web.Models.TenantRegistration
{
    [AutoMapFrom(typeof(EditionsSelectOutput))]
    public class EditionsSelectViewModel : EditionsSelectOutput
    {
    }
}
