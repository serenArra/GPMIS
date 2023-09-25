using Abp.Modules;
using Abp.Reflection.Extensions;

namespace MFAE.Jobs
{
    [DependsOn(typeof(JobsCoreSharedModule))]
    public class JobsApplicationSharedModule : AbpModule
    {
        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention(typeof(JobsApplicationSharedModule).GetAssembly());
        }
    }
}