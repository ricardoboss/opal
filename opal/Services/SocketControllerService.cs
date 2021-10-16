using Microsoft.Extensions.DependencyInjection;
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
    internal class SocketControllerService : NoopBackgroundService
    {
        private readonly ISubscription<ServerCommand> _serverCommandSubscription;
        private readonly ISubscription<ServerState> _serverStateSubscription;
        private readonly IContract<Exception> _exceptionContract;

        private CancellationTokenSource? socketServiceCancellation;
        private SocketService? socketService;
        private ServerState lastState = ServerState.Stopped;

        public SocketControllerService(ISubscription<ServerCommand> serverCommandSubscription, ISubscription<ServerState> serverStateSubscription, IContract<Exception> exceptionContract)
        {
            _serverCommandSubscription = serverCommandSubscription;
            _serverStateSubscription = serverStateSubscription;
            _exceptionContract = exceptionContract;

            _serverCommandSubscription.Published += ServerCommandPublished;
            _serverStateSubscription.Published += (sender, state) => lastState = state;
        }

        private void ServerCommandPublished(ISubscription<ServerCommand> sender, ServerCommand packet)
        {
            try
            {
                switch (packet)
                {
                    case ServerCommand.Start:
                        if (!lastState.CanStart())
                            return;

                        socketServiceCancellation = new();
                        socketService = App.Current.Services.GetRequiredService<SocketService>();
                        socketService.StartAsync(socketServiceCancellation.Token).Wait();
                        break;
                    case ServerCommand.Stop:
                        if (!lastState.CanStop())
                            return;

                        socketServiceCancellation?.Cancel();

                        socketService?.StopAsync(socketServiceCancellation?.Token ?? App.Current.StoppingToken).Wait();

                        socketServiceCancellation?.Dispose();
                        socketServiceCancellation = null;
                        break;
                    default:
                        throw new NotImplementedException();
                }
            }
            catch (Exception ex)
            {
                _exceptionContract.Publish(ex);
            }
        }
    }
}
