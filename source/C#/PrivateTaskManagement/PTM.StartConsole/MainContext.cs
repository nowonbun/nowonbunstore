using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using System.ComponentModel;
using System.Drawing;
using System.Threading;
using System.IO;
using PTM.Httpd;
using PTM.ORM;
using PTM.WindowForm;
using PTM.Httpd.Util;

namespace PTM.StartConsole
{
    class MainContext : ApplicationContext
    {
        private NotifyIcon notify;
        private IContainer components;
        public MainContext()
        {
            ORMFactory.Initialize();
            this.components = new Container();
            this.notify = new NotifyIcon(this.components);
            this.notify.Icon = new Icon(Path.Combine(Path.GetDirectoryName(Application.ExecutablePath), "favicon.ico"));
            this.notify.Visible = true;
            this.notify.ContextMenu = new ContextMenu();
            this.notify.ContextMenu.MenuItems.AddRange(SetMenuItem());

            this.notify.Text = "Private Task Management";
            ThreadPool.QueueUserWorkItem(c =>
            {
                int port = 9999;
                try
                {
                    port = Convert.ToInt32(ConfigSystem.GetSettingPort());
                }
                catch { }
                string path = Path.GetDirectoryName(Application.ExecutablePath);
                var server = ServerFactory.NewInstance(port);
                var flow = new Flow();
                var message = new Message(this);
                server.SetDefaultFile("index.html");
                server.SetZip(path + "\\html.data");
                //server.SetRootPath(webpath);
                /*server.Set("/", (res, req) =>
                {
                    //req.SetCookie("test", "aaa", DateTime.Now.AddMinutes(5));
                    //req.SetSession("aaaaa", "asdfasfd");
                    req.ReadFile(webpath + @"\index.html");
                });*/
                server.Set("/js/define.js", (res, req) =>
                {
                    req.ContextType = "text / javascript; charset = UTF - 8";
                    req.Body = "var wsurl = \"ws://localhost:" + Convert.ToInt32(ConfigSystem.GetSettingPort()) + "/menu\";";
                });
                server.Set("/Start", (res, req) =>
                {
                    ShowForm();
                    req.StateOK();
                });
                server.SetWebSocket(mes =>
                {
                    Console.WriteLine(mes);
                    WSNode node = WSNode.ToNode(mes.ToString());
                    if (node.Type == 1)
                    {
                        flow.Execute(node.Key, node);
                    }
                    else if (node.Type == 2)
                    {
                        message.Execute(node.Key, node);
                    }
                    return new WebSocketNode() { OPCode = Opcode.BINARY, Message = node.ToString2() };
                });
            });
            if (String.Equals(ConfigSystem.GetWindowStart(), "on"))
            {
                ShowForm();
            }
        }

        private void ShowForm()
        {
            if (this.MainForm == null)
            {
                this.MainForm = new MainForm(ConfigSystem.GetSettingPort());
                (this.MainForm as MainForm).SetSize(ConfigSystem.GetWIndowSize());
            }
            (this.MainForm as MainForm).Booting();
        }

        private MenuItem[] SetMenuItem()
        {
            List<MenuItem> items = new List<MenuItem>();
            items.Add(new MenuItem() { Index = 0, Text = "Window" });
            items.Add(new MenuItem() { Index = 0, Text = "Exit" });

            items.AsParallel().ForAll(o =>
            {
                o.Click += (s, e) =>
                {
                    if (String.Equals("Window", (s as MenuItem).Text))
                    {
                        ShowForm();
                    }
                    if (String.Equals("Exit", (s as MenuItem).Text))
                    {
                        this.notify.Visible = false;
                        if (this.MainForm != null)
                        {
                            (this.MainForm as MainForm).Exit();
                        }
                        Application.Exit();
                    }
                };
            });

            return items.ToArray();
        }

        [Obsolete("not used", true)]
        private String2 ReadFile(String path)
        {
            FileInfo info = new FileInfo(path);
            if (!info.Exists)
            {
                return null;
            }
            String2 temp = new String2((int)info.Length);
            using (FileStream stream = new FileStream(info.FullName, FileMode.Open, FileAccess.Read))
            {
                stream.Read(temp.ToBytes(), 0, temp.Length);
            }
            return temp;
        }
    }
}
