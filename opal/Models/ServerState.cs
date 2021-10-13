using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace opal.Models
{
    public enum ServerState
    {
        Undefined = -1,
        Stopped,
        Starting,
        Running,
        Stopping,
        Crashed,
    }

    public static class ServerStateExtensions
    {
        public static bool CanStart(this ServerState state)
        {
            return state is ServerState.Stopped or ServerState.Crashed or ServerState.Undefined;
        }

        public static bool CanStop(this ServerState state)
        {
            return state is ServerState.Running;
        }
    }
}
