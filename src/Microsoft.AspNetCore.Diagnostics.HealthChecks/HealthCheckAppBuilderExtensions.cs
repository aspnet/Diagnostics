using System;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.AspNetCore.Http;

namespace Microsoft.AspNetCore.Builder
{
    public static class HealthCheckAppBuilderExtensions
    {
        public static IApplicationBuilder UseHealthChecks(this IApplicationBuilder applicationBuilder, PathString path) =>
            UseHealthChecks(applicationBuilder, path, TimeSpan.Zero);

        public static IApplicationBuilder UseHealthChecks(this IApplicationBuilder applicationBuilder, PathString path, TimeSpan cacheDuration)
        {
            return applicationBuilder.UseMiddleware<HealthCheckMiddleware>(new HealthCheckOptions()
            {
                Path = path,
                CacheDuration = cacheDuration
            });
        }
    }
}
