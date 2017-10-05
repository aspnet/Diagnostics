using System.Collections.Generic;
using Xunit;

namespace Microsoft.Extensions.Diagnostics.HealthChecks.Test
{
    public class CompositeHealthCheckResultTest
    {
        [Theory]
        [InlineData(HealthCheckStatus.Healthy)]
        [InlineData(HealthCheckStatus.Degraded)]
        [InlineData(HealthCheckStatus.Unhealthy)]
        [InlineData(HealthCheckStatus.Failed)]
        public void Status_MatchesWorstStatusInResults(HealthCheckStatus statusValue)
        {
            var result = new CompositeHealthCheckResult(new Dictionary<string, HealthCheckResult>()
            {
                {"Foo", HealthCheckResult.Healthy() },
                {"Bar", HealthCheckResult.Healthy() },
                {"Baz", new HealthCheckResult(statusValue, exception: null, description: null, data: null) },
                {"Quick", HealthCheckResult.Healthy() },
                {"Quack", HealthCheckResult.Healthy() },
                {"Quock", HealthCheckResult.Healthy() },
            });

            Assert.Equal(statusValue, result.Status);
        }
    }
}
