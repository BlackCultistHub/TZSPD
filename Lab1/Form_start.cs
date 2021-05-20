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
    public partial class Form_start : Form
    {
        public void ReadLogFile()
        {
            ArrayList log = new ArrayList();
            if (!File.Exists("global_log.log"))
                using (File.Create("global_log.log")) { }
            StreamReader logfile = new StreamReader(Directory.GetCurrentDirectory() + "\\global_log.log");
            textBox1.Clear();
            if (logfile != null)
            {
                string line = "";
                while ((line = logfile.ReadLine()) != null)
                    log.Add(line);
                logfile.Dispose();
                logfile.Close();
            }
            foreach (string line in log)
                textBox1.Text += line + Environment.NewLine;
        }

        public void Dostats()
        {
            string text = textBox1.Text;
            string reg1 = @".*LAB1.*";
            string reg2 = @".*LAB2.*";
            string reg3 = @".*LAB3.*";
            string reg1tz = @".*LAB1_TZSPD.*";
            string reg2tz = @".*LAB2_TZSPD.*";
            string reg3tz = @".*LAB3_TZSPD.*";
            string reg4tz = @".*LAB4_TZSPD.*";

            int count1 = 0;
            int count2 = 0;
            int count3 = 0;
            int count1tz = 0;
            int count2tz = 0;
            int count3tz = 0;
            int count4tz = 0;

            foreach (System.Text.RegularExpressions.Match match in System.Text.RegularExpressions.Regex.Matches(text, reg1, System.Text.RegularExpressions.RegexOptions.IgnoreCase))
                count1++;
            foreach (System.Text.RegularExpressions.Match match in System.Text.RegularExpressions.Regex.Matches(text, reg2, System.Text.RegularExpressions.RegexOptions.IgnoreCase))
                count2++;
            foreach (System.Text.RegularExpressions.Match match in System.Text.RegularExpressions.Regex.Matches(text, reg3, System.Text.RegularExpressions.RegexOptions.IgnoreCase))
                count3++;
            foreach (System.Text.RegularExpressions.Match match in System.Text.RegularExpressions.Regex.Matches(text, reg1tz, System.Text.RegularExpressions.RegexOptions.IgnoreCase))
                count1tz++;
            foreach (System.Text.RegularExpressions.Match match in System.Text.RegularExpressions.Regex.Matches(text, reg2tz, System.Text.RegularExpressions.RegexOptions.IgnoreCase))
                count2tz++;
            foreach (System.Text.RegularExpressions.Match match in System.Text.RegularExpressions.Regex.Matches(text, reg3tz, System.Text.RegularExpressions.RegexOptions.IgnoreCase))
                count3tz++;
            foreach (System.Text.RegularExpressions.Match match in System.Text.RegularExpressions.Regex.Matches(text, reg4tz, System.Text.RegularExpressions.RegexOptions.IgnoreCase))
                count4tz++;

            lab1_logs.Text = count1.ToString();
            lab2_logs.Text = count2.ToString();
            lab3_logs.Text = count3.ToString();
            lab1tz_logs.Text = count1tz.ToString();
            lab2tz_logs.Text = count2tz.ToString();
            lab3tz_logs.Text = count3tz.ToString();
            lab4tz_logs.Text = count4tz.ToString();
        }
        public Form_start()
        {
            InitializeComponent();
            this.FormBorderStyle = FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.CenterToScreen();

            ReadLogFile();
            Dostats();
            //if (!File.Exists("settings.ini"))
            //    File.WriteAllText("settings.ini", "");
        }
        private void Form_start_Shown(object sender, EventArgs e)
        {
            tryInitDB();
        }

        private void tryInitDB()
        {
            if ((new Form_Params()).dataBaseEndabled())
            {
                if (!DatabaseOperations.initDB())
                {
                    Form shadow = new Form();
                    shadow.MinimizeBox = false;
                    shadow.MaximizeBox = false;
                    shadow.ControlBox = false;

                    shadow.Text = "";
                    shadow.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
                    shadow.Size = this.Size;
                    shadow.BackColor = Color.Black;
                    shadow.Opacity = 0.3;
                    shadow.Show();
                    shadow.Location = this.Location;
                    shadow.Enabled = false;

                    var msgBox = new Form_DB_init_error();
                    msgBox.ShowDialog();
                    shadow.Dispose();
                    shadow.Close();
                }
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Hide();
            var lab1 = new Form1();
            lab1.Closed += (s, args) => this.Close();
            lab1.Show();
        }

        private void лР1ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Hide();
            var lab1 = new Form1_tz();
            lab1.Closed += (s, args) => this.Close();
            lab1.Show();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Hide();
            var lab2 = new Form2();
            lab2.Closed += (s, args) => this.Close();
            lab2.Show();
        }
        private void button4_Click(object sender, EventArgs e)
        {
            this.Hide();
            var lab2 = new Form2_tz();
            lab2.Closed += (s, args) => this.Close();
            lab2.Show();
        }

        private void лР2ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Hide();
            var lab1 = new Form1();
            lab1.Closed += (s, args) => this.Close();
            lab1.Show();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            this.Hide();
            var lab1 = new Form1_tz();
            lab1.Closed += (s, args) => this.Close();
            lab1.Show();
        }

        private void лР2ToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            this.Hide();
            var lab2 = new Form2();
            lab2.Closed += (s, args) => this.Close();
            lab2.Show();
        }

        private void лР2ToolStripMenuItem2_Click(object sender, EventArgs e)
        {
            this.Hide();
            var lab2 = new Form2_tz();
            lab2.Closed += (s, args) => this.Close();
            lab2.Show();
        }

        private void справкаToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var info = new Form_Info();
            info.Show();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            this.Hide();
            var lab2 = new Form3_tz();
            lab2.Closed += (s, args) => this.Close();
            lab2.Show();
        }

        private void button6_Click(object sender, EventArgs e)
        {
            this.Hide();
            var lab2 = new Form4_tz();
            lab2.Closed += (s, args) => this.Close();
            lab2.Show();
        }

        private void button7_Click(object sender, EventArgs e)
        {
            this.Hide();
            var lab3 = new Form3();
            lab3.Closed += (s, args) => this.Close();
            lab3.Show();
        }

        private void button8_Click(object sender, EventArgs e)
        {
            var db = new Form_showDB();
            db.ShowDialog();
        }

        private void параметрыToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            var paramms = new Form_Params();
            paramms.ShowDialog();
            tryInitDB();
        }

        private void логБазыДанныхToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var db = new Form_showDB();
            db.ShowDialog();
        }

        private void парсерToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var parser = new Form_parse();
            parser.Show();
        }

        private void сменитьПарольToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var newpwd = new pwd_change();
            newpwd.ShowDialog();
        }
    }
}
