using Abp.AutoMapper;
using Abp.Modules;
using Abp.Reflection.Extensions;

namespace MFAE.Jobs.Startup
{
    [DependsOn(typeof(JobsCoreModule))]
    public class JobsGraphQLModule : AbpModule
    {
        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention(typeof(JobsGraphQLModule).GetAssembly());
        }

        public override void PreInitialize()
        {
            base.PreInitialize();

            //Adding custom AutoMapper configuration
            Configuration.Modules.AbpAutoMapper().Configurators.Add(CustomDtoMapper.CreateMappings);
        }
    }
}