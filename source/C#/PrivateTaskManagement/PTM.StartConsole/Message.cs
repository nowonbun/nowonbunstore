using System;
using System.Collections.Generic;
using System.Linq;
using PTM.ORM;
using PTM.ORM.Dao;
using PTM.ORM.Entity;
using Newtonsoft.Json;
using PTM.WindowForm;

namespace PTM.StartConsole
{
    class Message : Dictionary<String, Action<WSNode>>
    {
        private MainContext context;
        public Message(MainContext context)
        {
            this.context = context;
            Add("set_memo_insert", SetMemoInsert);
            Add("get_memo_list", GetMemoList);
            Add("get_memo_item", GetMemoItem);
            Add("excute_memo_delete", ExcuteMemoDelete);
            Add("set_memo_modify", SetMemoModify);
            Add("get_setting", GetSetting);
            Add("set_setting", SetSetting);
            Add("set_task_insert", SetTaskInsert);
            Add("get_task_list", GetTaskList);
            Add("set_process", SetProcess);
            Add("set_task_delete", SetTaskDelete);
            Add("set_task_modify", SetTaskModify);
        }

        private void Error(WSNode node)
        {
            node.Key = "error";
            node.Data = "Not exists key.";
            node.Type = -1;
        }
        private void SetTaskModify(WSNode node)
        {
            ITaskDao dao = ORMFactory.GetService<ITaskDao>(typeof(ITaskDao));
            String temp = node.Data;
            var map = GetParameter(node.Data);
            int idx = -1;
            if (!map.ContainsKey("idx"))
            {
                Error(node);
                return;
            }
            try
            {
                idx = Convert.ToInt32(map["idx"]);
            }
            catch
            {
                return;
            }
            Task task = dao.GetEntity(idx);
            if (!map.ContainsKey("title"))
            {
                Error(node);
                node.Data = "Title is nothing.";
                return;
            }
            task.Title = map["title"];
            task.Contents = map.ContainsKey("contents") && !String.IsNullOrEmpty(map["contents"].Trim()) ? map["contents"] : "";
            if (map.ContainsKey("importance") && !String.IsNullOrEmpty(map["importance"].Trim()))
            {
                try
                {
                    task.Importance = Convert.ToInt32(map["importance"]);
                }
                catch
                {

                }
            }
            dao.Update(task);
        }
        private void SetTaskDelete(WSNode node)
        {
            ITaskDao dao = ORMFactory.GetService<ITaskDao>(typeof(ITaskDao));
            String temp = node.Data;
            var map = GetParameter(node.Data);
            int idx = -1;
            if (!map.ContainsKey("idx"))
            {
                Error(node);
                return;
            }
            try
            {
                idx = Convert.ToInt32(map["idx"]);
            }
            catch
            {
                return;
            }
            Task task = dao.GetEntity(idx);
            task.IsDelete = 1;
            //dao.Delete(task);
            dao.Update(task);
            node.Data = "";
        }
        private void SetProcess(WSNode node)
        {
            ITaskDao dao = ORMFactory.GetService<ITaskDao>(typeof(ITaskDao));
            Task entity = JsonConvert.DeserializeObject<Task>(node.Data);
            Task original = dao.GetEntity(entity.Idx);
            original.Tasktype = entity.Tasktype;
            original.Taskdate = DateTime.Now;
            dao.Update(original);
            node.Data = JsonConvert.SerializeObject(original);
        }

        private void GetTaskList(WSNode node)
        {
            DateTime now;
            try
            {
                String[] temp = node.Data.Split('/');
                for (int i = 0; i < temp.Length; i++)
                {
                    temp[i] = temp[i].Trim();
                }
                now = new DateTime(Convert.ToInt32(temp[0]), Convert.ToInt32(temp[1]), Convert.ToInt32(temp[2]));
            }
            catch
            {
                now = DateTime.Now;
            }
            ITaskDao dao = ORMFactory.GetService<ITaskDao>(typeof(ITaskDao));
            List<Task> list = dao.Select() as List<Task>;
            list = list.OrderBy(t =>
            {
                return t.Tasktype;
            }).Where(t =>
            {
                if (t.IsDelete == 1)
                {
                    return false;
                }
                if (Util.DayTick(t.Taskdate) > Util.DayTick(now))
                {
                    return false;
                }
                if(Util.DayTick(t.Taskdate) < Util.DayTick(now) && t.Tasktype == 2)
                {
                    return false;
                }
                return true;
            }).OrderBy(t =>
            {
                return t.Importance;
            }).ToList();
            list.ForEach(t =>
            {
                if (Util.DayTick(t.Taskdate) != Util.DayTick(now))
                {
                    t.Tasktype = 4;
                }
            });
            string json = JsonConvert.SerializeObject(list);
            node.Data = json;
        }

