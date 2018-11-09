namespace NetworkSettingsChangeApp
{
    partial class Form1
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
            this.ProxyOnBtn = new System.Windows.Forms.Button();
            this.eventListBox = new System.Windows.Forms.ListBox();
            this.proxyGroupBox = new System.Windows.Forms.GroupBox();
            this.proxyAdapterTB = new System.Windows.Forms.TextBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.normalAdapterTB = new System.Windows.Forms.TextBox();
            this.ProxyOffBtn = new System.Windows.Forms.Button();
            this.proxyGroupBox.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.SuspendLayout();
            // 
            // ProxyOnBtn
            // 
            this.ProxyOnBtn.Location = new System.Drawing.Point(12, 13);
            this.ProxyOnBtn.Name = "ProxyOnBtn";
            this.ProxyOnBtn.Size = new System.Drawing.Size(75, 23);
            this.ProxyOnBtn.TabIndex = 0;
            this.ProxyOnBtn.Text = "ProxyOn";
            this.ProxyOnBtn.UseVisualStyleBackColor = true;
            this.ProxyOnBtn.Click += new System.EventHandler(this.EnableProxy);
            // 
            // eventListBox
            // 
            this.eventListBox.FormattingEnabled = true;
            this.eventListBox.Location = new System.Drawing.Point(13, 104);
            this.eventListBox.Name = "eventListBox";
            this.eventListBox.Size = new System.Drawing.Size(603, 329);
            this.eventListBox.TabIndex = 1;
            // 
            // proxyGroupBox
            // 
            this.proxyGroupBox.Controls.Add(this.proxyAdapterTB);
            this.proxyGroupBox.Location = new System.Drawing.Point(224, 13);
            this.proxyGroupBox.Name = "proxyGroupBox";
            this.proxyGroupBox.Size = new System.Drawing.Size(193, 55);
            this.proxyGroupBox.TabIndex = 2;
            this.proxyGroupBox.TabStop = false;
            this.proxyGroupBox.Text = "Proxy\'li Adaptör";
            // 
            // proxyAdapterTB
            // 
            this.proxyAdapterTB.Location = new System.Drawing.Point(6, 19);
            this.proxyAdapterTB.Name = "proxyAdapterTB";
            this.proxyAdapterTB.Size = new System.Drawing.Size(181, 20);
            this.proxyAdapterTB.TabIndex = 0;
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.normalAdapterTB);
            this.groupBox2.Location = new System.Drawing.Point(423, 13);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(193, 55);
            this.groupBox2.TabIndex = 3;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Proxysiz Adaptör";
            // 
            // normalAdapterTB
            // 
            this.normalAdapterTB.Location = new System.Drawing.Point(6, 19);
            this.normalAdapterTB.Name = "normalAdapterTB";
            this.normalAdapterTB.Size = new System.Drawing.Size(181, 20);
            this.normalAdapterTB.TabIndex = 1;
            // 
            // ProxyOffBtn
            // 
            this.ProxyOffBtn.Location = new System.Drawing.Point(13, 42);
            this.ProxyOffBtn.Name = "ProxyOffBtn";
            this.ProxyOffBtn.Size = new System.Drawing.Size(75, 23);
            this.ProxyOffBtn.TabIndex = 4;
            this.ProxyOffBtn.Text = "ProxyOff";
            this.ProxyOffBtn.UseVisualStyleBackColor = true;
            this.ProxyOffBtn.Click += new System.EventHandler(this.DisableProxy);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(628, 444);
            this.Controls.Add(this.ProxyOffBtn);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.proxyGroupBox);
            this.Controls.Add(this.eventListBox);
            this.Controls.Add(this.ProxyOnBtn);
            this.Name = "Form1";
            this.Text = "Form1";
            this.proxyGroupBox.ResumeLayout(false);
            this.proxyGroupBox.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button ProxyOnBtn;
        private System.Windows.Forms.ListBox eventListBox;
        private System.Windows.Forms.GroupBox proxyGroupBox;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.TextBox proxyAdapterTB;
        private System.Windows.Forms.TextBox normalAdapterTB;
        private System.Windows.Forms.Button ProxyOffBtn;
    }
}

