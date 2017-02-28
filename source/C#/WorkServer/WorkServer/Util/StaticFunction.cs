using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security.Cryptography;

namespace WorkServer
{
    class StaticFunction
    {
        private static SHA1 SHA = null;

        public static String2 ComputeHash(String2 key)
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
