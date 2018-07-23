using System;
using Gecko;
using System.IO;
using System.Windows.Forms;

namespace PTM.WindowForm
{
    class WebBrowser : GeckoWebBrowser
    {
        public WebBrowser(String port)
        {
            Xpcom.EnableProfileMonitoring = false;
            var app_dir = Path.GetDirectoryName(Application.ExecutablePath);
            Xpcom.Initialize(Path.Combine(app_dir, "Firefox"));

            Dock = System.Windows.Forms.DockStyle.Fill;
            FrameEventsPropagateToMainWindow = false;
            TabIndex = 0;
            UseHttpActivityObserver = false;
            Navigate("http://localhost:" + port);
        }
    }
}
