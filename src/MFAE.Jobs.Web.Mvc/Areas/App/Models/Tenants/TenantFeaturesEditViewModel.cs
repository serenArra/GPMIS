using Abp.AutoMapper;
using MFAE.Jobs.MultiTenancy;
using MFAE.Jobs.MultiTenancy.Dto;
using MFAE.Jobs.Web.Areas.App.Models.Common;

namespace MFAE.Jobs.Web.Areas.App.Models.Tenants
{
    [AutoMapFrom(typeof (GetTenantFeaturesEditOutput))]
    public class TenantFeaturesEditViewModel : GetTenantFeaturesEditOutput, IFeatureEditViewModel
    {
        public Tenant Tenant { get; set; }
    }
}