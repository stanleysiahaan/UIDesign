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
    public partial class ucEngineTestingMenu : UserControl
    {
        public ucEngineTestingMenu()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {

            ucChooseProject ucChooseProject = new ucChooseProject();
            this.Hide();
            this.Parent.Controls.Add(ucChooseProject);

        }

        private void button2_Click(object sender, EventArgs e)
        {
            ucCreateProject ucCreateParameter = new ucCreateProject();
            this.Hide();
            this.Parent.Controls.Add(ucCreateParameter);
        }


    }
}
