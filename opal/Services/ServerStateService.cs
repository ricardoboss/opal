using Microsoft.Extensions.Hosting;

using opal.Models;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

using yapsi;

namespace opal.Services
{
    public abstract class ServerStateService : BackgroundService
    {
        private readonly IContract<ServerState> _serverStateContract;
        private readonly IContract<Exception> _exceptionContract;

        public ServerStateService(IContract<ServerState> serverStateContract, IContract<Exception> exceptionContract)
        {
            _serverStateContract = serverStateContract;
            _exceptionContract = exceptionContract;

            _serverStateContract.Publish(ServerState.Stopped);
        }

        public override async Task StartAsync(CancellationToken cancellationToken)
        {
            _serverStateContract.Publish(ServerState.Starting);

            await PrepareAsync(cancellationToken);

            await base.StartAsync(cancellationToken);
        }

        public override async Task StopAsync(CancellationToken cancellationToken)
        {
            _serverStateContract.Publish(ServerState.Stopping);

            await base.StopAsync(cancellationToken);
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _serverStateContract.Publish(ServerState.Running);

            try
            {
                await RunAsync(stoppingToken);

                _serverStateContract.Publish(ServerState.Stopped);
            }
            catch (Exception ex)
            {
                _exceptionContract.Publish(ex);
                _serverStateContract.Publish(ServerState.Crashed);
            }
            finally
            {
                await CleanupAsync(App.Current.StoppingToken);
            }
        }

        protected abstract Task PrepareAsync(CancellationToken stoppingToken);
        protected abstract Task RunAsync(CancellationToken stoppingToken);
        protected abstract Task CleanupAsync(CancellationToken stoppingToken);
    }
}
