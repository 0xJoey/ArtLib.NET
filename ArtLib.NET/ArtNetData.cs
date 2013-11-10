using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArtLib
{
    public class ArtNetData
    {
        //Basically, a class for Art-Net data.
        short universe = 0;
        byte physical = 0;
        byte sequence = 0;
        short length = 0;
        Pixel[] Pixels;
        public ArtNetData(short universe, byte physical, byte sequence, short len, Pixel[] Pixels, byte[] raw)
        {
            this.universe = universe;
            this.physical = physical;
            this.sequence = sequence;
            this.length = len;
            this.Pixels = Pixels;
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
    }
}
