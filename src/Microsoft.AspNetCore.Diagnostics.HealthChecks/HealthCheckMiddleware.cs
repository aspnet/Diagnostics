using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Microsoft.AspNetCore.Diagnostics.HealthChecks
{
    public class HealthCheckMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly HealthCheckOptions _healthCheckOptions;
        private readonly IHealthCheckService _healthCheckService;

        public HealthCheckMiddleware(RequestDelegate next, HealthCheckOptions healthCheckOptions, IHealthCheckService healthCheckService)
        {
            _next = next;
            _healthCheckOptions = healthCheckOptions;
            _healthCheckService = healthCheckService;
        }

        /// <summary>
        /// Process an individual request.
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public async Task InvokeAsync(HttpContext context)
        {
            // TODO: Configurable
            if (context.Request.Path == _healthCheckOptions.Path)
            {
                // TODO: Caching
                // Get results
                var result = await _healthCheckService.CheckHealthAsync(context.RequestAborted);

                // Map status to response code
                switch (result.Status)
                {
                    case HealthCheckStatus.Failed:
                        // REVIEW: Maybe we shouldn't distinguish between failed health checks and unhealthy?
                        context.Response.StatusCode = StatusCodes.Status500InternalServerError;
                        break;
                    case HealthCheckStatus.Unhealthy:
                        context.Response.StatusCode = StatusCodes.Status503ServiceUnavailable;
                        break;
                    case HealthCheckStatus.Degraded:
                        // Degraded doesn't mean unhealthy so we return 200, but the content will contain more details
                        context.Response.StatusCode = StatusCodes.Status200OK;
                        break;
                    case HealthCheckStatus.Healthy:
                        context.Response.StatusCode = StatusCodes.Status200OK;
                        break;
                    default:
                        break;
                }

                // Render results to JSON
                // TODO: Authentication policy to control this? Sensitive data hiding? etc.
                var json = new JObject(
                    new JProperty("status", result.Status.ToString()),
                    new JProperty("results", new JObject(result.Results.Select(pair =>
                        new JProperty(pair.Key, new JObject(
                            new JProperty("status", pair.Value.Status.ToString()),
                            new JProperty("description", pair.Value.Description),
                            new JProperty("data", new JObject(pair.Value.Data.Select(p => new JProperty(p.Key, p.Value))))))))));
                await context.Response.WriteAsync(json.ToString(Formatting.None));
            }
            else
            {
                await _next(context);
            }
        }
    }
}
