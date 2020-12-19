namespace Lab1
{
    partial class Form4_tz_scan
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form4_tz_scan));
            this.dataGridView_ip_table = new System.Windows.Forms.DataGridView();
            this.ip = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.mac = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.button = new System.Windows.Forms.DataGridViewButtonColumn();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.toolStripStatusLabel_scanStatus = new System.Windows.Forms.ToolStripStatusLabel();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView_ip_table)).BeginInit();
            this.statusStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // dataGridView_ip_table
            // 
            this.dataGridView_ip_table.AllowUserToAddRows = false;
            this.dataGridView_ip_table.AllowUserToDeleteRows = false;
            this.dataGridView_ip_table.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView_ip_table.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.ip,
            this.mac,
            this.button});
            this.dataGridView_ip_table.Location = new System.Drawing.Point(12, 12);
            this.dataGridView_ip_table.Name = "dataGridView_ip_table";
            this.dataGridView_ip_table.RowHeadersVisible = false;
            this.dataGridView_ip_table.Size = new System.Drawing.Size(304, 302);
            this.dataGridView_ip_table.TabIndex = 0;
            this.dataGridView_ip_table.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridView_ip_table_CellClick);
            // 
            // ip
            // 
            this.ip.HeaderText = "IP address";
            this.ip.Name = "ip";
            this.ip.ReadOnly = true;
            // 
            // mac
            // 
            this.mac.HeaderText = "MAC address";
            this.mac.Name = "mac";
            this.mac.ReadOnly = true;
            // 
            // button
            // 
            this.button.HeaderText = "Copy IP";
            this.button.Name = "button";
            this.button.ReadOnly = true;
            // 
            // statusStrip1
            // 
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripStatusLabel_scanStatus});
            this.statusStrip1.Location = new System.Drawing.Point(0, 330);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(330, 22);
            this.statusStrip1.TabIndex = 1;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // toolStripStatusLabel_scanStatus
            // 
            this.toolStripStatusLabel_scanStatus.Name = "toolStripStatusLabel_scanStatus";
            this.toolStripStatusLabel_scanStatus.Size = new System.Drawing.Size(96, 17);
            this.toolStripStatusLabel_scanStatus.Text = "Сканирование...";
            // 
            // Form4_tz_scan
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSize = true;
            this.ClientSize = new System.Drawing.Size(330, 352);
            this.Controls.Add(this.statusStrip1);
            this.Controls.Add(this.dataGridView_ip_table);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "Form4_tz_scan";
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.Text = "Сканер подсети";
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView_ip_table)).EndInit();
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.DataGridView dataGridView_ip_table;
        private System.Windows.Forms.DataGridViewTextBoxColumn ip;
        private System.Windows.Forms.DataGridViewTextBoxColumn mac;
        private System.Windows.Forms.DataGridViewButtonColumn button;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel_scanStatus;
    }
}