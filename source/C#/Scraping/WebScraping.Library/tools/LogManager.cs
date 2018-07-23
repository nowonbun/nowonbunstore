using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace WebScraping.Library.tools
{
    public enum LogType { Daily, Monthly }
    public class LogManager
    {
        private string _path;

        public LogManager(string path, LogType logtype, string prefix, string postfix)
        {
            _path = path;
            _SetLogPath(logtype, prefix, postfix);
        }

        public LogManager(string prefix, string postfix)
            : this(Path.Combine(Application.Root, "Log"),LogType.Daily,prefix,postfix)
        {

        }

        public LogManager()
            : this(Path.Combine(Application.Root, "Log"),LogType.Daily,null,null)
        {

        }

        private void _SetLogPath(LogType logtype, string prefix, string postfix)
        {
            string path = String.Empty;
            string name = String.Empty;

            switch (logtype)
            {
                case LogType.Daily:
                    path = String.Format(@"{0}/{1}", DateTime.Now.Year, DateTime.Now.ToString("MM"));
                    name = DateTime.Now.ToString("yyyyMMdd");
                    break;
                case LogType.Monthly:
                    path = String.Format(@"{0}/", DateTime.Now.Year);
                    name = DateTime.Now.ToString("yyyyMM");
                    break;
            }

            _path = Path.Combine(_path, path);


            if (!Directory.Exists(_path))
                Directory.CreateDirectory(_path);

            if (!String.IsNullOrEmpty(prefix))
                name = prefix + name;
            if (!String.IsNullOrEmpty(postfix))
                name = name + postfix;

            name += ".txt";

            _path = Path.Combine(_path, name);

        }

        public void Write(string data)
        {
            try
            {
                using (StreamWriter writer = new StreamWriter(_path, true))
                {
                    writer.Write(data);
                }

            }
            catch (System.Exception ex)
            {
                ex.ToString();
            }
        }

        public void WriteLine(string data)
        {
            try
            {
                using (StreamWriter writer = new StreamWriter(_path, true))
                {
                    writer.WriteLine(DateTime.Now.ToString("yyyyMMdd HH:mm:ss.fff\t") + data);
                }

            }
            catch (System.Exception ex)
            {
                ex.ToString();
            }
        }

        
    }
}
