using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;

namespace ArtLib
{
    //////////////////
    // Art-Net packet:
    // 1-8:     "Art-Net"
    // 9-10:    OpCode(0x5000 for DMX)
    // 10-11:   Protocol Version(14)
    // 12:      Sequence
    // 13:      Physical
    // 14-15:   Universe
    // 16-17:   DMX Data Length (in bytes)
    // 18...:   DMX Data
    //////////////////
    //Class for Receiving Art-Net messages.
    public class Receiver
    {
        private UdpClient Server;
        private int Port;
        public Receiver(int Port)
        {
            this.Port = Port;
            this.Server = new UdpClient(Port);
        }

        public Receiver()
        {
            this.Port = 16454; //Port 16454, because of problems with sending to self.
            this.Server = new UdpClient(this.Port);
        }

        //Checks if 'data' is a correct Art-Net package
        public bool validate(byte[] data)
        {
            string artCheck = Encoding.ASCII.GetString(data).Substring(0, 7);
            int opCode = data[8] * (0x01) + data[9] * (0x100);
            if (artCheck == "Art-Net" && opCode == 0x5000)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        //Receives raw packets
        public byte[] receiveRaw()
        {
            IPEndPoint remote = new IPEndPoint(IPAddress.Any, this.Port);
            byte[] packet;
            packet = this.Server.Receive(ref remote);
            return packet;
        }

        //Wrapper for the receiveRaw() function to get and return the data as an ArtNetData object.
        public ArtNetData receiveArtNet()
        {
            byte[] packet;
            while (true)
            {
                packet = this.receiveRaw();

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
