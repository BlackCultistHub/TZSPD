using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using System.Collections;
using System.IO;
using System.Runtime;
using System.Runtime.InteropServices;

namespace Lab1
{
    public partial class Form3_tz : Form
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

        //SETTINGS
        bool useY = true;
        bool useCb = true;
        bool useCr = true;
        int N = 100;
        int C1_x = 6;
        int C1_y = 3;
        int C2_x = 3;
        int C2_y = 5;

        //HIDE
        Bitmap imageBitMap; // container raw
        List<Pack_1H1V> packs; //packs of matrixes INPUT
        int final_width;
        int final_height;
        int container_capacity;
        byte[] cvz;
        int cvzSize;
        byte[] input_key;
        int input_key_zeros;
        int finalMsgSize;

        //REVEAL
        Bitmap imageSteganoBitMap;
        List<Pack_1H1V> packsStegano; //packs of matrixes INPUT
        byte[] out_cvz;
        byte[] output_key;

        private void button_apply_settings_Click(object sender, EventArgs e)
        {
            applySettings();
        }

        private void applySettings()
        {
            try
            {
                useY = checkBox_use_Y.Checked;
                useCb = checkBox_use_Cb.Checked;
                useCr = checkBox_use_Cr.Checked;
                N = int.Parse(textBox_setting_N.Text);
                C1_x = int.Parse(textBox_C1_x.Text);
                C1_y = int.Parse(textBox_C1_y.Text);
                C2_x = int.Parse(textBox_C2_x.Text);
                C2_y = int.Parse(textBox_C2_y.Text);
                System.Media.SoundPlayer simpleSound = new System.Media.SoundPlayer(@"C:\Windows\Media\Windows Notify.wav");
                simpleSound.Play();
            }
            catch (Exception ex)
            {
                var logLine = DateTime.Now.ToString() + ex.Message;
                try
                {
                    var time_ms = MSSQL_logging.log_error_onTimer(DateTime.Now, "LAB3_TZSPD", "LAB3_TZSPD: " + ex.Message);
                    toolStripStatusLabel1.Text = "Логирование выполнено за " + time_ms + "мс";
                }
                catch { }
                log.Add(logLine);
                File.AppendAllText(Directory.GetCurrentDirectory() + "\\global_log.log", DateTime.Now.ToString() + ": LAB3_TZSPD: " + ex.Message + Environment.NewLine);
                MessageBox.Show(ex.Message, "Error.", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Invoke(new UpdateLogBoxDelegate(InvokeUpdateLogBox));
            }
        }

        public Form3_tz()
        {
            InitializeComponent();
            this.FormBorderStyle = FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.CenterToScreen();

            timer_UI_update.Start();
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
            SaveFileDialog saveFileDialog1 = new SaveFileDialog();

            saveFileDialog1.FileName = "lab3_tz.log";
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

        private void справкаToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var info = new Form_Info();
            info.Show();
        }

        private void button1_Click(object sender, EventArgs e) // CONTAINER
        {
            OpenFileDialog openFileDialog1 = new OpenFileDialog();
            openFileDialog1.Filter = "JPG image (*.jpg)|*.jpg|PNG image (*.png)|*.png|BMP image (*.bmp)|*.bmp|TIFF image (*.tiff)|*.tiff";

            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                textBox_container_path.Text = openFileDialog1.FileName;
                textBox_container_path.Refresh();
                toolStripStatusLabel_status.Text = "Чтение цветовой палитры...";
                statusStrip1.Refresh();

                imageBitMap = new Bitmap(Image.FromFile(openFileDialog1.FileName));

                toolStripStatusLabel_status.Text = "Конвертация цветовой палитры...";
                statusStrip1.Refresh();

                //unpack bitmap
                var NewBitMap = new Bitmap_YCbCr(imageBitMap);
                NewBitMap.apply2h2v();

                final_width = NewBitMap.width;
                final_height = NewBitMap.height;

                toolStripStatusLabel_status.Text = "Разметка на матрицы...";
                statusStrip1.Refresh();

                var BitMapRawMatrixes = NewBitMap.GetRawMatrixes();

                toolStripStatusLabel_status.Text = "Разметка на пачки...";
                statusStrip1.Refresh();

                //RAW MATRIXES
                packs = new List<Pack_1H1V>();
                foreach (var matrix in BitMapRawMatrixes)
                    packs.Add(new Pack_1H1V(matrix));

                //define capacity
                int multiplier = 0;
                if (useY)
                    multiplier++;
                if (useCb)
                    multiplier++;
                if (useCr)
                    multiplier++;
                label_container_capacity.Text = (packs.Count*multiplier/8 - 2).ToString()+" символов";
                container_capacity = (packs.Count * multiplier / 8) - 2;
                label_container_capacity.Refresh();

                toolStripStatusLabel_status.Text = "Просчёт ДКП...";
                toolStripProgressBar_work_progress.Visible = true;
                toolStripProgressBar_work_progress.Maximum = packs.Count;
                label_statusProgressBar_text_new.Visible = true;
                label_statusProgressBar_text_new.Text = "0/" + packs.Count;
                statusStrip1.Refresh();

                //FOR PSNR
                var YCbCr_packs_before_DCT = packs.ToArray();

                //DCT
                int working_pack = 0;
                foreach (var pack in packs)
                {
                    pack.Y_Matrix = DCT.DCTMatrix(DCT.DCTRangeMatrix(pack.Y_Matrix));
                    pack.Cb_Matrix = DCT.DCTMatrix(DCT.DCTRangeMatrix(pack.Cb_Matrix));
                    pack.Cr_Matrix = DCT.DCTMatrix(DCT.DCTRangeMatrix(pack.Cr_Matrix));
                        working_pack++;
                        toolStripProgressBar_work_progress.Value = working_pack;
                        label_statusProgressBar_text_new.Text = working_pack + "/" + packs.Count;
                        statusStrip1.Refresh();
                }
                label_statusProgressBar_text_new.Visible = false;
                toolStripProgressBar_work_progress.Visible = false;
                toolStripStatusLabel_status.Text = "Готово.";
                statusStrip1.Refresh();
                button_hide.Enabled = true;
                pictureBox_container_status.Image = Lab1.Properties.Resources.LED_green;
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog1 = new OpenFileDialog();
            openFileDialog1.Filter = "Hide file (*.*)|*.*";

            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                using (var file = openFileDialog1.OpenFile())
                {
                    BinaryReader binaryReader = new BinaryReader(file);
                    if (file != null)
                    {
                        cvz = binaryReader.ReadBytes((int)binaryReader.BaseStream.Length);
                        textBox_cvz_path.Text = openFileDialog1.FileName;
                        textBox_cvz_path.Refresh();
                        file.Dispose();
                        file.Close();
                    }
                }
            }
            label_message_size.Text = cvz.Length.ToString()+" байт";
            cvzSize = cvz.Length/2;
            label_message_size.Refresh();
        }

