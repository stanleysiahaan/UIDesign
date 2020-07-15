namespace UIDesign
{
    partial class ucSettings
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
            this.label1 = new System.Windows.Forms.Label();
            this.btnCallClient = new System.Windows.Forms.Button();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.tbGateway = new System.Windows.Forms.TextBox();
            this.tbGatewayPort = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.tbOilCoolant = new System.Windows.Forms.TextBox();
            this.tbOilCoolantPort = new System.Windows.Forms.TextBox();
            this.label8 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.tbWaterCoolant = new System.Windows.Forms.TextBox();
            this.tbWaterCoolantPort = new System.Windows.Forms.TextBox();
            this.label10 = new System.Windows.Forms.Label();
            this.label11 = new System.Windows.Forms.Label();
            this.tbDAQ = new System.Windows.Forms.TextBox();
            this.tbDAQPort = new System.Windows.Forms.TextBox();
            this.btnSaveIP = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Futura Std Medium", 21.75F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic))));
            this.label1.Location = new System.Drawing.Point(77, 80);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(114, 38);
            this.label1.TabIndex = 0;
            this.label1.Text = "Settings";
            // 
            // btnCallClient
            // 
            this.btnCallClient.Location = new System.Drawing.Point(165, 236);
            this.btnCallClient.Name = "btnCallClient";
            this.btnCallClient.Size = new System.Drawing.Size(75, 23);
            this.btnCallClient.TabIndex = 5;
            this.btnCallClient.TabStop = false;
            this.btnCallClient.Text = "Call Client";
            this.btnCallClient.UseVisualStyleBackColor = true;
            this.btnCallClient.Click += new System.EventHandler(this.btnCallClient_Click);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(81, 128);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(106, 13);
            this.label4.TabIndex = 1;
            this.label4.Text = "Gateway (Texcel V4)";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(372, 128);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(26, 13);
            this.label5.TabIndex = 1;
            this.label5.Text = "Port";
            // 
            // tbGateway
            // 
            this.tbGateway.Location = new System.Drawing.Point(220, 125);
            this.tbGateway.Name = "tbGateway";
            this.tbGateway.Size = new System.Drawing.Size(146, 20);
            this.tbGateway.TabIndex = 1;
            // 
            // tbGatewayPort
            // 
            this.tbGatewayPort.Location = new System.Drawing.Point(404, 125);
            this.tbGatewayPort.Name = "tbGatewayPort";
            this.tbGatewayPort.Size = new System.Drawing.Size(59, 20);
            this.tbGatewayPort.TabIndex = 2;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(81, 153);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(112, 13);
            this.label6.TabIndex = 1;
            this.label6.Text = "IP Modbus Oil Coolant";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(372, 153);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(26, 13);
            this.label7.TabIndex = 1;
            this.label7.Text = "Port";
            // 
            // tbOilCoolant
            // 
            this.tbOilCoolant.Location = new System.Drawing.Point(220, 150);
            this.tbOilCoolant.Name = "tbOilCoolant";
            this.tbOilCoolant.Size = new System.Drawing.Size(146, 20);
            this.tbOilCoolant.TabIndex = 3;
            // 
            // tbOilCoolantPort
            // 
            this.tbOilCoolantPort.Location = new System.Drawing.Point(404, 150);
            this.tbOilCoolantPort.Name = "tbOilCoolantPort";
            this.tbOilCoolantPort.Size = new System.Drawing.Size(59, 20);
            this.tbOilCoolantPort.TabIndex = 4;
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(81, 178);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(129, 13);
            this.label8.TabIndex = 1;
            this.label8.Text = "IP Modbus Water Coolant";
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(372, 178);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(26, 13);
            this.label9.TabIndex = 1;
            this.label9.Text = "Port";
            // 
            // tbWaterCoolant
            // 
            this.tbWaterCoolant.Location = new System.Drawing.Point(220, 175);
            this.tbWaterCoolant.Name = "tbWaterCoolant";
            this.tbWaterCoolant.Size = new System.Drawing.Size(146, 20);
            this.tbWaterCoolant.TabIndex = 5;
            // 
            // tbWaterCoolantPort
            // 
            this.tbWaterCoolantPort.Location = new System.Drawing.Point(404, 175);
            this.tbWaterCoolantPort.Name = "tbWaterCoolantPort";
            this.tbWaterCoolantPort.Size = new System.Drawing.Size(59, 20);
            this.tbWaterCoolantPort.TabIndex = 6;
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(81, 205);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(81, 13);
            this.label10.TabIndex = 1;
            this.label10.Text = "IP DAQ Module";
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(372, 205);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(26, 13);
            this.label11.TabIndex = 1;
            this.label11.Text = "Port";
            // 
            // tbDAQ
            // 
            this.tbDAQ.Location = new System.Drawing.Point(220, 202);
            this.tbDAQ.Name = "tbDAQ";
            this.tbDAQ.Size = new System.Drawing.Size(146, 20);
            this.tbDAQ.TabIndex = 7;
            // 
            // tbDAQPort
            // 
            this.tbDAQPort.Location = new System.Drawing.Point(404, 202);
            this.tbDAQPort.Name = "tbDAQPort";
            this.tbDAQPort.Size = new System.Drawing.Size(59, 20);
            this.tbDAQPort.TabIndex = 8;
            // 
            // btnSaveIP
            // 
            this.btnSaveIP.Location = new System.Drawing.Point(84, 236);
            this.btnSaveIP.Name = "btnSaveIP";
            this.btnSaveIP.Size = new System.Drawing.Size(75, 23);
            this.btnSaveIP.TabIndex = 9;
            this.btnSaveIP.Text = "Save";
            this.btnSaveIP.UseVisualStyleBackColor = true;
            this.btnSaveIP.Click += new System.EventHandler(this.btnSaveIP_Click);
            // 
            // ucSettings
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.btnSaveIP);
            this.Controls.Add(this.btnCallClient);
            this.Controls.Add(this.tbDAQPort);
            this.Controls.Add(this.tbWaterCoolantPort);
            this.Controls.Add(this.tbOilCoolantPort);
            this.Controls.Add(this.tbGatewayPort);
            this.Controls.Add(this.tbDAQ);
            this.Controls.Add(this.tbWaterCoolant);
            this.Controls.Add(this.tbOilCoolant);
            this.Controls.Add(this.tbGateway);
            this.Controls.Add(this.label11);
            this.Controls.Add(this.label9);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label10);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label1);
            this.Name = "ucSettings";
            this.Size = new System.Drawing.Size(1791, 984);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button btnCallClient;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox tbGateway;
        private System.Windows.Forms.TextBox tbGatewayPort;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.TextBox tbOilCoolant;
        private System.Windows.Forms.TextBox tbOilCoolantPort;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.TextBox tbWaterCoolant;
        private System.Windows.Forms.TextBox tbWaterCoolantPort;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.TextBox tbDAQ;
        private System.Windows.Forms.TextBox tbDAQPort;
        private System.Windows.Forms.Button btnSaveIP;
    }
}
