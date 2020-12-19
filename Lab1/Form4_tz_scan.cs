using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Lab1
{
    public partial class Form4_tz_scan : Form
    {
        public Form4_tz_scan()
        {
            InitializeComponent();
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.CenterToScreen();
        }

        private void dataGridView_ip_table_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex == dataGridView_ip_table.Columns["button"].Index)
            {
                int row = e.RowIndex;
                string ip = (dataGridView_ip_table.Rows[row]).Cells[0].Value.ToString();
                Clipboard.SetText(ip);
            }
        }

        public void addRow(string ip, string hwadr)
        {
            dataGridView_ip_table.Rows.Add(ip, hwadr, "Copy IP");
        }

        public void SetStatus(string status)
        {
            toolStripStatusLabel_scanStatus.Text = status;
        }
    }
}
