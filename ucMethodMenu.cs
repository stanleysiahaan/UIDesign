using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing.Drawing2D;

namespace UIDesign
{
    public partial class ucMethodMenu : UserControl
    {

        //private static parameterMenu _instance;
        //public static parameterMenu Instance
        //{
        //    get
        //    {
        //        if (_instance == null)
        //            _instance = new parameterMenu();
        //        return _instance;
        //    }
        //}

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
