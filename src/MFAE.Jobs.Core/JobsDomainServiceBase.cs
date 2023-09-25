using Abp.Domain.Services;

namespace MFAE.Jobs
{
    public abstract class JobsDomainServiceBase : DomainService
    {
        /* Add your common members for all your domain services. */

        protected JobsDomainServiceBase()
        {
            LocalizationSourceName = JobsConsts.LocalizationSourceName;
        }
    }
}
