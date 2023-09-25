using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using MFAE.Jobs.EntityFrameworkCore;

namespace MFAE.Jobs.HealthChecks
{
    public class JobsDbContextHealthCheck : IHealthCheck
    {
        private readonly DatabaseCheckHelper _checkHelper;

        public JobsDbContextHealthCheck(DatabaseCheckHelper checkHelper)
        {
            _checkHelper = checkHelper;
        }

        public Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = new CancellationToken())
        {
            if (_checkHelper.Exist("db"))
            {
                return Task.FromResult(HealthCheckResult.Healthy("JobsDbContext connected to database."));
            }

            return Task.FromResult(HealthCheckResult.Unhealthy("JobsDbContext could not connect to database"));
        }
    }
}
