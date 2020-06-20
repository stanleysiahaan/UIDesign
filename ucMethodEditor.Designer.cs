namespace UIDesign
{
    partial class ucMethodEditor
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
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.Step = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Torque = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.RPM = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.hour = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.minute = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.second = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ramp_time = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.oil_temp = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.cool_temp = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.label1 = new System.Windows.Forms.Label();
            this.tbMethodName = new System.Windows.Forms.TextBox();
            this.btnSubmitMethod = new System.Windows.Forms.Button();
            this.textBox1 = new System.Windows.Forms.TextBox();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            this.SuspendLayout();
            // 
            // dataGridView1
            // 
            this.dataGridView1.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.dataGridView1.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.Step,
            this.Torque,
            this.RPM,
            this.hour,
            this.minute,
            this.second,
            this.ramp_time,
            this.oil_temp,
            this.cool_temp});
            this.dataGridView1.Location = new System.Drawing.Point(44, 66);
            this.dataGridView1.Margin = new System.Windows.Forms.Padding(2);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.RowHeadersVisible = false;
            this.dataGridView1.RowHeadersWidth = 51;
            this.dataGridView1.RowTemplate.Height = 24;
            this.dataGridView1.Size = new System.Drawing.Size(1707, 880);
            this.dataGridView1.TabIndex = 0;
            this.dataGridView1.CellEnter += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridView1_CellEnter);
            this.dataGridView1.RowPostPaint += new System.Windows.Forms.DataGridViewRowPostPaintEventHandler(this.dataGridView1_RowPostPaint);
            // 
            // Step
            // 
            this.Step.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.ColumnHeader;
            this.Step.HeaderText = "Step";
            this.Step.MinimumWidth = 6;
            this.Step.Name = "Step";
            this.Step.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.Step.Width = 35;
            // 
            // Torque
            // 
            this.Torque.HeaderText = "Torque";
            this.Torque.MinimumWidth = 6;
            this.Torque.Name = "Torque";
            // 
            // RPM
            // 
            this.RPM.HeaderText = "RPM";
            this.RPM.MinimumWidth = 6;
            this.RPM.Name = "RPM";
            // 
            // hour
            // 
            this.hour.HeaderText = "Duration (Hours)";
            this.hour.MinimumWidth = 6;
            this.hour.Name = "hour";
            // 
            // minute
            // 
            this.minute.HeaderText = "Duration (Minutes)";
            this.minute.MinimumWidth = 6;
            this.minute.Name = "minute";
            // 
            // second
            // 
            this.second.HeaderText = "Duration (Seconds)";
            this.second.MinimumWidth = 6;
            this.second.Name = "second";
            // 
            // ramp_time
            // 
            this.ramp_time.HeaderText = "Ramp Time (second)";
            this.ramp_time.Name = "ramp_time";
            // 
            // oil_temp
            // 
            this.oil_temp.HeaderText = "Oil Temperature";
            this.oil_temp.MinimumWidth = 6;
            this.oil_temp.Name = "oil_temp";
            // 
            // cool_temp
            // 
            this.cool_temp.HeaderText = "Coolant Temperature";
            this.cool_temp.MinimumWidth = 6;
            this.cool_temp.Name = "cool_temp";
            // 
            // label1
            // 
            this.label1.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(40, 42);
            this.label1.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(74, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = "Method Name";
            // 
            // tbMethodName
            // 
            this.tbMethodName.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.tbMethodName.Location = new System.Drawing.Point(131, 42);
            this.tbMethodName.Margin = new System.Windows.Forms.Padding(2);
            this.tbMethodName.Name = "tbMethodName";
            this.tbMethodName.Size = new System.Drawing.Size(197, 20);
            this.tbMethodName.TabIndex = 2;
            // 
            // btnSubmitMethod
            // 
            this.btnSubmitMethod.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.btnSubmitMethod.Location = new System.Drawing.Point(331, 42);
            this.btnSubmitMethod.Margin = new System.Windows.Forms.Padding(2);
            this.btnSubmitMethod.Name = "btnSubmitMethod";
            this.btnSubmitMethod.Size = new System.Drawing.Size(56, 19);
            this.btnSubmitMethod.TabIndex = 3;
            this.btnSubmitMethod.Text = "Submit";
            this.btnSubmitMethod.UseVisualStyleBackColor = true;
            this.btnSubmitMethod.Click += new System.EventHandler(this.btnSubmitMethod_Click);
            // 
            // textBox1
            // 
            this.textBox1.Location = new System.Drawing.Point(393, 40);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(100, 20);
            this.textBox1.TabIndex = 4;
            // 
            // ucMethodEditor
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSize = true;
            this.Controls.Add(this.textBox1);
            this.Controls.Add(this.btnSubmitMethod);
            this.Controls.Add(this.tbMethodName);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.dataGridView1);
            this.Margin = new System.Windows.Forms.Padding(2);
            this.Name = "ucMethodEditor";
            this.Size = new System.Drawing.Size(1791, 984);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.DataGridView dataGridView1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox tbMethodName;
        private System.Windows.Forms.Button btnSubmitMethod;
        private System.Windows.Forms.DataGridViewTextBoxColumn Step;
        private System.Windows.Forms.DataGridViewTextBoxColumn Torque;
        private System.Windows.Forms.DataGridViewTextBoxColumn RPM;
        private System.Windows.Forms.DataGridViewTextBoxColumn hour;
        private System.Windows.Forms.DataGridViewTextBoxColumn minute;
        private System.Windows.Forms.DataGridViewTextBoxColumn second;
        private System.Windows.Forms.DataGridViewTextBoxColumn ramp_time;
        private System.Windows.Forms.DataGridViewTextBoxColumn oil_temp;
        private System.Windows.Forms.DataGridViewTextBoxColumn cool_temp;
        private System.Windows.Forms.TextBox textBox1;
    }
}
