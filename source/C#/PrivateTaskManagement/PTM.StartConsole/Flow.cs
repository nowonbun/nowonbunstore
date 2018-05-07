using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PTM.Httpd;

namespace PTM.StartConsole
{
    class Flow : Dictionary<String, Action<WSNode>>
    {
        public Flow()
        {
            Add("cardmenu", CardMenu);
            Add("memolist", MemoList);
            Add("memoinsert", MemoInsert);
            Add("setting", Setting);
            Add("task", _Task);
        }
        private void Error(WSNode node)
        {
            node.Key = "error";
            node.Data = "Not exists key.";
            node.Type = -1;
        }
        private void _Task(WSNode node)
        {
            node.Data = ServerFactory.GetInstance().GetZipFile(@"/flow/task.html").ToString();
        }
        private void Setting(WSNode node)
        {
            node.Data = ServerFactory.GetInstance().GetZipFile(@"/flow/setting.html").ToString();
        }
        private void MemoInsert(WSNode node)
        {
            if (!String.IsNullOrEmpty(node.Data))
            {
                node.ResponseKey = "get_memo_item";
                node.ResponseData = node.Data;
            }
            node.Data = ServerFactory.GetInstance().GetZipFile(@"/flow/memoinsert.html").ToString();
        }
        private void CardMenu(WSNode node)
        {
            node.Data = ServerFactory.GetInstance().GetZipFile(@"/flow/cardmenu.html").ToString();
        }
        private void MemoList(WSNode node)
        {
            node.Data = ServerFactory.GetInstance().GetZipFile(@"/flow/memolist.html").ToString();
        }
        public void Execute(String key, WSNode node)
        {
            if (base.ContainsKey(key))
            {
                base[key](node);
                return;
            }
            Error(node);
        }
    }
}
