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
        private readonly IContract<Exception> _exceptionContract;

        public SocketService(IContract<ServerState> serverStateContract, IContract<Exception> exceptionContract) : base(serverStateContract)
        {
            _exceptionContract = exceptionContract;
        }

        protected override async Task RunAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    using Socket socket = new(AddressFamily.InterNetworkV6, SocketType.Stream, ProtocolType.Tcp);
                    var address = new IPEndPoint(IPAddress.IPv6Loopback, 9999);
                    socket.Bind(address);
                    socket.Listen();

                    while (!stoppingToken.IsCancellationRequested)
                    {
                        var client = await socket.AcceptAsync(stoppingToken);

                        client.Close();
                    }
                }
                catch (Exception ex)
                {
                    _exceptionContract.Publish(ex);
                }
                finally
                {
                    await Task.Delay(30 * 1000, stoppingToken);
                }
            }
        }
    }
}
