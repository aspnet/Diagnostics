using Microsoft.Extensions.DependencyInjection;

namespace Microsoft.Extensions.Diagnostics.HealthChecks
{
    internal class HealthCheckBuilder : IHealthCheckBuilder
    {
        public IServiceCollection Services { get; }

        public HealthCheckBuilder(IServiceCollection services)
        {
            Services = services;
        }
    }
}
