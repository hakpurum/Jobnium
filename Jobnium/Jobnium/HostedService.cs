using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;

namespace Jobnium
{
    public abstract class HostedService : IHostedService, IDisposable
    {
        private Task _currentTask;
        private readonly CancellationTokenSource _cancellationTokenSource = new CancellationTokenSource();

        protected abstract Task ExecuteAsync(CancellationToken cToken);

        public virtual Task StartAsync(CancellationToken cancellationToken)
        {
            _currentTask = ExecuteAsync(_cancellationTokenSource.Token);
            return _currentTask.IsCompleted ? _currentTask : Task.CompletedTask;
        }

        public virtual async Task StopAsync(CancellationToken cancellationToken)
        {
            if (_currentTask == null)
                return;

            try
            {
                _cancellationTokenSource.Cancel();
            }
            finally
            {
                await Task.WhenAny(_currentTask, Task.Delay(Timeout.Infinite, cancellationToken));
            }
        }
        public virtual void Dispose()
        {
            _cancellationTokenSource.Cancel();
        }
    }
}