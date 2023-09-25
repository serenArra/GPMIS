using Abp.AutoMapper;
using Abp.Modules;
using Abp.Reflection.Extensions;
using MFAE.Jobs.Authorization;

namespace MFAE.Jobs
{
    /// <summary>
    /// Application layer module of the application.
    /// </summary>
    [DependsOn(
        typeof(JobsApplicationSharedModule),
        typeof(JobsCoreModule)
        )]
    public class JobsApplicationModule : AbpModule
    {
        public override void PreInitialize()
        {
            //Adding authorization providers
            Configuration.Authorization.Providers.Add<AppAuthorizationProvider>();

            //Adding custom AutoMapper configuration
            Configuration.Modules.AbpAutoMapper().Configurators.Add(CustomDtoMapper.CreateMappings);
        }

        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention(typeof(JobsApplicationModule).GetAssembly());
        }
    }
}