using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace UIDesign
{
    public partial class ucHomeMenu : UserControl
    {
        private static ucHomeMenu _instance;
        public static ucHomeMenu Instance
        {
            get
            {
                if (_instance == null)
                    _instance = new ucHomeMenu();
                return _instance;
            }
        }

        public ucHomeMenu()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            ucChooseProject ucChooseProject = new ucChooseProject();
            this.Hide();
            this.Parent.Controls.Add(ucChooseProject);
        }

        private void button3_Click(object sender, EventArgs e)
        {
            ucMethodEditor prmntrEdtr = new ucMethodEditor();
            this.Hide();//because usercontrols have not Close() property as forms
            this.Parent.Controls.Add(prmntrEdtr);
        }
    }
}
