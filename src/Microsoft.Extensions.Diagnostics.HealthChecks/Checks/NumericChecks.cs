// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using Microsoft.Extensions.Diagnostics.HealthChecks;

// We put these in Microsoft.Extensions.DependencyInjection because the user is much more likely to have it in scope when
// adding Health Checks. The fact that these hang off IHealthCheckBuilder mean they still won't spam the IServiceCollection with extension methods.
namespace Microsoft.Extensions.DependencyInjection
{
    public static partial class IHealthCheckBuilderExtensions
    {
        // Numeric checks

        public static IHealthCheckBuilder AddMinValueCheck<T>(this IHealthCheckBuilder builder, string name, T minValue, Func<T> currentValueFunc)
            where T : IComparable<T>
        {
            builder = builder ?? throw new ArgumentNullException(nameof(builder));
            name = !string.IsNullOrEmpty(name) ? name : throw new ArgumentException("Value cannot be an empty string.", nameof(name));
            currentValueFunc = currentValueFunc ?? throw new ArgumentNullException(nameof(currentValueFunc));

            builder.AddCheck(name, () =>
            {
                var currentValue = currentValueFunc();
                var description = $"min={minValue}, current={currentValue}";
                var data = new Dictionary<string, object> { { "min", minValue }, { "current", currentValue } };
                if(currentValue.CompareTo(minValue) >= 0)
                {
                    return HealthCheckResult.Healthy(description, data);
                }
                else
                {
                    return HealthCheckResult.Unhealthy(description, data);
                }
            });

            return builder;
        }

        public static IHealthCheckBuilder AddMaxValueCheck<T>(this IHealthCheckBuilder builder, string name, T maxValue, Func<T> currentValueFunc)
            where T : IComparable<T>
        {
            builder = builder ?? throw new ArgumentNullException(nameof(builder));
            name = !string.IsNullOrEmpty(name) ? name : throw new ArgumentException("Value cannot be an empty string.", nameof(name));
            currentValueFunc = currentValueFunc ?? throw new ArgumentNullException(nameof(currentValueFunc));

            builder.AddCheck(name, () =>
            {
                var currentValue = currentValueFunc();
                var description = $"max={maxValue}, current={currentValue}";
                var data = new Dictionary<string, object> { { "max", maxValue }, { "current", currentValue } };
                if(currentValue.CompareTo(maxValue) <= 0)
                {
                    return HealthCheckResult.Healthy(description, data);
                }
                else
                {
                    return HealthCheckResult.Unhealthy(description, data);
                }
            });

            return builder;
        }
    }
}
