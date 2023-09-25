using Microsoft.AspNetCore.Mvc;
using MFAE.Jobs.Web.Controllers;

namespace MFAE.Jobs.Web.Public.Controllers
{
    public class AboutController : JobsControllerBase
    {
        public ActionResult Index()
        {
            return View();
        }
    }
}