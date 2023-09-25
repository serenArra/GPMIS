using Abp.Auditing;
using MFAE.Jobs.Configuration.Dto;

namespace MFAE.Jobs.Configuration.Tenants.Dto
{
    public class TenantEmailSettingsEditDto : EmailSettingsEditDto
    {
        public bool UseHostDefaultEmailSettings { get; set; }
    }
}