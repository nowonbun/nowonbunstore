using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using log4net;
using log4net.Config;

namespace travel
{
    public partial class Form1 : Form
    {
        private ILog logger = LogManager.GetLogger(typeof(Form1));

        private Server server = new Server();
        
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            logger.Info("Server Start!!");
            this.button1.Enabled = false;
            server.Run();
        }
    }
}
