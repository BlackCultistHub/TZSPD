using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Npgsql;

namespace Lab1
{
    public partial class Form_showDB : Form
    {
        public Form_showDB()
        {
            InitializeComponent();
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.CenterToScreen();
        }

        private void refreshTable()
        {
            try
            {
                dataGridView1.Rows.Clear();

                var logs = MSSQL_logging.get_database_logs();

                foreach (var log in logs)
                {
                    dataGridView1.Rows.Add(log.logDateTime, log.labnumber, log.logtext);
                }
            }
            catch
            {
                MessageBox.Show("Не удалось получить информацию из базы данных!", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void Form_showDB_Shown(object sender, EventArgs e)
        {
            refreshTable();
            dataGridView1.Refresh();
        }
    }
}
