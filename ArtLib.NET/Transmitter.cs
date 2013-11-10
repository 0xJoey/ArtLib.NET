using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;

namespace ArtLib
{
    public class Transmitter
    {
        private Pixel[] pixels = new Pixel[150];
        private short selectedUni = 0;
        private UdpClient Client;
        bool sendOnEdit = false;
        IPEndPoint remote;
        public Transmitter(string addr, int port, bool sendOnEdit, short universe)
        {
            for (int i = 0; i < 150; i++)
            {
                pixels[i] = new Pixel();
            }
            IPAddress IP = IPAddress.Parse(addr);
            Client = new UdpClient();
            this.remote = new IPEndPoint(IP, port);
            //Client.Connect(IP, port);

            this.sendOnEdit = sendOnEdit;
        }

        public void setUniverse(short newUni)
        {
            selectedUni = newUni;
        }

        public byte[] getHeader(short universe)
        {
            byte[] Header;
            byte[] ArtStr = ASCIIEncoding.ASCII.GetBytes("Art-Net");
            byte[] Ver = { 0x00, 0x00, 0x50 };
            byte[] OpCode = { 0x00, 0x50 };
            byte[] SeqPhy = { 0x00, 0x00 };
            //byte[] Uni = {0x68, 0x00};
            byte[] Uni = getUniBytes(universe);
            //Console.WriteLine(Uni[0].ToString() + " " + Uni[1].ToString());
            byte[] Len = { 0x01, 0xC2 };
            Header = Combine(ArtStr, Ver, OpCode, SeqPhy, Uni, Len);
            return Header;
        }

        static byte[] getUniBytes(short number)
        {
            byte b2 = (byte)(number >> 8);
            byte b1 = (byte)(number & 255);
            byte[] outB = new byte[2];
            outB[1] = b2;
            outB[0] = b1;
            return outB;
        }

        public void sendData(short universe, IPEndPoint ip)
        {
            byte[] Header = getHeader(universe);
            byte[] Data = new byte[450];
            for (int i = 0; i < 150; i++)
            {
                Data[i * 3] = pixels[i].R;
                Data[(i * 3) + 1] = pixels[i].G;
                Data[(i * 3) + 2] = pixels[i].B;

            }
            byte[] Packet = Combine(Header, Data);
            Client.Send(Packet, Packet.Length, ip);
        }

        public void sendData(short universe)
        {
            byte[] Header = getHeader(universe);
            byte[] Data = new byte[450];
            for (int i = 0; i < 150; i++)
            {
                Data[i * 3] = pixels[i].R;
                Data[(i * 3) + 1] = pixels[i].G;
                Data[(i * 3) + 2] = pixels[i].B;

            }
            byte[] Packet = Combine(Header, Data);
            Client.Send(Packet, Packet.Length, this.remote);
        }

        public void sendData()
        {
            sendData(selectedUni);
        }

        public void sendArtNetData(ArtNetData data, IPEndPoint ip)
        {
            this.pixels = data.getPixels();
            sendData(data.getUni(),ip);
        }

        public void sendRaw(byte[] packet, IPEndPoint ip)
        {
            Client.Send(packet, packet.Length, ip);
        }

        public Pixel[] getPixels()
        {
            return pixels;
        }

        public void setPixels(Pixel[] Pixs)
        {
            this.pixels = Pixs;
            if (sendOnEdit)
            {
                sendData();
            }
        }

        public void setPixel(int num, Pixel Pix)
        {
            pixels[num].R = Pix.R;
            pixels[num].G = Pix.G;
            pixels[num].B = Pix.B;
            if (sendOnEdit)
            {
                sendData();
            }
        }

        public Pixel getPixel(int num)
        {
            return pixels[num];
        }

        public void setAllPixels(Pixel Pix)
        {
            for(int i = 0; i < 150; i++)
            {
                pixels[i].R = Pix.R;
                pixels[i].G = Pix.G;
                pixels[i].B = Pix.B;
            }
            if (sendOnEdit)
            {
                sendData();
            }
        }

        public static byte[] Combine(params byte[][] arrays)
        {
            byte[] rv = new byte[arrays.Sum(a => a.Length)];
            int offset = 0;
            foreach (byte[] array in arrays)
            {
                System.Buffer.BlockCopy(array, 0, rv, offset, array.Length);
                offset += array.Length;
            }
            return rv;
        }
    }
}