        private void button3_Click(object sender, EventArgs e) //STEGANO
        {
            OpenFileDialog openFileDialog1 = new OpenFileDialog();
            openFileDialog1.Filter = "JPG image (*.jpg)|*.jpg|PNG image (*.png)|*.png|BMP image (*.bmp)|*.bmp|TIFF image (*.tiff)|*.tiff";

            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                textBox_stegano_path.Text = openFileDialog1.FileName;
                textBox_stegano_path.Refresh();
                toolStripStatusLabel_status.Text = "Чтение цветовой палитры...";
                statusStrip1.Refresh();

                imageSteganoBitMap = new Bitmap(Image.FromFile(openFileDialog1.FileName));

                toolStripStatusLabel_status.Text = "Конвертация цветовой палитры...";
                statusStrip1.Refresh();

                //unpack bitmap
                var NewBitMap = new Bitmap_YCbCr(imageSteganoBitMap);
                NewBitMap.apply2h2v();

                toolStripStatusLabel_status.Text = "Разметка на матрицы...";
                statusStrip1.Refresh();

                var BitMapRawMatrixes = NewBitMap.GetRawMatrixes();

                toolStripStatusLabel_status.Text = "Разметка на пачки...";
                statusStrip1.Refresh();

                //RAW MATRIXES
                packsStegano = new List<Pack_1H1V>();
                foreach (var matrix in BitMapRawMatrixes)
                    packsStegano.Add(new Pack_1H1V(matrix));

                toolStripStatusLabel_status.Text = "Просчёт ДКП...";
                toolStripProgressBar_work_progress.Visible = true;
                toolStripProgressBar_work_progress.Maximum = packsStegano.Count;
                label_statusProgressBar_text_new.Visible = true;
                label_statusProgressBar_text_new.Text = "0/" + packsStegano.Count;
                statusStrip1.Refresh();

                //DCT
                int working_pack = 0;
                foreach (var pack in packsStegano)
                {
                    pack.Y_Matrix = DCT.DCTMatrix(DCT.DCTRangeMatrix(pack.Y_Matrix));
                    pack.Cb_Matrix = DCT.DCTMatrix(DCT.DCTRangeMatrix(pack.Cb_Matrix));
                    pack.Cr_Matrix = DCT.DCTMatrix(DCT.DCTRangeMatrix(pack.Cr_Matrix));
                        working_pack++;
                        toolStripProgressBar_work_progress.Value = working_pack;
                        label_statusProgressBar_text_new.Text = working_pack + "/" + packsStegano.Count;
                        statusStrip1.Refresh();
                }
                label_statusProgressBar_text_new.Visible = false;
                toolStripProgressBar_work_progress.Visible = false;
                toolStripStatusLabel_status.Text = "Готово.";
                statusStrip1.Refresh();
                button_reveal.Enabled = true;
                pictureBox_stegano_status.Image = Lab1.Properties.Resources.LED_green;
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            SaveFileDialog saveFileDialog1 = new SaveFileDialog();

            saveFileDialog1.FileName = "CVZ";
            saveFileDialog1.Filter = "Revealed file (*.*)|*.*";
            saveFileDialog1.FilterIndex = 2;
            saveFileDialog1.RestoreDirectory = true;

            if (saveFileDialog1.ShowDialog() == DialogResult.OK)
            {
                File.WriteAllBytes(saveFileDialog1.FileName, out_cvz);
            }
            textBox_output_path.Text = saveFileDialog1.FileName;
            textBox_output_path.Refresh();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            try
            {
                var format = System.Drawing.Imaging.ImageFormat.Jpeg;
                string formatS = "";
                //DEFINE SAVE PARAMS
                SaveFileDialog saveFileDialog1 = new SaveFileDialog();

                string savePath = "";

                saveFileDialog1.FileName = "SteganoContainer";
                saveFileDialog1.Filter = "JPG image (*.jpg)|*.jpg|PNG image (*.png)|*.png|BMP image (*.bmp)|*.bmp|TIFF image (*.tiff)|*.tiff";
                saveFileDialog1.FilterIndex = 2;
                saveFileDialog1.RestoreDirectory = true;

                if (saveFileDialog1.ShowDialog() == DialogResult.OK)
                {
                    formatS = saveFileDialog1.FileName;
                    formatS = Path.GetExtension(formatS);
                    switch (formatS)
                    {
                        case ".png":
                            format = System.Drawing.Imaging.ImageFormat.Png;
                            break;
                        case ".tiff":
                            format = System.Drawing.Imaging.ImageFormat.Tiff;
                            break;
                        case ".bmp":
                            format = System.Drawing.Imaging.ImageFormat.Bmp;
                            break;
                        default:
                            break;
                    }


                    savePath = saveFileDialog1.FileName;
                }
                //get cvz
                toolStripStatusLabel_status.Text = "Чтение ЦВЗ...";
                statusStrip1.Refresh();

                if (radioButton_cvz_input_text.Checked)
                {
                    List<byte> cvz_temp = new List<byte>();
                    foreach (var symb in textBox_cvz.Text)
                    {
                        cvz_temp.Add((byte)(symb >> 8));
                        cvz_temp.Add((byte)symb);
                    }
                    cvz = cvz_temp.ToArray();
                }

                List<byte> cvzArray = new List<byte>();
                for (int i = 0; i < cvz.Length * 8; i++)
                {
                    if (getBit(cvz[i / 8], 7 - i % 8))
                        cvzArray.Add(1);
                    else
                        cvzArray.Add(0);
                }
                //get key
                toolStripStatusLabel_status.Text = "Чтение ключа...";
                statusStrip1.Refresh();

                
                List<byte> keyArray = new List<byte>();
                if (checkBox_use_key.Checked)
                {
                    if (radioButton_key_input_text.Checked)
                    {
                        if (textBox_input_key.TextLength == 0)
                        {
                            for (int i = 0; i < 8; i++)
                                keyArray.Add(1);
                            input_key = keyArray.ToArray();
                        }
                        else
                        {
                            foreach (var symb in textBox_input_key.Text)
                            {
                                for (int i = 0; i < 8; i++) //USING ONLY FIRST BYTE
                                {
                                    if (getBit(symb, 7 - i))
                                        keyArray.Add(1);
                                    else
                                    {
                                        keyArray.Add(0);
                                        input_key_zeros++;
                                    }
                                }
                            }
                            input_key = keyArray.ToArray();
                        }
                    }
                }
                else
                {
                    for (int i = 0; i < 8; i++)
                        keyArray.Add(1);
                    input_key = keyArray.ToArray();
                }
                

                if (cvzArray.Count + input_key_zeros > container_capacity*8)
                    throw new Exception("Итоговый размер сообщения больше допустимого!");

                //hide
                toolStripStatusLabel_status.Text = "Модификация ДКП...";
                toolStripProgressBar_work_progress.Visible = true;
                toolStripProgressBar_work_progress.Maximum = packs.Count;
                label_statusProgressBar_text_new.Visible = true;
                label_statusProgressBar_text_new.Text = "0/" + packs.Count;
                statusStrip1.Refresh();

                //insert message len in the beginning
                int size = cvzArray.Count + 16;
                byte len_high = (byte)(size >> 8);
                byte len_low = (byte)(size & 0x00FF);
                for (int i = 0; i < 8; i++)
                {
                    if (getBit(len_low, i))
                        cvzArray.Insert(0, 1);
                    else
                        cvzArray.Insert(0, 0);
                }
                for (int i = 0; i < 8; i++)
                {
                    if (getBit(len_high, i))
                        cvzArray.Insert(0, 1);
                    else
                        cvzArray.Insert(0, 0);
                }
                //cvzArray.Insert(0, len_low);
                //cvzArray.Insert(0, len_high);

                int keyPointer = 0;
                int cvzPointer = 0;
                int working_pack = 0;
                foreach (var pack in packs)
                {
                    //Y
                    if (useY)
                    {
                        if (keyPointer == input_key.Length)
                            keyPointer = 0;
                        if (cvzPointer == cvzArray.Count)
                            break;
                        if (input_key[keyPointer] == 1)
                        {
                            keyPointer++;
                            if (cvzArray[cvzPointer] == 1)
                            {
                                pack.Y_Matrix[C1_y, C1_x] -= N / 2;
                                pack.Y_Matrix[C2_y, C2_x] += N / 2;
                            }
                            else
                            {
                                pack.Y_Matrix[C1_y, C1_x] += N / 2;
                                pack.Y_Matrix[C2_y, C2_x] -= N / 2;
                            }
                            cvzPointer++;
                        }
                        else
                            keyPointer++;
                    }
                    //Cb
                    if (useCb)
                    {
                        if (keyPointer == input_key.Length)
                            keyPointer = 0;
                        if (cvzPointer == cvzArray.Count)
                            break;
                        if (input_key[keyPointer] == 1)
                        {
                            keyPointer++;
                            if (cvzArray[cvzPointer] == 1)
                            {
                                pack.Cb_Matrix[C1_y, C1_x] -= N / 2;
                                pack.Cb_Matrix[C2_y, C2_x] += N / 2;
                            }
                            else
                            {
                                pack.Cb_Matrix[C1_y, C1_x] += N / 2;
                                pack.Cb_Matrix[C2_y, C2_x] -= N / 2;
                            }
                            cvzPointer++;
                        }
                        else
                            keyPointer++;
                    }
                    //Cr
                    if (useCr)
                    {
                        if (keyPointer == input_key.Length)
                            keyPointer = 0;
                        if (cvzPointer == cvzArray.Count)
                            break;
                        if (input_key[keyPointer] == 1)
                        {
                            keyPointer++;
                            if (cvzArray[cvzPointer] == 1)
                            {
                                pack.Cr_Matrix[C1_y, C1_x] -= N / 2;
                                pack.Cr_Matrix[C2_y, C2_x] += N / 2;
                            }
                            else
                            {
                                pack.Cr_Matrix[C1_y, C1_x] += N / 2;
                                pack.Cr_Matrix[C2_y, C2_x] -= N / 2;
                            }
                            cvzPointer++;
                        }
                        else
                            keyPointer++;
                    }
                    //::::UI
                    working_pack++;
                    toolStripProgressBar_work_progress.Value = working_pack;
                    label_statusProgressBar_text_new.Text = working_pack + "/" + packs.Count;
                    statusStrip1.Refresh();
                }

                //PACK BACK to bitMap
                //IDCT
                toolStripStatusLabel_status.Text = "Просчёт ИДКП...";
                toolStripProgressBar_work_progress.Maximum = packs.Count;
                working_pack = 0;
                toolStripProgressBar_work_progress.Value = 0;
                label_statusProgressBar_text_new.Text = "0/" + packs.Count;
                statusStrip1.Refresh();

                foreach (var pack in packs)
                {
                    pack.Y_Matrix = DCT.IDCTRangeMatrix(DCT.IDCTMatrix(pack.Y_Matrix));
                    pack.Cb_Matrix = DCT.IDCTRangeMatrix(DCT.IDCTMatrix(pack.Cb_Matrix));
                    pack.Cr_Matrix = DCT.IDCTRangeMatrix(DCT.IDCTMatrix(pack.Cr_Matrix));
                    //::::UI
                    working_pack++;
                    toolStripProgressBar_work_progress.Value = working_pack;
                    label_statusProgressBar_text_new.Text = working_pack + "/" + packs.Count;
                    statusStrip1.Refresh();
                }
                toolStripProgressBar_work_progress.Value = 0;
                toolStripProgressBar_work_progress.Visible = false;
                label_statusProgressBar_text_new.Visible = false;

                toolStripStatusLabel_status.Text = "Конвертация цветовой палитры...";
                statusStrip1.Refresh();

                var RGBMatrixes = new List<Color[,]>();
                foreach (var pack in packs)
                    RGBMatrixes.Add(pack.Get_Pixel_Matrix_RGB());

                toolStripStatusLabel_status.Text = "Формирование матрицы картинки...";
                toolStripProgressBar_work_progress.Value = 0;
                toolStripProgressBar_work_progress.Visible = true;
                toolStripProgressBar_work_progress.Maximum = RGBMatrixes.Count;
                label_statusProgressBar_text_new.Visible = true;
                label_statusProgressBar_text_new.Text = "0/" + RGBMatrixes.Count;
                statusStrip1.Refresh();
                
                working_pack = 0;

                Color[,] newMatrix = new Color[final_height, final_width];
                int x_offset = 0;
                int y_offset = 0;
                foreach (var mini_matrix in RGBMatrixes)
                {
                    //check offsets
                    if (x_offset >= final_width)
                    {
                        x_offset = 0;
                        y_offset += 8;
                    }
                    //y_offset cannot be > height here

                    //write 8x8 matrix
                    for (int i = 0; i < 8; i++)
                    {
                        for (int j = 0; j < 8; j++)
                        {
                            newMatrix[i + y_offset, j + x_offset] = mini_matrix[i, j];
                        }
                    }
                    //move offsets
                    x_offset += 8;

                    //::::UI
                    working_pack++;
                    toolStripProgressBar_work_progress.Value = working_pack;
                    label_statusProgressBar_text_new.Text = working_pack + "/" + RGBMatrixes.Count;
                    statusStrip1.Refresh();
                }
                toolStripProgressBar_work_progress.Value = 0;
                toolStripProgressBar_work_progress.Visible = false;
                label_statusProgressBar_text_new.Visible = false;

                toolStripStatusLabel_status.Text = "Создание карты...";
                statusStrip1.Refresh();

                var newImg = makeBitmap(newMatrix, final_width, final_height);

                toolStripStatusLabel_status.Text = "Сохранение...";
                statusStrip1.Refresh();

                //ENCODER SETTING
                var quality = 100L;

                System.Drawing.Imaging.ImageCodecInfo formatEncoder = GetEncoder(format);
                System.Drawing.Imaging.Encoder encoder = System.Drawing.Imaging.Encoder.Quality;                                //dummy encoder for params
                System.Drawing.Imaging.EncoderParameters encoderParameters = new System.Drawing.Imaging.EncoderParameters(1);
                System.Drawing.Imaging.EncoderParameter qualityParameter = new System.Drawing.Imaging.EncoderParameter(encoder, quality);
                encoderParameters.Param[0] = qualityParameter;

                newImg.Save(savePath, formatEncoder, encoderParameters);

                //PSNR
                int PSNR_Y = 0;
                int PSNR_Cb = 0;
                int PSNR_Cr = 0;
                int PSNR_R = 0;
                int PSNR_G = 0;
                int PSNR_B = 0;
                toolStripStatusLabel_status.Text = "Вычисление PSNR...";
                statusStrip1.Refresh();


                toolStripStatusLabel_status.Text = "Готово.";
                statusStrip1.Refresh();

                button_hide.Enabled = false;
                pictureBox_container_status.Image = Lab1.Properties.Resources.LED_red;
            }
            catch (Exception ex)
            {
                var logLine = DateTime.Now.ToString() + ex.Message;
                try
                {
                    var time_ms = MSSQL_logging.log_error_onTimer(DateTime.Now, "LAB3_TZSPD", "LAB3_TZSPD: " + ex.Message);
                    toolStripStatusLabel1.Text = "Логирование выполнено за " + time_ms + "мс";
                }
                catch { }
                log.Add(logLine);
                File.AppendAllText(Directory.GetCurrentDirectory() + "\\global_log.log", DateTime.Now.ToString() + ": LAB3_TZSPD: " + ex.Message + Environment.NewLine);
                MessageBox.Show(ex.Message, "Error.", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Invoke(new UpdateLogBoxDelegate(InvokeUpdateLogBox));
            }
        }

        private void button_reveal_Click(object sender, EventArgs e)
        {
            try
            {
                toolStripStatusLabel_status.Text = "Чтение ключа...";
                statusStrip1.Refresh();
                
                List<byte> keyArray = new List<byte>();
                if (checkBox_stegano_use_key.Checked)
                {
                    if (radioButton_output_key_text.Checked)
                    {
                        if (textBox_output_key.TextLength == 0)
                        {
                            for (int i = 0; i < 8; i++)
                                keyArray.Add(1);
                            output_key = keyArray.ToArray();
                        }
                        else
                        {
                            foreach (var symb in textBox_output_key.Text)
                            {
                                for (int i = 0; i < 8; i++) //USING ONLY FIRST BYTE
                                {
                                    if (getBit(symb, 7 - i))
                                        keyArray.Add(1);
                                    else
                                        keyArray.Add(0);
                                }
                            }
                            output_key = keyArray.ToArray();
                        }
                    }
                }
                else
                {
                    for (int i = 0; i < 8; i++)
                        keyArray.Add(1);
                    output_key = keyArray.ToArray();
                }

                toolStripStatusLabel_status.Text = "Экспорт размера сообщения...";
                statusStrip1.Refresh();

                //extract size
                int expectedSize = 0;
                int size_part_extracted = 0; //16 bit

                int keyPointer = 0;
                int working_pack = 0;
                foreach (var pack in packsStegano)
                {
                    //Y
                    if (useY)
                    {
                        if (keyPointer == output_key.Length)
                            keyPointer = 0;
                        if (size_part_extracted == 16)
                            break;
                        if (output_key[keyPointer] == 1)
                        {
                            keyPointer++;
                            int C1 = pack.Y_Matrix[C1_y, C1_x];
                            int C2 = pack.Y_Matrix[C2_y, C2_x];
                            if (C1 < C2)
                                expectedSize += 1;
                            expectedSize <<= 1;
                            size_part_extracted++;
                        }
                        else
                            keyPointer++;
                    }
                    //Cb
                    if (useCb)
                    {
                        if (keyPointer == output_key.Length)
                            keyPointer = 0;
                        if (size_part_extracted == 16)
                            break;
                        if (output_key[keyPointer] == 1)
                        {
                            keyPointer++;
                            int C1 = pack.Cb_Matrix[C1_y, C1_x];
                            int C2 = pack.Cb_Matrix[C2_y, C2_x];
                            if (C1 < C2)
                                expectedSize += 1;
                            expectedSize <<= 1;
                            size_part_extracted++;
                        }
                        else
                            keyPointer++;
                    }
                    //Cr
                    if (useCr)
                    {
                        if (keyPointer == output_key.Length)
                            keyPointer = 0;
                        if (size_part_extracted == 16)
                            break;
                        if (output_key[keyPointer] == 1)
                        {
                            keyPointer++;
                            int C1 = pack.Cr_Matrix[C1_y, C1_x];
                            int C2 = pack.Cr_Matrix[C2_y, C2_x];
                            if (C1 < C2)
                                expectedSize += 1;
                            expectedSize <<= 1;
                            size_part_extracted++;
                        }
                        else
                            keyPointer++;
                    }
                    //::::UI
                    working_pack++;
                    toolStripProgressBar_work_progress.Value = working_pack;
                    label_statusProgressBar_text_new.Text = working_pack + "/" + packsStegano.Count;
                    statusStrip1.Refresh();
                }
                expectedSize >>= 1;

                if (expectedSize == 0)
                    throw new Exception("Считанная длина 0. Возможно контейнер пуст или повреждён?");

                //extract message
                toolStripStatusLabel_status.Text = "Экспорт сообщения...";
                statusStrip1.Refresh();

                keyPointer = 0;
                int bitsRead = 0; // 8
                byte temp_byte = 0;
                List<byte> message_raw = new List<byte>();
                foreach (var pack in packsStegano)
                {
                    //Y
                    if (useY)
                    {
                        if (keyPointer == output_key.Length)
                            keyPointer = 0;
                        if (bitsRead == 8)
                        {
                            bitsRead = 0;
                            message_raw.Add((byte)(temp_byte >> 1));
                            temp_byte = 0;
                        }
                        if (message_raw.Count == expectedSize / 8)
                            break;
                        if (output_key[keyPointer] == 1)
                        {
                            keyPointer++;
                            int C1 = pack.Y_Matrix[C1_y, C1_x];
                            int C2 = pack.Y_Matrix[C2_y, C2_x];
                            if (C1 < C2)
                                temp_byte += 1;
                            temp_byte <<= 1;
                            bitsRead++;
                        }
                        else
                            keyPointer++;
                    }
                    //Cb
                    if (useCb)
                    {
                        if (keyPointer == output_key.Length)
                            keyPointer = 0;
                        if (bitsRead == 8)
                        {
                            bitsRead = 0;
                            message_raw.Add((byte)(temp_byte >> 1));
                            temp_byte = 0;
                        }
                        if (message_raw.Count == expectedSize / 8)
                            break;
                        if (output_key[keyPointer] == 1)
                        {
                            keyPointer++;
                            int C1 = pack.Cb_Matrix[C1_y, C1_x];
                            int C2 = pack.Cb_Matrix[C2_y, C2_x];
                            if (C1 < C2)
                                temp_byte += 1;
                            temp_byte <<= 1;
                            bitsRead++;
                        }
                        else
                            keyPointer++;
                    }
                    //Cr
                    if (useCr)
                    {
                        if (keyPointer == output_key.Length)
                            keyPointer = 0;
                        if (bitsRead == 8)
                        {
                            bitsRead = 0;
                            message_raw.Add((byte)(temp_byte >> 1));
                            temp_byte = 0;
                        }
                        if (message_raw.Count == expectedSize / 8)
                            break;
                        if (output_key[keyPointer] == 1)
                        {
                            keyPointer++;
                            int C1 = pack.Cr_Matrix[C1_y, C1_x];
                            int C2 = pack.Cr_Matrix[C2_y, C2_x];
                            if (C1 < C2)
                                temp_byte += 1;
                            temp_byte <<= 1;
                            bitsRead++;
                        }
                        else
                            keyPointer++;
                        //::::UI
                        working_pack++;
                        toolStripProgressBar_work_progress.Value = working_pack;
                        label_statusProgressBar_text_new.Text = working_pack + "/" + packsStegano.Count;
                        statusStrip1.Refresh();
                    }
                }

                //remove size
                message_raw.RemoveAt(0);
                message_raw.RemoveAt(0);
                out_cvz = message_raw.ToArray();

                toolStripStatusLabel_status.Text = "Отображение...";
                statusStrip1.Refresh();

                //shrink to char
                textBox_output.Text = "";
                for (int i = 0; i < message_raw.Count; i += 2) 
                {
                    if (i == 200)//FIRST 100 chars
                        break;
                    char tempChar;
                    tempChar = (char)message_raw[i];
                    tempChar <<= 8;
                    tempChar += (char)message_raw[i + 1];
                    textBox_output.Text += tempChar;
                }
                toolStripStatusLabel_status.Text = "Готово.";
                statusStrip1.Refresh();
                button_reveal.Enabled = false;
                pictureBox_stegano_status.Image = Lab1.Properties.Resources.LED_red;
            }
            catch (Exception ex)
            {
                var logLine = DateTime.Now.ToString() + ex.Message;
                try
                {
                    var time_ms = MSSQL_logging.log_error_onTimer(DateTime.Now, "LAB3_TZSPD", "LAB3_TZSPD: " + ex.Message);
                    toolStripStatusLabel1.Text = "Логирование выполнено за " + time_ms + "мс";
                }
                catch { }
                log.Add(logLine);
                File.AppendAllText(Directory.GetCurrentDirectory() + "\\global_log.log", DateTime.Now.ToString() + ": LAB3_TZSPD: " + ex.Message + Environment.NewLine);
                MessageBox.Show(ex.Message, "Error.", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Invoke(new UpdateLogBoxDelegate(InvokeUpdateLogBox));
            }
        }

        private System.Drawing.Imaging.ImageCodecInfo GetEncoder(System.Drawing.Imaging.ImageFormat format)
        {
            System.Drawing.Imaging.ImageCodecInfo[] codecs = System.Drawing.Imaging.ImageCodecInfo.GetImageDecoders();
            foreach (System.Drawing.Imaging.ImageCodecInfo codec in codecs)
            {
                if (codec.FormatID == format.Guid)
                {
                    return codec;
                }
            }
            return null;
        }

        private static Bitmap makeBitmap(Color[,] newMatrix, int width, int height)
        {
            Bitmap newBitMap = new Bitmap(width, height, System.Drawing.Imaging.PixelFormat.Format24bppRgb);

            Rectangle BoundsRect = new Rectangle(0, 0, width, height);
            System.Drawing.Imaging.BitmapData bitMapData = newBitMap.LockBits(BoundsRect,
                                            System.Drawing.Imaging.ImageLockMode.WriteOnly,
                                            newBitMap.PixelFormat);
            IntPtr ptr = bitMapData.Scan0;

            int scanWidth = bitMapData.Stride;
            byte[] rawImg = new byte[width * height * 3];

            int rawImgCounter = 0;
            for (int i = 0; i < height; i++)
            {
                for (int j = 0; j < width; j++)
                {
                    rawImg[rawImgCounter] = newMatrix[i, j].R;
                    rawImgCounter++;
                    rawImg[rawImgCounter] = newMatrix[i, j].G;
                    rawImgCounter++;
                    rawImg[rawImgCounter] = newMatrix[i, j].B;
                    rawImgCounter++;
                }
            }

            // fill in rgbValues
            Marshal.Copy(rawImg, 0, ptr, rawImg.Length);
            newBitMap.UnlockBits(bitMapData);
            return newBitMap;
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
        public int binaryLen(ulong msg)
        {
            int i;
            for (i = 0; msg >= (ulong)Math.Pow(2, i); i++) { }
            return i;
        }

        public int binaryLenHuffCodes(ulong msg)
        {
            int i;
            for (i = 0; msg >= (ulong)Math.Pow(2, i); i++) { }
            return --i;
        }

        public static int FairRound(double numb)
        {
            int real = (int)numb;
            double tail = numb - real;
            if (tail < 0.5)
                return real;
            else
                return real + 1;
        }

        public static class DCT
        {
            public static int[,] DCTRangeMatrix(int[,] srcMatrix) //make -128 =< value-128 =< 127 OK
            {
                int[,] srcShifted = new int[8, 8];
                for (int i = 0; i < 8; i++)
                {
                    for (int j = 0; j < 8; j++)
                    {
                        srcShifted[i, j] = Math.Max(Math.Min(srcMatrix[i, j] - 128, 127), -128);
                    }
                }
                return srcShifted;
            }
            public static int[,] DCTMatrix(int[,] srcMatrix) //OK
            {
                int[,] srcDCT = new int[8, 8];
                for (int u = 0; u < 8; u++)
                {
                    for (int v = 0; v < 8; v++)
                    {
                        double au = (u == 0 ? (1 / Math.Sqrt(2)) : 1);
                        double av = (v == 0 ? (1 / Math.Sqrt(2)) : 1);
                        double mult = (au * av) / 4;
                        double summ1x = 0;
                        for (int x = 0; x < 8; x++)
                        {
                            double summ2y = 0;
                            for (int y = 0; y < 8; y++)
                            {
                                double valcos1 = (((2 * x) + 1) * u * Math.PI) / 16;
                                double multcos1 = Math.Cos(valcos1);
                                double valcos2 = (((2 * y) + 1) * v * Math.PI) / 16;
                                double multcos2 = Math.Cos(valcos2);
                                summ2y += srcMatrix[y, x] * multcos1 * multcos2;
                            }
                            summ1x += summ2y;
                        }
                        srcDCT[v, u] = (int)(mult * summ1x);
                    }
                }
                return srcDCT;
            }

            public static int[,] IDCTRangeMatrix(int[,] srcMatrix) //make 0 =< value+128 =< 255 OK
            {
                int[,] srcShifted = new int[8, 8];
                for (int i = 0; i < 8; i++)
                {
                    for (int j = 0; j < 8; j++)
                    {
                        srcShifted[i, j] = Math.Min(Math.Max(0, srcMatrix[i, j] + 128), 255);
                    }
                }
                return srcShifted;
            }

            public static int[,] IDCTMatrix(int[,] srcMatrix)
            {
                int[,] IDCT = new int[8, 8];
                for (int x = 0; x < 8; x++)
                {
                    for (int y = 0; y < 8; y++)
                    {
                        double mult = 0.25;
                        double summ1x = 0;
                        for (int u = 0; u < 8; u++)
                        {
                            double summ2y = 0;
                            for (int v = 0; v < 8; v++)
                            {
                                double Cu = (u == 0 ? 1 / Math.Sqrt(2) : 1);
                                double Cv = (v == 0 ? 1 / Math.Sqrt(2) : 1);
                                double valcos1 = (((2 * x) + 1) * u * Math.PI) / 16;
                                double multcos1 = Math.Cos(valcos1);
                                double valcos2 = (((2 * y) + 1) * v * Math.PI) / 16;
                                double multcos2 = Math.Cos(valcos2);
                                summ2y += srcMatrix[v, u] * Cu * Cv * multcos1 * multcos2;
                            }
                            summ1x += summ2y;
                        }
                        IDCT[y, x] = (int)(mult * summ1x);
                    }
                }
                return IDCT;
            }
        }
        public class Bitmap_YCbCr
        {
            public Bitmap_YCbCr(Bitmap oldBitMap)
            {
                int newWidth = oldBitMap.Width;
                if (oldBitMap.Width % 16 != 0)
                    newWidth = oldBitMap.Width + (16 - oldBitMap.Width % 16);
                int newHeight = oldBitMap.Height;
                if (oldBitMap.Height % 16 != 0)
                    newHeight = oldBitMap.Height + (16 - oldBitMap.Height % 16);
                this.width = newWidth;
                this.height = newHeight;
                bitmap = new Pixel[newHeight, newWidth];

                var resizedOldBitMap = ResizeImage(oldBitMap, newWidth, newHeight);

                for (int i = 0; i < resizedOldBitMap.Height; i++)
                {
                    for (int j = 0; j < resizedOldBitMap.Width; j++)
                    {
                        var pixel = resizedOldBitMap.GetPixel(j, i);
                        bitmap[i, j] = new Pixel(pixel.R, pixel.G, pixel.B);
                    }
                }
            }
            public Bitmap_YCbCr(int width, int height)
            {
                int newWidth = width;
                if (width % 16 != 0)
                    newWidth = width + (16 - width % 16);
                int newHeight = height;
                if (height % 16 != 0)
                    newHeight = height + (16 - height % 16);
                bitmap = new Pixel[newHeight, newWidth];
            }

            public Bitmap_YCbCr(Bitmap_YCbCr bitmap_)
            {
                bitmap = new Pixel[bitmap_.height, bitmap_.width];
                for (int i = 0; i < bitmap_.height; i++)
                {
                    for (int j = 0; j < bitmap_.width; j++)
                    {
                        var pixel = bitmap_.GetPixel(j, i);
                        bitmap[i, j] = new Pixel(pixel.Y, pixel.Cb, pixel.Cr, true);
                    }
                }
            }

            public Bitmap_YCbCr Copy()
            {
                return new Bitmap_YCbCr(this);
            }

            public void addPixel(int x, int y, int R, int G, int B)
            {
                bitmap[y, x] = new Pixel(R, G, B);
            }
            public Pixel GetPixel(int x, int y)
            {
                return bitmap[y, x];
            }

            public void apply2h2v()
            {
                this.Sampling_Mode = JPEG_Sampling_Mode._2H2V;
                for (int i = 0; i < this.height - 1; i += 2)
                {
                    for (int j = 0; j < this.width; j += 2)
                    {
                        var thisPixel = this.GetPixel(j, i);
                        var nextPixel = this.GetPixel(j + 1, i);
                        var this_nextPixel = this.GetPixel(j, i + 1);
                        var next_nextPixel = this.GetPixel(j + 1, i + 1);
                        int averageCb = (thisPixel.Cb + nextPixel.Cb + this_nextPixel.Cb + next_nextPixel.Cb) / 4;
                        int averageCr = (thisPixel.Cr + nextPixel.Cr + this_nextPixel.Cr + next_nextPixel.Cr) / 4;
                        //Cb
                        this.bitmap[i, j].Cb = averageCb;
                        this.bitmap[i, j + 1].Cb = averageCb;
                        this.bitmap[i + 1, j].Cb = averageCb;
                        this.bitmap[i + 1, j + 1].Cb = averageCb;
                        //Cr
                        this.bitmap[i, j].Cr = averageCr;
                        this.bitmap[i, j + 1].Cr = averageCr;
                        this.bitmap[i + 1, j].Cr = averageCr;
                        this.bitmap[i + 1, j + 1].Cr = averageCr;
                    }
                }
            }

            public JPEG_Sampling_Mode GetSampling_Mode()
            {
                return this.Sampling_Mode;
            }

            public List<Bitmap_YCbCr.Pixel[,]> GetRawMatrixes()
            {
                List<Bitmap_YCbCr.Pixel[,]> rawMatrixes = new List<Pixel[,]>();

                for (int i = 0; i < height / 8; i++)
                {
                    for (int j = 0; j < width / 8; j++)
                    {
                        Bitmap_YCbCr.Pixel[,] newMatrix = new Pixel[8, 8];
                        for (int k = 0; k < 8; k++)
                        {
                            for (int h = 0; h < 8; h++)
                            {
                                int global_i = i * 8 + k;
                                int global_j = j * 8 + h;
                                newMatrix[k, h] = bitmap[global_i, global_j];
                            }
                        }
                        rawMatrixes.Add(newMatrix);
                    }
                }
                return rawMatrixes;
            }

            public int width;
            public int height;

            private JPEG_Sampling_Mode Sampling_Mode = JPEG_Sampling_Mode._None;

            public Pixel[,] bitmap;

            public enum JPEG_Sampling_Mode
            {
                _None,
                _2H2V,
                _2H1V
            }
            public class Pixel
            {
                public Pixel(int R, int G, int B)
                {
                    //METHOD 1
                    //this.Y = (int)((0.299 * R) + (0.587 * G) + (0.114 * B));
                    //this.Cb = (int)(128 - (0.168736 * R) - (0.331264 * G) + (0.5 * B));
                    //this.Cr = (int)(128 + (0.5 * R) - (0.418688 * G) - (0.081312 * B));
                    ////test for back
                    //double Rn = this.Y + (1.402 * (this.Cr - 128));
                    //double Gn = this.Y - (0.34414 * (this.Cb - 128)) - (0.71414 * (this.Cr - 128));
                    //double Bn = this.Y + (1.772 * (this.Cb - 128));

                    //METHOD 2
                    this.Y = FairRound(16 + ((65.738 * R) / 256) + ((129.057 * G) / 256) + ((25.064 * B) / 256));
                    this.Cb = FairRound(128 + ((-37.945 * R) / 256) - ((74.494 * G) / 256) + ((112.439 * B) / 256));
                    this.Cr = FairRound(128 + ((112.439 * R) / 256) - ((94.154 * G) / 256) - ((18.285 * B) / 256));
                    //check
                    double Rn = ((298.082 * this.Y) / 256) + ((408.583 * this.Cr) / 256) - 222.921;
                    double Gn = ((298.082 * this.Y) / 256) - ((100.291 * this.Cb) / 256) - ((208.120 * this.Cr) / 256) + 135.576;
                    double Bn = ((298.082 * this.Y) / 256) + ((516.412 * this.Cb) / 256) - 276.836;

                    int Ri = FairRound(Rn);
                    int Gi = FairRound(Gn);
                    int Bi = FairRound(Bn);
                }
                public Pixel(int Y, int Cb, int Cr, bool noConvert = true)
                {
                    this.Y = Y;
                    this.Cb = Cb;
                    this.Cr = Cr;
                }

                public int Y;
                public int Cb;
                public int Cr;
            }
            /// <summary>
            /// Resize the image to the specified width and height.
            /// </summary>
            /// <param name="image">The image to resize.</param>
            /// <param name="width">The width to resize to.</param>
            /// <param name="height">The height to resize to.</param>
            /// <returns>The resized image.</returns>
            public static Bitmap ResizeImage(Image image, int width, int height)
            {
                var destRect = new Rectangle(0, 0, width, height);
                var destImage = new Bitmap(width, height);

                destImage.SetResolution(image.HorizontalResolution, image.VerticalResolution);

                using (var graphics = Graphics.FromImage(destImage))
                {
                    graphics.CompositingMode = System.Drawing.Drawing2D.CompositingMode.SourceCopy;
                    graphics.CompositingQuality = System.Drawing.Drawing2D.CompositingQuality.HighQuality;
                    graphics.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
                    graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
                    graphics.PixelOffsetMode = System.Drawing.Drawing2D.PixelOffsetMode.HighQuality;

                    using (var wrapMode = new System.Drawing.Imaging.ImageAttributes())
                    {
                        wrapMode.SetWrapMode(System.Drawing.Drawing2D.WrapMode.TileFlipXY);
                        graphics.DrawImage(image, destRect, 0, 0, image.Width, image.Height, GraphicsUnit.Pixel, wrapMode);
                    }
                }

                return destImage;
            }
        }

        public class Pack_1H1V
        {
            public Pack_1H1V(Bitmap_YCbCr.Pixel[,] pixel_Matrix)
            {
                Y_Matrix = new int[8, 8];
                Cb_Matrix = new int[8, 8];
                Cr_Matrix = new int[8, 8];
                for (int i = 0; i < 8; i++)
                {
                    for (int j = 0; j < 8; j++)
                    {
                        Y_Matrix[i, j] = pixel_Matrix[i, j].Y;
                        Cb_Matrix[i, j] = pixel_Matrix[i, j].Cb;
                        Cr_Matrix[i, j] = pixel_Matrix[i, j].Cr;
                    }
                }
            }

            public Color[,] Get_Pixel_Matrix_RGB()
            {
                Color[,] pixel_Matrix_RGB = new Color[8, 8];
                for (int i = 0; i < 8; i++)
                {
                    for (int j = 0; j < 8; j++)
                    {
                        //METHOD 1
                        //int R = Math.Max(0, Math.Min(255, (int)(Y_Matrix[i, j] + (1.402 * (Cr_Matrix[i, j] - 128)))));
                        //int G = Math.Max(0, Math.Min(255, (int)(Y_Matrix[i, j] - (0.34414 * (Cb_Matrix[i, j] - 128)) - (0.71414 * (Cr_Matrix[i, j] - 128)))));
                        //int B = Math.Max(0, Math.Min(255, (int)(Y_Matrix[i, j] + (1.772 * (Cb_Matrix[i, j] - 128)))));
                        //pixel_Matrix_RGB[i, j] = Color.FromArgb(B, G, R);

                        //working
                        //int R = Math.Max(0, Math.Min(255, (int)(Y_Matrix[i, j] + (1.402 * (Cb_Matrix[i, j] - 128)))));
                        //int G = Math.Max(0, Math.Min(255, (int)(Y_Matrix[i, j] - (0.34414 * (Cr_Matrix[i, j] - 128)) - (0.71414 * (Cb_Matrix[i, j] - 128)))));
                        //int B = Math.Max(0, Math.Min(255, (int)(Y_Matrix[i, j] + (1.772 * (Cr_Matrix[i, j] - 128)))));
                        //pixel_Matrix_RGB[i, j] = Color.FromArgb(R, G, B); //так надо. не трогать.

                        //METHOD 2
                        double Rn = ((298.082 * Y_Matrix[i, j]) / 256) + ((408.583 * Cr_Matrix[i, j]) / 256) - 222.921;
                        double Gn = ((298.082 * Y_Matrix[i, j]) / 256) - ((100.291 * Cb_Matrix[i, j]) / 256) - ((208.120 * Cr_Matrix[i, j]) / 256) + 135.576;
                        double Bn = ((298.082 * Y_Matrix[i, j]) / 256) + ((516.412 * Cb_Matrix[i, j]) / 256) - 276.836;
                        int R = Math.Max(0, Math.Min(255, FairRound(Rn)));
                        int G = Math.Max(0, Math.Min(255, FairRound(Gn)));
                        int B = Math.Max(0, Math.Min(255, FairRound(Bn)));
                        pixel_Matrix_RGB[i, j] = Color.FromArgb(B, G, R);
                    }
                }
                return pixel_Matrix_RGB;
            }

            public int[,] Y_Matrix;
            public int[,] Cb_Matrix;
            public int[,] Cr_Matrix;
        }

        public class Pack_2H2V
        {
            public Pack_2H2V()
            {
            }

            public void insertM00(Bitmap_YCbCr.Pixel[,] M00_)
            {
                M00 = M00_;
            }
            public void insertM01(Bitmap_YCbCr.Pixel[,] M01_)
            {
                M01 = M01_;
            }
            public void insertM10(Bitmap_YCbCr.Pixel[,] M10_)
            {
                M10 = M10_;
            }
            public void insertM11(Bitmap_YCbCr.Pixel[,] M11_)
            {
                M11 = M11_;
            }

            public void calcSampling()
            {
                //fill Y
                for (int i = 0; i < 8; i++)
                {
                    for (int j = 0; j < 8; j++)
                    {
                        Y0_matrix[i, j] = M00[i, j].Y;
                        Y1_matrix[i, j] = M01[i, j].Y;
                        Y2_matrix[i, j] = M10[i, j].Y;
                        Y3_matrix[i, j] = M11[i, j].Y;
                    }
                }
                //fill Cb Cr
                int[,] tempCb = new int[16, 16];
                int[,] tempCr = new int[16, 16];
                //block 00
                for (int i = 0; i < 8; i++)
                {
                    for (int j = 0; j < 8; j++)
                    {
                        tempCb[i, j] = M00[i, j].Cb;
                        tempCr[i, j] = M00[i, j].Cr;
                    }
                }
                //block 01
                for (int i = 0; i < 8; i++)
                {
                    for (int j = 0; j < 8; j++)
                    {
                        tempCb[i, j + 8] = M01[i, j].Cb;
                        tempCr[i, j + 8] = M01[i, j].Cr;
                    }
                }
                //block 10
                for (int i = 0; i < 8; i++)
                {
                    for (int j = 0; j < 8; j++)
                    {
                        tempCb[i + 8, j] = M10[i, j].Cb;
                        tempCr[i + 8, j] = M10[i, j].Cr;
                    }
                }
                //block 11
                for (int i = 0; i < 8; i++)
                {
                    for (int j = 0; j < 8; j++)
                    {
                        tempCb[i + 8, j + 8] = M11[i, j].Cb;
                        tempCr[i + 8, j + 8] = M11[i, j].Cr;
                    }
                }
                //squish to 8x8
                for (int i = 0; i < 16; i += 2)
                {
                    for (int j = 0; j < 16; j += 2)
                    {
                        Cb_matrix[i / 2, j / 2] = tempCb[i, j];
                        Cr_matrix[i / 2, j / 2] = tempCr[i, j];
                    }
                }
            }
            public void appyZigZag()
            {
                strip_Y0_matrix = transform(Y0_matrix);
                strip_Y1_matrix = transform(Y1_matrix);
                strip_Y2_matrix = transform(Y2_matrix);
                strip_Y3_matrix = transform(Y3_matrix);
                strip_Cb_matrix = transform(Cb_matrix);
                strip_Cr_matrix = transform(Cr_matrix);
            }

            private int[] transform(int[,] srcMatrix)
            {
                int[] strip_matrix = new int[64];
                for (int i = 0; i < 8; i++)
                {
                    for (int j = 0; j < 8; j++)
                    {
                        strip_matrix[(zigZagOrder[i, j])] = srcMatrix[i, j];
                    }
                }
                return strip_matrix;
            }

            public void applyDCT()
            {
                Y0_matrix = DCTRangeMatrix(Y0_matrix);
                Y0_matrix = DCTMatrix(Y0_matrix);
                Y1_matrix = DCTRangeMatrix(Y1_matrix);
                Y1_matrix = DCTMatrix(Y1_matrix);
                Y2_matrix = DCTRangeMatrix(Y2_matrix);
                Y2_matrix = DCTMatrix(Y2_matrix);
                Y3_matrix = DCTRangeMatrix(Y3_matrix);
                Y3_matrix = DCTMatrix(Y3_matrix);
                Cb_matrix = DCTRangeMatrix(Cb_matrix);
                Cb_matrix = DCTMatrix(Cb_matrix);
                Cr_matrix = DCTRangeMatrix(Cr_matrix);
                Cr_matrix = DCTMatrix(Cr_matrix);
            }

            public void applyIDCT()
            {

            }

            int[,] testMatrix =
            {
                { -76, -73, -67, -62, -58, -67, -64, -55 },
                { -65, -69, -73, -38, -19, -43, -59, -56 },
                { -66, -69, -60, -15,  16, -24, -62, -55 },
                {  65,  70,  57,   6,  26,  22,  58,  59 },
                { -61, -67, -60, -24,  -2, -40, -60, -58 },
                { -49, -63, -68, -58, -51, -60, -70, -53 },
                {  43,  57,  64,  69,  73,  67,  63,  45 },
                { -41, -49, -59, -60, -63, -52, -50, -34 }
            };

            private int[,] DCTMatrix(int[,] srcMatrix) //OK
            {
                int[,] srcDCT = new int[8, 8];
                for (int u = 0; u < 8; u++)
                {
                    for (int v = 0; v < 8; v++)
                    {
                        double au = (u == 0 ? (1 / Math.Sqrt(2)) : 1);
                        double av = (v == 0 ? (1 / Math.Sqrt(2)) : 1);
                        double mult = (au * av) / 4;
                        double summ1x = 0;
                        for (int x = 0; x < 8; x++)
                        {
                            double summ2y = 0;
                            for (int y = 0; y < 8; y++)
                            {
                                double valcos1 = (((2 * x) + 1) * u * Math.PI) / 16;
                                double multcos1 = Math.Cos(valcos1);
                                double valcos2 = (((2 * y) + 1) * v * Math.PI) / 16;
                                double multcos2 = Math.Cos(valcos2);
                                summ2y += srcMatrix[y, x] * multcos1 * multcos2;
                            }
                            summ1x += summ2y;
                        }
                        srcDCT[v, u] = (int)(mult * summ1x);
                    }
                }
                return srcDCT;
            }

            private int[,] DCTRangeMatrix(int[,] srcMatrix) //make -128 =< value-128 =< 127 OK
            {
                int[,] srcShifted = new int[8, 8];
                for (int i = 0; i < 8; i++)
                {
                    for (int j = 0; j < 8; j++)
                    {
                        srcShifted[i, j] = Math.Max(Math.Min(srcMatrix[i, j] - 128, 127), -128);
                    }
                }
                return srcShifted;
            }

            private void IDCTMatrix(int[,] srcMatrix)
            {
                int[,] IDCT = new int[8, 8];
                for (int x = 0; x < 8; x++)
                {
                    for (int y = 0; y < 8; y++)
                    {
                        double mult = 0.25;
                        double summ1x = 0;
                        for (int u = 0; u < 8; u++)
                        {
                            double summ2y = 0;
                            for (int v = 0; v < 8; v++)
                            {
                                double Cu = (u == 0 ? 1 / Math.Sqrt(2) : 1);
                                double Cv = (v == 0 ? 1 / Math.Sqrt(2) : 1);
                                double valcos1 = (((2 * x) + 1) * u * Math.PI) / 16;
                                double multcos1 = Math.Cos(valcos1);
                                double valcos2 = (((2 * y) + 1) * v * Math.PI) / 16;
                                double multcos2 = Math.Cos(valcos2);
                                summ2y += srcMatrix[v, u] * Cu * Cv * multcos1 * multcos2;
                            }
                            summ1x += summ2y;
                        }
                        IDCT[y, x] = (int)(mult * summ1x);
                    }
                }
            }

            public void applyQuantY(List<byte> quantMatrix)
            {
                for (int i = 0; i < 64; i++)
                {
                    strip_Y0_matrix[i] /= quantMatrix[i];
                    strip_Y1_matrix[i] /= quantMatrix[i];
                    strip_Y2_matrix[i] /= quantMatrix[i];
                    strip_Y3_matrix[i] /= quantMatrix[i];
                }
            }

            public void applyQuantCbCr(List<byte> quantMatrix)
            {
                for (int i = 0; i < 64; i++)
                {
                    strip_Cb_matrix[i] /= quantMatrix[i];
                    strip_Cr_matrix[i] /= quantMatrix[i];
                }
            }

            public Bitmap_YCbCr.Pixel[,] M00;
            public Bitmap_YCbCr.Pixel[,] M01;
            public Bitmap_YCbCr.Pixel[,] M10;
            public Bitmap_YCbCr.Pixel[,] M11;

            int[,] Y0_matrix = new int[8, 8];
            int[,] Y1_matrix = new int[8, 8];
            int[,] Y2_matrix = new int[8, 8];
            int[,] Y3_matrix = new int[8, 8];

            int[,] Cb_matrix = new int[8, 8];
            int[,] Cr_matrix = new int[8, 8];

            //strips
            public int[] strip_Y0_matrix;
            public int[] strip_Y1_matrix;
            public int[] strip_Y2_matrix;
            public int[] strip_Y3_matrix;

            public int[] strip_Cb_matrix;
            public int[] strip_Cr_matrix;



            int[,] zigZagOrder =
            {
                {  0,  1,  5,  6, 14, 15, 27, 28 },
                {  2,  4,  7, 13, 16, 26, 29, 42 },
                {  3,  8, 12, 17, 25, 30, 41, 43 },
                {  9, 11, 18, 24, 31, 40, 44, 53 },
                { 10, 19, 23, 32, 39, 45, 52, 54 },
                { 20, 22, 33, 38, 46, 51, 55, 60 },
                { 21, 34, 37, 47, 50, 56, 59, 61 },
                { 35, 36, 48, 49, 57, 58, 62, 63 }
            };
        }

        public void diffDC(List<Pack_2H2V> packs)
        {
            int lastYDC = 0;
            int lastCbDC = 0;
            int lastCrDC = 0;
            foreach (var pack in packs)
            {
                int tempDC = 0;
                //Y1
                tempDC = pack.strip_Y0_matrix[0];
                pack.strip_Y0_matrix[0] -= lastYDC;
                lastYDC = tempDC;
                //Y2
                tempDC = pack.strip_Y1_matrix[0];
                pack.strip_Y1_matrix[0] -= lastYDC;
                lastYDC = tempDC;
                //Y3
                tempDC = pack.strip_Y2_matrix[0];
                pack.strip_Y2_matrix[0] -= lastYDC;
                lastYDC = tempDC;
                //Y4
                tempDC = pack.strip_Y3_matrix[0];
                pack.strip_Y3_matrix[0] -= lastYDC;
                lastYDC = tempDC;
                //Cb
                tempDC = pack.strip_Cb_matrix[0];
                pack.strip_Cb_matrix[0] -= lastCbDC;
                lastCbDC = tempDC;
                //Cr
                tempDC = pack.strip_Cr_matrix[0];
                pack.strip_Cr_matrix[0] -= lastCrDC;
                lastCrDC = tempDC;
            }
        }

        private void timer_UI_update_Tick(object sender, EventArgs e)
        {
            //:::::HIDE TAB
            //INPUT CVZ METHOD
            if (radioButton_cvz_input_file.Checked)
            {
                textBox_cvz.ReadOnly = true;
                button_locate_cvz.Enabled = true;
            }
            else if (radioButton_cvz_input_text.Checked)
            {
                textBox_cvz.ReadOnly = false;
                button_locate_cvz.Enabled = false;
                label_message_size.Text = textBox_cvz.TextLength.ToString() + " символов";
                cvzSize = textBox_cvz.TextLength;
                label_message_size.Refresh();
            }
            //INPUT KEY
            int zeros_in_key = 0;
            if (checkBox_use_key.Checked)
            {
                groupBox_input_key.Enabled = true;
                if (radioButton_key_input_file.Checked)
                {
                    textBox_input_key.ReadOnly = true;
                    button_locate_input_key.Enabled = true;
                }
                else if (radioButton_key_input_text.Checked)
                {
                    textBox_input_key.ReadOnly = false;
                    button_locate_input_key.Enabled = false;
                    label_key_unique_symbols.Text = textBox_input_key.TextLength.ToString() + " символов";
                    label_key_unique_symbols.Refresh();
                    //recalc zeros
                    foreach (var symb in textBox_input_key.Text)
                    {
                        for (int i = 0; i < 8; i++) //USING ONLY FIRST BYTE
                        {
                            if (!getBit(symb, 7 - i))
                                zeros_in_key++;
                        }
                    }
                }
                //recalc final capacity
                finalMsgSize = cvzSize + zeros_in_key/8;
                label_final_msg_size.Text = finalMsgSize + " символов";
                label_final_msg_size.Refresh();
            }
            else
            {
                groupBox_input_key.Enabled = false;
                finalMsgSize = cvzSize;
                label_final_msg_size.Text = finalMsgSize + " символов";
                label_final_msg_size.Refresh();
            }
            //:::::::REVEAL TAB
            if (checkBox_stegano_use_key.Checked)
            {
                groupBox_output_key.Enabled = true;
                if (radioButton_output_key_file.Checked)
                {
                    textBox_output_key.ReadOnly = true;
                    button_locate_output_key.Enabled = true;
                }
                else if (radioButton_output_key_text.Checked)
                {
                    textBox_output_key.ReadOnly = false;
                    button_locate_output_key.Enabled = false;
                }
            }
            else
                groupBox_output_key.Enabled = false;
        }

        private void button_locate_input_key_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog1 = new OpenFileDialog();
            openFileDialog1.Filter = "Key source (*.txt)|*.txt";

            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                using (var file = openFileDialog1.OpenFile())
                {
                    BinaryReader binaryReader = new BinaryReader(file);
                    if (file != null)
                    {
                        input_key = binaryReader.ReadBytes((int)binaryReader.BaseStream.Length);
                        textBox_input_key_path.Text = openFileDialog1.FileName;
                        textBox_input_key_path.Refresh();
                        file.Dispose();
                        file.Close();
                    }
                }
            }
            label_key_unique_symbols.Text = input_key.Length.ToString() + " байт";
            label_key_unique_symbols.Refresh();
        }

        private void button_locate_output_key_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog1 = new OpenFileDialog();
            openFileDialog1.Filter = "Key source (*.txt)|*.txt";

            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                using (var file = openFileDialog1.OpenFile())
                {
                    BinaryReader binaryReader = new BinaryReader(file);
                    if (file != null)
                    {
                        output_key = binaryReader.ReadBytes((int)binaryReader.BaseStream.Length);
                        textBox_output_key_path.Text = openFileDialog1.FileName;
                        textBox_output_key_path.Refresh();
                        file.Dispose();
                        file.Close();
                    }
                }
            }
        }

        private void button_settings_default_Click(object sender, EventArgs e)
        {
            checkBox_use_Y.Checked = true;
            checkBox_use_Cb.Checked = true;
            checkBox_use_Cr.Checked = true;
            textBox_setting_N.Text = "100";
            textBox_C1_x.Text = "6";
            textBox_C1_y.Text = "3";
            textBox_C2_x.Text = "3";
            textBox_C2_y.Text = "5";
        }
    }
}
