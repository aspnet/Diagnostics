using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;

namespace HealthChecksSample
{
    internal class MemoryWasterService : IHostedService
    {
        private const int MaxMemoryToWaste = 1_000_000_000;
        private CancellationTokenSource _cancellationTokenSource = new CancellationTokenSource();
        private Task _backgroundTask;

        public Task StartAsync(CancellationToken cancellationToken)
        {
            _backgroundTask = Task.Run(() => BackgroundTask(_cancellationTokenSource.Token));
            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            _cancellationTokenSource.Cancel();
            return _backgroundTask;
        }

        private async Task BackgroundTask(CancellationToken token)
        {
            var buffers = new List<byte[]>();
            while(!token.IsCancellationRequested && Process.GetCurrentProcess().WorkingSet64 < MaxMemoryToWaste)
            {
                await Task.Delay(2500);
                buffers.Add(new byte[1024 * 1024 * 1024]);
            }
        }
    }
}
