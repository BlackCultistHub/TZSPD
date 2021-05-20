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
{    public partial class Form1 : Form
    {
        public class SystemCharException : Exception
        {
            public SystemCharException(string message, int code_):base(message)
            {
                code = code_;
            }
            public int code;
        }
        
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

        public Form1()
        {
            InitializeComponent();
            this.FormBorderStyle = FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.CenterToScreen();

            timer1.Start();
        }

        private void addition_Click(object sender, EventArgs e)
        {   
            string str="";
            try
            {
                int minlen, maxlen;
                string min = null,
                    max = null;
                char symbol1 = ' ';
                char symbol2 = ' ';
                if (name.Text.Length < surname.Text.Length)
                {
                    minlen = name.Text.Length;
                    maxlen = surname.Text.Length;
                    min = name.Text;
                    max = surname.Text;
                }
                else
                {
                    maxlen = name.Text.Length;
                    minlen = surname.Text.Length;
                    max = name.Text;
                    min = surname.Text;
                }
                tail.Text = "";
                for (int i = minlen; i < maxlen; i++)
                {
                    tail.Text += max[i];
                }
                for (int i = 0; i < minlen; i++)
                {
                    symbol1 = name.Text[i];
                    symbol2 = surname.Text[i];
                    int symbol = symbol1 ^ symbol2;
                    if (symbol > 255)
                        throw new ArgumentOutOfRangeException("name_surname", "End-char size is out of ASCII range.");
                    //if (symbol == 3 || symbol == 0)
                    //    throw new SystemCharException("System char exception!", symbol);
                    else
                    {
                        char csymb = (char)symbol;
                        str += csymb.ToString();
                    }
                }
                if (((int)str[0] == 3 || (int)str[0] == 0) && str.Length == 1)
                    throw new SystemCharException("System char exception!", (int)str[0]);

                for (int i = minlen; i < maxlen; i++)
                {
                    str += max[i];
                }
                result.Text = str;


                result_ascii.Text = "";
                foreach (char symb in result.Text)
                {
                    result_ascii.Text += (int)symb + "; ";
                }


                if (name.Text.Any(char.IsDigit) || name.Text.Any(char.IsWhiteSpace))
                    throw new ArgumentException("Name contains unsupportable characters.");
                else if (surname.Text.Any(char.IsDigit) || surname.Text.Any(char.IsWhiteSpace))
                    throw new ArgumentException("Surname contains unsupportable characters.");
            }
            catch (ArgumentOutOfRangeException ex)
            {
                result.Text = "Out of Range!";
                var logLine = DateTime.Now.ToString() + ": " + ex.Message + "; INFO: name=" + name.Text + ",surname=" + surname.Text;
                var lineNoTime = ex.Message + "; INFO: name=" + name.Text + ",surname=" + surname.Text;
                File.AppendAllText(Directory.GetCurrentDirectory() + "\\global_log.log", DateTime.Now.ToString() + ": LAB1: " + ex.Message + "; INFO: name=" + name.Text + ",surname=" + surname.Text + Environment.NewLine);
                log.Add(logLine);
                try
                {
                    var time_ms = MSSQL_logging.log_error_onTimer(DateTime.Now, "LAB1", "LAB1:" + ex.Message + "; INFO: name=" + name.Text + ",surname=" + surname.Text);
                    toolStripStatusLabel1.Text = "Логирование выполнено за " + time_ms + "мс";
                }
                catch { }
            }
            catch (ArgumentException ex)
            {
                result.Text = "Incorrect inp";
                var logLine = DateTime.Now.ToString() + ": " + ex.Message + "; INFO: name=" + name.Text + ",surname=" + surname.Text;
                var lineNoTime = ex.Message + "; INFO: name=" + name.Text + ",surname=" + surname.Text;
                File.AppendAllText(Directory.GetCurrentDirectory() + "\\global_log.log", DateTime.Now.ToString() + ": LAB1: " + ex.Message + "; INFO: name=" + name.Text + ",surname=" + surname.Text + Environment.NewLine);
                log.Add(logLine);
                try
                {
                    var time_ms = MSSQL_logging.log_error_onTimer(DateTime.Now, "LAB1", "LAB1:" + ex.Message + "; INFO: name=" + name.Text + ",surname=" + surname.Text);
                    toolStripStatusLabel1.Text = "Логирование выполнено за " + time_ms + "мс";
                }
                catch { }
            }
            catch (IndexOutOfRangeException ex)
            {
                result.Text = "Index OOR!";
                var logLine = DateTime.Now.ToString() + ": " + ex.Message + "; INFO: name=" + name.Text + ",surname=" + surname.Text;
                var lineNoTime = ex.Message + "; INFO: name=" + name.Text + ",surname=" + surname.Text;
                File.AppendAllText(Directory.GetCurrentDirectory() + "\\global_log.log", DateTime.Now.ToString() + ": LAB1: " + ex.Message + "; INFO: name=" + name.Text + ",surname=" + surname.Text + Environment.NewLine);
                log.Add(logLine);
                try
                {
                    var time_ms = MSSQL_logging.log_error_onTimer(DateTime.Now, "LAB1", "LAB1:" + ex.Message + "; INFO: name=" + name.Text + ",surname=" + surname.Text);
                    toolStripStatusLabel1.Text = "Логирование выполнено за " + time_ms + "мс";
                }
                catch { }
            }
            catch (SystemCharException ex)
            {
                result.Text = "System char!";
                result_ascii.Text = ex.code.ToString();
                var logLine = DateTime.Now.ToString() + ": " + ex.Message + "; INFO: name=" + name.Text + ",surname=" + surname.Text + ",char=" + ex.code;
                var lineNoTime = ex.Message + "; INFO: name=" + name.Text + ",surname=" + surname.Text + ",char=" + ex.code;
                File.AppendAllText(Directory.GetCurrentDirectory() + "\\global_log.log", DateTime.Now.ToString() + ": LAB1: " + ex.Message + "; INFO: name=" + name.Text + ",surname=" + surname.Text + Environment.NewLine);
                log.Add(logLine);
                try
                {
                    var time_ms = MSSQL_logging.log_error_onTimer(DateTime.Now, "LAB1", "LAB1:" + ex.Message + "; INFO: name=" + name.Text + ",surname=" + surname.Text);
                    toolStripStatusLabel1.Text = "Логирование выполнено за " + time_ms + "мс";
                }
                catch { }
            }
            catch (Exception ex)
            {
                result.Text = ex.Message;
                var logLine = DateTime.Now.ToString() + ": " + ex.Message + "; INFO: name=" + name.Text + ",surname=" + surname.Text;
                var lineNoTime = ex.Message + "; INFO: name=" + name.Text + ",surname=" + surname.Text;
                File.AppendAllText(Directory.GetCurrentDirectory() + "\\global_log.log", DateTime.Now.ToString() + ": LAB1: Unhandled Exception; INFO: name=" + name.Text + ",surname=" + surname.Text + Environment.NewLine);
                log.Add(logLine);
                try
                {
                    var time_ms = MSSQL_logging.log_error_onTimer(DateTime.Now, "LAB1", "LAB1:" + ex.Message + "; INFO: name=" + name.Text + ",surname=" + surname.Text);
                    toolStripStatusLabel1.Text = "Логирование выполнено за " + time_ms + "мс";
                }
                catch { }
            }
            finally
            {
                Invoke(new UpdateLogBoxDelegate(InvokeUpdateLogBox));
                name_ascii.Text = "";
                surname_ascii.Text = "";
                tail_ascii.Text = "";
                foreach (char symb in name.Text)
                {
                    name_ascii.Text += (int)symb + "; ";
                }
                foreach (char symb in surname.Text)
                {
                    surname_ascii.Text += (int)symb + "; ";
                }
                foreach (char symb in tail.Text)
                {
                    tail_ascii.Text += (int)symb + "; ";
                }
            }
            
        }

        private void справкаToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var info = new Form_Info();
            info.Show();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            toolStripStatusLabel1.Text = DateTime.Now.ToString();
        }

        private void лР2ToolStripMenuItem2_Click(object sender, EventArgs e)
        {
            this.Hide();
            var lab2 = new Form2();
            lab2.Closed += (s, args) => this.Close();
            lab2.Show();
        }

        private void лР1ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Hide();
            var lab1 = new Form1_tz();
            lab1.Closed += (s, args) => this.Close();
            lab1.Show();
        }

        private void лР2ToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            this.Hide();
            var lab2 = new Form2_tz();
            lab2.Closed += (s, args) => this.Close();
            lab2.Show();
        }

        private void сохранитьЛогToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SaveFileDialog saveFileDialog1 = new SaveFileDialog();

            saveFileDialog1.FileName = "lab1.log";
            saveFileDialog1.Filter = "Log files (*.log)|*.log";
            saveFileDialog1.FilterIndex = 2;
            saveFileDialog1.RestoreDirectory = true;

            if (saveFileDialog1.ShowDialog() == DialogResult.OK)
            {
                StreamWriter logfile = new StreamWriter(saveFileDialog1.OpenFile());
                if (logfile != null)
                {
                    UnicodeEncoding uniEncoding = new UnicodeEncoding();
                    foreach (string line in log)
                    {
                        logfile.WriteLine(line);
                    }
                    logfile.Dispose();
                    logfile.Close();
                }
            }
        }

        private void загрузитьЛогToolStripMenuItem_Click(object sender, EventArgs e)
        {
            log.Clear();
            OpenFileDialog openFileDialog1 = new OpenFileDialog();
            openFileDialog1.Filter = "Log files (*.log)|*.log";

            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                StreamReader logfile = new StreamReader(openFileDialog1.OpenFile());
                if (logfile != null)
                {
                    string line;
                    while ((line = logfile.ReadLine()) != null)
                    {
                        log.Add(line);
                    }
                    logfile.Dispose();
                    logfile.Close();
                }
            }
            Invoke(new UpdateLogBoxDelegate(InvokeUpdateLogBox));
        }

        private void менюToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Hide();
            var form = new Form_start();
            form.Closed += (s, args) => this.Close();
            form.Show();
        }
    }
}
