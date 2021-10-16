using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace opal.Protocol
{
    public class InvalidPacketException : FormatException
    {
        public InvalidPacketException() : base() { }
        public InvalidPacketException(string? message) : base(message) { }
        public InvalidPacketException(string? message, Exception? innerException) : base(message, innerException) { }
    }
}
