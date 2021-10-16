using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using opal.Models;

using yapsi;

namespace opal.Services
{
    public class ClientHandlerService : NoopBackgroundService
    {
        private readonly ISubscription<Socket> _socketSubscription;

        public ClientHandlerService(ISubscription<Socket> socketSubscription)
        {
            _socketSubscription = socketSubscription;

            _socketSubscription.Published += SocketPublished;
        }

        private void SocketPublished(ISubscription<Socket> sender, Socket client)
        {
            client.Close();
        }
    }
}
