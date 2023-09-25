using Abp.AutoMapper;
using Abp.Configuration.Startup;
using Abp.Modules;
using Abp.Reflection.Extensions;
using MFAE.Jobs.ApiClient;
using MFAE.Jobs.Mobile.MAUI.Core.ApiClient;

namespace MFAE.Jobs
{
    [DependsOn(typeof(JobsClientModule), typeof(AbpAutoMapperModule))]

    public class JobsMobileMAUIModule : AbpModule
    {
        public override void PreInitialize()
        {
            Configuration.Localization.IsEnabled = false;
            Configuration.BackgroundJobs.IsJobExecutionEnabled = false;

            Configuration.ReplaceService<IApplicationContext, MAUIApplicationContext>();
        }

        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention(typeof(JobsMobileMAUIModule).GetAssembly());
        }
    }
}