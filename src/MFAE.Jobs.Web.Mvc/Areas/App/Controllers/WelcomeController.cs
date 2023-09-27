using Abp.AspNetCore.Mvc.Authorization;
using Microsoft.AspNetCore.Mvc;
using MFAE.Jobs.Web.Controllers;
using MFAE.Jobs.Web.Areas.App.Models.Welcomes;
using System.Threading.Tasks;
using MFAE.Jobs.Authorization.Welcomes;

namespace MFAE.Jobs.Web.Areas.App.Controllers
{
    [Area("App")]
    [AbpMvcAuthorize]
    public class WelcomeController : JobsControllerBase
    {
        private readonly IWelcomeAppService _welcomeAppService;
         
        public WelcomeController(IWelcomeAppService welcomeAppService)
        {
            _welcomeAppService = welcomeAppService;
        }

        public async Task<ActionResult> Index()
        {
            var getWelcomeUserForView = await _welcomeAppService.GetWelcomeUserForView();
            var model = new WelcomesViewModel()
            {
                User = getWelcomeUserForView.User,
                JobAdvertisement = getWelcomeUserForView.JobAdvertisement
            };

            return View(model);
        }
    }
}