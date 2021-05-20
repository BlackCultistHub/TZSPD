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
using System.Security.Cryptography;

namespace Lab1
{
    public partial class Form1_tz : Form
    {
        private int spaces = 0;
        private int msg_max_len = 0;
        private int byte_size = 1;

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

        public delegate void UpdateInfoDelegate();
        public Form1_tz()
        {
            InitializeComponent();
            this.FormBorderStyle = FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.CenterToScreen();

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
            if (byte_size == 1)
                msg_max_len = (int)Math.Floor((double)(spaces / 16));
            if (byte_size == 2)
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
                var logLine = DateTime.Now.ToString() + ": Сообщение слишком длинное для данного контейнера!";
                var lineNoTime = "LAB1_TZSPD: Сообщение слишком длинное для данного контейнера!";
                log.Add(logLine);
                File.AppendAllText(Directory.GetCurrentDirectory() + "\\global_log.log", DateTime.Now.ToString() + ": LAB1_TZSPD: Сообщение слишком длинное для данного контейнера!" + Environment.NewLine);
                MessageBox.Show("Сообщение слишком длинное для данного контейнера!", "Error.", MessageBoxButtons.OK, MessageBoxIcon.Error);
                try
                {
                    var time_ms = MSSQL_logging.log_error_onTimer(DateTime.Now, "LAB1_TZSPD", lineNoTime);
                    toolStripStatusLabel1.Text = "Логирование выполнено за " + time_ms + "мс";
                }
                catch { }
                Invoke(new UpdateLogBoxDelegate(InvokeUpdateLogBox));
                return;
            }
            if (textBox_message.TextLength + 256 > msg_max_len && checkBox_sha256.Checked)
            {
                var logLine = DateTime.Now.ToString() + ": Сообщение С контрольной суммой SHA-256 слишком длинное для данного контейнера!";
                var lineNoTime = "LAB1_TZSPD: Сообщение С контрольной суммой SHA-256 слишком длинное для данного контейнера!";
                log.Add(logLine);
                File.AppendAllText(Directory.GetCurrentDirectory() + "\\global_log.log", DateTime.Now.ToString() + ": LAB1_TZSPD: Сообщение С контрольной суммой SHA-256 слишком длинное для данного контейнера!" + Environment.NewLine);
                MessageBox.Show("Сообщение С контрольной суммой SHA-256 слишком длинное для данного контейнера!", "Error.", MessageBoxButtons.OK, MessageBoxIcon.Error);
                try
                {
                    var time_ms = MSSQL_logging.log_error_onTimer(DateTime.Now, "LAB1_TZSPD", lineNoTime);
                    toolStripStatusLabel1.Text = "Логирование выполнено за " + time_ms + "мс";
                }
                catch { }
                Invoke(new UpdateLogBoxDelegate(InvokeUpdateLogBox));
                return;
            }
            if (textBoxContainer.Text.Contains("  "))
            {
                var logLine = DateTime.Now.ToString() + ": В контейнере двойные или более пробелы! Пожалуйста, нормализуйте контейнер сначала!";
                var lineNoTime = "LAB1_TZSPD: В контейнере двойные или более пробелы! Пожалуйста, нормализуйте контейнер сначала!";
                log.Add(logLine);
                File.AppendAllText(Directory.GetCurrentDirectory() + "\\global_log.log", DateTime.Now.ToString() + ": LAB1_TZSPD: В контейнере двойные или более пробелы! Пожалуйста, нормализуйте контейнер сначала!" + Environment.NewLine);
                MessageBox.Show("В контейнере двойные или более пробелы! Пожалуйста, нормализуйте контейнер сначала!", "Error.", MessageBoxButtons.OK, MessageBoxIcon.Error);
                try
                {
                    var time_ms = MSSQL_logging.log_error_onTimer(DateTime.Now, "LAB1_TZSPD", lineNoTime);
                    toolStripStatusLabel1.Text = "Логирование выполнено за " + time_ms + "мс";
                }
                catch { }
                Invoke(new UpdateLogBoxDelegate(InvokeUpdateLogBox));
                return;
            }
            errorProvider1.Clear();
            string hashBits = "";
            if (checkBox_sha256.Checked)
            {
                SHA256 checksumm = SHA256.Create();
                byte[] bytes = checksumm.ComputeHash(Encoding.UTF8.GetBytes(textBoxContainer.Text));
                StringBuilder builder = new StringBuilder();
                for (int i = 0; i < bytes.Length; i++)
                {
                    builder.Append(bytes[i].ToString("x2"));
                }
                textBox_checkSumm.Text = builder.ToString();
                //checksumm in binary
                for (int i = 0; i < textBox_checkSumm.TextLength; i++)
                {
                    string tempBin = "";
                    for (int j = 0; j < sizeof(byte) * 8; j++)
                    {
                        if (getBit(textBox_checkSumm.Text[i], j))
                            tempBin += "1";
                        else
                            tempBin += "0";
                    }
                    for (int j = tempBin.Length - 1; j != -1; j--)
                    {
                        hashBits += tempBin[j];
                    }
                }
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
            if (checkBox_sha256.Checked)
            {
                string temp = msgBits;
                msgBits = hashBits + temp;
            }
            ArrayList spacesListTemp = new ArrayList();
            for (int i = 0; i < textBoxContainer.TextLength; i++)
            {
                if (textBoxContainer.Text[i] == ' ')
                    spacesListTemp.Add(i);
            }
            int msgPointer = 0;
            ArrayList spacesList = new ArrayList();
            foreach (int spacePos in spacesListTemp)
            {
                if (msgPointer == msgBits.Length)
                    break;
                if (msgBits[msgPointer] == '0')
                    spacesList.Add(spacePos);
                msgPointer++;
            }
            spacesListTemp.Clear();
            int startPos = 0;
            foreach (int space in spacesList)
            {
                textBox_result.Text += textBoxContainer.Text.Substring(startPos, space+1- startPos) + " ";
                startPos = space + 1;
            }
            textBox_result.Text += textBoxContainer.Text.Substring(startPos);
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
            //sha-256
            if (checkBox_sha256.Checked)
            {
                for (int i = 0; i < 64 * 8; i++)
                {
                    tempChar += msgBits[i];
                    if ((i + 1) % (sizeof(char) * 8/2) == 0 && (i != 0))
                    {
                        char Symb = (char)binStringToInt(tempChar);
                        textBox_checkSumm.Text += Symb;
                        tempChar = "";
                    }
                }
            }
            //message
            for (int i = checkBox_sha256.Checked?512:0; i < msgBits.Length; i++)
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

        private void button3_Click(object sender, EventArgs e)
        {
            string[] words = textBoxContainer.Text.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
            textBoxContainer.Text = String.Join(" ", words);
        }

        private void сохранитьЛогToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SaveFileDialog saveFileDialog1 = new SaveFileDialog();

            saveFileDialog1.FileName = "lab1_tzspd.log";
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

        private void button4_Click(object sender, EventArgs e)
        {
            richTextBox1.Clear();
            //read msg in binary
            string msgBits = "";
            for (int i = 0; i < textBox_result.TextLength; i++)
            {
                if (textBox_result.Text[i] == ' ' && i + 1 != textBox_result.TextLength)
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
            //cut-off message
            string tempChar = "";
            string cutString = "";
            for (int i = 0; i < msgBits.Length; i++)
            {
                tempChar += msgBits[i];
                if ((i + 1) % (sizeof(char) * 8 / byte_size) == 0 && (i != 0))
                {
                    if (tempChar == "1111111111111111" || tempChar == "11111111")
                        break;
                    cutString += tempChar;
                    tempChar = "";
                }
            }
            msgBits = cutString;
            //normalize
            string[] words = textBox_result.Text.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
            string normalized = String.Join(" ", words);
            //insert symbols
            int spacecounter = 0;
            for (int i = 0; i < normalized.Length; i++)
            {
                if (normalized[i] == ' ' && spacecounter != msgBits.Length)
                {
                    if (msgBits[spacecounter] == '0')
                        richTextBox1.AppendText("0", Color.Red);
                    else
                        richTextBox1.AppendText("1", Color.Red);
                    spacecounter++;
                }
                else
                    richTextBox1.AppendText(normalized[i].ToString(), Color.Green);
            }
        }
    }

    public static class RichTextBoxExtensions
    {
        public static void AppendText(this RichTextBox box, string text, Color color, bool bg = false)
        {
            if (bg)
            {
                box.SelectionStart = box.TextLength;
                box.SelectionLength = text.Length;

                box.SelectionBackColor = color;
                box.AppendText(text);
                box.SelectionBackColor = box.BackColor;
            }
            else
            {
                box.SelectionStart = box.TextLength;
                box.SelectionLength = text.Length;

                box.SelectionColor = color;
                box.AppendText(text);
                box.SelectionColor = box.ForeColor;
            }
        }
    }
}
