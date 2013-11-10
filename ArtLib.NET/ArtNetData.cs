using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArtLib
{
    public class ArtNetData
    {
        short universe = 0;
        byte physical = 0;
        byte sequence = 0;
        short length = 0;
        byte[] rawPacket = new byte[0];
        Pixel[] Pixels;
        public ArtNetData(short universe, byte physical, byte sequence, short len, Pixel[] Pixels, byte[] raw)
        {
            this.universe = universe;
            this.physical = physical;
            this.sequence = sequence;
            this.length = len;
            this.Pixels = Pixels;
            this.rawPacket = raw;
        }

        public void setSeq(byte seq)
        {
            this.sequence = seq;
        }

        public short getUni()
        {
            return universe;
        }

        public void setUni(short uni)
        {
            this.universe = uni;
        }

        public short getLen()
        {
            return length;
        }

        public Pixel[] getPixels()
        {
            return Pixels;
        }

        public byte[] getRaw()
        {
            return rawPacket;
        }
    }
}
