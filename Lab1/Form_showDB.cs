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
                var settings = (new Form_Params()).getSettings();
                var cs = "Host=" + settings[0] + ";Port=" + settings[1] + ";Username=" + settings[2] + ";Password=" + settings[3] + ";Database=tzspd_user_errors";

                var con = new NpgsqlConnection(cs);
                con.Open();
                var cmd = new NpgsqlCommand();
                cmd.Connection = con;
                cmd.CommandText = "select * from error_data;";
                NpgsqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    dataGridView1.Rows.Add(reader.GetValue(0).ToString(), reader.GetValue(1).ToString(), reader.GetValue(2).ToString());
                }
                reader.Close();
                cmd.Cancel();
                con.Close();
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
