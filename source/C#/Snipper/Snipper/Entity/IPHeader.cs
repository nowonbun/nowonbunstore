using System;
using System.IO;
using System.Net;
using System.Text;

namespace Snipper.Entity
{
    public class IPHeader
    {
        private byte byVersionAndHeaderLength;
        private byte byDifferentiatedServices;
        private ushort usTotalLength;
        private ushort usIdentification;
        private ushort usFlagsAndOffset;
        private byte byTTL;
        private byte byProtocol;
        private short sChecksum;
        private uint uiSourceIPAddress;
        private uint uiDestinationIPAddress;
        private byte byHeaderLength;
        private byte[] byIPData = new byte[4096];

        public IPHeader(byte[] byBuffer, int nReceived)
        {
            using (MemoryStream memoryStream = new MemoryStream(byBuffer, 0, nReceived))
            {
                using (BinaryReader binaryReader = new BinaryReader(memoryStream))
                {
                    byVersionAndHeaderLength = binaryReader.ReadByte();
                    byDifferentiatedServices = binaryReader.ReadByte();
                    usTotalLength = (ushort)IPAddress.NetworkToHostOrder(binaryReader.ReadInt16());
                    usIdentification = (ushort)IPAddress.NetworkToHostOrder(binaryReader.ReadInt16());
                    usFlagsAndOffset = (ushort)IPAddress.NetworkToHostOrder(binaryReader.ReadInt16());
                    byTTL = binaryReader.ReadByte();
                    byProtocol = binaryReader.ReadByte();
                    sChecksum = IPAddress.NetworkToHostOrder(binaryReader.ReadInt16());
                    uiSourceIPAddress = (uint)(binaryReader.ReadInt32());
                    uiDestinationIPAddress = (uint)(binaryReader.ReadInt32());
                    byHeaderLength = byVersionAndHeaderLength;
                    byHeaderLength <<= 4;
                    byHeaderLength >>= 4;
                    byHeaderLength *= 4;
                    Array.Copy(byBuffer, byHeaderLength, byIPData, 0, usTotalLength - byHeaderLength);
                }
            }
        }
        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("***************************************Header***************************************").AppendLine();
            sb.Append(this.SourceAddress.ToString()).Append(" - ").Append(this.DestinationAddress.ToString()).AppendLine();
            sb.Append("byVersionAndHeaderLength - " + byVersionAndHeaderLength).AppendLine();
            sb.Append("byDifferentiatedServices - " + byVersionAndHeaderLength).AppendLine();
            sb.Append("usTotalLength - " + byVersionAndHeaderLength).AppendLine();
            sb.Append("usIdentification - " + byVersionAndHeaderLength).AppendLine();
            sb.Append("usFlagsAndOffset - " + byVersionAndHeaderLength).AppendLine();
            sb.Append("byTTL - " + byVersionAndHeaderLength).AppendLine();
            sb.Append("byProtocol - " + byVersionAndHeaderLength).AppendLine();
            sb.Append("sChecksum - " + byVersionAndHeaderLength).AppendLine();
            sb.Append("uiSourceIPAddress - " + byVersionAndHeaderLength).AppendLine();
            sb.Append("uiDestinationIPAddress - " + byVersionAndHeaderLength).AppendLine();
            sb.Append("byHeaderLength - " + byVersionAndHeaderLength).AppendLine();
            sb.Append("byIPData").AppendLine();
            sb.Append(Encoding.Default.GetString(byIPData));
            sb.Append("***************************************Header***************************************").AppendLine();
            return sb.ToString();
        }

        public string Version
        {
            get
            {
                if ((byVersionAndHeaderLength >> 4) == 4)
                {
                    return "IP v4";
                }
                else if ((byVersionAndHeaderLength >> 4) == 6)
                {
                    return "IP v6";
                }
                else
                {
                    return "Unknown";
                }
            }
        }
        public string HeaderLength
        {
            get
            {
                return byHeaderLength.ToString();
            }
        }
        public ushort MessageLength
        {
            get
            {
                return (ushort)(usTotalLength - byHeaderLength);
            }
        }
        public string DifferentiatedServices
        {
            get
            {
                return string.Format("0x{0:x2} ({1})", byDifferentiatedServices, byDifferentiatedServices);
            }
        }
        public string Flags
        {
            get
            {
                int nFlags = usFlagsAndOffset >> 13;
                if (nFlags == 2)
                {
                    return "Don't fragment";
                }
                else if (nFlags == 1)
                {
                    return "More fragments to come";
                }
                else
                {
                    return nFlags.ToString();
                }
            }
        }
        public string FragmentationOffset
        {
            get
            {
                int nOffset = usFlagsAndOffset << 3;
                nOffset >>= 3;
                return nOffset.ToString();
            }
        }

        public string TTL
        {
            get
            {
                return byTTL.ToString();
            }
        }
        // 6 - TCP
        // 17 - UDP
        // Unknown
        public int ProtocolType
        {
            get
            {
                return byProtocol;
            }
        }

        public string Checksum
        {
            get
            {
                return string.Format("0x{0:x2}", sChecksum);
            }
        }

        public IPAddress SourceAddress
        {
            get
            {
                return new IPAddress(uiSourceIPAddress);
            }
        }

        public IPAddress DestinationAddress
        {
            get
            {
                return new IPAddress(uiDestinationIPAddress);
            }
        }

        public string TotalLength
        {
            get
            {
                return usTotalLength.ToString();
            }
        }

        public string Identification
        {
            get
            {
                return usIdentification.ToString();
            }
        }

        public byte[] Data
        {
            get
            {
                return byIPData;
            }
        }
    }
}

