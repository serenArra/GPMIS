using Abp.AspNetCore.Mvc.Authorization;
using Microsoft.AspNetCore.Mvc;
using MFAE.Jobs.Authorization;
using MFAE.Jobs.Caching;
using MFAE.Jobs.Web.Areas.App.Models.Maintenance;
using MFAE.Jobs.Web.Controllers;

namespace MFAE.Jobs.Web.Areas.App.Controllers
{
    [Area("App")]
    [AbpMvcAuthorize(AppPermissions.Pages_Administration_Host_Maintenance)]
    public class MaintenanceController : JobsControllerBase
    {
        private readonly ICachingAppService _cachingAppService;

        public MaintenanceController(ICachingAppService cachingAppService)
        {
            _cachingAppService = cachingAppService;
        }

        public ActionResult Index()
        {
            var model = new MaintenanceViewModel
            {
                Caches = _cachingAppService.GetAllCaches().Items,
                CanClearAllCaches = _cachingAppService.CanClearAllCaches()
            };

            return View(model);
        }
    }
}