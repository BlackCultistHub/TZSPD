using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using System.IO;
using System.Runtime;

namespace Lab1
{
    public partial class Form2 : Form
    {
        public ArrayList log = new ArrayList();

        public delegate void UpdateLogBoxDelegate();
        public void InvokeUpdateLogBox()
        {
            logBox.Text = "";
            foreach (string line in log)
            {
                logBox.Text += line + Environment.NewLine;
            }
        }

        private delegate void UpdateInfoDelegate();
        private delegate void UpdateInfo2Delegate();
        public Form2()
        {
            InitializeComponent();
            this.FormBorderStyle = FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.CenterToScreen();

            timer1.Start();
            timer2.Start();
        }

        private bool IsDigitsOnly(string str)
        {
            foreach (char c in str)
            {
                if (c < '0' || c > '9')
                    return false;
            }

            return true;
        }

        private void HelpSelect_Click(object sender, EventArgs e)
        {
            var info = new Form_Info();
            info.Show();
        }

        private void лР1ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Hide();
            var lab1 = new Form1();
            lab1.Closed += (s, args) => this.Close();
            lab1.Show();
        }

        private void лР1ToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            this.Hide();
            var lab1 = new Form1_tz();
            lab1.Closed += (s, args) => this.Close();
            lab1.Show();
        }

        private void лР2ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Hide();
            var lab2 = new Form2_tz();
            lab2.Closed += (s, args) => this.Close();
            lab2.Show();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            Invoke(new UpdateInfoDelegate(InvokeUpdateInfo));
        }

        private void InvokeUpdateInfo()
        {
            //checks
            bool f_ok = true;
            if (!IsDigitsOnly(numb_a.Text))
            {
                errorProvider1.SetError(numb_a, "Field must be number!");
                var logLine = DateTime.Now.ToString() + ": В поле A не число.";
                var lineNoTime = "LAB2: В поле A не число.";
                log.Add(logLine);
                File.AppendAllText(Directory.GetCurrentDirectory() + "\\global_log.log", DateTime.Now.ToString() + ": LAB2: В поле A не число.");
                try
                {
                    var time_ms = MSSQL_logging.log_error_onTimer(DateTime.Now, "LAB2", lineNoTime);
                    toolStripStatusLabel1.Text = "Логирование выполнено за " + time_ms + "мс";
                }
                catch { }
                f_ok = false;
            }
            if (!IsDigitsOnly(numb_b.Text))
            {
                errorProvider1.SetError(numb_b, "Field must be number!");
                var logLine = DateTime.Now.ToString() + ": В поле B не число.";
                var lineNoTime = "LAB2: В поле B не число.";
                log.Add(logLine);
                File.AppendAllText(Directory.GetCurrentDirectory() + "\\global_log.log", DateTime.Now.ToString() + ": LAB2: В поле B не число.");
                try
                {
                    var time_ms = MSSQL_logging.log_error_onTimer(DateTime.Now, "LAB2", lineNoTime);
                    toolStripStatusLabel1.Text = "Логирование выполнено за " + time_ms + "мс";
                }
                catch { }
                f_ok = false;
            }
            if (!IsDigitsOnly(numb_n.Text))
            {
                errorProvider1.SetError(numb_n, "Field must be number!");
                var logLine = DateTime.Now.ToString() + ": В поле N не число.";
                var lineNoTime = "LAB2: В поле N не число.";
                log.Add(logLine);
                File.AppendAllText(Directory.GetCurrentDirectory() + "\\global_log.log", DateTime.Now.ToString() + ": LAB2: В поле N не число.");
                try
                {
                    var time_ms = MSSQL_logging.log_error_onTimer(DateTime.Now, "LAB2", lineNoTime);
                    toolStripStatusLabel1.Text = "Логирование выполнено за " + time_ms + "мс";
                }
                catch { }
                f_ok = false;
            }
            if (!f_ok)
            {
                label5.Text = "?";
                label7.Text = "?";
                label9.Text = "?";
                textBox_area.Text = "Error";
                textBox_digits.Text = "Error";
                textBox3.Text = "Error";
                textBox4.Text = "Error";
                textBox5.Text = "Error";
                Invoke(new UpdateLogBoxDelegate(InvokeUpdateLogBox));
                return;
            }
            errorProvider1.Clear();

            //
            if (numb_a.TextLength != 0)
            {
                label5.Text = numb_a.Text;
                label7.Text = numb_a.Text;
            }
            if (numb_b.TextLength != 0)
            {
                label9.Text = numb_b.Text;
            }
            if (numb_a.TextLength != 0 && numb_b.TextLength != 0 && numb_n.TextLength != 0)
            {
                //1
                int digits = 0;
                for (int i = 0; i < numb_n.TextLength; i++)
                {
                    if (int.Parse(numb_n.Text[i].ToString()) > int.Parse(numb_a.Text))
                        digits++;
                }
                textBox_digits.Text = digits.ToString();
                //2
                if (int.Parse(numb_a.Text) < int.Parse(numb_n.Text) && int.Parse(numb_n.Text) < int.Parse(numb_b.Text))
                    textBox_area.Text = "Да";
                else
                    textBox_area.Text = "Нет";
                //3
                if (int.Parse(numb_n.Text) % 3 == 0)
                    textBox3.Text = "Да";
                else
                    textBox3.Text = "Нет";
                if (int.Parse(numb_n.Text) % 4 == 0)
                    textBox4.Text = "Да";
                else
                    textBox4.Text = "Нет";
                if (int.Parse(numb_n.Text) % 5 == 0)
                    textBox5.Text = "Да";
                else
                    textBox5.Text = "Нет";
            }
        }

        private void timer2_Tick(object sender, EventArgs e)
        {
            Invoke(new UpdateInfo2Delegate(InvokeUpdateInfo2));
        }

        private void InvokeUpdateInfo2()
        {
            //checks
            bool s_ok = true;
            if (!IsDigitsOnly(numb_a_2.Text))
            {
                errorProvider2.SetError(numb_a_2, "Field must be number!");
                s_ok = false;
            }
            if (!IsDigitsOnly(numb_b_2.Text))
            {
                errorProvider2.SetError(numb_b_2, "Field must be number!");
                s_ok = false;
            }
            if (!s_ok)
            {
                textBox_summ.Text = "Error";
                return;
            }
            else
            {
                errorProvider2.Clear();
            }
            //
            int summ = 0;
            if (numb_a_2.TextLength != 0 && numb_b_2.TextLength != 0)
            {
                for (int i = (int)Math.Ceiling(double.Parse(numb_a_2.Text)); i < (int)Math.Floor(double.Parse(numb_b_2.Text)); i++)
                {
                    if (i % 13 == 0 && i % 5 == 0)
                        summ += i;
                }
            }
            textBox_summ.Text = summ.ToString();
        }

        private void менюToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Hide();
            var form = new Form_start();
            form.Closed += (s, args) => this.Close();
            form.Show();
        }

        private void сохранитьЛогToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }
    }
}
