using Abp.AspNetCore.Mvc.Views;
using Abp.Runtime.Session;
using Microsoft.AspNetCore.Mvc.Razor.Internal;

namespace MFAE.Jobs.Web.Public.Views
{
    public abstract class JobsRazorPage<TModel> : AbpRazorPage<TModel>
    {
        [RazorInject]
        public IAbpSession AbpSession { get; set; }

        protected JobsRazorPage()
        {
            LocalizationSourceName = JobsConsts.LocalizationSourceName;
        }
    }
}
