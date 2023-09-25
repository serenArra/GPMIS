using Abp.Modules;
using Abp.Reflection.Extensions;

namespace MFAE.Jobs
{
    public class JobsCoreSharedModule : AbpModule
    {
        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention(typeof(JobsCoreSharedModule).GetAssembly());
        }
    }
}