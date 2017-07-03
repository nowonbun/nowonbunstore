using System.Net;
using System.Text;
using System;
using System.IO;

namespace Snipper.Entity
{
    public class UDPHeader
    {
        private ushort usSourcePort;
        private ushort usDestinationPort;
        private ushort usLength;
        private short sChecksum;

        private byte[] byUDPData = new byte[4096];

        public UDPHeader(byte[] byBuffer, int nReceived)
        {
            using (MemoryStream memoryStream = new MemoryStream(byBuffer, 0, nReceived))
            {
                using (BinaryReader binaryReader = new BinaryReader(memoryStream))
                {
                    usSourcePort = (ushort)IPAddress.NetworkToHostOrder(binaryReader.ReadInt16());
                    usDestinationPort = (ushort)IPAddress.NetworkToHostOrder(binaryReader.ReadInt16());
                    usLength = (ushort)IPAddress.NetworkToHostOrder(binaryReader.ReadInt16());

                    sChecksum = IPAddress.NetworkToHostOrder(binaryReader.ReadInt16());
                    Array.Copy(byBuffer, 8, byUDPData, 0, nReceived - 8);
                }
            }            
        }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("***************************************UDPHeader***************************************").AppendLine();
            sb.Append("usSourcePort - " + usSourcePort).AppendLine();
            sb.Append("usDestinationPort - " + usDestinationPort).AppendLine();
            sb.Append("usLength - " + usLength).AppendLine();
            sb.Append("sChecksum - " + sChecksum).AppendLine();
            sb.Append(Encoding.Default.GetString(byUDPData));
            sb.AppendLine();
            sb.Append("***************************************UDPHeader***************************************").AppendLine();
            return sb.ToString();
        }

        public string SourcePort
        {
            get
            {
                return usSourcePort.ToString();
            }
        }

        public string DestinationPort
        {
            get
            {
                return usDestinationPort.ToString();
            }
        }

        public string Length
        {
            get
            {
                return usLength.ToString();
            }
        }

        public string Checksum
        {
            get
            {
                return string.Format("0x{0:x2}", sChecksum);
            }
        }

        public byte[] Data
        {
            get
            {
                return byUDPData;
            }
        }
    }
}