using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace opal.Protocol
{
    public static class PacketExtensions
    {
        public static IEnumerable<Packet> FromArray(this byte[] data)
        {
            if (data.Length < Packet.MinSize)
                throw new InvalidPacketException("Length of data is less than SourcePacket.MinSize");

            var pointer = 0;
            while (pointer < data.Length)
            {
                var builder = new Packet.Builder();

                yield return builder.Build();
            }
        }
    }
}
