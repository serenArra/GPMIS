using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using MFAE.Jobs.Web.Areas.App.Models.Layout;
using MFAE.Jobs.Web.Views;

namespace MFAE.Jobs.Web.Areas.App.Views.Shared.Components.AppToggleDarkMode
{
    public class AppToggleDarkModeViewComponent : JobsViewComponent
    {
        public Task<IViewComponentResult> InvokeAsync(string cssClass, bool isDarkModeActive)
        {
            return Task.FromResult<IViewComponentResult>(View(new ToggleDarkModeViewModel(cssClass, isDarkModeActive)));
        }
    }
}