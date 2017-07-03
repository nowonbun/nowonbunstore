using System;
using System.Collections.Generic;
using System.Text;
using System.Net;
using System.Net.Sockets;
using System.IO;
using System.Collections;
using Snipper.Entity;

namespace Snipper
{
    public class RawSocket : Socket
    {
        byte[] byTrue = new byte[4] { 1, 0, 0, 0 };
        byte[] byOut = new byte[4] { 1, 0, 0, 0 };
        byte[] byteData = new byte[4096];

        public RawSocket()
            : base(AddressFamily.InterNetwork, SocketType.Raw, ProtocolType.IP)
        {
            base.Bind(new IPEndPoint(IPAddress.Parse("192.168.0.3"), 0));
            base.SetSocketOption(SocketOptionLevel.IP, SocketOptionName.HeaderIncluded, true);
            base.IOControl(IOControlCode.ReceiveAll, byTrue, byOut);
        }
        public void Start()
        {
            using (FileStream stream = new FileStream("D:\\work\\Snipper.txt", FileMode.Append, FileAccess.Write))
            {
                using (StreamWriter sw = new StreamWriter(stream))
                {
                    while (true)
                    {
                        int nReceived = base.Receive(byteData, SocketFlags.None);

                        IPHeader ipheader = new IPHeader(byteData, nReceived);
                        sw.WriteLine("-----------------------------------------------------------------------------------");
                        sw.Write(ipheader.ToString());
                        sw.WriteLine();
                        switch (ipheader.ProtocolType)
                        {
                            case 6:
                                TCPHeader tcpHeader = new TCPHeader(ipheader.Data, ipheader.MessageLength);
                                Console.WriteLine(ipheader.SourceAddress.ToString() + "-" + ipheader.DestinationAddress.ToString());
                                sw.Write(tcpHeader.ToString());
                                break;
                            case 7:
                                UDPHeader udpHeader = new UDPHeader(ipheader.Data, ipheader.MessageLength);
                                Console.WriteLine(ipheader.SourceAddress.ToString() + "-" + ipheader.DestinationAddress.ToString());
                                sw.Write(udpHeader.ToString());
                                break;
                        }
                        sw.WriteLine("-----------------------------------------------------------------------------------");
                        sw.WriteLine();
                        sw.Flush();
                    }
                }
            }
        }
    }
}
