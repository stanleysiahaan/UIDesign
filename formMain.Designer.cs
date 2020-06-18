namespace UIDesign
{
    partial class formMain
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(formMain));
            this.topBar = new System.Windows.Forms.Panel();
            this.navLabel = new System.Windows.Forms.Label();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.sideBar = new System.Windows.Forms.Panel();
            this.panelContainer = new System.Windows.Forms.Panel();
            this.btnET = new System.Windows.Forms.Button();
            this.btnExport = new System.Windows.Forms.Button();
            this.btnHome = new System.Windows.Forms.Button();
            this.btnPrmtr = new System.Windows.Forms.Button();
            this.btnSettings = new System.Windows.Forms.Button();
            this.btnDAQ = new System.Windows.Forms.Button();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.menuIcon = new System.Windows.Forms.PictureBox();
            this.topBar.SuspendLayout();
            this.sideBar.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.menuIcon)).BeginInit();
            this.SuspendLayout();
            // 
            // topBar
            // 
            this.topBar.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(44)))), ((int)(((byte)(62)))), ((int)(((byte)(80)))));
            this.topBar.Controls.Add(this.pictureBox1);
            this.topBar.Controls.Add(this.menuIcon);
            this.topBar.Controls.Add(this.navLabel);
            this.topBar.Dock = System.Windows.Forms.DockStyle.Top;
            this.topBar.Location = new System.Drawing.Point(0, 0);
            this.topBar.Margin = new System.Windows.Forms.Padding(2);
            this.topBar.Name = "topBar";
            this.topBar.Size = new System.Drawing.Size(1904, 57);
            this.topBar.TabIndex = 0;
            // 
            // navLabel
            // 
            this.navLabel.AutoSize = true;
            this.navLabel.BackColor = System.Drawing.Color.Transparent;
            this.navLabel.Font = new System.Drawing.Font("Futura Std Medium", 19.8F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic))), System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.navLabel.ForeColor = System.Drawing.Color.White;
            this.navLabel.Location = new System.Drawing.Point(38, 7);
            this.navLabel.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.navLabel.Name = "navLabel";
            this.navLabel.Size = new System.Drawing.Size(81, 35);
            this.navLabel.TabIndex = 2;
            this.navLabel.Text = "Menu";
            this.navLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // timer1
            // 
            this.timer1.Interval = 10;
            // 
            // sideBar
            // 
            this.sideBar.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.sideBar.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(44)))), ((int)(((byte)(62)))), ((int)(((byte)(80)))));
            this.sideBar.Controls.Add(this.btnET);
            this.sideBar.Controls.Add(this.btnExport);
            this.sideBar.Controls.Add(this.btnHome);
            this.sideBar.Controls.Add(this.btnPrmtr);
            this.sideBar.Controls.Add(this.btnSettings);
            this.sideBar.Controls.Add(this.btnDAQ);
            this.sideBar.Dock = System.Windows.Forms.DockStyle.Left;
            this.sideBar.Location = new System.Drawing.Point(0, 57);
            this.sideBar.Margin = new System.Windows.Forms.Padding(2);
            this.sideBar.Name = "sideBar";
            this.sideBar.Size = new System.Drawing.Size(113, 984);
            this.sideBar.TabIndex = 1;
            // 
            // panelContainer
            // 
            this.panelContainer.BackColor = System.Drawing.Color.White;
            this.panelContainer.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelContainer.Location = new System.Drawing.Point(113, 57);
            this.panelContainer.Margin = new System.Windows.Forms.Padding(2);
            this.panelContainer.Name = "panelContainer";
            this.panelContainer.Size = new System.Drawing.Size(1791, 984);
            this.panelContainer.TabIndex = 2;
            // 
            // btnET
            // 
            this.btnET.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(44)))), ((int)(((byte)(62)))), ((int)(((byte)(80)))));
            this.btnET.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.btnET.FlatAppearance.BorderSize = 0;
            this.btnET.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnET.Font = new System.Drawing.Font("Futura Std Medium", 12F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic))), System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnET.ForeColor = System.Drawing.Color.White;
            this.btnET.Image = global::UIDesign.Properties.Resources.icons8_engine_50px;
            this.btnET.Location = new System.Drawing.Point(0, 79);
            this.btnET.Margin = new System.Windows.Forms.Padding(2);
            this.btnET.Name = "btnET";
            this.btnET.Size = new System.Drawing.Size(113, 74);
            this.btnET.TabIndex = 3;
            this.btnET.Text = "Engine Test";
            this.btnET.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.btnET.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.btnET.UseVisualStyleBackColor = false;
            this.btnET.Click += new System.EventHandler(this.btnET_Click);
            // 
            // btnExport
            // 
            this.btnExport.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(44)))), ((int)(((byte)(62)))), ((int)(((byte)(80)))));
            this.btnExport.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.btnExport.FlatAppearance.BorderSize = 0;
            this.btnExport.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnExport.Font = new System.Drawing.Font("Futura Std Medium", 12F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic))), System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnExport.ForeColor = System.Drawing.Color.White;
            this.btnExport.Image = global::UIDesign.Properties.Resources.icons8_export_50px;
            this.btnExport.Location = new System.Drawing.Point(0, 394);
            this.btnExport.Margin = new System.Windows.Forms.Padding(2);
            this.btnExport.Name = "btnExport";
            this.btnExport.Size = new System.Drawing.Size(113, 78);
            this.btnExport.TabIndex = 7;
            this.btnExport.Text = "Export";
            this.btnExport.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.btnExport.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.btnExport.UseVisualStyleBackColor = false;
            this.btnExport.Click += new System.EventHandler(this.btnExport_Click);
            // 
            // btnHome
            // 
            this.btnHome.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(44)))), ((int)(((byte)(62)))), ((int)(((byte)(80)))));
            this.btnHome.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.btnHome.FlatAppearance.BorderSize = 0;
            this.btnHome.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(28)))), ((int)(((byte)(135)))), ((int)(((byte)(219)))));
            this.btnHome.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnHome.Font = new System.Drawing.Font("Futura Std Medium", 12F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic))), System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnHome.ForeColor = System.Drawing.Color.White;
            this.btnHome.Image = global::UIDesign.Properties.Resources.icons8_home_50px;
            this.btnHome.Location = new System.Drawing.Point(0, 0);
            this.btnHome.Margin = new System.Windows.Forms.Padding(2);
            this.btnHome.Name = "btnHome";
            this.btnHome.Size = new System.Drawing.Size(113, 74);
            this.btnHome.TabIndex = 2;
            this.btnHome.Text = "Home";
            this.btnHome.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.btnHome.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.btnHome.UseVisualStyleBackColor = false;
            this.btnHome.Click += new System.EventHandler(this.btnHome_Click);
            // 
            // btnPrmtr
            // 
            this.btnPrmtr.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(44)))), ((int)(((byte)(62)))), ((int)(((byte)(80)))));
            this.btnPrmtr.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.btnPrmtr.FlatAppearance.BorderSize = 0;
            this.btnPrmtr.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnPrmtr.Font = new System.Drawing.Font("Futura Std Medium", 12F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic))), System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnPrmtr.ForeColor = System.Drawing.Color.White;
            this.btnPrmtr.Image = global::UIDesign.Properties.Resources.icons8_coordinate_system_50px;
            this.btnPrmtr.Location = new System.Drawing.Point(0, 236);
            this.btnPrmtr.Margin = new System.Windows.Forms.Padding(2);
            this.btnPrmtr.Name = "btnPrmtr";
            this.btnPrmtr.Size = new System.Drawing.Size(113, 74);
            this.btnPrmtr.TabIndex = 5;
            this.btnPrmtr.Text = "Method";
            this.btnPrmtr.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.btnPrmtr.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.btnPrmtr.UseVisualStyleBackColor = false;
            this.btnPrmtr.Click += new System.EventHandler(this.btnPrmtr_Click);
            // 
            // btnSettings
            // 
            this.btnSettings.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(44)))), ((int)(((byte)(62)))), ((int)(((byte)(80)))));
            this.btnSettings.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.btnSettings.FlatAppearance.BorderSize = 0;
            this.btnSettings.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnSettings.Font = new System.Drawing.Font("Futura Std Medium", 12F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic))), System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnSettings.ForeColor = System.Drawing.Color.White;
            this.btnSettings.Image = global::UIDesign.Properties.Resources.icons8_settings_50px;
            this.btnSettings.Location = new System.Drawing.Point(0, 315);
            this.btnSettings.Margin = new System.Windows.Forms.Padding(2);
            this.btnSettings.Name = "btnSettings";
            this.btnSettings.Size = new System.Drawing.Size(113, 74);
            this.btnSettings.TabIndex = 6;
            this.btnSettings.Text = "Settings";
            this.btnSettings.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.btnSettings.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.btnSettings.UseVisualStyleBackColor = false;
            this.btnSettings.Click += new System.EventHandler(this.btnSettings_Click);
            // 
            // btnDAQ
            // 
            this.btnDAQ.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(44)))), ((int)(((byte)(62)))), ((int)(((byte)(80)))));
            this.btnDAQ.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.btnDAQ.FlatAppearance.BorderSize = 0;
            this.btnDAQ.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnDAQ.Font = new System.Drawing.Font("Futura Std Medium", 12F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic))), System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnDAQ.ForeColor = System.Drawing.Color.White;
            this.btnDAQ.Image = global::UIDesign.Properties.Resources.icons8_caliper_50px;
            this.btnDAQ.Location = new System.Drawing.Point(0, 158);
            this.btnDAQ.Margin = new System.Windows.Forms.Padding(2);
            this.btnDAQ.Name = "btnDAQ";
            this.btnDAQ.Size = new System.Drawing.Size(113, 74);
            this.btnDAQ.TabIndex = 4;
            this.btnDAQ.Text = "DAQ";
            this.btnDAQ.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.btnDAQ.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.btnDAQ.UseVisualStyleBackColor = false;
            this.btnDAQ.Click += new System.EventHandler(this.btnDAQ_Click);
            // 
            // pictureBox1
            // 
            this.pictureBox1.BackColor = System.Drawing.Color.Transparent;
            this.pictureBox1.Image = global::UIDesign.Properties.Resources.icons8_info_50px;
            this.pictureBox1.Location = new System.Drawing.Point(1851, 3);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(50, 50);
            this.pictureBox1.TabIndex = 3;
            this.pictureBox1.TabStop = false;
            // 
            // menuIcon
            // 
            this.menuIcon.Cursor = System.Windows.Forms.Cursors.Hand;
            this.menuIcon.Image = global::UIDesign.Properties.Resources.menuicon;
            this.menuIcon.Location = new System.Drawing.Point(9, 10);
            this.menuIcon.Margin = new System.Windows.Forms.Padding(2);
            this.menuIcon.Name = "menuIcon";
            this.menuIcon.Size = new System.Drawing.Size(26, 28);
            this.menuIcon.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.menuIcon.TabIndex = 2;
            this.menuIcon.TabStop = false;
            // 
            // formMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(1904, 1041);
            this.Controls.Add(this.panelContainer);
            this.Controls.Add(this.sideBar);
            this.Controls.Add(this.topBar);
            this.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(21)))), ((int)(((byte)(29)))), ((int)(((byte)(38)))));
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(2);
            this.Name = "formMain";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Engine Test Program";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.topBar.ResumeLayout(false);
            this.topBar.PerformLayout();
            this.sideBar.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.menuIcon)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel topBar;
        private System.Windows.Forms.PictureBox menuIcon;
        private System.Windows.Forms.Label navLabel;
        private System.Windows.Forms.Timer timer1;
        private System.Windows.Forms.Button btnHome;
        private System.Windows.Forms.Button btnET;
        private System.Windows.Forms.Button btnDAQ;
        private System.Windows.Forms.Button btnExport;
        private System.Windows.Forms.Button btnSettings;
        private System.Windows.Forms.Button btnPrmtr;
        private System.Windows.Forms.Panel sideBar;
        private System.Windows.Forms.Panel panelContainer;
        private System.Windows.Forms.PictureBox pictureBox1;
    }
}