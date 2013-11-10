using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArtLib
{
    public struct Pixel
    {
        public byte R, G, B;
        public Pixel(byte R, byte G, byte B)
        {
            this.R = R;
            this.G = G;
            this.B = B;
        }
    }
}
