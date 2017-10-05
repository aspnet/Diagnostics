// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.Extensions.Diagnostics.HealthChecks.Internal;

// We put these in Microsoft.Extensions.DependencyInjection because the user is much more likely to have it in scope when
// adding Health Checks. The fact that these hang off IHealthCheckBuilder mean they still won't spam the IServiceCollection with extension methods.
namespace Microsoft.Extensions.DependencyInjection
{
    public static partial class IHealthCheckBuilderExtensions
    {
        // Default URL check
        public static IHealthCheckBuilder AddUrlCheck(this IHealthCheckBuilder builder, string url)
            => AddUrlCheck(builder, url, response => Task.FromResult(UrlChecker.DefaultUrlCheck(response)));

        // Func returning HealthCheckResult
        public static IHealthCheckBuilder AddUrlCheck(this IHealthCheckBuilder builder, string url,
                                                     Func<HttpResponseMessage, Task<HealthCheckResult>> checkFunc)
        {
            builder = builder ?? throw new ArgumentNullException(nameof(builder));
            url = !string.IsNullOrEmpty(url) ? url : throw new ArgumentException("Value cannot be an empty string.", nameof(url));
            checkFunc = checkFunc ?? throw new ArgumentNullException(nameof(checkFunc));

            var urlCheck = new UrlChecker(checkFunc, url);
            builder.AddCheck($"UrlCheck({url})", () => urlCheck.CheckAsync());
            return builder;
        }
    }
}
