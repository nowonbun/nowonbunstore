using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace WorkServer
{
    class WebSocketMessageBuilder
    {
        private IDictionary<string, object> Column = new Dictionary<string, object>();
        public static WebSocketMessageBuilder GetMessage(MessageType messageType)
        {
            return new WebSocketMessageBuilder((int)messageType);
        }
        public WebSocketMessageBuilder(int messageType)
        {
            Column.Add("TYPE", messageType.ToString());
        }
        public void SetWorkTitle(String title)
        {
            Column.Add("WORKTITLE", title);
        }
        public void SetMessage(String message)
        {
            Column.Add("MESSAGE", message);
        }
        public void SetFileList(IEnumerable<String> files)
        {
            Column.Add("LIST", files);
        }
        public String2 Build()
        {
            return new String2(JsonConvert.SerializeObject(Column), Encoding.UTF8);
        }
    }
}
