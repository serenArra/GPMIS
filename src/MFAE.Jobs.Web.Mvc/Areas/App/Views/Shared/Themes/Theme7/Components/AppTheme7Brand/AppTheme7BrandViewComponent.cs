using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using MFAE.Jobs.Web.Areas.App.Models.Layout;
using MFAE.Jobs.Web.Session;
using MFAE.Jobs.Web.Views;

namespace MFAE.Jobs.Web.Areas.App.Views.Shared.Themes.Theme7.Components.AppTheme7Brand
{
    public class AppTheme7BrandViewComponent : JobsViewComponent
    {
        private readonly IPerRequestSessionCache _sessionCache;

        public AppTheme7BrandViewComponent(IPerRequestSessionCache sessionCache)
        {
            _sessionCache = sessionCache;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var headerModel = new HeaderViewModel
            {
                LoginInformations = await _sessionCache.GetCurrentLoginInformationsAsync(),
            };

            return View(headerModel);
        }
    }
}
