using Microsoft.Extensions.Diagnostics.HealthChecks;
using System.Reflection;


namespace Rafeek.Application.HealthCheck
{
    public class ApplicationHealthCheck : IHealthCheck
    {
        public Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = new CancellationToken())
        {
            var assembly = Assembly.GetEntryAssembly();
            var versionNumber = assembly.GetName().Version;

            return Task.FromResult(HealthCheckResult.Healthy(description: $"Build {versionNumber}"));
        }
    }
}
