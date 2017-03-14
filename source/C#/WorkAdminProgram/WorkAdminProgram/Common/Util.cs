using System;
using System.Runtime.InteropServices;
using System.Text;
using System.IO;

namespace WorkAdminProgram
{
    public static class Util
    {
        [DllImport("kernel32")]
        private static extern long WritePrivateProfileString(string section, string key, string val, string filePath);
        [DllImport("kernel32")]
        private static extern int GetPrivateProfileString(string section, string key, string def, StringBuilder retVal, int size, string filePath);

        private static int BUFFER_SIZE = 1024;
        public static string GetIni(string section, string key, string file)
        {
            //Program.CONFIG_PATH + Path.DirectorySeparatorChar + Define.CONFIG
            StringBuilder buffer = new StringBuilder();
            GetPrivateProfileString(section, key, "", buffer, BUFFER_SIZE, file);
            return buffer.ToString();
        }
    }
}