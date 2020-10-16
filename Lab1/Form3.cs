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
    public partial class Form3 : Form
    {
        public Form3()
        {
            InitializeComponent();
        }

        private void HelpSelect_Click(object sender, EventArgs e)
        {
            var info = new Form_Info();
            info.Show();
        }

        private void menuToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Hide();
            var menu = new Form_start();
            menu.Closed += (s, args) => this.Close();
            menu.Show();
        }
    }
}
