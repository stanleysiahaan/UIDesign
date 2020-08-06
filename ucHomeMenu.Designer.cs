namespace UIDesign
{
    partial class ucHomeMenu
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.button3 = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.button1 = new System.Windows.Forms.Button();
            this.pnlExcuteTesting = new System.Windows.Forms.Panel();
            this.pnlCreateMethod = new System.Windows.Forms.Panel();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.panel1 = new System.Windows.Forms.Panel();
            this.panel2 = new System.Windows.Forms.Panel();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.pnlExcuteTesting.SuspendLayout();
            this.pnlCreateMethod.SuspendLayout();
            this.SuspendLayout();
            // 
            // button3
            // 
            this.button3.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.button3.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(44)))), ((int)(((byte)(62)))), ((int)(((byte)(80)))));
            this.button3.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button3.Font = new System.Drawing.Font("Futura Std Medium", 20.25F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic))));
            this.button3.ForeColor = System.Drawing.Color.White;
            this.button3.Image = global::UIDesign.Properties.Resources.icons8_list_127px_2;
            this.button3.ImageAlign = System.Drawing.ContentAlignment.TopCenter;
            this.button3.Location = new System.Drawing.Point(231, 605);
            this.button3.Margin = new System.Windows.Forms.Padding(2);
            this.button3.Name = "button3";
            this.button3.Padding = new System.Windows.Forms.Padding(0, 8, 0, 20);
            this.button3.Size = new System.Drawing.Size(247, 190);
            this.button3.TabIndex = 2;
            this.button3.Text = "Create Method";
            this.button3.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.button3.UseVisualStyleBackColor = false;
            this.button3.Click += new System.EventHandler(this.button3_Click);
            // 
            // button2
            // 
            this.button2.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.button2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(44)))), ((int)(((byte)(62)))), ((int)(((byte)(80)))));
            this.button2.Enabled = false;
            this.button2.Font = new System.Drawing.Font("Futura Std Medium", 20.25F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic))));
            this.button2.ForeColor = System.Drawing.Color.White;
            this.button2.Image = global::UIDesign.Properties.Resources.icons8_caliper_100px;
            this.button2.ImageAlign = System.Drawing.ContentAlignment.TopCenter;
            this.button2.Location = new System.Drawing.Point(1524, 778);
            this.button2.Margin = new System.Windows.Forms.Padding(2);
            this.button2.Name = "button2";
            this.button2.Padding = new System.Windows.Forms.Padding(0, 15, 0, 20);
            this.button2.Size = new System.Drawing.Size(247, 190);
            this.button2.TabIndex = 1;
            this.button2.Text = "DAQ Settings";
            this.button2.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.button2.UseVisualStyleBackColor = false;
            this.button2.Visible = false;
            // 
            // button1
            // 
            this.button1.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.button1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(44)))), ((int)(((byte)(62)))), ((int)(((byte)(80)))));
            this.button1.FlatAppearance.BorderSize = 0;
            this.button1.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(28)))), ((int)(((byte)(135)))), ((int)(((byte)(219)))));
            this.button1.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button1.Font = new System.Drawing.Font("Futura Std Medium", 20.25F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic))));
            this.button1.ForeColor = System.Drawing.Color.White;
            this.button1.Image = global::UIDesign.Properties.Resources.icons8_graph_127px_1;
            this.button1.ImageAlign = System.Drawing.ContentAlignment.TopCenter;
            this.button1.Location = new System.Drawing.Point(231, 221);
            this.button1.Margin = new System.Windows.Forms.Padding(2);
            this.button1.Name = "button1";
            this.button1.Padding = new System.Windows.Forms.Padding(0, 0, 0, 20);
            this.button1.Size = new System.Drawing.Size(247, 190);
            this.button1.TabIndex = 0;
            this.button1.Text = "Execute Project";
            this.button1.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.button1.UseVisualStyleBackColor = false;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // pnlExcuteTesting
            // 
            this.pnlExcuteTesting.Controls.Add(this.panel1);
            this.pnlExcuteTesting.Controls.Add(this.label1);
            this.pnlExcuteTesting.Controls.Add(this.label2);
            this.pnlExcuteTesting.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(44)))), ((int)(((byte)(62)))), ((int)(((byte)(80)))));
            this.pnlExcuteTesting.Location = new System.Drawing.Point(483, 221);
            this.pnlExcuteTesting.Name = "pnlExcuteTesting";
            this.pnlExcuteTesting.Size = new System.Drawing.Size(992, 190);
            this.pnlExcuteTesting.TabIndex = 3;
            // 
            // pnlCreateMethod
            // 
            this.pnlCreateMethod.Controls.Add(this.panel2);
            this.pnlCreateMethod.Controls.Add(this.label3);
            this.pnlCreateMethod.Controls.Add(this.label4);
            this.pnlCreateMethod.Location = new System.Drawing.Point(483, 605);
            this.pnlCreateMethod.Name = "pnlCreateMethod";
            this.pnlCreateMethod.Size = new System.Drawing.Size(992, 190);
            this.pnlCreateMethod.TabIndex = 3;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.BackColor = System.Drawing.Color.Transparent;
            this.label1.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.label1.Font = new System.Drawing.Font("Futura Std Medium", 24F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic))), System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(44)))), ((int)(((byte)(62)))), ((int)(((byte)(80)))));
            this.label1.Location = new System.Drawing.Point(3, 42);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(341, 42);
            this.label1.TabIndex = 0;
            this.label1.Text = "Execute Existing Project";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Arial", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(7, 96);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(668, 44);
            this.label2.TabIndex = 1;
            this.label2.Text = "Start engine testing from previously made project.\r\nYou can create a new project " +
    "from engine test submenu on side navigation bar.";
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(231)))), ((int)(((byte)(49)))), ((int)(((byte)(53)))));
            this.panel1.ForeColor = System.Drawing.Color.Transparent;
            this.panel1.Location = new System.Drawing.Point(12, 83);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(379, 5);
            this.panel1.TabIndex = 2;
            // 
            // panel2
            // 
            this.panel2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(231)))), ((int)(((byte)(49)))), ((int)(((byte)(53)))));
            this.panel2.ForeColor = System.Drawing.Color.Transparent;
            this.panel2.Location = new System.Drawing.Point(12, 91);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(379, 5);
            this.panel2.TabIndex = 5;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.BackColor = System.Drawing.Color.Transparent;
            this.label3.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.label3.Font = new System.Drawing.Font("Futura Std Medium", 24F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic))), System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(44)))), ((int)(((byte)(62)))), ((int)(((byte)(80)))));
            this.label3.Location = new System.Drawing.Point(3, 50);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(397, 42);
            this.label3.TabIndex = 3;
            this.label3.Text = "Create New Testing Method";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Arial", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(44)))), ((int)(((byte)(62)))), ((int)(((byte)(80)))));
            this.label4.Location = new System.Drawing.Point(7, 104);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(583, 44);
            this.label4.TabIndex = 4;
            this.label4.Text = "Testing method is a sequence of command that will be sent to texcel\r\nSelect this " +
    "menu to create a new one.";
            // 
            // ucHomeMenu
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSize = true;
            this.BackColor = System.Drawing.Color.White;
            this.Controls.Add(this.pnlCreateMethod);
            this.Controls.Add(this.pnlExcuteTesting);
            this.Controls.Add(this.button3);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.button1);
            this.Margin = new System.Windows.Forms.Padding(2);
            this.Name = "ucHomeMenu";
            this.Size = new System.Drawing.Size(1791, 984);
            this.pnlExcuteTesting.ResumeLayout(false);
            this.pnlExcuteTesting.PerformLayout();
            this.pnlCreateMethod.ResumeLayout(false);
            this.pnlCreateMethod.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Button button3;
        private System.Windows.Forms.Panel pnlExcuteTesting;
        private System.Windows.Forms.Panel pnlCreateMethod;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
    }
}
