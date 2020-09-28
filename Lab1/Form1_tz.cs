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
    public partial class Form1_tz : Form
    {
        private int spaces = 0;
        private int msg_max_len = 0;
        private int byte_size = 1;

        public delegate void UpdateInfoDelegate();
        public Form1_tz()
        {
            InitializeComponent();
            timer1.Start();
        }

        private int binStringToInt(string binString) //string as array
        {
            int messageInt = 0;
            for (int i = 0; i < binString.Length; i++)
            {
                messageInt <<= 1;
                if (binString[i] == '1')
                {
                    messageInt++;
                }
            }
            return messageInt;
        }
        private bool getBit(int code, int pointer)
        {
            int pointerBit = 0, temp = 0;
            pointerBit = (int)Math.Pow(2, pointer);
            temp = code ^ pointerBit;
            if (temp < code)
                return true;
            else
                return false;
        }
        private void InvokeUpdateInfo()
        {
            //spaces count
            spaces = textBoxContainer.Text.Count(symb => symb == ' ');
            textBox_spaces.Text = spaces.ToString();
            //max length count
            msg_max_len = (int)Math.Floor((double)(spaces / 8));
            textBox_maxMsgLen.Text = msg_max_len.ToString();
        }
        private void drawBinaryMsg(string msg)
        {
            //draw
            for (int i = 0; i < msg.Length; i++)
            {
                if (i == 0)
                {
                    textBox_messageBin.Text += msg[i];
                    continue;
                }
                
                if (i % (16/byte_size) == 0)
                    textBox_messageBin.Text += Environment.NewLine;
                else if (i % (8 / byte_size) == 0)
                    textBox_messageBin.Text += " ";
                else if (i % (4 / byte_size) == 0)
                    textBox_messageBin.Text += "'";
                textBox_messageBin.Text += msg[i];
            }
        }
        private void timer1_Tick(object sender, EventArgs e)
        {
            Invoke(new UpdateInfoDelegate(InvokeUpdateInfo));
        }
        private void button1_Click(object sender, EventArgs e)
        {
            if (textBox_message.TextLength > msg_max_len)
            {
                MessageBox.Show("Message is too long for this container!", "Error.", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            if (textBoxContainer.Text.Contains("  "))
            {
                MessageBox.Show("You have double or more spaces in the container!", "Error.", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            string msgBits = "";
            textBox_messageBin.Text = "";
            textBox_result.Text = "";
            //message in binary
            for (int i = 0; i < textBox_message.TextLength; i++)
            {
                string tempBin = "";
                for (int j = 0; j < sizeof(char) * 8 / byte_size; j++)
                {
                    if (getBit(textBox_message.Text[i], j))
                        tempBin += "1";
                    else
                        tempBin += "0";
                }
                for (int j = tempBin.Length-1; j != -1; j--)
                {
                    msgBits += tempBin[j];
                }
            }
            drawBinaryMsg(msgBits); // draw
            //steganography
            int containerCounter = 0;
            int msgBitCounter = 0;
            bool begin = true;
            for (int i = 0; i < textBoxContainer.TextLength; i++)
            {
                if ((textBoxContainer.Text[i] == ' ') && (msgBitCounter != msgBits.Length))
                {
                    if (begin)
                    {
                        for (int j = 0; j <= i; j++)
                            textBox_result.Text += textBoxContainer.Text[j];
                        begin = false;
                    }
                    else
                    {
                        for (int j = containerCounter; j <= i; j++)
                            textBox_result.Text += textBoxContainer.Text[j];
                    }
                    containerCounter = i+1;
                    if (msgBits[msgBitCounter] == '0')
                        textBox_result.Text += ' ';
                    msgBitCounter++;
                }
                else if (msgBitCounter == msgBits.Length)
                {
                    textBox_result.Text += textBoxContainer.Text[i];
                }
            }
        }
        private void button2_Click(object sender, EventArgs e)
        {
            textBox_messageBin.Text = "";
            textBox_message.Text = "";
            //read msg to binary
            string msgBits = "";
            for (int i = 0; i < textBox_result.TextLength; i++)
            {
                if (textBox_result.Text[i] == ' ' && i+1 != textBox_result.TextLength)
                {
                    if (textBox_result.Text[i + 1] == ' ')
                    {
                        msgBits += "0";
                        i++;
                    }
                    else
                        msgBits += "1";
                }
            }
            string msgDraw = "";
            string tempChar = "";
            for (int i = 0; i < msgBits.Length; i++)
            {
                tempChar += msgBits[i];
                if ((i+1)%(sizeof(char)*8/byte_size) == 0 && (i != 0))
                {
                    if (tempChar == "1111111111111111" || tempChar == "11111111")
                        break;
                    msgDraw += tempChar;
                    char Symb = (char)binStringToInt(tempChar);
                    textBox_message.Text += Symb;
                    tempChar = "";
                }
            }
            drawBinaryMsg(msgDraw); // draw
        }

        private void textBox_message_Validating(object sender, CancelEventArgs e)
        {
            if (textBox_message.TextLength > msg_max_len)
                errorProvider1.SetError(textBox_message, "Слишком длинное сообщение");
        }

        private void comboBox1_SelectedValueChanged(object sender, EventArgs e)
        {
            if (comboBox1.SelectedItem == "1 байт")
                byte_size = 2;
            else if (comboBox1.SelectedItem == "2 байта")
                byte_size = 1;
        }

        private void справкаToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var info = new Form_Info();
            info.Show();
        }



        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            textBoxContainer.Text = "";
            OpenFileDialog openFileDialog1 = new OpenFileDialog();
            openFileDialog1.Filter = "Text file (*.txt)|*.txt";

            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                StreamReader file = new StreamReader(openFileDialog1.OpenFile());
                if (file != null)
                {
                    string line;
                    while ((line = file.ReadLine()) != null)
                    {
                        textBoxContainer.Text += line + Environment.NewLine;
                    }
                    file.Dispose();
                    file.Close();
                }
            }
        }

        private void toolStripButton2_Click(object sender, EventArgs e)
        {
            textBox_message.Text = "";
            OpenFileDialog openFileDialog1 = new OpenFileDialog();
            openFileDialog1.Filter = "Text file (*.txt)|*.txt";

            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                StreamReader file = new StreamReader(openFileDialog1.OpenFile());
                if (file != null)
                {
                    string line;
                    while ((line = file.ReadLine()) != null)
                    {
                        textBox_message.Text += line + Environment.NewLine;
                    }
                    file.Dispose();
                    file.Close();
                }
            }
        }

        private void toolStripButton4_Click(object sender, EventArgs e)
        {
            textBox_result.Text = "";
            OpenFileDialog openFileDialog1 = new OpenFileDialog();
            openFileDialog1.Filter = "Text file (*.txt)|*.txt";

            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                StreamReader file = new StreamReader(openFileDialog1.OpenFile());
                if (file != null)
                {
                    string line;
                    while ((line = file.ReadLine()) != null)
                    {
                        textBox_result.Text += line + Environment.NewLine;
                    }
                    file.Dispose();
                    file.Close();
                }
            }
        }

        private void toolStripButton3_Click(object sender, EventArgs e)
        {
            SaveFileDialog saveFileDialog1 = new SaveFileDialog();

            saveFileDialog1.FileName = "Message.txt";
            saveFileDialog1.Filter = "Text file (*.txt)|*.txt";
            saveFileDialog1.FilterIndex = 2;
            saveFileDialog1.RestoreDirectory = true;

            if (saveFileDialog1.ShowDialog() == DialogResult.OK)
            {
                StreamWriter file = new StreamWriter(saveFileDialog1.OpenFile());
                if (file != null)
                {
                    UnicodeEncoding uniEncoding = new UnicodeEncoding();
                    foreach (string line in textBox_message.Lines)
                    {
                        file.WriteLine(line);
                    }
                    file.Dispose();
                    file.Close();
                }
            }
        }

        private void toolStripButton6_Click(object sender, EventArgs e)
        {
            SaveFileDialog saveFileDialog1 = new SaveFileDialog();

            saveFileDialog1.FileName = "Contained_message.txt";
            saveFileDialog1.Filter = "Text file (*.txt)|*.txt";
            saveFileDialog1.FilterIndex = 2;
            saveFileDialog1.RestoreDirectory = true;

            if (saveFileDialog1.ShowDialog() == DialogResult.OK)
            {
                StreamWriter file = new StreamWriter(saveFileDialog1.OpenFile());
                if (file != null)
                {
                    UnicodeEncoding uniEncoding = new UnicodeEncoding();
                    foreach (string line in textBox_result.Lines)
                    {
                        file.WriteLine(line);
                    }
                    file.Dispose();
                    file.Close();
                }
            }
        }

        private void лР1ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Hide();
            var lab1 = new Form1();
            lab1.Closed += (s, args) => this.Close();
            lab1.Show();
        }

        private void лР2ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Hide();
            var lab2 = new Form2();
            lab2.Closed += (s, args) => this.Close();
            lab2.Show();
        }

        private void лР2ToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            this.Hide();
            var lab2 = new Form2_tz();
            lab2.Closed += (s, args) => this.Close();
            lab2.Show();
        }
    }
}
