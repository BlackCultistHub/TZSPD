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
            this.MinimizeBox = false;
            this.CenterToScreen();

            ReadLogFile();
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
    }
}
