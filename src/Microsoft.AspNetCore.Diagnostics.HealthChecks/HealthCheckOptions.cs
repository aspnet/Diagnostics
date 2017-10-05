using Microsoft.AspNetCore.Http;

namespace Microsoft.AspNetCore.Diagnostics.HealthChecks
{
    public class HealthCheckOptions
    {
        public PathString Path { get; set; }
    }
}
