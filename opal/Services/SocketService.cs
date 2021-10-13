using Microsoft.Extensions.Options;

using opal.Models;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

using yapsi;

namespace opal.Services
{
    public class SocketService : ServerStateService
    {
        private readonly SocketServiceOptions _options;
        private Socket? _socket;

        public SocketService(IOptions<SocketServiceOptions> options, IContract<ServerState> serverStateContract, IContract<Exception> exceptionContract) : base(serverStateContract, exceptionContract)
        {
            _options = options.Value;
        }

        protected override Task PrepareAsync(CancellationToken cancellationToken)
        {
            _socket = new(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

            var address = new IPEndPoint(IPAddress.Any, 9999);
            _socket.Bind(address);

            return Task.CompletedTask;
        }

        protected override async Task RunAsync(CancellationToken stoppingToken)
        {
            if (_socket is null)
                throw new NullReferenceException();

            _socket.Listen();

            try
            {
                while (!stoppingToken.IsCancellationRequested)
                {
                    var client = await _socket.AcceptAsync(stoppingToken);

                    client.Close();
                }
            }
            catch (OperationCanceledException)
            {
                // expected when stopping the service and stoppingToken gets cancelled
                if (!stoppingToken.IsCancellationRequested)
                    throw;
            }

            _socket.Close();
        }

        protected override Task CleanupAsync(CancellationToken cancellationToken)
        {
            if (_socket is not null)
            {
                _socket.Dispose();
                _socket = null;
            }

            return Task.CompletedTask;
        }
    }
}
