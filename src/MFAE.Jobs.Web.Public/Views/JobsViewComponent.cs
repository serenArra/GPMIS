using Abp.AspNetCore.Mvc.ViewComponents;

namespace MFAE.Jobs.Web.Public.Views
{
    public abstract class JobsViewComponent : AbpViewComponent
    {
        protected JobsViewComponent()
        {
            LocalizationSourceName = JobsConsts.LocalizationSourceName;
        }
    }
}