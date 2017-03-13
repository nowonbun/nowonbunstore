using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.InteropServices;
using System.IO;

namespace WorkServer
{
    class ConfigReader
    {
        [DllImport("kernel32")]
        private static extern long WritePrivateProfileString(string section, string key, string val, string filePath);
        [DllImport("kernel32")]
        private static extern int GetPrivateProfileString(string section, string key, string def, StringBuilder retVal, int size, string filePath);

        private static int BUFFER_SIZE = 1024;
        public static string GetIni(string section, string key)
        {
            StringBuilder buffer = new StringBuilder();
            GetPrivateProfileString(section, key, "", buffer, BUFFER_SIZE, Program.CONFIG_PATH + Path.DirectorySeparatorChar + Define.CONFIG);
            return buffer.ToString();
        }
    }
}
