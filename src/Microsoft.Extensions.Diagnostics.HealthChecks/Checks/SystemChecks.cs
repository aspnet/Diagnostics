// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Diagnostics;
using Microsoft.Extensions.Diagnostics.HealthChecks;

// We put these in Microsoft.Extensions.DependencyInjection because the user is much more likely to have it in scope when
// adding Health Checks. The fact that these hang off IHealthCheckBuilder mean they still won't spam the IServiceCollection with extension methods.
namespace Microsoft.Extensions.DependencyInjection
{
    public static partial class IHealthCheckBuilderExtensions
    {
        // System checks
        public static IHealthCheckBuilder AddPrivateMemorySizeCheck(this IHealthCheckBuilder builder, long maxSize)
            => AddMaxValueCheck(builder, $"PrivateMemorySize({maxSize})", maxSize, () => Process.GetCurrentProcess().PrivateMemorySize64);

        public static IHealthCheckBuilder AddVirtualMemorySizeCheck(this IHealthCheckBuilder builder, long maxSize)
            => AddMaxValueCheck(builder, $"VirtualMemorySize({maxSize})", maxSize, () => Process.GetCurrentProcess().VirtualMemorySize64);

        public static IHealthCheckBuilder AddWorkingSetCheck(this IHealthCheckBuilder builder, long maxSize)
            => AddMaxValueCheck(builder, $"WorkingSet({maxSize})", maxSize, () => Process.GetCurrentProcess().WorkingSet64);
    }
}
