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
}
