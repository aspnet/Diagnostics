using System;
using Microsoft.AspNetCore.Http;

namespace Microsoft.AspNetCore.Diagnostics.HealthChecks
{
    public class HealthCheckOptions
    {
        public PathString Path { get; set; }
        public TimeSpan CacheDuration { get; set; }
    }
}
