namespace NetworkSettingsChangeApp
{
    partial class MainForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            this.ProxyOnBtn = new System.Windows.Forms.Button();
            this.ProxyOffBtn = new System.Windows.Forms.Button();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
            this.progressBar1 = new System.Windows.Forms.ProgressBar();
            this.progressTextLabel = new System.Windows.Forms.Label();
            this.tableLayoutPanel1.SuspendLayout();
            this.tableLayoutPanel2.SuspendLayout();
            this.SuspendLayout();
            // 
            // ProxyOnBtn
            // 
            this.ProxyOnBtn.BackColor = System.Drawing.Color.DarkGreen;
            this.ProxyOnBtn.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ProxyOnBtn.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ProxyOnBtn.ForeColor = System.Drawing.Color.White;
            this.ProxyOnBtn.Location = new System.Drawing.Point(3, 3);
            this.ProxyOnBtn.Name = "ProxyOnBtn";
            this.ProxyOnBtn.Size = new System.Drawing.Size(305, 125);
            this.ProxyOnBtn.TabIndex = 0;
            this.ProxyOnBtn.Text = "ProxyOn";
            this.ProxyOnBtn.UseVisualStyleBackColor = false;
            this.ProxyOnBtn.Click += new System.EventHandler(this.EnableProxy);
            this.ProxyOnBtn.MouseEnter += new System.EventHandler(this.ProxyOnBtn_MouseEnter);
            this.ProxyOnBtn.MouseLeave += new System.EventHandler(this.ProxyOnBtn_MouseLeave);
            // 
            // ProxyOffBtn
            // 
            this.ProxyOffBtn.BackColor = System.Drawing.Color.DarkRed;
            this.ProxyOffBtn.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ProxyOffBtn.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ProxyOffBtn.ForeColor = System.Drawing.Color.White;
            this.ProxyOffBtn.Location = new System.Drawing.Point(314, 3);
            this.ProxyOffBtn.Name = "ProxyOffBtn";
            this.ProxyOffBtn.Size = new System.Drawing.Size(305, 125);
            this.ProxyOffBtn.TabIndex = 4;
            this.ProxyOffBtn.Text = "ProxyOff";
            this.ProxyOffBtn.UseVisualStyleBackColor = false;
            this.ProxyOffBtn.Click += new System.EventHandler(this.DisableProxy);
            this.ProxyOffBtn.MouseEnter += new System.EventHandler(this.ProxyOffBtn_MouseEnter);
            this.ProxyOffBtn.MouseLeave += new System.EventHandler(this.ProxyOffBtn_MouseLeave);
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 2;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.Controls.Add(this.ProxyOnBtn, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.ProxyOffBtn, 1, 0);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(3, 3);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 1;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 131F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(622, 131);
            this.tableLayoutPanel1.TabIndex = 5;
            // 
            // tableLayoutPanel2
            // 
            this.tableLayoutPanel2.ColumnCount = 1;
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel2.Controls.Add(this.tableLayoutPanel1, 0, 0);
            this.tableLayoutPanel2.Controls.Add(this.progressBar1, 0, 2);
            this.tableLayoutPanel2.Controls.Add(this.progressTextLabel, 0, 1);
            this.tableLayoutPanel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel2.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel2.Name = "tableLayoutPanel2";
            this.tableLayoutPanel2.RowCount = 3;
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel2.Size = new System.Drawing.Size(628, 177);
            this.tableLayoutPanel2.TabIndex = 6;
            // 
            // progressBar1
            // 
            this.progressBar1.Location = new System.Drawing.Point(3, 160);
            this.progressBar1.Name = "progressBar1";
            this.progressBar1.Size = new System.Drawing.Size(622, 14);
            this.progressBar1.Step = 25;
            this.progressBar1.TabIndex = 6;
            // 
            // progressTextLabel
            // 
            this.progressTextLabel.AutoSize = true;
            this.progressTextLabel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.progressTextLabel.Location = new System.Drawing.Point(3, 137);
            this.progressTextLabel.Name = "progressTextLabel";
            this.progressTextLabel.Size = new System.Drawing.Size(622, 20);
            this.progressTextLabel.TabIndex = 7;
            this.progressTextLabel.Text = "label1";
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(628, 177);
            this.Controls.Add(this.tableLayoutPanel2);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "MainForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Proxy Aç Kapa";
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel2.ResumeLayout(false);
            this.tableLayoutPanel2.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button ProxyOnBtn;
        private System.Windows.Forms.Button ProxyOffBtn;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel2;
        private System.Windows.Forms.ProgressBar progressBar1;
        private System.Windows.Forms.Label progressTextLabel;
    }
}

