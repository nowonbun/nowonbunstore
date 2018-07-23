using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.Windows.Forms;

namespace WindowExample
{
    class QueueThread
    {
        private static QueueThread singleton = null;
        private List<Thread> queuelist = new List<Thread>();
        private static QueueThread Instance()
        {
            if (singleton == null)
            {
                singleton = new QueueThread();
            }
            return singleton;
        }
        private QueueThread()
        {
            ThreadPool.UnsafeQueueUserWorkItem((c) =>
            {
                while (true)
                {
                    var list = queuelist.Where(x => !x.IsAlive).ToList();
                    lock (queuelist)
                    {
                        foreach (var l in list)
                        {
                            queuelist.Remove(l);
                        }
                    }
                    Thread.Sleep(1000);
                }
            }, null);
        }

        public static void Start()
        {
            Instance();
        }

        public static void Push(Action func, int delay = 0)
        {

            Thread _thread = new Thread(() =>
            {
                if (delay > 0)
                {
                    Thread.Sleep(delay);
                }
                func();
            });
            Instance().queuelist.Add(_thread);
            _thread.Start();
        }
        public static void InvokeForm(Form form, Action func)
        {
            if (form.InvokeRequired)
            {
                form.Invoke(func);
            }
            else
            {
                func();
            }
        }
        public static T InvokeForm<T>(Form form, Func<T> func)
        {
            if (form.InvokeRequired)
            {
                return (T)form.Invoke(func);
            }
            else
            {
                return func();
            }
        }
        public static void InvokeControl(Control ctl, Action func)
        {
            if (ctl.InvokeRequired)
            {
                ctl.Invoke(func);
            }
            else
            {
                func();
            }
        }
        public static T InvokeControl<T>(Control ctl, Func<T> func)
        {
            if (ctl.InvokeRequired)
            {
                return (T)ctl.Invoke(func);
            }
            else
            {
                return func();
            }
        }
    }
}
