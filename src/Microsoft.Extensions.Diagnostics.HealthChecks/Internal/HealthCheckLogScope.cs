using System;
using System.Collections;
using System.Collections.Generic;

namespace Microsoft.Extensions.Diagnostics.HealthChecks.Internal
{
    /// <summary>
    /// Represents a logging scope for a health check. This type is not designed for use in application code.
    /// </summary>
    public class HealthCheckLogScope : IReadOnlyList<KeyValuePair<string, object>>
    {
        /// <summary>
        /// Gets the name of the health check being executed.
        /// </summary>
        public string HealthCheckName { get; }

        int IReadOnlyCollection<KeyValuePair<string, object>>.Count { get; } = 1;

        KeyValuePair<string, object> IReadOnlyList<KeyValuePair<string, object>>.this[int index]
        {
            get
            {
                if (index == 0)
                {
                    return new KeyValuePair<string, object>(nameof(HealthCheckName), HealthCheckName);
                }

                throw new ArgumentOutOfRangeException(nameof(index));
            }
        }

        /// <summary>
        /// Creates a new instance of <see cref="HealthCheckLogScope"/> with the provided name.
        /// </summary>
        /// <param name="healthCheckName">The name of the health check being executed.</param>
        public HealthCheckLogScope(string healthCheckName)
        {
            HealthCheckName = healthCheckName;
        }

        IEnumerator<KeyValuePair<string, object>> IEnumerable<KeyValuePair<string, object>>.GetEnumerator()
        {
            yield return new KeyValuePair<string, object>(nameof(HealthCheckName), HealthCheckName);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return ((IEnumerable<KeyValuePair<string, object>>)this).GetEnumerator();
        }
    }
}
