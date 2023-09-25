using Abp.Modules;
using MFAE.Jobs.Test.Base;

namespace MFAE.Jobs.Tests
{
    [DependsOn(typeof(JobsTestBaseModule))]
    public class JobsTestModule : AbpModule
    {
       
    }
}
