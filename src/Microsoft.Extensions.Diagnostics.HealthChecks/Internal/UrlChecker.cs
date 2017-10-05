// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace Microsoft.Extensions.Diagnostics.HealthChecks.Internal
{
    public class UrlChecker
    {
        private readonly Func<HttpResponseMessage, Task<HealthCheckResult>> _checkFunc;
        private readonly string _url;

        public UrlChecker(Func<HttpResponseMessage, Task<HealthCheckResult>> checkFunc, string url)
        {
            checkFunc = checkFunc ?? throw new ArgumentNullException(nameof(checkFunc));
            url = !string.IsNullOrEmpty(url) ? url : throw new ArgumentException("Value cannot be an empty string.", nameof(url));

            _checkFunc = checkFunc;
            _url = url;
        }

        public async Task<HealthCheckResult> CheckAsync()
        {
            using (var httpClient = CreateHttpClient())
            {
                try
                {
                    var response = await httpClient.GetAsync(_url).ConfigureAwait(false);
                    return await _checkFunc(response);
                }
                catch (Exception ex)
                {
                    var data = new Dictionary<string, object> { { "url", _url } };
                    return HealthCheckResult.Unhealthy($"Exception during check: {ex.GetType().FullName}", ex, data);
                }
            }
        }

        private HttpClient CreateHttpClient()
        {
            var httpClient = GetHttpClient();
            httpClient.DefaultRequestHeaders.CacheControl = new CacheControlHeaderValue { NoCache = true };
            return httpClient;
        }

        public static HealthCheckResult DefaultUrlCheck(HttpResponseMessage response)
        {
            var data = new Dictionary<string, object>
            {
                { "url", response.RequestMessage.RequestUri.ToString() },
                { "status", (int)response.StatusCode },
                { "reason", response.ReasonPhrase }
            };
            var description = $"status code {response.StatusCode} ({(int)response.StatusCode})";
            return response.IsSuccessStatusCode ?
                HealthCheckResult.Healthy(description, data) :
                HealthCheckResult.Degraded(description, data);
        }

        protected virtual HttpClient GetHttpClient()
            => new HttpClient();
    }
}
