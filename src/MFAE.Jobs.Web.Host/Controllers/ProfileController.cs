using Abp.AspNetCore.Mvc.Authorization;
using MFAE.Jobs.Authorization.Users.Profile;
using MFAE.Jobs.Graphics;
using MFAE.Jobs.Storage;

namespace MFAE.Jobs.Web.Controllers
{
    [AbpMvcAuthorize]
    public class ProfileController : ProfileControllerBase
    {
        public ProfileController(
            ITempFileCacheManager tempFileCacheManager,
            IProfileAppService profileAppService,
            IImageValidator imageValidator) :
            base(tempFileCacheManager, profileAppService, imageValidator)
        {
        }
    }
}