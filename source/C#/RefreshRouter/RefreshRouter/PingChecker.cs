using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;

namespace RefreshRouter
{
    class PingChecker : Process
    {
        public PingChecker()
            : base()
        {
            base.StartInfo = new ProcessStartInfo();
            base.StartInfo.FileName = "cmd";
            //base.StartInfo.FileName = "ping";
            //base.StartInfo.Arguments = "8.8.8.8 -n 1";
            base.StartInfo.CreateNoWindow = true;
            base.StartInfo.UseShellExecute = false;
            base.StartInfo.RedirectStandardInput = true;
            base.StartInfo.RedirectStandardOutput = true;
            base.StartInfo.RedirectStandardError = false;
        }

        public bool Run()
        {
            Start();
            StandardInput.Write("chcp 437" + Environment.NewLine);
            StandardInput.Write("ping 8.8.8.8 -n 1" + Environment.NewLine);
            StandardInput.Close();
            String output = StandardOutput.ReadToEnd();
            WaitForExit();
            Close();
            //var a = from line in output.Split('\n')
            //        where line.IndexOf("Reply from 8.8.8.8: ") != -1
            //        let c =  line.Replace("Reply from 8.8.8.8: ","").Split(' ')
            var list = output.Split('\n').Where(c =>
            {
                return c.IndexOf("Reply from 8.8.8.8: ") != -1;
            }).Select(c =>
            {
                return c.Replace("Reply from 8.8.8.8: ", "").Replace("\r", "").Split(' ');
            }).FirstOrDefault();
            if (list != null)
            {
                var dic = list.Select(t =>
                {
                    return t.Split('=');
                }).Select(m =>
                {
                    return new { k = m[0], v = m[1] };
                }).ToDictionary(i => i.k, v => v.v);
                if (dic.ContainsKey("time"))
                {
                    return true;
                }
            }
            return false;
        }
    }
}
