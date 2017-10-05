using Microsoft.Extensions.DependencyInjection;

namespace Microsoft.Extensions.Diagnostics.HealthChecks
{
    internal class HealthChecksBuilder : IHealthChecksBuilder
    {
        public IServiceCollection Services { get; }

        public HealthChecksBuilder(IServiceCollection services)
        {
            Services = services;
        }
    }
}
