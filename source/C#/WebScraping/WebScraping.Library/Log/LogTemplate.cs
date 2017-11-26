using System.IO;
using System.Text;
using System;

namespace WebScraping.Library.Log
{
    public static class LogTemplate
    {
        public static Stream GetLogTemp(string filepath)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("<?xml version=\"1.0\" encoding=\"utf-8\" ?>");
            sb.Append("<log4net>");
            sb.Append("<appender name=\"Console\" type=\"log4net.Appender.ConsoleAppender\">");
            sb.Append("<layout type=\"log4net.Layout.PatternLayout\">");
            sb.Append("<conversionPattern value=\"%d [%t] %-5p %c - %m%n\" />");
            sb.Append("</layout>");
            sb.Append("</appender>");
            sb.Append("<appender name=\"RollingFile\" type=\"log4net.Appender.RollingFileAppender\">");
            sb.Append("<file value=\"").Append(filepath).Append("\" />");
            sb.Append("<appendToFile value=\"true\" />");
            sb.Append("<datePattern value=\"-yyyy-MM-dd\" />");
            sb.Append("<rollingStyle value=\"Date\" />");
            sb.Append("<layout type=\"log4net.Layout.PatternLayout\">");
            sb.Append("<conversionPattern value=\"%d [%t] %-5p %c - %m%n\" />");
            sb.Append("</layout>");
            sb.Append("</appender>");
            sb.Append("<root>");
            sb.Append("<level value=\"DEBUG\" />");
            sb.Append("<appender-ref ref=\"Console\" />");
            sb.Append("<appender-ref ref=\"RollingFile\" />");
            sb.Append("</root>");
            sb.Append("</log4net>");
            return new MemoryStream(Encoding.UTF8.GetBytes(sb.ToString()));
        }
    }
}
