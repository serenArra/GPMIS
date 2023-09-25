using Abp.AspNetCore.Mvc.Views;

namespace MFAE.Jobs.Web.Views
{
    public abstract class JobsRazorPage<TModel> : AbpRazorPage<TModel>
    {
        protected JobsRazorPage()
        {
            LocalizationSourceName = JobsConsts.LocalizationSourceName;
        }
    }
}
