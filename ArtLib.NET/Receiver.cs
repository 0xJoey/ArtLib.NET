using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;

namespace ArtLib
{
    public class Receiver
    {
        private UdpClient Server;
        public Receiver()
        {
            this.Server = new UdpClient(16454);
        }

        public byte[] receiveRaw()
        {
            IPEndPoint remote = new IPEndPoint(IPAddress.Any, 16454);
            byte[] packet;
            packet = this.Server.Receive(ref remote);
            string artCheck = Encoding.ASCII.GetString(packet).Substring(0,7);
            return packet;
        }

        public ArtNetData receiveArtNet()
        {
            byte[] packet;
            while (true)
            {
                packet = this.receiveRaw();
                string artCheck = Encoding.ASCII.GetString(packet).Substring(0, 7);
                int opCode = packet[8] * (0x01) + packet[9] * (0x100);
                if (artCheck == "Art-Net" && opCode == 0x5000)
                {
                    break;
                }
            }
            byte seq = packet[12];
            byte physical = packet[13];
            short uni = (short)(packet[14] * (0x01) + packet[15] * (0x100));
            short len = (short)(packet[16] * (0x100) + packet[17] * (0x01));
            Pixel[] Pixels = new Pixel[len];
            for(int i = 0; i < (len/3); i+=1)
            {
                Pixels[i] = new Pixel(packet[18 + (i * 3)], packet[18 + (i * 3) + 1], packet[18 + (i * 3) + 2]);
            }
            return new ArtNetData(uni, physical, seq, len, Pixels, packet);
        }
    }
}
