using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace opal.Protocol
{
    public partial class Packet
    {
        public const int MinSize = 4;

        public int Size { get; }

        public Packet(int size)
        {
            Size = size;
        }

        public class Builder
        {
            private int size;

            public Builder SetSize(int size)
            {
                this.size = size;

                return this;
            }

            public Packet Build()
            {
                return new Packet(size);
            }
        }
    }
}
