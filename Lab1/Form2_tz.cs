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
using System.Windows.Forms.VisualStyles;
using System.Drawing.Imaging;

namespace Lab1
{
    public partial class Form2_tz : Form
    {
        public ArrayList container_bytes = new ArrayList();
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
        public Form2_tz()
        {
            InitializeComponent();
            this.FormBorderStyle = FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.CenterToScreen();
        }
        public static string Reverse(string s)
        {
            char[] charArray = s.ToCharArray();
            Array.Reverse(charArray);
            return new string(charArray);
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

        private long readHex(ArrayList hexStream, int position, int bytesToRead)
        {
            long numb = 0;
            for (int i = position; i < position + bytesToRead; i++)
            {
                numb <<= 8;
                numb += (byte)hexStream[i];
            }
            return numb;
        }

        private long readHexReverse(ArrayList hexStream, int position, int bytesToRead)
        {
            long numb = 0;
            for (int i = position; i >= position - bytesToRead + 1; i--)
            {
                numb <<= 8;
                numb += (byte)hexStream[i];
            }
            return numb;
        }

        private void справкаToolStripMenuItem_Click_1(object sender, EventArgs e)
        {
            var info = new Form_Info();
            info.Show();
        }

        private void менюToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Hide();
            var form = new Form_start();
            form.Closed += (s, args) => this.Close();
            form.Show();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Container.Image = null;
            container_bytes.Clear();
            OpenFileDialog openFileDialog1 = new OpenFileDialog();
            openFileDialog1.Filter = "BMP image file (*.bmp)|*.bmp";

            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                using (var file = openFileDialog1.OpenFile())
                {
                    if (file != null)
                    {
                        label_loading_container.Visible = true;
                        label_loading_container.Refresh();
                        Container.Image = Image.FromStream(file);
                        BinaryReader binaryReader = new BinaryReader(file);
                        binaryReader.BaseStream.Position = 0;
                        /*while (binaryReader.BaseStream.Position != binaryReader.BaseStream.Length)
                        {
                            container_bytes.Add(binaryReader.ReadByte());
                        }*/
                        byte[] tempBytes = binaryReader.ReadBytes((int)binaryReader.BaseStream.Length);
                        container_bytes.AddRange(tempBytes);
                        label_loading_container.Visible = false;
                        label_loading_container.Refresh();
                    }
                }
            }
            //ImageSize (pixels)
            int maxLen = (int)readHexReverse(container_bytes, 0x25, 4); //bytes
            maxLen /= 3; //pixels
            maxLen -= 2; //header 2 chars
            maxLen = (int)Math.Floor((double)(maxLen/16)); //chars in 2b
            textBox_maxMsgLen.Text = maxLen.ToString();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            try
            {
                /*container_bytes.Clear();
                if (Container.Image != null)
                {
                    using (MemoryStream bits = new MemoryStream())
                    {
                        using (Bitmap bmp = new Bitmap(Container.Image.Width, Container.Image.Height, System.Drawing.Imaging.PixelFormat.Format24bppRgb))
                        {
                            using (Graphics g = Graphics.FromImage(bmp))
                            {
                                g.DrawImage(Container.Image, new Point(0, 0));
                            }
                            bmp.Save(bits, System.Drawing.Imaging.ImageFormat.Bmp);
                        }
                        //Container.Image.Save(bits, ImageFormat.Bmp);
                        using (BinaryReader binaryReader = new BinaryReader(bits))
                        {
                            byte[] tempBytes = binaryReader.ReadBytes((int)binaryReader.BaseStream.Length);
                            container_bytes.AddRange(tempBytes);
                        }
                    }
                }*/
                
                if (int.Parse(textBox_maxMsgLen.Text) < textBox_message.TextLength)
                {
                    MessageBox.Show("Размер сообщения слишком велик, будут задействованы более старшие биты.", "Info.", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                progress_steg.Visible = true;
                progress_steg.Refresh();
                label_progress.Visible = true;
                label_progress.Refresh();
                label_task.Visible = true;
                label_task.Refresh();
                if (SteganoContainer.Image != null)
                    SteganoContainer.Image.Dispose();

                string msgBits = "";
                int msgLenBytes = 0;
                //message in binary
                label_task.Text = "Подготовка";
                label_task.Refresh();
                for (int i = 0; i < textBox_message.TextLength; i++)
                {
                    int progress = 100 * i / textBox_message.TextLength;
                    progress_steg.Value = progress;
                    label_progress.Text = progress.ToString() + " %";
                    string tempBin = "";
                    for (int j = 0; j < sizeof(char) * 8; j++)
                    {
                        if (getBit(textBox_message.Text[i], j))
                            tempBin += "1";
                        else
                            tempBin += "0";
                    }
                    for (int j = tempBin.Length - 1; j != -1; j--)
                    {
                        msgBits += tempBin[j];
                    }
                }
                msgLenBytes = msgBits.Length / 8;
                string msgLenBytesBits = "";
                //msgLen in binary
                for (int i = 0; i < 16; i++)
                {
                    if (getBit(msgLenBytes, i))
                        msgLenBytesBits += "1";
                    else
                        msgLenBytesBits += "0";
                }
                //reading headers
                //Header-offset
                int header_offset = (int)readHexReverse(container_bytes, 0x0D, 4);
                //ImageSize (pixels)
                int img_pixels = (int)readHexReverse(container_bytes, 0x25, 4);
                img_pixels /= 3;
                //Image width
                int img_width = (int)readHexReverse(container_bytes, 0x15, 4);
                //Image height
                int img_height = (int)readHexReverse(container_bytes, 0x19, 4);
                //Bit per pixel
                int bit_per_pixel = (int)readHexReverse(container_bytes, 0x1D, 2);
                int bytes_per_pixel = bit_per_pixel / 8;
                //Compression
                int img_compression = (int)readHexReverse(container_bytes, 0x21, 4);
                if (img_compression != 0)
                    throw new Exception("Метод компрессии BMP не поддерживается!");
                //padding calc
                int delta = img_width % 4;
                int padding_per_line = Convert.ToBoolean(delta) ? 4 - delta : 0;
                //hiding
                label_task.Text = "В процессе";
                label_task.Refresh();
                string combinedMsg = msgLenBytesBits + msgBits;
                int messagePointer = 0;
                int rank = -1;
                bool doneMsg = false;
                while (messagePointer < combinedMsg.Length)
                {
                    rank++;
                    int pixelMemoryPointer = header_offset + 2; //To set to LSB
                    for (int i = 0; (i < img_height) && !doneMsg; i++) //height
                    {
                        for (int j = 0; (j < img_width) && !doneMsg; j++) //scans
                        {
                            int progress = (100 * messagePointer / msgBits.Length);
                            if (progress <= 100)
                                progress_steg.Value = progress;
                            progress_steg.Refresh();
                            label_progress.Text = (progress>100?100:progress).ToString() + " %";
                            label_progress.Refresh();
                            //3 bytes per pixel
                            if (combinedMsg[messagePointer] == '1' && !getBit((byte)container_bytes[pixelMemoryPointer], rank))
                            {
                                //increment
                                byte temp = (byte)container_bytes[pixelMemoryPointer];
                                temp++;
                                container_bytes.RemoveAt(pixelMemoryPointer);
                                container_bytes.Insert(pixelMemoryPointer, temp);
                            }
                            else if (combinedMsg[messagePointer] == '0' && getBit((byte)container_bytes[pixelMemoryPointer], rank))
                            {
                                //decrement
                                byte temp = (byte)container_bytes[pixelMemoryPointer];
                                temp--;
                                container_bytes.RemoveAt(pixelMemoryPointer);
                                container_bytes.Insert(pixelMemoryPointer, temp);
                            }
                            messagePointer++;
                            if (messagePointer >= combinedMsg.Length)
                                doneMsg = true;
                            pixelMemoryPointer += bytes_per_pixel;
                        }
                        pixelMemoryPointer += padding_per_line;
                    }

                }
                using (FileStream tempImg = File.Create("tempImg.bmp"))
                {
                    foreach (byte b in container_bytes)
                        tempImg.WriteByte(b);
                }
                Image img;
                using (var bmpTemp = new Bitmap("tempImg.bmp"))
                {
                    img = new Bitmap(bmpTemp);
                }
                SteganoContainer.Image = img;
                progress_steg.Visible = true;
                progress_steg.Refresh();
                label_progress.Visible = true;
                label_progress.Refresh();
                label_task.Visible = false;
                label_task.Refresh();
            }
            catch (Exception ex)
            {
                var logLine = DateTime.Now.ToString() + ": " + ex.Message;
                log.Add(logLine);
                File.AppendAllText(Directory.GetCurrentDirectory() + "\\global_log.log", DateTime.Now.ToString() + ": LAB2_TZSPD: " + ex.Message + Environment.NewLine);
                try
                {
                    var time_ms = MSSQL_logging.log_error_onTimer(DateTime.Now, "LAB2_TZSPD", "LAB2_TZSPD: " + ex.Message);
                    toolStripStatusLabel1.Text = "Логирование выполнено за " + time_ms + "мс";
                }
                catch { }
                Invoke(new UpdateLogBoxDelegate(InvokeUpdateLogBox));
                MessageBox.Show(ex.Message, "Error.", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            try
            {
                SaveFileDialog saveFileDialog1 = new SaveFileDialog();

                saveFileDialog1.FileName = "SteganoContainer.bmp";
                saveFileDialog1.Filter = "BMP image file (*.bmp)|*.bmp";
                saveFileDialog1.FilterIndex = 2;
                saveFileDialog1.RestoreDirectory = true;

                if (saveFileDialog1.ShowDialog() == DialogResult.OK)
                {
                    using (FileStream file = new FileStream(saveFileDialog1.FileName, FileMode.Create))
                    {
                        if (File.Exists("tempImg.bmp"))
                        {
                            byte[] imgContent = File.ReadAllBytes("tempImg.bmp");
                            file.Write(imgContent, 0, imgContent.Length);
                        }
                        else
                            throw new Exception("Попытка расшифровки без вспомогательного файла стеганоконтейнера!");
                    }
                }
            }
            catch (Exception ex)
            {
                var logLine = DateTime.Now.ToString() + ": " + ex.Message;
                log.Add(logLine);
                File.AppendAllText(Directory.GetCurrentDirectory() + "\\global_log.log", DateTime.Now.ToString() + ": LAB2_TZSPD: " + ex.Message + Environment.NewLine);
                try
                {
                    var time_ms = MSSQL_logging.log_error_onTimer(DateTime.Now, "LAB2_TZSPD", "LAB2_TZSPD: " + ex.Message);
                    toolStripStatusLabel1.Text = "Логирование выполнено за " + time_ms + "мс";
                }
                catch { }
                Invoke(new UpdateLogBoxDelegate(InvokeUpdateLogBox));
                MessageBox.Show(ex.Message, "Error.", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void button3_Click(object sender, EventArgs e)
        {
            //ArrayList tempBytes = new ArrayList();
            SteganoContainer.Image = null;
            OpenFileDialog openFileDialog1 = new OpenFileDialog();
            openFileDialog1.Filter = "BMP image file (*.bmp)|*.bmp";

            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                using (var file = openFileDialog1.OpenFile())
                {
                    if (file != null)
                    {
                        label_loading_stegano.Visible = true;
                        label_loading_stegano.Refresh();
                        SteganoContainer.Image = Image.FromStream(file);
                        BinaryReader binaryReader = new BinaryReader(file);
                        binaryReader.BaseStream.Position = 0;
                        
                        byte[] tempBytes = binaryReader.ReadBytes((int)binaryReader.BaseStream.Length);

                        label_loading_stegano.Visible = false;
                        label_loading_stegano.Refresh();

                        if (!File.Exists("tempImg.bmp"))
                        {
                            using (var tempImg = File.Create("tempImg.bmp"))
                            {
                                tempImg.Write(tempBytes, 0, tempBytes.Length);
                            }
                        }
                        else
                        {
                            using (var tempImg = File.Open("tempImg.bmp", FileMode.Open))
                            {
                                tempImg.Write(tempBytes, 0, tempBytes.Length);
                            }
                        }
                    }
                }
            }
            
        }
        private void button4_Click(object sender, EventArgs e)
        {
            try
            {
                progress_steg.Visible = true;
                progress_steg.Refresh();
                label_progress.Visible = true;
                label_progress.Refresh();
                textBox_message.Text = "";
                if (!File.Exists("tempImg.bmp"))
                    throw new Exception("Попытка расшифровки без загрузки стеганоконтейнера!");
                container_bytes.Clear();
                using (FileStream file = File.OpenRead("tempImg.bmp"))
                {
                    BinaryReader binaryReader = new BinaryReader(file);
                    binaryReader.BaseStream.Position = 0;
                    byte[] tempBytes = binaryReader.ReadBytes((int)binaryReader.BaseStream.Length);
                    container_bytes.AddRange(tempBytes);
                }
                //reading headers
                //Header-offset
                int header_offset = (int)readHexReverse(container_bytes, 0x0D, 4);
                //ImageSize (pixels)
                int img_pixels = (int)readHexReverse(container_bytes, 0x25, 4);
                img_pixels /= 3;
                //ImageSize (pixels - msgLen)
                int maxLen = (int)readHexReverse(container_bytes, 0x25, 4); //bytes
                maxLen /= 3; //pixels
                maxLen -= 2; //header 2 chars
                maxLen = (int)Math.Floor((double)(maxLen / 16)); //chars in 2b
                //Image width
                int img_width = (int)readHexReverse(container_bytes, 0x15, 4);
                //Image height
                int img_height = (int)readHexReverse(container_bytes, 0x19, 4);
                //Bit per pixel
                int bit_per_pixel = (int)readHexReverse(container_bytes, 0x1D, 2);
                int bytes_per_pixel = bit_per_pixel / 8;
                //Compression
                int img_compression = (int)readHexReverse(container_bytes, 0x21, 4);
                if (img_compression != 0)
                    throw new Exception("Метод компрессии BMP не поддерживается!");
                //padding calc
                int delta = img_width % 4;
                int padding_per_line = Convert.ToBoolean(delta) ? 4 - delta : 0;
                //decoding
                //read msglen
                string msgBits = "";
                for (int i = header_offset + 2; i < header_offset + 2 + (bytes_per_pixel * 16); i+= bytes_per_pixel)
                {
                    if (getBit((byte)container_bytes[i], 0))
                        msgBits += "1";
                    else
                        msgBits += "0";
                }
                msgBits = Reverse(msgBits);
                int msgLen = binStringToInt(msgBits);

                int msgBitsRead = 0;
                int rank = -1;
                bool doneMsg = false;
                msgBits = "";

                while (msgBitsRead < (msgLen * 8) + 16)
                {
                    rank++;
                    int memoryPointer = header_offset + 2;
                    for (int i = 0; (i < img_height) && !doneMsg; i++) //height
                    {
                        for (int j = 0; (j < img_width) && !doneMsg; j++) //scans
                        {
                            if (getBit((byte)container_bytes[memoryPointer], rank))
                                msgBits += "1";
                            else
                                msgBits += "0";
                            msgBitsRead++;
                            memoryPointer += bytes_per_pixel;
                            if (msgBitsRead >= (msgLen * 8) + 16)
                            {
                                doneMsg = true;
                                break;
                            }
                        }
                    }
                }
                msgBits = msgBits.Substring(16);

                /*for (int i = header_offset + 2 + (bytes_per_pixel * 16); i < header_offset + 2 + (bytes_per_pixel * 16) + (bytes_per_pixel * 8 * msgLen); i+= bytes_per_pixel)
                {
                    if (getBit((byte)container_bytes[i], 0))
                                msgBits += "1";
                            else
                                msgBits += "0";
                }*/
                string tempChar = "";
                for (int i = 0; i < msgBits.Length; i++)
                {
                    tempChar += msgBits[i];
                    if ((i + 1) % (sizeof(char) * 8) == 0 && (i != 0))
                    {
                        char Symb = (char)binStringToInt(tempChar);
                        textBox_message.Text += Symb;
                        tempChar = "";
                    }
                }
                progress_steg.Visible = true;
                progress_steg.Refresh();
                label_progress.Visible = true;
                label_progress.Refresh();
            }
            catch (Exception ex)
            {
                var logLine = DateTime.Now.ToString() + ": " + ex.Message;
                log.Add(logLine);
                File.AppendAllText(Directory.GetCurrentDirectory() + "\\global_log.log", DateTime.Now.ToString() + ": LAB2_TZSPD: " + ex.Message + Environment.NewLine);
                try
                {
                    var time_ms = MSSQL_logging.log_error_onTimer(DateTime.Now, "LAB2_TZSPD", "LAB2_TZSPD: " + ex.Message);
                    toolStripStatusLabel1.Text = "Логирование выполнено за " + time_ms + "мс";
                }
                catch { }
                Invoke(new UpdateLogBoxDelegate(InvokeUpdateLogBox));
                MessageBox.Show(ex.Message, "Error.", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void сохранитьЛогToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }
    }
}
