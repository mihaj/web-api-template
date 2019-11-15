using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace WebApi.Extensions.HealthChecks
{
    public class DummyHealthCheck : IHealthCheck
    {
        public Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = default)
        {
            //Dummy logic
            var heathCheckOk = true;

            if (heathCheckOk)
            {
                return Task.FromResult(HealthCheckResult.Healthy("Health check OK."));
            }
            else
            {
                return Task.FromResult(
                    HealthCheckResult.Unhealthy("Health check failed."));
            }
        }
    }
}
