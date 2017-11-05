using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace ScrappingHttpCore
{
    class ResponseHeader
    {
        private String State = "HTTP/1.1 200 OK";
        private String Date = "Date: " + DateTime.UtcNow.ToString("yyyy-MM-dd HH:mm:ss.fff");
        private String Server = "Server: ScrappingServer";
        private String Pragma = "Pragma: no-cache";
        private String ContentType = "Content-Type: text/html;charset=utf-8";
        private String ContentLanguage = "Content-Langth: 0";
        private String BodyData;
        public ResponseHeader(String bodydata)
        {
            ContentLanguage = "Content-Length: " + Encoding.Default.GetByteCount(bodydata);
            BodyData = bodydata;
        }
        public void WriteSend(Stream stream)
        {
            try
            {
                StreamWriter writer = new StreamWriter(stream, Encoding.UTF8);
                writer.WriteLine(State);
                writer.WriteLine(Date);
                writer.WriteLine(Server);
                writer.WriteLine(Pragma);
                writer.WriteLine(ContentType);
                writer.WriteLine(ContentLanguage);
                writer.WriteLine();
                writer.WriteLine(BodyData);
                writer.WriteLine();
                writer.WriteLine();
                writer.Flush();
            }
            catch (Exception)
            {

            }
        }

    }
}
