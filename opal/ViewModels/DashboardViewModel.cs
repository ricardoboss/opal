using HostedWpf.ViewModels;

using opal.Models;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using yapsi;

namespace opal.ViewModels
{
    public class DashboardViewModel : BaseViewModel
    {
        private readonly ISubscription<ServerState> _serverStateSubscription;
        private readonly IContract<ServerCommand> _serverCommandContract;

        private ServerState _serverState = ServerState.Undefined;

        public ServerState ServerState
        {
            get => _serverState;
            set
            {
                SetProperty(ref _serverState, value);

                OnPropertyChanged(nameof(ServerStateLabel));
                OnPropertyChanged(nameof(CanStopServer));
                OnPropertyChanged(nameof(CanStartServer));
            }
        }

        public string ServerStateLabel => _serverState switch
        {
            ServerState.Stopped => "Stopped",
            ServerState.Starting => "Starting...",
            ServerState.Running => "Running",
            ServerState.Stopping => "Stopping...",
            ServerState.Crashed => "Crashed",
            ServerState.Undefined => "Undefined",
            _ => throw new NotImplementedException(),
        };

        public bool CanStopServer => _serverState.CanStop();
        public bool CanStartServer => _serverState.CanStart();

        public DashboardViewModel(ISubscription<ServerState> serverStateSubscription, IContract<ServerCommand> serverCommandContract)
        {
            _serverStateSubscription = serverStateSubscription;
            _serverCommandContract = serverCommandContract;

            _serverStateSubscription.Published += ServerState_Published;
        }

        private void ServerState_Published(ISubscription<ServerState> sender, ServerState packet)
        {
            ServerState = packet;
        }

        public void StartServer()
        {
            _serverCommandContract.Publish(ServerCommand.Start);
        }

        public void StopServer()
        {
            _serverCommandContract.Publish(ServerCommand.Stop);
        }
    }
}
