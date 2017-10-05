using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace Microsoft.Extensions.Diagnostics.HealthChecks.Test
{
    public class HealthChecksBuilderTests
    {
        [Fact]
        public void ChecksCanBeRegisteredInMultipleCallsToAddHealthChecks()
        {
            var services = new ServiceCollection();
            services.AddHealthChecks()
                .AddCheck("Foo", () => Task.FromResult(HealthCheckResult.Healthy()));
            services.AddHealthChecks()
                .AddCheck("Bar", () => Task.FromResult(HealthCheckResult.Healthy()));

            // Act
            var healthCheckService = services.BuildServiceProvider().GetRequiredService<IHealthCheckService>();

            // Assert
            Assert.Collection(healthCheckService.Checks,
                actual => Assert.Equal("Foo", actual.Key),
                actual => Assert.Equal("Bar", actual.Key));
        }
    }
}
