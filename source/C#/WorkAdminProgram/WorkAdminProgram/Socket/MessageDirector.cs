using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using WorkServer;

namespace WorkAdminProgram
{
    class MessageDirector
    {
        private static MessageDirector singleton = null;

        public static MessageDirector Instance()
        {
            if (singleton == null)
            {
                singleton = new MessageDirector();
            }
            return singleton;
        }

        private MessageDirector() { }

        //TODO: This source should be modified what "try ~ catch" syntax .
        public MessageNode GetNodeFromJson(String2 json)
        {
            try
            {
                IDictionary<String, String> buffer = JsonConvert.DeserializeObject<Dictionary<String, String>>(json.ToString());
                MessageNode node = new MessageNode();
                if(buffer.ContainsKey("TYPE")) node.MessageType = (MessageType)Int32.Parse(buffer["TYPE"]);
                if (buffer.ContainsKey("MESSAGE")) node.Message = buffer["MESSAGE"];
                if (buffer.ContainsKey("WORKTITLE")) node.WorkTitle = buffer["WORKTITLE"];
                return node;
            }
            catch (Exception)
            {
                return null;
            }
            
        }
        public String2 GetJsonFromNode(MessageNode node)
        {
            IDictionary<String, Object> buffer = new Dictionary<String, Object>();
            buffer.Add("TYPE",(int)node.MessageType);
            buffer.Add("WORKTITLE", node.WorkTitle);
            buffer.Add("MESSAGE", node.Message);
            buffer.Add("LIST", node.Files);
            return new String2(JsonConvert.SerializeObject(buffer), Encoding.UTF8);
        }
        public MessageNode CreateNode()
        {
            return new MessageNode();
        }
    }
}
