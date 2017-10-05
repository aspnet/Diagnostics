// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Diagnostics.HealthChecks;

// We put these in Microsoft.Extensions.DependencyInjection because the user is much more likely to have it in scope when
// adding Health Checks. The fact that these hang off IHealthCheckBuilder mean they still won't spam the IServiceCollection with extension methods.
namespace Microsoft.Extensions.DependencyInjection
{
    public static partial class IHealthCheckBuilderExtensions
    {
        // Lambda versions of AddCheck for Func/Func<Task>
        public static IHealthCheckBuilder AddCheck(this IHealthCheckBuilder builder, string name, Func<HealthCheckResult> check)
        {
            builder = builder ?? throw new ArgumentNullException(nameof(builder));

            return builder.AddCheck(new HealthCheck(name, _ => Task.FromResult(check())));
        }

        public static IHealthCheckBuilder AddCheck(this IHealthCheckBuilder builder, string name, Func<CancellationToken, HealthCheckResult> check)
        {
            builder = builder ?? throw new ArgumentNullException(nameof(builder));

            return builder.AddCheck(new HealthCheck(name, ct => Task.FromResult(check(ct))));
        }

        public static IHealthCheckBuilder AddCheck(this IHealthCheckBuilder builder, string name, Func<Task<HealthCheckResult>> check)
        {
            builder = builder ?? throw new ArgumentNullException(nameof(builder));

            return builder.AddCheck(new HealthCheck(name, _ => check()));
        }

        public static IHealthCheckBuilder AddCheck(this IHealthCheckBuilder builder, string name, Func<CancellationToken, Task<HealthCheckResult>> check)
        {
            builder = builder ?? throw new ArgumentNullException(nameof(builder));

            return builder.AddCheck(new HealthCheck(name, check));
        }

        // Instance version of AddCheck
        public static IHealthCheckBuilder AddCheck(this IHealthCheckBuilder builder, IHealthCheck check)
        {
            builder = builder ?? throw new ArgumentNullException(nameof(builder));

            builder.Services.AddSingleton(check);
            return builder;
        }

        // Type versions of AddCheck

        public static IHealthCheckBuilder AddCheck<TCheck>(this IHealthCheckBuilder builder) where TCheck : class, IHealthCheck
        {
            builder = builder ?? throw new ArgumentNullException(nameof(builder));

            builder.Services.AddSingleton<TCheck>();
            return builder;
        }
    }
}
