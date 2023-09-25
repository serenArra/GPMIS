using Abp.Modules;
using Abp.Reflection.Extensions;

namespace MFAE.Jobs
{
    public class JobsClientModule : AbpModule
    {
        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention(typeof(JobsClientModule).GetAssembly());
        }
    }
}
