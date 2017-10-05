using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.AspNetCore.Http;

namespace Microsoft.AspNetCore.Builder
{
    public static class HealthCheckAppBuilderExtensions
    {
        public static IApplicationBuilder UseHealthChecks(this IApplicationBuilder applicationBuilder, PathString path)
        {
            return applicationBuilder.UseMiddleware<HealthCheckMiddleware>(new HealthCheckOptions()
            {
                Path = path
            });
        }
    }
}
