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

        public ServerStateService(IContract<ServerState> serverStateContract)
        {
            _serverStateContract = serverStateContract;
        }

        public override async Task StartAsync(CancellationToken cancellationToken)
        {
            _serverStateContract.Publish(ServerState.Starting);

            await base.StartAsync(cancellationToken);
        }

        public override async Task StopAsync(CancellationToken cancellationToken)
        {
            await base.StopAsync(cancellationToken);

            _serverStateContract.Publish(ServerState.Stopped);
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _serverStateContract.Publish(ServerState.Running);

            await RunAsync(stoppingToken);

            _serverStateContract.Publish(ServerState.Stopping);
        }

        protected abstract Task RunAsync(CancellationToken stoppingToken);
    }
}
