using System;
using System.Windows.Forms;

namespace UIDesign
{
    public partial class ucMethodMenu : UserControl
    {
        public ucMethodMenu()
        {
            InitializeComponent();
        }

        private void btnPrmtrNew_Click(object sender, EventArgs e)
        {
            ucMethodEditor prmntrEdtr = new ucMethodEditor();
            this.Hide();//because usercontrols have not Close() property as forms
            this.Parent.Controls.Add(prmntrEdtr);
        }

        private void btnPrmtrEdit_Click(object sender, EventArgs e)
        {
            ucMethodList prmtrLst = new ucMethodList();
            this.Hide();
            this.Parent.Controls.Add(prmtrLst);
        }
    }
}