        private void SetTaskInsert(WSNode node)
        {
            ITaskDao dao = ORMFactory.GetService<ITaskDao>(typeof(ITaskDao));
            String temp = node.Data;
            Task task = new Task();
            var map = GetParameter(node.Data);
            if (!map.ContainsKey("title"))
            {
                Error(node);
                node.Data = "Title is nothing.";
                return;
            }
            task.Title = map["title"];
            task.Contents = map.ContainsKey("contents") && !String.IsNullOrEmpty(map["contents"].Trim()) ? map["contents"] : "";
            try
            {
                task.Importance = map.ContainsKey("importance") && !String.IsNullOrEmpty(map["importance"].Trim()) ? Convert.ToInt32(map["importance"]) : 2;
            }
            catch
            {
                task.Importance = 2;
            }
            task.Tasktype = 0;
            task.Taskdate = DateTime.Now;
            task.IsDelete = 0;
            int scope = dao.InsertAndScope(task);
            task.Idx = scope;
            node.Data = JsonConvert.SerializeObject(task);
        }

        private void SetSetting(WSNode node)
        {
            var data = JsonConvert.DeserializeObject<Dictionary<String, String>>(node.Data);
            foreach (var n in data)
            {
                ConfigSystem.WriteConfig("Config", "Setting", n.Key, n.Value);
            }
            if (this.context.MainForm != null)
            {
                (this.context.MainForm as MainForm).SetSize(ConfigSystem.GetWIndowSize());
            }
        }

        private void GetSetting(WSNode node)
        {
            var obj = new
            {
                Port = ConfigSystem.GetSettingPort(),
                Size = ConfigSystem.GetWIndowSize(),
                Start = ConfigSystem.GetWindowStart()
            };

            String json = JsonConvert.SerializeObject(obj);
            node.Data = json;
        }

        private void SetMemoModify(WSNode node)
        {
            IMemoDao dao = ORMFactory.GetService<IMemoDao>(typeof(IMemoDao));
            String temp = node.Data;
            Memo memo = new Memo();
            var map = GetParameter(node.Data);
            memo.Idx = Convert.ToInt32(map["idx"]);
            memo = dao.GetEneity(memo.Idx);
            memo.Title = map.ContainsKey("title") && !String.IsNullOrEmpty(map["title"].Trim()) ? map["title"] : "No title";
            memo.Contents = map.ContainsKey("contents") && !String.IsNullOrEmpty(map["contents"].Trim()) ? map["contents"] : "";
            memo.RecentlyDate = DateTime.Now;
            dao.Update(memo);
        }

        private void ExcuteMemoDelete(WSNode node)
        {
            IMemoDao dao = ORMFactory.GetService<IMemoDao>(typeof(IMemoDao));
            int idx = Convert.ToInt32(node.Data);
            var entity = dao.GetEneity(idx);
            dao.Delete(entity);
        }

        private void GetMemoList(WSNode node)
        {
            IMemoDao dao = ORMFactory.GetService<IMemoDao>(typeof(IMemoDao));
            var list = dao.Select();
            list = list.OrderByDescending((m) =>
            {
                return m.RecentlyDate;
            }).ToList();
            string json = JsonConvert.SerializeObject(list);
            node.Data = json;
        }

        private void SetMemoInsert(WSNode node)
        {
            IMemoDao dao = ORMFactory.GetService<IMemoDao>(typeof(IMemoDao));
            String temp = node.Data;
            Memo memo = new Memo();
            var map = GetParameter(node.Data);
            memo.Title = map.ContainsKey("title") && !String.IsNullOrEmpty(map["title"].Trim()) ? map["title"] : "No title";
            memo.Contents = map.ContainsKey("contents") && !String.IsNullOrEmpty(map["contents"].Trim()) ? map["contents"] : "";
            memo.RecentlyDate = DateTime.Now;
            int scope = dao.InsertAndScope(memo);
            node.Data = scope.ToString();
        }

        private void GetMemoItem(WSNode node)
        {
            int idx;
            try
            {
                idx = Convert.ToInt32(node.Data);
            }
            catch (Exception)
            {
                Error(node);
                return;
            }
            IMemoDao dao = ORMFactory.GetService<IMemoDao>(typeof(IMemoDao));
            var item = dao.GetEneity(idx);
            node.Data = JsonConvert.SerializeObject(item);
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

        private Dictionary<String, String> GetParameter(String data)
        {
            Dictionary<String, String> ret = new Dictionary<string, string>();
            foreach (var b in data.Split('&'))
            {
                int pos = b.IndexOf("=");
                if (pos < 0)
                {
                    continue;
                }
                String key = b.Substring(0, pos);
                String value = b.Substring(pos + 1, b.Length - (pos + 1));
                if (ret.ContainsKey(key))
                {
                    ret[key] = value;
                }
                else
                {
                    ret.Add(key, value);
                }
            }
            return ret;
        }
    }
}
