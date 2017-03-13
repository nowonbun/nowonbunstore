using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security.Cryptography;

namespace WorkSocketServer
{
    partial class WorkSocketImpl : WorkSocket
    {
        private static SHA1 SHA = null;

        private void SendHandShake(String2 key)
        {
            String2 temp = new String2(Encoding.UTF8);
            temp += "HTTP/1.1 101 Switching Protocols" + String2.CRLF;
            temp += "Upgrade: websocket" + String2.CRLF;
            temp += "Connection: Upgrade" + String2.CRLF;
            temp += "Sec-WebSocket-Accept:" + ComputeHash(key) + String2.CRLF + String2.CRLF;
            client.Send(temp.ToBytes());
        }

        public String2 ComputeHash(String2 key)
        {
            if (SHA == null)
            {
                SHA = SHA1CryptoServiceProvider.Create();
            }
            String buffer = key.Trim().ToString() + Define.GUID;
            byte[] hash = SHA.ComputeHash(Encoding.ASCII.GetBytes(buffer));
            return new String2(Convert.ToBase64String(hash), Encoding.UTF8);
        }
    }
}
