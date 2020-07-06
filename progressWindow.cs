using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Timers;

namespace UIDesign
{
    public partial class progressWindow : Form
    {
        System.Timers.Timer timer1;
        int _max;
        public int _progress { get; set; }
        public progressWindow(ucMethodEditor ucME, int max)
        {
            InitializeComponent();

            timer1 = new System.Timers.Timer(1);
            timer1.Elapsed += new ElapsedEventHandler(refreshProgress);
            timer1.Enabled = true;
            timer1.SynchronizingObject = this;
            progressBar1.Maximum = max;
            _max = max - 1;
        }

        public void refreshProgress(object sender, EventArgs e)
        {
            label2.Text = String.Format("Inserting {0} of {1}", _progress, _max);
            progressBar1.Value = _progress;            
            if (progressBar1.Value == _max - 1)
            {
                this.Close();
                timer1.Enabled = false;
            }
        }
    }
}
