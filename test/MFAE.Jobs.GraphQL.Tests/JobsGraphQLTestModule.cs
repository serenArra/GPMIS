using Abp.Modules;
using Abp.Reflection.Extensions;
using Castle.Windsor.MsDependencyInjection;
using Microsoft.Extensions.DependencyInjection;
using MFAE.Jobs.Configure;
using MFAE.Jobs.Startup;
using MFAE.Jobs.Test.Base;

namespace MFAE.Jobs.GraphQL.Tests
{
    [DependsOn(
        typeof(JobsGraphQLModule),
        typeof(JobsTestBaseModule))]
    public class JobsGraphQLTestModule : AbpModule
    {
        public override void PreInitialize()
        {
            IServiceCollection services = new ServiceCollection();
            
            services.AddAndConfigureGraphQL();

            WindsorRegistrationHelper.CreateServiceProvider(IocManager.IocContainer, services);
        }

        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention(typeof(JobsGraphQLTestModule).GetAssembly());
        }
    }
}