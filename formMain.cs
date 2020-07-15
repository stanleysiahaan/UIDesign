using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security.AccessControl;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;


namespace UIDesign
{
    public partial class formMain : Form
    {
        public string projectID { get; set; }
        ucMethodEditor ucME = new ucMethodEditor();
        public formMain()
        {
            InitializeComponent();
            ucHomeMenu hmMn = new ucHomeMenu();
            panelContainer.Controls.Add(hmMn);
            
        }
        
        private void mainForm_Load(object sender, EventArgs e)
        {
            
        }

        private void btnHome_Click(object sender, EventArgs e)
        {
            if (navLabel.Text != "Home")
            {
                navLabel.Text = "Home";
                btnHome.BackColor = System.Drawing.Color.FromArgb(28, 135, 219);
                btnET.BackColor = System.Drawing.Color.FromArgb(44, 62, 80);
                btnDAQ.BackColor = System.Drawing.Color.FromArgb(44, 62, 80);
                btnPrmtr.BackColor = System.Drawing.Color.FromArgb(44, 62, 80);
                btnSettings.BackColor = System.Drawing.Color.FromArgb(44, 62, 80);
                btnExport.BackColor = System.Drawing.Color.FromArgb(44, 62, 80);
            }
            panelContainer.Controls.Clear();
            ucHomeMenu hmMn = new ucHomeMenu();
            panelContainer.Controls.Add(hmMn);
        }

        private void btnET_Click(object sender, EventArgs e)
        {
            if (navLabel.Text != "Engine Test")
            {
                navLabel.Text = "Engine Test";
                btnHome.BackColor = System.Drawing.Color.FromArgb(44, 62, 80);
                btnET.BackColor = System.Drawing.Color.FromArgb(28, 135, 219);
                btnDAQ.BackColor = System.Drawing.Color.FromArgb(44, 62, 80);
                btnPrmtr.BackColor = System.Drawing.Color.FromArgb(44, 62, 80);
                btnSettings.BackColor = System.Drawing.Color.FromArgb(44, 62, 80);
                btnExport.BackColor = System.Drawing.Color.FromArgb(44, 62, 80);
            }
            panelContainer.Controls.Clear();
            ucEngineTestingMenu ucETMn = new ucEngineTestingMenu();
            panelContainer.Controls.Add(ucETMn);
            
        }

        private void btnDAQ_Click(object sender, EventArgs e)
        {
            if (navLabel.Text != "DAQ")
            {
                navLabel.Text = "DAQ";
                btnHome.BackColor = System.Drawing.Color.FromArgb(44, 62, 80);
                btnET.BackColor = System.Drawing.Color.FromArgb(44, 62, 80);
                btnDAQ.BackColor = System.Drawing.Color.FromArgb(28, 135, 219);
                btnPrmtr.BackColor = System.Drawing.Color.FromArgb(44, 62, 80);
                btnSettings.BackColor = System.Drawing.Color.FromArgb(44, 62, 80);
                btnExport.BackColor = System.Drawing.Color.FromArgb(44, 62, 80);
            }
            else
            {
            }
        }

        private void btnPrmtr_Click(object sender, EventArgs e)
        {
            if (navLabel.Text != "Parameter")
            {
                navLabel.Text = "Parameter";
                btnHome.BackColor = System.Drawing.Color.FromArgb(44, 62, 80);
                btnET.BackColor = System.Drawing.Color.FromArgb(44, 62, 80);
                btnDAQ.BackColor = System.Drawing.Color.FromArgb(44, 62, 80);
                btnPrmtr.BackColor = System.Drawing.Color.FromArgb(28, 135, 219);
                btnSettings.BackColor = System.Drawing.Color.FromArgb(44, 62, 80);
                btnExport.BackColor = System.Drawing.Color.FromArgb(44, 62, 80);
            }
            panelContainer.Controls.Clear();
            ucMethodMenu prmtrMn = new ucMethodMenu();
            panelContainer.Controls.Add(prmtrMn);
        }

        private void btnSettings_Click(object sender, EventArgs e)
        {
            if (navLabel.Text != "Settings")
            {
                navLabel.Text = "Settings";
                btnHome.BackColor = System.Drawing.Color.FromArgb(44, 62, 80);
                btnET.BackColor = System.Drawing.Color.FromArgb(44, 62, 80);
                btnDAQ.BackColor = System.Drawing.Color.FromArgb(44, 62, 80);
                btnPrmtr.BackColor = System.Drawing.Color.FromArgb(44, 62, 80);
                btnSettings.BackColor = System.Drawing.Color.FromArgb(28, 135, 219);
                btnExport.BackColor = System.Drawing.Color.FromArgb(44, 62, 80);
            }
            panelContainer.Controls.Clear();
            ucSettings ucSettings = new ucSettings();
            panelContainer.Controls.Add(ucSettings);
        }

        private void btnExport_Click(object sender, EventArgs e)
        {
            if (navLabel.Text != "Export")
            {
                navLabel.Text = "Export";
                btnHome.BackColor = System.Drawing.Color.FromArgb(44, 62, 80);
                btnET.BackColor = System.Drawing.Color.FromArgb(44, 62, 80);
                btnDAQ.BackColor = System.Drawing.Color.FromArgb(44, 62, 80);
                btnPrmtr.BackColor = System.Drawing.Color.FromArgb(44, 62, 80);
                btnSettings.BackColor = System.Drawing.Color.FromArgb(44, 62, 80);
                btnExport.BackColor = System.Drawing.Color.FromArgb(28, 135, 219);
            }
            panelContainer.Controls.Clear();
            ucExportMenu ucEM = new ucExportMenu();
            panelContainer.Controls.Add(ucEM);
        }

    }
}
