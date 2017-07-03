using System.Net;
using System.Text;
using System;
using System.IO;

namespace Snipper.Entity
{
    public class TCPHeader
    {
        private ushort usSourcePort;
        private ushort usDestinationPort;
        private uint uiSequenceNumber = 555;
        private uint uiAcknowledgementNumber = 555;
        private ushort usDataOffsetAndFlags = 555;
        private ushort usWindow = 555;
        private short sChecksum = 555;
        private ushort usUrgentPointer;

        private byte byHeaderLength;
        private ushort usMessageLength;
        private byte[] byTCPData = new byte[4096];

        public TCPHeader(byte[] byBuffer, int nReceived)
        {
            using (MemoryStream memoryStream = new MemoryStream(byBuffer, 0, nReceived))
            {
                using (BinaryReader binaryReader = new BinaryReader(memoryStream))
                {
                    usSourcePort = (ushort)IPAddress.NetworkToHostOrder(binaryReader.ReadInt16());
                    usDestinationPort = (ushort)IPAddress.NetworkToHostOrder(binaryReader.ReadInt16());
                    uiSequenceNumber = (uint)IPAddress.NetworkToHostOrder(binaryReader.ReadInt32());
                    uiAcknowledgementNumber = (uint)IPAddress.NetworkToHostOrder(binaryReader.ReadInt32());
                    usDataOffsetAndFlags = (ushort)IPAddress.NetworkToHostOrder(binaryReader.ReadInt16());
                    usWindow = (ushort)IPAddress.NetworkToHostOrder(binaryReader.ReadInt16());
                    sChecksum = (short)IPAddress.NetworkToHostOrder(binaryReader.ReadInt16());
                    usUrgentPointer = (ushort)IPAddress.NetworkToHostOrder(binaryReader.ReadInt16());
                    byHeaderLength = (byte)(usDataOffsetAndFlags >> 12);
                    byHeaderLength *= 4;
                    usMessageLength = (ushort)(nReceived - byHeaderLength);
                    Array.Copy(byBuffer, byHeaderLength, byTCPData, 0, nReceived - byHeaderLength);
                }
            }
        }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("***************************************TCPHeader***************************************").AppendLine();
            sb.Append("usSourcePort - " + usSourcePort).AppendLine();
            sb.Append("usDestinationPort - " + usDestinationPort).AppendLine();
            sb.Append("uiSequenceNumber - " + uiSequenceNumber).AppendLine();
            sb.Append("uiAcknowledgementNumber - " + uiAcknowledgementNumber).AppendLine();
            sb.Append("usDataOffsetAndFlags - " + usDataOffsetAndFlags).AppendLine();
            sb.Append("usWindow - " + usWindow).AppendLine();
            sb.Append("sChecksum - " + sChecksum).AppendLine();
            sb.Append("usUrgentPointer - " + usUrgentPointer).AppendLine();
            sb.Append("byHeaderLength - " + byHeaderLength).AppendLine();
            sb.Append("usMessageLength - " + usMessageLength).AppendLine();
            sb.Append(Encoding.Default.GetString(byTCPData));
            sb.AppendLine();
            sb.Append("***************************************TCPHeader***************************************").AppendLine();
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

        public string SequenceNumber
        {
            get
            {
                return uiSequenceNumber.ToString();
            }
        }

        public string AcknowledgementNumber
        {
            get
            {
                if ((usDataOffsetAndFlags & 0x10) != 0)
                {
                    return uiAcknowledgementNumber.ToString();
                }
                else
                    return "";
            }
        }

        public string HeaderLength
        {
            get
            {
                return byHeaderLength.ToString();
            }
        }

        public string WindowSize
        {
            get
            {
                return usWindow.ToString();
            }
        }

        public string UrgentPointer
        {
            get
            {
                if ((usDataOffsetAndFlags & 0x20) != 0)
                {
                    return usUrgentPointer.ToString();
                }
                else
                    return "";
            }
        }

        public string Flags
        {
            get
            {
                int nFlags = usDataOffsetAndFlags & 0x3F;
                string strFlags = string.Format("0x{0:x2} (", nFlags);
                if ((nFlags & 0x01) != 0)
                {
                    strFlags += "FIN, ";
                }
                if ((nFlags & 0x02) != 0)
                {
                    strFlags += "SYN, ";
                }
                if ((nFlags & 0x04) != 0)
                {
                    strFlags += "RST, ";
                }
                if ((nFlags & 0x08) != 0)
                {
                    strFlags += "PSH, ";
                }
                if ((nFlags & 0x10) != 0)
                {
                    strFlags += "ACK, ";
                }
                if ((nFlags & 0x20) != 0)
                {
                    strFlags += "URG";
                }
                strFlags += ")";

                if (strFlags.Contains("()"))
                {
                    strFlags = strFlags.Remove(strFlags.Length - 3);
                }
                else if (strFlags.Contains(", )"))
                {
                    strFlags = strFlags.Remove(strFlags.Length - 3, 2);
                }

                return strFlags;
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
                return byTCPData;
            }
        }

        public ushort MessageLength
        {
            get
            {
                return usMessageLength;
            }
        }
    }
}