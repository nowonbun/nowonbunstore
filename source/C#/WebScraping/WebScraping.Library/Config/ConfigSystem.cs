using System.Text;
using System.IO;
using System.Windows.Forms;
using System.Runtime.InteropServices;

namespace WebScraping.Library.Config
{
    public static class ConfigSystem
    {
        [DllImport("kernel32")]
        private static extern long WritePrivateProfileString(string section, string key, string val, string filePath);
        [DllImport("kernel32")]
        private static extern int GetPrivateProfileString(string section, string key, string def, StringBuilder retVal, int size, string filePath);

        public static void WriteConfig(string file, string section, string key, string val)
        {
            WritePrivateProfileString(section, key, val, GetFile(file));
        }
        public static string ReadConfig(string file, string section, string key)
        {
            StringBuilder temp = new StringBuilder(255);
            int ret = GetPrivateProfileString(section, key, "", temp, 255, GetFile(file));
            return temp.ToString();
        }
        private static string GetFile(string file)
        {
            return Path.Combine(Path.GetDirectoryName(Application.ExecutablePath), file + ".ini");
        }
    }
}
