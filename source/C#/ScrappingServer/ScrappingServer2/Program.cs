using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using System.Text;
using log4net;
using log4net.Config;

namespace ScrappingServer
{
    static class Program
    {
        [STAThread]
        static void Main()
        {
            StringBuilder sb = new StringBuilder();
            char[] pathBuffer = Application.ExecutablePath.ToCharArray();
            for (int i = pathBuffer.Length - 1; i >= 0; i--)
            {
                if (object.Equals(pathBuffer[i], '\\') || object.Equals(pathBuffer[i], '/'))
                {
                    for (int j = 0; j < i; j++)
                    {
                        sb.Append(pathBuffer[j]);
                    }
                    sb.Append("\\log4net.xml");
                    break;
                }
            }
            XmlConfigurator.Configure(new System.IO.FileInfo(sb.ToString()));

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new ScrappingForm());
        }
    }
}
