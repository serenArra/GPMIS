using Abp.AspNetZeroCore;
using Abp.Configuration.Startup;
using Abp.Dependency;
using Abp.Modules;
using Abp.Reflection.Extensions;
using Abp.Threading.BackgroundWorkers;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using MFAE.Jobs.Auditing;
using MFAE.Jobs.Authorization.Users.Password;
using MFAE.Jobs.Configuration;
using MFAE.Jobs.EntityFrameworkCore;
using MFAE.Jobs.MultiTenancy;
using MFAE.Jobs.Web.Areas.App.Startup;

namespace MFAE.Jobs.Web.Startup
{
    [DependsOn(
        typeof(JobsWebCoreModule)
    )]
    public class JobsWebMvcModule : AbpModule
    {
        private readonly IConfigurationRoot _appConfiguration;

        public JobsWebMvcModule(IWebHostEnvironment env)
        {
            _appConfiguration = env.GetAppConfiguration();
        }

        public override void PreInitialize()
        {
            Configuration.Modules.AbpWebCommon().MultiTenancy.DomainFormat = _appConfiguration["App:WebSiteRootAddress"] ?? "https://localhost:44302/";
            Configuration.Modules.AspNetZero().LicenseCode = _appConfiguration["AbpZeroLicenseCode"];
            Configuration.Navigation.Providers.Add<AppNavigationProvider>();

            IocManager.Register<DashboardViewConfiguration>();
        }

        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention(typeof(JobsWebMvcModule).GetAssembly());
        }

        public override void PostInitialize()
        {
            if (!IocManager.Resolve<IMultiTenancyConfig>().IsEnabled)
            {
                return;
            }

            using (var scope = IocManager.CreateScope())
            {
                if (!scope.Resolve<DatabaseCheckHelper>().Exist(_appConfiguration["ConnectionStrings:Default"]))
                {
                    return;
                }
            }

            var workManager = IocManager.Resolve<IBackgroundWorkerManager>();
            workManager.Add(IocManager.Resolve<SubscriptionExpirationCheckWorker>());
            workManager.Add(IocManager.Resolve<SubscriptionExpireEmailNotifierWorker>());
            workManager.Add(IocManager.Resolve<SubscriptionPaymentNotCompletedEmailNotifierWorker>());

            var expiredAuditLogDeleterWorker = IocManager.Resolve<ExpiredAuditLogDeleterWorker>();
            if (Configuration.Auditing.IsEnabled && expiredAuditLogDeleterWorker.IsEnabled)
            {
                workManager.Add(expiredAuditLogDeleterWorker);
            }

            workManager.Add(IocManager.Resolve<PasswordExpirationBackgroundWorker>());
        }
    }
}