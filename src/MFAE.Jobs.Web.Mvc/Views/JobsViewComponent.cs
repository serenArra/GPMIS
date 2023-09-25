using Abp.AspNetCore.Mvc.ViewComponents;

namespace MFAE.Jobs.Web.Views
{
    public abstract class JobsViewComponent : AbpViewComponent
    {
        protected JobsViewComponent()
        {
            LocalizationSourceName = JobsConsts.LocalizationSourceName;
        }
    }
}