namespace UIDesign
{
    partial class ucMethodMenu
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
            this.btnPrmtrEdit = new System.Windows.Forms.Button();
            this.btnPrmtrNew = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // btnPrmtrEdit
            // 
            this.btnPrmtrEdit.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.btnPrmtrEdit.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(44)))), ((int)(((byte)(62)))), ((int)(((byte)(80)))));
            this.btnPrmtrEdit.Font = new System.Drawing.Font("Futura Std Medium", 20.25F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic))), System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnPrmtrEdit.ForeColor = System.Drawing.Color.White;
            this.btnPrmtrEdit.Image = global::UIDesign.Properties.Resources.icons8_edit_file_127px;
            this.btnPrmtrEdit.ImageAlign = System.Drawing.ContentAlignment.TopCenter;
            this.btnPrmtrEdit.Location = new System.Drawing.Point(1022, 375);
            this.btnPrmtrEdit.Name = "btnPrmtrEdit";
            this.btnPrmtrEdit.Padding = new System.Windows.Forms.Padding(0, 0, 0, 15);
            this.btnPrmtrEdit.Size = new System.Drawing.Size(247, 190);
            this.btnPrmtrEdit.TabIndex = 0;
            this.btnPrmtrEdit.Text = "View/Edit";
            this.btnPrmtrEdit.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.btnPrmtrEdit.UseVisualStyleBackColor = false;
            this.btnPrmtrEdit.Click += new System.EventHandler(this.btnPrmtrEdit_Click);
            // 
            // btnPrmtrNew
            // 
            this.btnPrmtrNew.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.btnPrmtrNew.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(44)))), ((int)(((byte)(62)))), ((int)(((byte)(80)))));
            this.btnPrmtrNew.Font = new System.Drawing.Font("Futura Std Medium", 20.25F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic))), System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnPrmtrNew.ForeColor = System.Drawing.Color.White;
            this.btnPrmtrNew.Image = global::UIDesign.Properties.Resources.icons8_create_127px;
            this.btnPrmtrNew.ImageAlign = System.Drawing.ContentAlignment.TopCenter;
            this.btnPrmtrNew.Location = new System.Drawing.Point(466, 375);
            this.btnPrmtrNew.Name = "btnPrmtrNew";
            this.btnPrmtrNew.Padding = new System.Windows.Forms.Padding(0, 0, 0, 20);
            this.btnPrmtrNew.Size = new System.Drawing.Size(247, 190);
            this.btnPrmtrNew.TabIndex = 0;
            this.btnPrmtrNew.Text = "New";
            this.btnPrmtrNew.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.btnPrmtrNew.UseVisualStyleBackColor = false;
            this.btnPrmtrNew.Click += new System.EventHandler(this.btnPrmtrNew_Click);
            // 
            // ucMethodMenu
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSize = true;
            this.Controls.Add(this.btnPrmtrEdit);
            this.Controls.Add(this.btnPrmtrNew);
            this.Name = "ucMethodMenu";
            this.Size = new System.Drawing.Size(1791, 984);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btnPrmtrNew;
        private System.Windows.Forms.Button btnPrmtrEdit;
    }
}
