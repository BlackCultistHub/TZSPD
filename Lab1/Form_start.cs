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
        public Form_start()
        {
            InitializeComponent();
            this.FormBorderStyle = FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.CenterToScreen();

            ReadLogFile();
            if (!File.Exists("settings.ini"))
                File.WriteAllText("settings.ini", "");
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
    }
}
