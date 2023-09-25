using Microsoft.Extensions.Configuration;

namespace MFAE.Jobs.Configuration
{
    public interface IAppConfigurationAccessor
    {
        IConfigurationRoot Configuration { get; }
    }
}
