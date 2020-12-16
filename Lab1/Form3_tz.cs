﻿using System;
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

        byte[] container;
        byte[] cvz;
        byte[] steganoContainer;
        byte[] out_cvz;
        int container_capacity;
        int cvz_length;

        const int N = 0;
        readonly int[] C1 = { 3, 6 };
        readonly int C1_ind = 30;
        readonly int[] C2 = { 5, 3 };
        readonly int C2_ind = 23;

        public Form3_tz()
        {
            InitializeComponent();
            this.FormBorderStyle = FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.CenterToScreen();
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

        private void button1_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog1 = new OpenFileDialog();
            openFileDialog1.Filter = "Jpg image (*.jpg)|*.jpg";

            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                using (var file = openFileDialog1.OpenFile())
                {
                    BinaryReader binaryReader = new BinaryReader(file);
                    if (file != null)
                    {
                        container = binaryReader.ReadBytes((int)binaryReader.BaseStream.Length);
                        textBox_container_path.Text = openFileDialog1.FileName;
                        textBox_container_path.Refresh();
                        file.Dispose();
                        file.Close();
                    }
                }
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog1 = new OpenFileDialog();
            openFileDialog1.Filter = "Text file (*.txt)|*.txt";

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
        }

        private void button3_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog1 = new OpenFileDialog();
            openFileDialog1.Filter = "Jpg image (*.jpg)|*.jpg";

            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                using (var file = openFileDialog1.OpenFile())
                {
                    BinaryReader binaryReader = new BinaryReader(file);
                    if (file != null)
                    {
                        steganoContainer = binaryReader.ReadBytes((int)binaryReader.BaseStream.Length);
                        textBox_stegano_path.Text = openFileDialog1.FileName;
                        textBox_stegano_path.Refresh();
                        file.Dispose();
                        file.Close();
                    }
                }
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            SaveFileDialog saveFileDialog1 = new SaveFileDialog();

            saveFileDialog1.FileName = "SteganoContainer.jpg";
            saveFileDialog1.Filter = "Jpg image (*.jpg)|*.jpg";
            saveFileDialog1.FilterIndex = 2;
            saveFileDialog1.RestoreDirectory = true;

            if (saveFileDialog1.ShowDialog() == DialogResult.OK)
            {
                using (FileStream file = new FileStream(saveFileDialog1.FileName, FileMode.Create))
                {
                    file.Write(out_cvz, 0, out_cvz.Length);
                }
            }
        }
        private int getBit(int code, int pointer)
        {
            int pointerBit = 0, temp = 0;
            pointerBit = (int)Math.Pow(2, pointer);
            temp = code ^ pointerBit;
            if (temp < code)
                return 1;
            else
                return 0;
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

        private void button5_Click(object sender, EventArgs e)
        {
            byte p1 = 0;
            byte p2 = 0;
            List<int> huff_tables_types = new List<int>();
            List<int> huff_tables_inds = new List<int>();
            //Amounts of huffman codes of various lengths in bits
            List<int[]> huff_tables_codeLens = new List<int[]>();
            //Huffman codes - nodes of tree
            List<int[]> huff_tables_codes = new List<int[]>(); //component of decode
            //Decoded Huffman codes in binary from tree
            //List<int[]> huff_tables_codes_binary = new List<int[]>(); 
            List<List<int>> huff_tables_codes_binary = new List<List<int>>(); //target to decode
            //quantization matrixes
            List<int> quant_matrix_ids = new List<int>();
            List<List<int>> quant_matrixes = new List<List<int>>();
            //picture matrixes
            List<List<int>> picture_matrixes = new List<List<int>>();
            int container_pointer = 0;
            //read FFDB
            while (true)
            {
                while (!(p1 == 0xFF && p2 == 0xDB))
                {
                    p1 = container[container_pointer];
                    p2 = container[container_pointer + 1];
                    container_pointer++;
                    if (p1 == 0xFF && p2 == 0xC4)
                    {
                        //container_pointer++;
                        goto FFC4;
                    }
                }
                container_pointer++;
                p1 = 0;
                p2 = 0;
                //read quant table
                //read table len
                int block_len = 0;
                block_len = container[container_pointer];
                block_len <<= 8;
                container_pointer++;
                block_len += container[container_pointer];
                //read type + index
                container_pointer++;
                byte temp_byte = container[container_pointer];
                int temp_type = temp_byte & 0xF0;
                temp_type >>= 4;
                //if (temp_type == 1) //2 bytes per value
                //    throw shit;
                quant_matrix_ids.Add(temp_byte & 0x0F);
                container_pointer++;
                quant_matrixes.Add(new List<int>());
                quant_matrixes[quant_matrixes.Count - 1].Add(container_pointer + 23); // no need to take a full matrix
                quant_matrixes[quant_matrixes.Count - 1].Add(container_pointer + 30); // these two needed for hide
            }
        FFC4:
            //read FFC4
            int huff_table_ind = 0;
            
            while (true)
            {
                while (!(p1 == 0xFF && p2 == 0xC4))
                {
                    p1 = container[container_pointer];
                    p2 = container[container_pointer+1];
                    container_pointer++;
                    if (p1 == 0xFF && p2 == 0xDA)
                    {
                        container_pointer++;
                        goto FFDA;
                    }
                }
                container_pointer++;
                p1 = 0;
                p2 = 0;
                //read block
                //read block len
                int block_len = 0;
                block_len = container[container_pointer];
                block_len <<= 8;
                container_pointer++;
                block_len += container[container_pointer];
                //read table type and ind
                container_pointer++; //type and index byte
                byte temp_byte = container[container_pointer];
                int temp_type = temp_byte & 0xF0;
                temp_type >>= 4;
                huff_tables_types.Add(temp_type);
                huff_tables_inds.Add(temp_byte & 0x0F);
                //read code lens
                container_pointer++; //start of code lens
                huff_tables_codeLens.Add(new int[16]);
                for (int i = 0; i < 16; i++)
                {
                    (huff_tables_codeLens[huff_tables_codeLens.Count-1])[i] = container[container_pointer];
                    container_pointer++;
                }
                //int one_byte_vals = 0;
                ////count
                //for (int i = 0; i < 8; i++)
                //{
                //    one_byte_vals += (huff_tables_codeLens[huff_tables_codeLens.Count - 1])[i];
                //}
                //int two_byte_vals = 0;
                ////count
                //for (int i = 8; i < 16; i++)
                //{
                //    two_byte_vals += (huff_tables_codeLens[huff_tables_codeLens.Count - 1])[i];
                //}
                ////read codes
                //List<int> temp_code_vals = new List<int>();
                //for (int i = 0; i < one_byte_vals; i++)
                //{
                //    temp_code_vals.Add(container[container_pointer]);
                //    container_pointer++;
                //}
                //for (int i = 0; i < two_byte_vals; i++)
                //{
                //    int tempVal = container[container_pointer];
                //    tempVal <<= 8;
                //    container_pointer++;
                //    tempVal += container[container_pointer];
                //    container_pointer++;
                //    temp_code_vals.Add(tempVal);
                //}
                //huff_tables_codes.Add(temp_code_vals.ToArray());

                huff_tables_codes.Add(new int[block_len - 19]);
                for (int i = 0; i < block_len-19; i++)
                {
                    (huff_tables_codes[huff_tables_codes.Count - 1])[i] = container[container_pointer];
                    container_pointer++;
                }
                huff_table_ind++;
            }
        FFDA:
            if (huff_table_ind != 4)
                throw new Exception("not working with not 4-table jpeg");
            //read len
            /*
             * read
             */
            container_pointer += 2;
            //read channels
            byte channels = container[container_pointer];
            //if (channels != 3)
            //    throw shit;
            byte[] channels_table_match = new byte[3];
            container_pointer++;
            //read matches
            // ->01 xx 02 xx 03 xx
            container_pointer++; //channel 01
            channels_table_match[0] = container[container_pointer];
            container_pointer+=2; //channel 02
            channels_table_match[1] = container[container_pointer];
            container_pointer+=2; //channel 03
            channels_table_match[2] = container[container_pointer];
            container_pointer++;

            List<byte> serviceFFDAbytes = new List<byte>();
            serviceFFDAbytes.Add(container[container_pointer]); //1b
            container_pointer++;
            serviceFFDAbytes.Add(container[container_pointer]); //2b
            container_pointer++;
            serviceFFDAbytes.Add(container[container_pointer]); //3b
            container_pointer++;

            for (int i = 0; i < huff_tables_codes.Count; i++)
            {
                huff_tables_codes_binary.Add(new List<int>());
                int code = 2;
                for (int j = 0; j < huff_tables_codeLens[i].Length; j++)
                {
                    for (int k = 0; k < (huff_tables_codeLens[i])[j]; k++)
                    {
                        (huff_tables_codes_binary[huff_tables_codes_binary.Count - 1]).Add(code);
                        code++;
                    }
                    code <<= 1;
                }
            }

            for (int i = 0; i < huff_tables_codes_binary.Count; i++)
            {
                Console.WriteLine("table #" + i + " TYPE: " + (huff_tables_types[i]==0?"DC":"AC") + "-" + huff_tables_inds[i]);
                for (int j = 0; j < huff_tables_codes_binary[i].Count; j++)
                {
                    Console.Write("VALUE---> 0x" + (huff_tables_codes[i][j]>0x0F?"":"0"));
                    Console.Write(huff_tables_codes[i][j].ToString("X"));
                    Console.WriteLine(" + CODE---> " + ToBinaryString((uint)huff_tables_codes_binary[i][j]));
                }
            }

            //filling matrixes
            //match channels--tables
            int[] matched_dc = new int[6]; //contains table inds YYYYCbCr
            int[] matched_ac = new int[6]; //contains table inds YYYYCbCr

            //int[] matched_dc = new int[3]; //contains table inds YCbCr
            //int[] matched_ac = new int[3]; //contains table inds YCbCr
            //channel Y
            //DC search
            int target_index = channels_table_match[0];
            target_index >>= 4;
            int found_dc_index = -1;
            int found_ac_index = -1;
            for (int i = 0; i < huff_tables_codes.Count; i++)
            {
                if (huff_tables_types[i] == 0 && huff_tables_inds[i] == target_index) //DC table with target index
                {
                    found_dc_index = i;
                    break;
                }
            }
            target_index = channels_table_match[0];
            target_index &= 0x0F;
            //AC search
            for (int i = 0; i < huff_tables_codes.Count; i++)
            {
                if (huff_tables_types[i] == 1 && huff_tables_inds[i] == target_index) //AC table with target index
                {
                    found_ac_index = i;
                    break;
                }
            }
            //DC
            matched_dc[0] = found_dc_index;
            matched_dc[1] = found_dc_index;
            matched_dc[2] = found_dc_index;
            matched_dc[3] = found_dc_index;
            //AC
            matched_ac[0] = found_ac_index;
            matched_ac[1] = found_ac_index;
            matched_ac[2] = found_ac_index;
            matched_ac[3] = found_ac_index;
            //channel Cb
            found_dc_index = -1;
            found_ac_index = -1;
            target_index = channels_table_match[1];
            target_index >>= 4;
            //DC Search
            for (int i = 0; i < huff_tables_codes.Count; i++)
            {
                if (huff_tables_types[i] == 0 && huff_tables_inds[i] == target_index) //DC table with target index
                {
                    found_dc_index = i;
                    break;
                }
            }
            target_index = channels_table_match[1];
            target_index &= 0x0F;
            //AC search
            for (int i = 0; i < huff_tables_codes.Count; i++)
            {
                if (huff_tables_types[i] == 1 && huff_tables_inds[i] == target_index) //AC table with target index
                {
                    found_ac_index = i;
                    break;
                }
            }
            //DC
            matched_dc[4] = found_dc_index;
            //matched_dc[1] = found_dc_index;
            //AC
            matched_ac[4] = found_ac_index;
            //matched_ac[1] = found_ac_index;
            //channel Cr
            found_dc_index = -1;
            found_ac_index = -1;
            target_index = channels_table_match[2];
            target_index >>= 4;
            for (int i = 0; i < huff_tables_codes.Count; i++)
            {
                if (huff_tables_types[i] == 0 && huff_tables_inds[i] == target_index) //DC table with target index
                {
                    found_dc_index = i;
                    break;
                }
            }
            target_index = channels_table_match[2];
            target_index &= 0x0F;
            //AC search
            for (int i = 0; i < huff_tables_codes.Count; i++)
            {
                if (huff_tables_types[i] == 1 && huff_tables_inds[i] == target_index) //AC table with target index
                {
                    found_ac_index = i;
                    break;
                }
            }
            //DC
            matched_dc[5] = found_dc_index;
            //matched_dc[2] = found_dc_index;
            //AC
            matched_ac[5] = found_ac_index;
            //matched_ac[2] = found_ac_index;

            int current_table_ind = 0;
            bool use_dc = true;

            int pointerDataStart = container_pointer;
            int bits_counter = 0;
            int buffer = 2;
            List<int> tempMatrix = new List<int>();

            while (true)
            {
                if (container[pointerDataStart + (bits_counter / 8)] == 0xFF)
                {
                    if (container[pointerDataStart + (bits_counter / 8) + 1] == 0xD9)
                        break;
                }


                buffer += getBit(container[pointerDataStart + (bits_counter / 8)], 7 - (bits_counter % 8));
                var TRACKER_buffer = ToBinaryString((uint)buffer); //DEBUG=====================================================
                bits_counter++;

                int value = 0;
                if (use_dc)
                {
                    int[] values_list = huff_tables_codes[matched_dc[current_table_ind % matched_dc.Length]];
                    List<int> codes_list = huff_tables_codes_binary[matched_dc[current_table_ind % matched_ac.Length]];
                    value = checkCode(buffer, values_list, codes_list);
                }
                else //AC
                {
                    int[] values_list = huff_tables_codes[matched_ac[current_table_ind % 6]];
                    List<int> codes_list = huff_tables_codes_binary[matched_ac[current_table_ind % 6]];
                    value = checkCode(buffer, values_list, codes_list);
                }

                if (value == -1)
                {
                    buffer <<= 1;
                }
                else
                {
                    if (use_dc) //DC
                    {
                        use_dc = false;
                        if (value == 0x00) // Zero-DC-Coeff
                        {
                            tempMatrix.Add(0);
                        }
                        else
                        {
                            bool negative_coeff = false;
                            if (getBit(container[pointerDataStart + (bits_counter / 8)], 7 - (bits_counter % 8)) == 0)
                                negative_coeff = true;
                            int coeff_buffer = 0;

                            int end_of_coeff = bits_counter + value;
                            for (int i = bits_counter; i < end_of_coeff; i++)
                            {
                                coeff_buffer <<= 1;
                                coeff_buffer += getBit(container[pointerDataStart + (bits_counter / 8)], 7 - (bits_counter % 8));
                                var TRACKER_coeff_buffer = ToBinaryString((uint)coeff_buffer); //DEBUG=====================================================
                                bits_counter++;
                            }

                            int read_coeff = 0;
                            if (negative_coeff)
                            {
                                read_coeff = coeff_buffer + 1 - (int)Math.Pow(2, value);
                            }
                            else
                            {
                                read_coeff = coeff_buffer;
                            }
                            tempMatrix.Add(read_coeff);
                        }
                    }
                    else //AC
                    {
                        if (value == 0x00) //STOP-CODE
                        {
                            //fill until size is 64
                            int zerosToFill = 64 - tempMatrix.Count;
                            for (int i = 0; i < zerosToFill; i++)
                                tempMatrix.Add(0);
                            //END MATRIX
                            picture_matrixes.Add(new List<int>(tempMatrix.ToArray()));
                            tempMatrix.Clear();
                            use_dc = true;
                            current_table_ind++;
                        }
                        else
                        {
                            int zerosBefore = value >> 4;
                            int coeff_bits_len = (byte)value & 0x0F;

                            if (coeff_bits_len == 0)
                            {
                                for (int i = 0; i < 16; i++)
                                    tempMatrix.Add(0);
                            }
                            else
                            {
                                for (int i = 0; i < zerosBefore; i++)
                                    tempMatrix.Add(0);

                                bool negative_coeff = false;
                                if (getBit(container[pointerDataStart + (bits_counter / 8)], 7 - (bits_counter % 8)) == 0)
                                    negative_coeff = true;

                                int coeff_buffer = 0;

                                int end_of_coeff = bits_counter + coeff_bits_len;
                                for (int i = bits_counter; i < end_of_coeff; i++)
                                {
                                    coeff_buffer <<= 1;
                                    coeff_buffer += getBit(container[pointerDataStart + (bits_counter / 8)], 7 - (bits_counter % 8));
                                    var TRACKER_coeff_buffer = ToBinaryString((uint)coeff_buffer); //DEBUG=====================================================
                                    bits_counter++;
                                }

                                int read_coeff = 0;
                                if (negative_coeff)
                                {
                                    read_coeff = coeff_buffer + 1 - (int)Math.Pow(2, coeff_bits_len);
                                }
                                else
                                {
                                    read_coeff = coeff_buffer;
                                }
                                tempMatrix.Add(read_coeff);

                                if (tempMatrix.Count == 64) //END MATRIX
                                {
                                    picture_matrixes.Add(new List<int>(tempMatrix.ToArray()));
                                    tempMatrix.Clear();
                                    use_dc = true;
                                    current_table_ind++;
                                }
                            }
                        }
                    }
                    buffer = 2;
                }
            }

            for (int i = 0; i < picture_matrixes.Count; i++)
            {
                Console.WriteLine("matrix #" + i);
                for (int j = 0; j < picture_matrixes[i].Count; j++)
                {
                    Console.WriteLine(picture_matrixes[i][j]);
                }
            }

            //cvz max len define
            int cvz_max_len = picture_matrixes.Count/8;

            //quant
            //for (int i = 0; i < picture_matrixes.Count; i++)
            //{
            //    if ((i+1)%5 == 0 || (i+1)%6 == 0) //Cb Cr
            //    {
            //        (picture_matrixes[i])[C1_ind] *= (quant_matrixes[1])[1];
            //        (picture_matrixes[i])[C2_ind] *= (quant_matrixes[1])[1];
            //    }
            //    else //Y
            //    {
            //        (picture_matrixes[i])[C1_ind] *= (quant_matrixes[0])[1];
            //        (picture_matrixes[i])[C2_ind] *= (quant_matrixes[0])[1];
            //    }
            //}

            //hide
            //auto
            //int matrix_counter = 0;
            //for (int i = 0; i < cvz.Length; i++)
            //{
            //    for (int j = 0; j < 8; j++)
            //    {
            //        if (getBit(cvz[i], 7 - j) == 0)
            //        {
            //            (picture_matrixes[matrix_counter])[C1_ind] += (N / 2) + 5;
            //            (picture_matrixes[matrix_counter])[C2_ind] -= ((N / 2) + 5);
            //        }
            //        else
            //        {
            //            (picture_matrixes[matrix_counter])[C1_ind] -= (N / 2) + 5;
            //            (picture_matrixes[matrix_counter])[C2_ind] += ((N / 2) + 5);
            //        }
            //        matrix_counter++;
            //    }
            //}

            //MANUAL
            //(picture_matrixes[0])[C1_ind] -= 1; // 1
            //(picture_matrixes[0])[C2_ind] += 1;
            //(picture_matrixes[1])[C1_ind] += 1; // 0
            //(picture_matrixes[1])[C2_ind] -= 1;
            //(picture_matrixes[2])[C1_ind] -= 1; // 1
            //(picture_matrixes[2])[C2_ind] += 1;

            //back quant
            //for (int i = 0; i < picture_matrixes.Count; i++)
            //{
            //    if ((i + 1) % 5 == 0 || (i + 1) % 6 == 0) //Cb Cr
            //    {
            //        (picture_matrixes[i])[C1_ind] /= (quant_matrixes[1])[1];
            //        (picture_matrixes[i])[C2_ind] /= (quant_matrixes[1])[1];
            //    }
            //    else //Y
            //    {
            //        (picture_matrixes[i])[C1_ind] /= (quant_matrixes[0])[1];
            //        (picture_matrixes[i])[C2_ind] /= (quant_matrixes[0])[1];
            //    }
            //}

            //DC reveal and hide not needed

            //picture_matrixes[4][0] = 3;

            byte[] changedBlock = packWithHuffman(picture_matrixes, serviceFFDAbytes, 3);

            //copy headers before FFC4
            List<byte> newImg = new List<byte>();
            container_pointer = 0;
            while (!(p1 == 0xFF && p2 == 0xC4))
            {
                newImg.Add(container[container_pointer]);
                p1 = container[container_pointer];
                p2 = container[container_pointer + 1];
                container_pointer++;
            }
            newImg.RemoveAt(newImg.Count-1);

            newImg.AddRange(changedBlock);

            //temporary save
            File.WriteAllBytes("C:/Users/BlackCultist/Desktop/cursed.jpg", newImg.ToArray());

        }

        private unsafe byte[] packWithHuffman(List<List<int>> matrixes, List<byte> serviceFFDA, int channels) //returns from first FFC4 to FFD9
        {
            try
            {
                JpegShell jpeg = new JpegShell(matrixes, serviceFFDA, channels);
                return jpeg.getData();
            }
            catch
            {
                return null;
            }
        }

        //private unsafe void doStuff()
        //{
        //    //byte[] input = { 0, 1 };
        //    IntPtr Ptr_input = Marshal.AllocHGlobal(2 * sizeof(byte));
        //    byte* input = (byte*)Ptr_input;
        //    input[0] = 0;
        //    input[1] = 1;

        //    IntPtr codesLen = Marshal.AllocHGlobal(sizeof(int));
        //    IntPtr valuesLen = Marshal.AllocHGlobal(sizeof(int));

        //    IntPtr codes = Marshal.AllocHGlobal(sizeof(int)); //this is address of array[uint16_t]
        //    IntPtr values = Marshal.AllocHGlobal(sizeof(int)); //this is address of array[char]

        //    Encode_getCodes_Wrapper(Ptr_input.ToInt32(), 2, codes.ToInt32(), codesLen.ToInt32(), values.ToInt32(), valuesLen.ToInt32());
        //    int codesLenInt = Marshal.ReadInt32(codesLen);
        //    int valuesLenInt = Marshal.ReadInt32(valuesLen);
        //    Console.Write("CodesLen: ");
        //    Console.WriteLine(codesLenInt);
        //    Console.Write("ValuesLen: ");
        //    Console.WriteLine(valuesLenInt);
        //    ulong* codesArr = (ulong*)Marshal.ReadInt32(codes);
        //    byte* valuesArr = (byte*)Marshal.ReadInt32(values);

        //    Console.WriteLine("Code table:");
        //    Console.WriteLine("__|Value|------------|Code|__");
        //    for (int i = 0; i < codesLenInt; i++)
        //    {
        //        Console.Write("|    ");
        //        Console.Write(valuesArr[i]);
        //        Console.Write("  >----------->  ");
        //        Console.Write(codesArr[i]);
        //        Console.WriteLine("    |");
        //    }
        //    Console.WriteLine("-----------------------------");
        //}

        private int checkCode(int binaryCode, int[] codes_list, List<int> codes_binary_list)
        {
            for (int i = 0; i < codes_binary_list.Count; i++)
            {
                if (binaryCode == codes_binary_list[i])
                    return codes_list[i];
            }
            return -1;
        }
        public static string ToBinaryString(uint num)
        {
            return Convert.ToString(num, 2).PadLeft(16, '0');
        }

        
    }

    public class ChannelMode
    {
        //channel 1
        public bool is_Y = false;
        public int Y_DC_huffman_ind = 0;
        public int Y_AC_huffman_ind = 0;
        //channel 2
        public bool is_Cb = false;
        public int Cb_DC_huffman_ind = 0;
        public int Cb_AC_huffman_ind = 0;
        //channel 3
        public bool is_Cr = false;
        public int Cr_DC_huffman_ind = 0;
        public int Cr_AC_huffman_ind = 0;
    }

    public class JpegShell
    {
        public unsafe JpegShell(List<List<int>> matrixes, List<byte> serviceFFDA, int channels)
        {
            structMemHandler.serviceFFDA = serviceFFDA;
            structMemHandler.channels = channels;

            structMemHandler.matrixes_amount = matrixes.Count;
            this.HuffDLL = new HuffmanDLLHandler(rawMemHandler);

            JpegBuilder builder = new JpegBuilder(rawMemHandler, structMemHandler);
            builder.init(matrixes);
            this.HuffDLL.Encode();
            rawMemHandler.init_output();
            jpeg_changed_data.AddRange(builder.build());

        }

        public byte[] getData() { return jpeg_changed_data.ToArray(); }


        private HuffmanMemoryHandler rawMemHandler = new HuffmanMemoryHandler();
        private JpegMemoryHandler structMemHandler = new JpegMemoryHandler();
        private HuffmanDLLHandler HuffDLL = null;
        List<byte> jpeg_changed_data = new List<byte>();
    }

    public class HuffmanDLLHandler
    {
        [
            DllImport("C:\\Users\\BlackCultist\\source\\repos\\HaffmanPack\\Debug\\Huffman_RBClib.dll", 
            CharSet = CharSet.Unicode, 
            CallingConvention = CallingConvention.Cdecl)
        ]
        private static extern bool Encode_getCodes_Freq_Wrapper(int ADDR_input_stream, int input_stream_len,
                                                                int ADDR_OF_ARRAY_huffman_codes, int ADDR_huffman_codes_len,
                                                                int ADDR_OF_ARRAY_values, int ADDR_values_len,
                                                                int ADDR_OF_ARRAY_freqs, int ADDR_freqs_len);
        [
            DllImport("C:\\Users\\BlackCultist\\source\\repos\\HaffmanPack\\Debug\\Huffman_RBClib.dll",
            CharSet = CharSet.Unicode,
            CallingConvention = CallingConvention.Cdecl)
        ]
        private static extern bool Encode_getCodes_Wrapper(int ADDR_input_stream, int input_stream_len,
                                                                int ADDR_OF_ARRAY_huffman_codes, int ADDR_huffman_codes_len,
                                                                int ADDR_OF_ARRAY_values, int ADDR_values_len);
        [
            DllImport("C:\\Users\\BlackCultist\\source\\repos\\HaffmanPack\\Debug\\Huffman_RBClib.dll", 
            CharSet = CharSet.Unicode, 
            CallingConvention = CallingConvention.Cdecl)
        ]
        private static extern bool Free_Encode_getCodes_Wrapper(int ADDR_OF_ARRAY_huffman_codes, int ADDR_OF_ARRAY_values);
        public HuffmanDLLHandler(HuffmanMemoryHandler memHandler_) 
        {
            this.memHandler = memHandler_;
        }

        public bool Encode()
        {

            if (!Encode_getCodes_Freq_Wrapper(memHandler.OPtr_YDc_codelen.ToInt32(), memHandler.OPtr_YDc_codelen_Length,
                                        memHandler.codes_YDc.ToInt32(), memHandler.codesLen_YDc.ToInt32(),
                                        memHandler.values_YDc.ToInt32(), memHandler.valuesLen_YDc.ToInt32(),
                                        memHandler.freqs_YDc.ToInt32(), memHandler.freqsLen_YDc.ToInt32()))
                return false;
            if (!Encode_getCodes_Freq_Wrapper(memHandler.OPtr_YAc_codelen.ToInt32(), memHandler.OPtr_YAc_codelen_Length,
                                        memHandler.codes_YAc.ToInt32(), memHandler.codesLen_YAc.ToInt32(),
                                        memHandler.values_YAc.ToInt32(), memHandler.valuesLen_YAc.ToInt32(),
                                        memHandler.freqs_YAc.ToInt32(), memHandler.freqsLen_YAc.ToInt32()))
                return false;
            if (!Encode_getCodes_Freq_Wrapper(memHandler.OPtr_CbCrDc_codelen.ToInt32(), memHandler.OPtr_CbCrDc_codelen_Length,
                                        memHandler.codes_CbCrDc.ToInt32(), memHandler.codesLen_CbCrDc.ToInt32(),
                                        memHandler.values_CbCrDc.ToInt32(), memHandler.valuesLen_CbCrDc.ToInt32(),
                                        memHandler.freqs_CbCrDc.ToInt32(), memHandler.freqsLen_CbCrDc.ToInt32()))
                return false;
            if (!Encode_getCodes_Freq_Wrapper(memHandler.OPtr_CbCrAc_codelen.ToInt32(), memHandler.OPtr_CbCrAc_codelen_Length,
                                        memHandler.codes_CbCrAc.ToInt32(), memHandler.codesLen_CbCrAc.ToInt32(),
                                        memHandler.values_CbCrAc.ToInt32(), memHandler.valuesLen_CbCrAc.ToInt32(),
                                        memHandler.freqs_CbCrAc.ToInt32(), memHandler.freqsLen_CbCrAc.ToInt32()))
                return false;
            //if (!Encode_getCodes_Wrapper(memHandler.OPtr_YDc_codelen.ToInt32(), memHandler.OPtr_YDc_codelen_Length,
            //                            memHandler.codes_YDc.ToInt32(), memHandler.codesLen_YDc.ToInt32(),
            //                            memHandler.values_YDc.ToInt32(), memHandler.valuesLen_YDc.ToInt32()))
            //    return false;
            //if (!Encode_getCodes_Wrapper(memHandler.OPtr_YAc_codelen.ToInt32(), memHandler.OPtr_YAc_codelen_Length,
            //                            memHandler.codes_YAc.ToInt32(), memHandler.codesLen_YAc.ToInt32(),
            //                            memHandler.values_YAc.ToInt32(), memHandler.valuesLen_YAc.ToInt32()))
            //    return false;
            //if (!Encode_getCodes_Wrapper(memHandler.OPtr_CbCrDc_codelen.ToInt32(), memHandler.OPtr_CbCrDc_codelen_Length,
            //                            memHandler.codes_CbCrDc.ToInt32(), memHandler.codesLen_CbCrDc.ToInt32(),
            //                            memHandler.values_CbCrDc.ToInt32(), memHandler.valuesLen_CbCrDc.ToInt32()))
            //    return false;
            //if (!Encode_getCodes_Wrapper(memHandler.OPtr_CbCrAc_codelen.ToInt32(), memHandler.OPtr_CbCrAc_codelen_Length,
            //                            memHandler.codes_CbCrAc.ToInt32(), memHandler.codesLen_CbCrAc.ToInt32(),
            //                            memHandler.values_CbCrAc.ToInt32(), memHandler.valuesLen_CbCrAc.ToInt32()))
            //    return false;
            return true;
        }

        public bool FreeHuffmanMem()
        {
            /*
            * Free memory
            */
            //free ints
            //Marshal.FreeHGlobal(codesLen_YDc);
            //Marshal.FreeHGlobal(valuesLen_YDc);
            //Marshal.FreeHGlobal(codesLen_YAc);
            //Marshal.FreeHGlobal(valuesLen_YAc);
            //Marshal.FreeHGlobal(codesLen_CbCrDc);
            //Marshal.FreeHGlobal(valuesLen_CbCrDc);
            //Marshal.FreeHGlobal(codesLen_CbCrAc);
            //Marshal.FreeHGlobal(valuesLen_CbCrAc);
            ////free pointers
            //Free_Encode_getCodes_Wrapper(Marshal.ReadInt32(codes_YDc), Marshal.ReadInt32(values_YDc));
            //Free_Encode_getCodes_Wrapper(Marshal.ReadInt32(codes_YAc), Marshal.ReadInt32(values_YAc));
            //Free_Encode_getCodes_Wrapper(codes_CbCrDc.ToInt32(), values_CbCrDc.ToInt32()); //fix
            //Free_Encode_getCodes_Wrapper(codes_CbCrAc.ToInt32(), values_CbCrAc.ToInt32()); //fix
            //if (!Free_Encode_getCodes_Wrapper(int ADDR_OF_ARRAY_huffman_codes, int ADDR_OF_ARRAY_values))
            //    return false;
            //if (!Free_Encode_getCodes_Wrapper(int ADDR_OF_ARRAY_huffman_codes, int ADDR_OF_ARRAY_values))
            //    return false;
            //if (!Free_Encode_getCodes_Wrapper(int ADDR_OF_ARRAY_huffman_codes, int ADDR_OF_ARRAY_values))
            //    return false;
            //if (!Free_Encode_getCodes_Wrapper(int ADDR_OF_ARRAY_huffman_codes, int ADDR_OF_ARRAY_values))
            //    return false;
            return true;
        }

        HuffmanMemoryHandler memHandler = null;
    }

    public class JpegBuilder
    {
        public JpegBuilder(HuffmanMemoryHandler huffmanMemoryHandler_, JpegMemoryHandler memoryHandler_) 
        {
            this.huffmanMemoryHandler = huffmanMemoryHandler_;
            this.memoryHandler = memoryHandler_;
        }

        public void init(List<List<int>> matrixes)
        {
            make_coeffs(matrixes);
            make_codeLens(matrixes);
            huffmanMemoryHandler.init_input(memoryHandler.Y_DC_CodeLens, memoryHandler.Y_AC_CodeLens, memoryHandler.CbCr_DC_CodeLens, memoryHandler.CbCr_AC_CodeLens);
        }

        public byte[] build()
        {
            this.memoryHandler.CodesTable.Init(huffmanMemoryHandler);
            huffmanMemoryHandler.FreeInputStreams();
            make_codeLensAmount();
            bubbleSortTables();

            convertTreeToJPEG_style();

            byte[] FFC4 = packFFC4Headers();
            byte[] FFDA = packFFDAHeader();

            orderTables();
            reCalcCodeLens();
            printCodeTable();
            byte[] data = packFFDAData();

            byte[] endBytes = { 0xFF, 0xD9 };

            List<byte> concat = new List<byte>();
            concat.AddRange(FFC4);
            concat.AddRange(FFDA);
            concat.AddRange(data);
            concat.AddRange(endBytes);
            return concat.ToArray();
        }

        private void printCodeTable()
        {
            Console.WriteLine("===========GENERATED CODES===========");
            Console.WriteLine("Y-channel. DC-table.");
            for (int i = 0; i < memoryHandler.CodesTable.Y_DC_Table.Count; i++)
            {
                Console.Write("VALUE---> 0x" + (memoryHandler.CodesTable.Y_DC_Table[i].value > 0x0F ? "" : "0"));
                Console.Write(memoryHandler.CodesTable.Y_DC_Table[i].value.ToString("X"));
                Console.Write(" + CODE---> " + ToBinaryString((uint)memoryHandler.CodesTable.Y_DC_Table[i].code));
                Console.WriteLine(" F=" + memoryHandler.CodesTable.Y_DC_Table[i].frequency + 
                                " [ len(V)=" + memoryHandler.CodesTable.Y_DC_Table[i].value_length_bits + 
                                "; len(C)=" + memoryHandler.CodesTable.Y_DC_Table[i].code_length_bits + " ]");
            }
            Console.WriteLine("Y-channel. AC-table.");
            for (int i = 0; i < memoryHandler.CodesTable.Y_AC_Table.Count; i++)
            {
                Console.Write("VALUE---> 0x" + (memoryHandler.CodesTable.Y_AC_Table[i].value > 0x0F ? "" : "0"));
                Console.Write(memoryHandler.CodesTable.Y_AC_Table[i].value.ToString("X"));
                Console.Write(" + CODE---> " + ToBinaryString((uint)memoryHandler.CodesTable.Y_AC_Table[i].code));
                Console.WriteLine(" F=" + memoryHandler.CodesTable.Y_AC_Table[i].frequency +
                                " [ len(V)=" + memoryHandler.CodesTable.Y_AC_Table[i].value_length_bits +
                                "; len(C)=" + memoryHandler.CodesTable.Y_AC_Table[i].code_length_bits + " ]");
            }
            Console.WriteLine("CbCr-channel. DC-table.");
            for (int i = 0; i < memoryHandler.CodesTable.CbCr_DC_Table.Count; i++)
            {
                Console.Write("VALUE---> 0x" + (memoryHandler.CodesTable.CbCr_DC_Table[i].value > 0x0F ? "" : "0"));
                Console.Write(memoryHandler.CodesTable.CbCr_DC_Table[i].value.ToString("X"));
                Console.Write(" + CODE---> " + ToBinaryString((uint)memoryHandler.CodesTable.CbCr_DC_Table[i].code));
                Console.WriteLine(" F=" + memoryHandler.CodesTable.CbCr_DC_Table[i].frequency +
                                " [ len(V)=" + memoryHandler.CodesTable.CbCr_DC_Table[i].value_length_bits +
                                "; len(C)=" + memoryHandler.CodesTable.CbCr_DC_Table[i].code_length_bits + " ]");
            }
            Console.WriteLine("Y-channel. DC-table.");
            for (int i = 0; i < memoryHandler.CodesTable.CbCr_AC_Table.Count; i++)
            {
                Console.Write("VALUE---> 0x" + (memoryHandler.CodesTable.CbCr_AC_Table[i].value > 0x0F ? "" : "0"));
                Console.Write(memoryHandler.CodesTable.CbCr_AC_Table[i].value.ToString("X"));
                Console.Write(" + CODE---> " + ToBinaryString((uint)memoryHandler.CodesTable.CbCr_AC_Table[i].code));
                Console.WriteLine(" F=" + memoryHandler.CodesTable.CbCr_AC_Table[i].frequency +
                                " [ len(V)=" + memoryHandler.CodesTable.CbCr_AC_Table[i].value_length_bits +
                                "; len(C)=" + memoryHandler.CodesTable.CbCr_AC_Table[i].code_length_bits + " ]");
            }
            Console.WriteLine("===========GENERATED CODES END===========");
        }
        private static string ToBinaryString(uint num)
        {
            return Convert.ToString(num, 2).PadLeft(16, '0');
        }

        private static string ToBinaryString(byte num)
        {
            return Convert.ToString(num, 2).PadLeft(8, '0');
        }

        private static string ToBinaryString(int num, int numBits)
        {
            return Convert.ToString(num, 2).PadLeft(numBits, '0');
        }

        private void reCalcCodeLens()
        {
            //Y DC
            for (int i = 0; i < memoryHandler.JpegCodes.Y_DC_Table.Count; i++)
                memoryHandler.JpegCodes.Y_DC_Table[i].code_length_bits = binaryLenHuffCodes(memoryHandler.JpegCodes.Y_DC_Table[i].code);
            //Y AC
            for (int i = 0; i < memoryHandler.JpegCodes.Y_AC_Table.Count; i++)
                memoryHandler.JpegCodes.Y_AC_Table[i].code_length_bits = binaryLenHuffCodes(memoryHandler.JpegCodes.Y_AC_Table[i].code);
            //CbCr DC
            for (int i = 0; i < memoryHandler.JpegCodes.CbCr_DC_Table.Count; i++)
                memoryHandler.JpegCodes.CbCr_DC_Table[i].code_length_bits = binaryLenHuffCodes(memoryHandler.JpegCodes.CbCr_DC_Table[i].code);
            //CbCr AC
            for (int i = 0; i < memoryHandler.JpegCodes.CbCr_AC_Table.Count; i++)
                memoryHandler.JpegCodes.CbCr_AC_Table[i].code_length_bits = binaryLenHuffCodes(memoryHandler.JpegCodes.CbCr_AC_Table[i].code);
        }

        private void convertTreeToJPEG_style()
        {
            var new_Y_DC_codes = readHuffmanTree(memoryHandler.Y_DC_Amounts_of_code_lens);
            var new_Y_AC_codes = readHuffmanTree(memoryHandler.Y_AC_Amounts_of_code_lens);
            var new_CbCr_DC_codes = readHuffmanTree(memoryHandler.CbCr_DC_Amounts_of_code_lens);
            var new_CbCr_AC_codes = readHuffmanTree(memoryHandler.CbCr_AC_Amounts_of_code_lens);

            var new_Y_DC_codes_ulong = new_Y_DC_codes.Select(item => (ulong)item).ToArray();
            var new_Y_AC_codes_ulong = new_Y_AC_codes.Select(item => (ulong)item).ToArray();
            var new_CbCr_DC_codes_ulong = new_CbCr_DC_codes.Select(item => (ulong)item).ToArray();
            var new_CbCr_AC_codes_ulong = new_CbCr_AC_codes.Select(item => (ulong)item).ToArray();

            List<ulong[]> newCodes = new List<ulong[]>();
            newCodes.Add(new_Y_DC_codes_ulong);
            newCodes.Add(new_Y_AC_codes_ulong);
            newCodes.Add(new_CbCr_DC_codes_ulong);
            newCodes.Add(new_CbCr_AC_codes_ulong);

            memoryHandler.CodesTable.updateCodes(newCodes);

            //MOVE MOST 1 CODE
            MoveMost1Code();
        }

        private void MoveMost1Code()
        {
            if (getBit((int)memoryHandler.CodesTable.Y_DC_Table[memoryHandler.CodesTable.Y_DC_Table.Count - 1].code, 0) != 0)
            {
                memoryHandler.CodesTable.Y_DC_Table[memoryHandler.CodesTable.Y_DC_Table.Count - 1].code <<= 1;
                memoryHandler.Y_DC_Amounts_of_code_lens[memoryHandler.CodesTable.Y_DC_Table[memoryHandler.CodesTable.Y_DC_Table.Count - 1].code_length_bits - 1]--;
                memoryHandler.CodesTable.Y_DC_Table[memoryHandler.CodesTable.Y_DC_Table.Count - 1].code_length_bits++;
                memoryHandler.Y_DC_Amounts_of_code_lens[memoryHandler.CodesTable.Y_DC_Table[memoryHandler.CodesTable.Y_DC_Table.Count - 1].code_length_bits - 1]++;
            }
            if (getBit((int)memoryHandler.CodesTable.Y_AC_Table[memoryHandler.CodesTable.Y_AC_Table.Count - 1].code, 0) != 0)
            {
                memoryHandler.CodesTable.Y_AC_Table[memoryHandler.CodesTable.Y_AC_Table.Count - 1].code <<= 1;
                memoryHandler.Y_AC_Amounts_of_code_lens[memoryHandler.CodesTable.Y_AC_Table[memoryHandler.CodesTable.Y_AC_Table.Count - 1].code_length_bits - 1]--;
                memoryHandler.CodesTable.Y_AC_Table[memoryHandler.CodesTable.Y_AC_Table.Count - 1].code_length_bits++;
                memoryHandler.Y_AC_Amounts_of_code_lens[memoryHandler.CodesTable.Y_AC_Table[memoryHandler.CodesTable.Y_AC_Table.Count - 1].code_length_bits - 1]++;
            }
            if (getBit((int)memoryHandler.CodesTable.CbCr_DC_Table[memoryHandler.CodesTable.CbCr_DC_Table.Count - 1].code, 0) != 0)
            {
                memoryHandler.CodesTable.CbCr_DC_Table[memoryHandler.CodesTable.CbCr_DC_Table.Count - 1].code <<= 1;
                memoryHandler.CbCr_DC_Amounts_of_code_lens[memoryHandler.CodesTable.CbCr_DC_Table[memoryHandler.CodesTable.CbCr_DC_Table.Count - 1].code_length_bits - 1]--;
                memoryHandler.CodesTable.CbCr_DC_Table[memoryHandler.CodesTable.CbCr_DC_Table.Count - 1].code_length_bits++;
                memoryHandler.CbCr_DC_Amounts_of_code_lens[memoryHandler.CodesTable.CbCr_DC_Table[memoryHandler.CodesTable.CbCr_DC_Table.Count - 1].code_length_bits - 1]++;
            }
            if (getBit((int)memoryHandler.CodesTable.CbCr_AC_Table[memoryHandler.CodesTable.CbCr_AC_Table.Count - 1].code, 0) != 0)
            {
                memoryHandler.CodesTable.CbCr_AC_Table[memoryHandler.CodesTable.CbCr_AC_Table.Count - 1].code <<= 1;
                memoryHandler.CbCr_AC_Amounts_of_code_lens[memoryHandler.CodesTable.CbCr_AC_Table[memoryHandler.CodesTable.CbCr_AC_Table.Count - 1].code_length_bits - 1]--;
                memoryHandler.CodesTable.CbCr_AC_Table[memoryHandler.CodesTable.CbCr_AC_Table.Count - 1].code_length_bits++;
                memoryHandler.CbCr_AC_Amounts_of_code_lens[memoryHandler.CodesTable.CbCr_AC_Table[memoryHandler.CodesTable.CbCr_AC_Table.Count - 1].code_length_bits - 1]++;
            }
        }
        private int[] readHuffmanTree(int[] code_lens_amounts)
        {
            List<int> codes = new List<int>();

            int code = 2;
            for (int j = 0; j < code_lens_amounts.Length; j++)
            {
                for (int k = 0; k < code_lens_amounts[j]; k++)
                {
                    codes.Add(code);
                    code++;
                }
                code <<= 1;
            }
            return codes.ToArray();
        }
        private void make_codeLens(List<List<int>> matrixes)
        {
            memoryHandler.Y_AC_CodeLens.Add(0x00); //Stop-code val
            memoryHandler.CbCr_AC_CodeLens.Add(0x00); //Stop-code val
            for (int i = 0; i < matrixes.Count; i++)
            {
                if ((i + 1) % 5 == 0 || (i + 1) % 6 == 0) //Cb Cr
                {
                    //DC
                    {
                        int current_dc = (matrixes[i])[0];
                        byte dc_binary_len = (byte)binaryLen((ulong)Math.Abs(current_dc));
                        memoryHandler.CbCr_DC_CodeLens.Add(dc_binary_len);
                    }
                    //AC
                    {
                        byte zerosBefore = 0;
                        List<byte> tempBuffer = new List<byte>();
                        for (int j = 1; j < (matrixes[i]).Count; j++)
                        {
                            if ((matrixes[i])[j] == 0) //count zeros before coeff
                            {
                                if (zerosBefore == 0x0F)
                                {
                                    tempBuffer.Add(0xF0);
                                    zerosBefore = 1;
                                }
                                else
                                    zerosBefore++;
                            }
                            else //save val
                            {
                                //load buffer
                                for (int k = 0; k < tempBuffer.Count; k++)
                                {
                                    memoryHandler.CbCr_AC_CodeLens.Add(tempBuffer[k]);
                                }
                                tempBuffer.Clear();
                                //
                                int current_ac = (matrixes[i])[j];
                                byte ac_binary_len = (byte)binaryLen((ulong)Math.Abs(current_ac));
                                byte ac_val = (byte)((zerosBefore << 4) + ((byte)0x0F & ac_binary_len));
                                zerosBefore = 0;
                                memoryHandler.CbCr_AC_CodeLens.Add(ac_val);
                            }
                        }
                    }
                }
                else //Y
                {
                    //DC
                    {
                        int current_dc = (matrixes[i])[0];
                        byte dc_binary_len = (byte)binaryLen((ulong)Math.Abs(current_dc));
                        memoryHandler.Y_DC_CodeLens.Add(dc_binary_len);
                    }
                    //AC
                    {
                        byte zerosBefore = 0;
                        List<byte> tempBuffer = new List<byte>();
                        for (int j = 1; j < (matrixes[i]).Count; j++)
                        {
                            if ((matrixes[i])[j] == 0) //count zeros before coeff
                            {
                                if (zerosBefore == 0x0F)
                                {
                                    tempBuffer.Add(0xF0);
                                    zerosBefore = 1;
                                }
                                else
                                    zerosBefore++;
                            }
                            else //save val
                            {
                                //load buffer
                                for (int k = 0; k < tempBuffer.Count; k++)
                                {
                                    memoryHandler.Y_AC_CodeLens.Add(tempBuffer[k]);
                                }
                                tempBuffer.Clear();
                                //
                                int current_ac = (matrixes[i])[j];
                                byte ac_binary_len = (byte)binaryLen((ulong)Math.Abs(current_ac));
                                byte ac_val = (byte)((zerosBefore << 4) + ((byte)0x0F & ac_binary_len));
                                zerosBefore = 0;
                                memoryHandler.Y_AC_CodeLens.Add(ac_val);
                            }
                        }
                    }
                }
            }
        }

        private void make_coeffs(List<List<int>> matrixes)
        {
            int AC_MATRIX_STOP_HEX = 0x7FFF8080;
            //DC
            for (int i = 0; i < matrixes.Count; i++)
            {
                if ((i + 1) % 5 == 0 || (i + 1) % 6 == 0) //Cb Cr
                {
                    int current_dc = (matrixes[i])[0];
                    byte dc_binary_len = (byte)binaryLen((ulong)Math.Abs(current_dc));
                    if (current_dc < 0)
                        current_dc = (byte)((int)Math.Pow(2, dc_binary_len) - (int)Math.Abs(current_dc) - 1);
                    memoryHandler.CbCr_DC_coeffs.Add(new CoeffValue(current_dc, dc_binary_len, dc_binary_len));
                }
                else //Y
                {
                    int current_dc = (matrixes[i])[0];
                    byte dc_binary_len = (byte)binaryLen((ulong)Math.Abs(current_dc));
                    if (current_dc < 0)
                        current_dc = (byte)((int)Math.Pow(2, dc_binary_len) - (int)Math.Abs(current_dc) - 1);
                    memoryHandler.Y_DC_coeffs.Add(new CoeffValue(current_dc, dc_binary_len, dc_binary_len));
                }
            }
            //AC
            for (int i = 0; i < matrixes.Count; i++)
            {
                if ((i + 1) % 5 == 0 || (i + 1) % 6 == 0) //Cb Cr
                {
                    byte zerosBefore = 0;
                    int coeffs_written = 0;
                    List<CoeffValue> tempBuffer = new List<CoeffValue>();
                    for (int j = 1; j < (matrixes[i]).Count; j++)
                    {
                        if ((matrixes[i])[j] == 0)
                        {
                            if (zerosBefore == 0x0F)
                            {
                                tempBuffer.Add(new CoeffValue(0, 0, 0xF0));
                                zerosBefore = 1;
                            }
                            else
                                zerosBefore++;
                        }
                        //NON-ZEROS
                        if ((matrixes[i])[j] != 0)
                        {
                            //load buffer
                            for (int k = 0; k < tempBuffer.Count; k++)
                            {
                                memoryHandler.CbCr_AC_coeffs.Add(tempBuffer[k]);
                            }
                            tempBuffer.Clear();
                            //
                            int current_ac = (matrixes[i])[j];
                            byte ac_binary_len = (byte)binaryLen((ulong)Math.Abs(current_ac));
                            if (current_ac < 0)
                                current_ac = (byte)((int)Math.Pow(2, ac_binary_len) - (int)Math.Abs(current_ac) - 1);
                            byte ac_to_order = (byte)((zerosBefore << 4) + ((byte)0x0F & ac_binary_len));
                            memoryHandler.CbCr_AC_coeffs.Add(new CoeffValue(current_ac, ac_binary_len, ac_to_order));
                            coeffs_written += zerosBefore + 1;
                            zerosBefore = 0;
                        }
                    }
                    if (coeffs_written != 63)
                        memoryHandler.CbCr_AC_coeffs.Add(new CoeffValue(0, 0, AC_MATRIX_STOP_HEX));
                }
                else //Y
                {
                    byte zerosBefore = 0;
                    int coeffs_written = 0;
                    List<CoeffValue> tempBuffer = new List<CoeffValue>();
                    for (int j = 1; j < (matrixes[i]).Count; j++)
                    {
                        if ((matrixes[i])[j] == 0)
                        {
                            if (zerosBefore == 0x0F)
                            {
                                tempBuffer.Add(new CoeffValue(0, 0, 0xF0));
                                zerosBefore = 1;
                            }
                            else
                                zerosBefore++;
                        }
                        //NON-ZEROS
                        if ((matrixes[i])[j] != 0)
                        {
                            //load buffer
                            for (int k = 0; k < tempBuffer.Count; k++)
                            {
                                memoryHandler.Y_AC_coeffs.Add(tempBuffer[k]);
                            }
                            tempBuffer.Clear();
                            //
                            int current_ac = (matrixes[i])[j];
                            byte ac_binary_len = (byte)binaryLen((ulong)Math.Abs(current_ac));
                            if (current_ac < 0)
                                current_ac = (byte)((int)Math.Pow(2, ac_binary_len) - (int)Math.Abs(current_ac) - 1);
                            byte ac_to_order = (byte)((zerosBefore << 4) + ((byte)0x0F & ac_binary_len));
                            memoryHandler.Y_AC_coeffs.Add(new CoeffValue(current_ac, ac_binary_len, ac_to_order));
                            coeffs_written += zerosBefore + 1;
                            zerosBefore = 0;
                        }
                    }
                    if (coeffs_written != 63)
                        memoryHandler.Y_AC_coeffs.Add(new CoeffValue(0, 0, AC_MATRIX_STOP_HEX));
                }
            }
        }

        private void make_codeLensAmount()
        {
            /*
            * counting amount of codes of certain len
            */
            //Y DC
            for (int i = 0; i < memoryHandler.CodesTable.Y_DC_Table.Count; i++) //run through codes
            {
                memoryHandler.Y_DC_Amounts_of_code_lens[binaryLenHuffCodes(memoryHandler.CodesTable.Y_DC_Table[i].code) - 1]++;
                //memoryHandler.Y_DC_Amounts_of_code_lens[binaryLen(memoryHandler.CodesTable.Y_DC_Table[i].code) - 1]++;
            }
            //Y AC
            for (int i = 0; i < memoryHandler.CodesTable.Y_AC_Table.Count; i++) //run through codes
            {
                memoryHandler.Y_AC_Amounts_of_code_lens[binaryLenHuffCodes(memoryHandler.CodesTable.Y_AC_Table[i].code) - 1]++;
                //memoryHandler.Y_AC_Amounts_of_code_lens[binaryLen(memoryHandler.CodesTable.Y_AC_Table[i].code) - 1]++;
            }
            //CbCr DC
            for (int i = 0; i < memoryHandler.CodesTable.CbCr_DC_Table.Count; i++) //run through codes
            {
                memoryHandler.CbCr_DC_Amounts_of_code_lens[binaryLenHuffCodes(memoryHandler.CodesTable.CbCr_DC_Table[i].code) - 1]++;
               //memoryHandler.CbCr_DC_Amounts_of_code_lens[binaryLen(memoryHandler.CodesTable.CbCr_DC_Table[i].code) - 1]++;
            }
            //CbCr AC
            for (int i = 0; i < memoryHandler.CodesTable.CbCr_AC_Table.Count; i++) //run through codes
            {
                memoryHandler.CbCr_AC_Amounts_of_code_lens[binaryLenHuffCodes(memoryHandler.CodesTable.CbCr_AC_Table[i].code) - 1]++;
                //memoryHandler.CbCr_AC_Amounts_of_code_lens[binaryLen(memoryHandler.CodesTable.CbCr_AC_Table[i].code) - 1]++;
            }
        }

        private void bubbleSortTables()
        {
            bubbleSort(memoryHandler.CodesTable.Y_DC_Table);
            bubbleSort(memoryHandler.CodesTable.Y_AC_Table);
            bubbleSort(memoryHandler.CodesTable.CbCr_DC_Table);
            bubbleSort(memoryHandler.CodesTable.CbCr_AC_Table);
        }

        private void orderTables()
        {
            orderByValues(memoryHandler.Y_DC_coeffs, memoryHandler.CodesTable.Y_DC_Table, memoryHandler.JpegCodes.Y_DC_Table);
            orderByValues(memoryHandler.Y_AC_coeffs, memoryHandler.CodesTable.Y_AC_Table, memoryHandler.JpegCodes.Y_AC_Table);
            orderByValues(memoryHandler.CbCr_DC_coeffs, memoryHandler.CodesTable.CbCr_DC_Table, memoryHandler.JpegCodes.CbCr_DC_Table);
            orderByValues(memoryHandler.CbCr_AC_coeffs, memoryHandler.CodesTable.CbCr_AC_Table, memoryHandler.JpegCodes.CbCr_AC_Table);
        }


        private byte[] packFFC4Headers()
        {
            /*
            * Pack back
            */
            //Forming FFC4 blocks
            //================================================================================= Y DC
            byte[] headerFFC4_YDc = new byte[19 + memoryHandler.CodesTable.Y_DC_Table.Count];
            //length 2b
            if (19 + memoryHandler.CodesTable.Y_DC_Table.Count < 255)
            {
                headerFFC4_YDc[0] = (byte)0x00;
                headerFFC4_YDc[1] = (byte)(19 + memoryHandler.CodesTable.Y_DC_Table.Count);
            }
            else
            {
                int diByteLen = 19 + memoryHandler.CodesTable.Y_DC_Table.Count;
                headerFFC4_YDc[0] = (byte)(diByteLen >> 8);
                headerFFC4_YDc[1] = (byte)(diByteLen & 0x00FF);
            }
            //type 1b
            headerFFC4_YDc[2] = 0x00;
            //code lens 16b
            for (int i = 0; i < 16; i++)
                headerFFC4_YDc[i + 3] = (byte)memoryHandler.Y_DC_Amounts_of_code_lens[i];
            //values Nb
            for (int i = 0; i < memoryHandler.CodesTable.Y_DC_Table.Count; i++)
                headerFFC4_YDc[i + 19] = memoryHandler.CodesTable.Y_DC_Table[i].value;

            //=============================================================================== Y AC
            byte[] headerFFC4_YAc = new byte[19 + memoryHandler.CodesTable.Y_AC_Table.Count];
            //length 2b
            if (19 + memoryHandler.CodesTable.Y_AC_Table.Count < 255)
            {
                headerFFC4_YAc[0] = (byte)0x00;
                headerFFC4_YAc[1] = (byte)(19 + memoryHandler.CodesTable.Y_AC_Table.Count);
            }
            else
            {
                int diByteLen = 19 + memoryHandler.CodesTable.Y_AC_Table.Count;
                headerFFC4_YAc[0] = (byte)(diByteLen >> 8);
                headerFFC4_YAc[1] = (byte)(diByteLen & 0x00FF);
            }
            //type 1b
            headerFFC4_YAc[2] = 0x10;
            //code lens 16b
            for (int i = 0; i < 16; i++)
                headerFFC4_YAc[i + 3] = (byte)memoryHandler.Y_AC_Amounts_of_code_lens[i];
            //values Nb
            for (int i = 0; i < memoryHandler.CodesTable.Y_AC_Table.Count; i++)
                headerFFC4_YAc[i + 19] = memoryHandler.CodesTable.Y_AC_Table[i].value;

            //============================================================================= CbCr DC
            byte[] headerFFC4_CbCrDc = new byte[19 + memoryHandler.CodesTable.CbCr_DC_Table.Count];
            //length 2b
            if (19 + memoryHandler.CodesTable.CbCr_DC_Table.Count < 255)
            {
                headerFFC4_CbCrDc[0] = (byte)0x00;
                headerFFC4_CbCrDc[1] = (byte)(19 + memoryHandler.CodesTable.CbCr_DC_Table.Count);
            }
            else
            {
                int diByteLen = 19 + memoryHandler.CodesTable.CbCr_DC_Table.Count;
                headerFFC4_CbCrDc[0] = (byte)(diByteLen >> 8);
                headerFFC4_CbCrDc[1] = (byte)(diByteLen & 0x00FF);
            }
            //type 1b
            headerFFC4_CbCrDc[2] = 0x01;
            //code lens 16b
            for (int i = 0; i < 16; i++)
                headerFFC4_CbCrDc[i + 3] = (byte)memoryHandler.CbCr_DC_Amounts_of_code_lens[i];
            //values Nb
            for (int i = 0; i < memoryHandler.CodesTable.CbCr_DC_Table.Count; i++)
                headerFFC4_CbCrDc[i + 19] = memoryHandler.CodesTable.CbCr_DC_Table[i].value;
            //============================================================================== CbCr AC
            byte[] headerFFC4_CbCrAc = new byte[19 + memoryHandler.CodesTable.CbCr_AC_Table.Count];
            //length 2b
            if (19 + memoryHandler.CodesTable.CbCr_AC_Table.Count < 255)
            {
                headerFFC4_CbCrAc[0] = (byte)0x00;
                headerFFC4_CbCrAc[1] = (byte)(19 + memoryHandler.CodesTable.CbCr_AC_Table.Count);
            }
            else
            {
                int diByteLen = 19 + memoryHandler.CodesTable.CbCr_AC_Table.Count;
                headerFFC4_CbCrAc[0] = (byte)(diByteLen >> 8);
                headerFFC4_CbCrAc[1] = (byte)(diByteLen & 0x00FF);
            }
            //type 1b
            headerFFC4_CbCrAc[2] = 0x11;
            //code lens 16b
            for (int i = 0; i < 16; i++)
                headerFFC4_CbCrAc[i + 3] = (byte)memoryHandler.CbCr_AC_Amounts_of_code_lens[i];
            //values Nb
            for (int i = 0; i < memoryHandler.CodesTable.CbCr_AC_Table.Count; i++)
                headerFFC4_CbCrAc[i + 19] = memoryHandler.CodesTable.CbCr_AC_Table[i].value;
            //United FFC4 blocks
            List<byte> FFC4Union = new List<byte>();

            FFC4Union.Add(0xFF);
            FFC4Union.Add(0xC4);
            FFC4Union.AddRange(headerFFC4_YDc);
            FFC4Union.Add(0xFF);
            FFC4Union.Add(0xC4);
            FFC4Union.AddRange(headerFFC4_YAc);
            FFC4Union.Add(0xFF);
            FFC4Union.Add(0xC4);
            FFC4Union.AddRange(headerFFC4_CbCrDc);
            FFC4Union.Add(0xFF);
            FFC4Union.Add(0xC4);
            FFC4Union.AddRange(headerFFC4_CbCrAc);
            return FFC4Union.ToArray();
        }

        private byte[] packFFDAHeader()
        {
            //FFDA block=================================================
            List<byte> headerFFDA = new List<byte>();
            headerFFDA.Add(0xFF);
            headerFFDA.Add(0xDA);
            if (memoryHandler.channels == 1)
            {
                headerFFDA.Add(0x00);
                headerFFDA.Add(0x08);
                headerFFDA.Add(0x01);
                headerFFDA.Add(0x01);
                headerFFDA.Add(0x00); //?????
            }
            else if (memoryHandler.channels == 2)
            {
                headerFFDA.Add(0x00);
                headerFFDA.Add(0x0A);
                headerFFDA.Add(0x02);
                headerFFDA.Add(0x01); 
                headerFFDA.Add(0x00); //?????
                headerFFDA.Add(0x02);
                headerFFDA.Add(0x11); //?????
            }
            else if (memoryHandler.channels == 3)
            {
                headerFFDA.Add(0x00);
                headerFFDA.Add(0x0C);
                headerFFDA.Add(0x03);
                headerFFDA.Add(0x01);
                headerFFDA.Add(0x00);
                headerFFDA.Add(0x02);
                headerFFDA.Add(0x11);
                headerFFDA.Add(0x03);
                headerFFDA.Add(0x11);
            }

            headerFFDA.Add(memoryHandler.serviceFFDA[0]);
            headerFFDA.Add(memoryHandler.serviceFFDA[1]);
            headerFFDA.Add(memoryHandler.serviceFFDA[2]);
            return headerFFDA.ToArray();
        }

        private byte[] packFFDAData()
        {
            //FFDA DATA===================================================

            //million super counters (very impotrant please do not delete)
            int Pointer_Y_DC = 0;
            int Pointer_Y_AC = 0;
            int Pointer_Y_Coeff = 0;
            int Pointer_CbCr_DC = 0;
            int Pointer_CbCr_AC = 0;
            int Pointer_CbCr_Coeff = 0;

            List<byte> data = new List<byte>();
            BitWriter BinaryWriter = new BitWriter(data);
            using (StreamWriter sw = File.AppendText("garbageBin.log"))
            {
                for (int i = 0; i < memoryHandler.matrixes_amount; i++)
                {
                    if ((i + 1) % 5 == 0 || (i + 1) % 6 == 0) //Cb Cr
                    {
                        sw.WriteLine(":::::::::::::::::Writing CbCr. Matrix " + i);
                        sw.WriteLine(":::::::DC:");
                        sw.WriteLine("Writing code: " +
                                            ToBinaryString((int)memoryHandler.JpegCodes.CbCr_DC_Table[Pointer_CbCr_DC].code, memoryHandler.JpegCodes.CbCr_DC_Table[Pointer_CbCr_DC].code_length_bits));
                        BinaryWriter.Write(memoryHandler.JpegCodes.CbCr_DC_Table[Pointer_CbCr_DC].code, memoryHandler.JpegCodes.CbCr_DC_Table[Pointer_CbCr_DC].code_length_bits);
                        PrintGarbageBin(data, sw);
                        sw.WriteLine("");
                        sw.WriteLine("Then coeff: (" + memoryHandler.CbCr_DC_coeffs[Pointer_CbCr_DC].value + ")--> " +
                                            ToBinaryString(memoryHandler.CbCr_DC_coeffs[Pointer_CbCr_DC].value, memoryHandler.CbCr_DC_coeffs[Pointer_CbCr_DC].value_length_bits));
                        BinaryWriter.Write((ulong)memoryHandler.CbCr_DC_coeffs[Pointer_CbCr_DC].value, memoryHandler.CbCr_DC_coeffs[Pointer_CbCr_DC].value_length_bits);
                        PrintGarbageBin(data, sw);
                        sw.WriteLine("");
                        Pointer_CbCr_DC++;
                        int writtenCoeffs = 1;
                        sw.WriteLine(":::::::AC:");
                        while (memoryHandler.JpegCodes.CbCr_AC_Table[Pointer_CbCr_AC].value != 0x00)
                        {
                            sw.WriteLine("=====[ AC-" + writtenCoeffs + "]====");
                            sw.WriteLine("Writing code: " +
                                            ToBinaryString((int)memoryHandler.JpegCodes.CbCr_AC_Table[Pointer_CbCr_AC].code, memoryHandler.JpegCodes.CbCr_AC_Table[Pointer_CbCr_AC].code_length_bits));
                            BinaryWriter.Write(memoryHandler.JpegCodes.CbCr_AC_Table[Pointer_CbCr_AC].code, memoryHandler.JpegCodes.CbCr_AC_Table[Pointer_CbCr_AC].code_length_bits);
                            PrintGarbageBin(data, sw);
                            sw.WriteLine("");
                            sw.WriteLine("Then coeff: (" + memoryHandler.CbCr_AC_coeffs[Pointer_CbCr_AC].value + ")--> " +
                                            ToBinaryString(memoryHandler.CbCr_AC_coeffs[Pointer_CbCr_AC].value, memoryHandler.CbCr_AC_coeffs[Pointer_CbCr_AC].value_length_bits));
                            BinaryWriter.Write((ulong)memoryHandler.CbCr_AC_coeffs[Pointer_CbCr_AC].value, memoryHandler.CbCr_AC_coeffs[Pointer_CbCr_AC].value_length_bits);
                            PrintGarbageBin(data, sw);
                            sw.WriteLine("");
                            //count written coeffs with zeros
                            int zeros = memoryHandler.JpegCodes.CbCr_AC_Table[Pointer_CbCr_AC].value >> 4;
                            writtenCoeffs += zeros + 1;
                            Pointer_CbCr_AC++;
                            if (writtenCoeffs == 64)
                                break;
                        }
                        if (writtenCoeffs != 64)
                        {
                            //write stop code
                            sw.WriteLine("Writing STOP-code: " +
                                            ToBinaryString((int)memoryHandler.JpegCodes.CbCr_AC_Table[Pointer_CbCr_AC].code, memoryHandler.JpegCodes.CbCr_AC_Table[Pointer_CbCr_AC].code_length_bits));
                            BinaryWriter.Write(memoryHandler.JpegCodes.CbCr_AC_Table[Pointer_CbCr_AC].code, memoryHandler.JpegCodes.CbCr_AC_Table[Pointer_CbCr_AC].code_length_bits);
                            PrintGarbageBin(data, sw);
                            sw.WriteLine("");
                            Pointer_CbCr_AC++;
                        }
                    }
                    else //Y
                    {
                        sw.WriteLine(":::::::::::::::::Writing Y. Matrix " + i);
                        sw.WriteLine(":::::::DC:");
                        sw.WriteLine("Writing code: " +
                                            ToBinaryString((int)memoryHandler.JpegCodes.Y_DC_Table[Pointer_Y_DC].code, memoryHandler.JpegCodes.Y_DC_Table[Pointer_Y_DC].code_length_bits));
                        BinaryWriter.Write(memoryHandler.JpegCodes.Y_DC_Table[Pointer_Y_DC].code, memoryHandler.JpegCodes.Y_DC_Table[Pointer_Y_DC].code_length_bits);
                        PrintGarbageBin(data, sw);
                        sw.WriteLine("");
                        sw.WriteLine("Then coeff: (" + memoryHandler.Y_DC_coeffs[Pointer_Y_DC].value + ")--> " +
                                            ToBinaryString(memoryHandler.Y_DC_coeffs[Pointer_Y_DC].value, memoryHandler.Y_DC_coeffs[Pointer_Y_DC].value_length_bits));
                        BinaryWriter.Write((ulong)memoryHandler.Y_DC_coeffs[Pointer_Y_DC].value, memoryHandler.Y_DC_coeffs[Pointer_Y_DC].value_length_bits);
                        PrintGarbageBin(data, sw);
                        sw.WriteLine("");
                        Pointer_Y_DC++;
                        int writtenCoeffs = 1;
                        sw.WriteLine(":::::::AC:");
                        while (memoryHandler.JpegCodes.Y_AC_Table[Pointer_Y_AC].value != 0x00)
                        {
                            sw.WriteLine("=====[ AC-" + writtenCoeffs + "]====");
                            sw.WriteLine("Writing code: " +
                                            ToBinaryString((int)memoryHandler.JpegCodes.Y_AC_Table[Pointer_Y_AC].code, memoryHandler.JpegCodes.Y_AC_Table[Pointer_Y_AC].code_length_bits));
                            BinaryWriter.Write(memoryHandler.JpegCodes.Y_AC_Table[Pointer_Y_AC].code, memoryHandler.JpegCodes.Y_AC_Table[Pointer_Y_AC].code_length_bits);
                            PrintGarbageBin(data, sw);
                            sw.WriteLine("");
                            sw.WriteLine("Then coeff: (" + memoryHandler.Y_AC_coeffs[Pointer_Y_AC].value + ")--> " +
                                            ToBinaryString(memoryHandler.Y_AC_coeffs[Pointer_Y_AC].value, memoryHandler.Y_AC_coeffs[Pointer_Y_AC].value_length_bits));
                            BinaryWriter.Write((ulong)memoryHandler.Y_AC_coeffs[Pointer_Y_AC].value, memoryHandler.Y_AC_coeffs[Pointer_Y_AC].value_length_bits);
                            PrintGarbageBin(data, sw);
                            sw.WriteLine("");
                            //count written coeffs with zeros
                            int zeros = memoryHandler.JpegCodes.Y_AC_Table[Pointer_Y_AC].value >> 4;
                            writtenCoeffs += zeros + 1;
                            Pointer_Y_AC++;
                            if (writtenCoeffs == 64)
                                break;
                        }
                        if (writtenCoeffs != 64)
                        {
                            //write stop code
                            sw.WriteLine("Writing STOP-code: " +
                                            ToBinaryString((int)memoryHandler.JpegCodes.Y_AC_Table[Pointer_Y_AC].code, memoryHandler.JpegCodes.Y_AC_Table[Pointer_Y_AC].code_length_bits));
                            BinaryWriter.Write(memoryHandler.JpegCodes.Y_AC_Table[Pointer_Y_AC].code, memoryHandler.JpegCodes.Y_AC_Table[Pointer_Y_AC].code_length_bits);
                            PrintGarbageBin(data, sw);
                            sw.WriteLine("");
                            Pointer_Y_AC++;
                        }
                    }
                }
                sw.WriteLine("TOTAL::::::::::::::::");
                PrintGarbageBin(data, sw);
            }

            //print stream
            Console.WriteLine("WRITTEN BINARY STREAM:::::::");
            for (int i=0; i < data.Count; i++)
            {
                Console.Write(ToBinaryString(data[i]));
                if (i % 8 == 0 && i != 0)
                    Console.WriteLine("");
            }
            Console.WriteLine("");
            Console.WriteLine("WRITTEN BINARY STREAM END:::::::");

            return data.ToArray();
        }

        private void PrintGarbageBin(List<byte> target, StreamWriter sw)
        {
            sw.WriteLine("BINARY STREAM:::::::");
            for (int i = 0; i < target.Count; i++)
            {
                sw.Write(ToBinaryString(target[i]));
                if (i % 8 == 0 && i != 0)
                    sw.WriteLine("");
            }
            sw.WriteLine("");
            sw.WriteLine("BINARY STREAM END:::::::");
        }

        class BitWriter
        {
            private List<byte> dataBlock = null;
            private int magic_counter = 0;
            public BitWriter(List<byte> dataBlock_)
            {
                this.dataBlock = dataBlock_;
            }
            public void Write(ulong value, int value_len_bits)
            {
                if (magic_counter % 8 == 0)
                    dataBlock.Add(0);
                int freeBitsInLastByte = 8 - (magic_counter % 8);

                byte temp = dataBlock[magic_counter / 8];
                editByte(ref temp, value, (int)(magic_counter % 8), 0, value_len_bits);
                dataBlock[magic_counter / 8] = temp;

                if (freeBitsInLastByte < value_len_bits)
                {
                    dataBlock.Add(0);
                    //
                    temp = dataBlock[(magic_counter / 8) + 1];
                    editByte(ref temp, value, 0, 8 - (int)(magic_counter % 8), value_len_bits);
                    //
                    dataBlock[(magic_counter / 8) + 1] = temp;
                }
                if (freeBitsInLastByte + 8 < value_len_bits)
                {
                    dataBlock.Add(0);
                    //
                    temp = dataBlock[(magic_counter / 8) + 2];
                    editByte(ref temp, value, 0, 16 - (int)(magic_counter % 8), value_len_bits);
                    //
                    dataBlock[(magic_counter / 8) + 2] = temp;
                }
                magic_counter += value_len_bits;
            }
            private void editByte(ref byte targetToEdit, ulong value, int offsetByte, int offsetValue, int valueLen)
            {
                ulong temp = value << (64 - valueLen + offsetValue);
                temp >>= 56 + offsetByte;
                targetToEdit += (byte)temp;
            }
        }

        private int getBit(int code, int pointer)
        {
            int pointerBit = 0, temp = 0;
            pointerBit = (int)Math.Pow(2, pointer);
            temp = code ^ pointerBit;
            if (temp < code)
                return 1;
            else
                return 0;
        }

        private int binaryLen(ulong msg)
        {
            int i;
            for (i = 0; msg >= (ulong)Math.Pow(2, i); i++) { }
            return i;
        }

        private int binaryLenHuffCodes(ulong msg)
        {
            int i;
            for (i = 0; msg >= (ulong)Math.Pow(2, i); i++) { }
            return --i;
        }

        private void bubbleSort(List<HuffmanCode> Y_DC_Table)
        {
            HuffmanCode temp;
            for (int j = 0; j <= Y_DC_Table.Count - 2; j++)
            {
                for (int i = 0; i <= Y_DC_Table.Count - 2; i++)
                {
                    if (Y_DC_Table[i].code > Y_DC_Table[i + 1].code)
                    {
                        temp = Y_DC_Table[i + 1];
                        Y_DC_Table[i + 1] = Y_DC_Table[i];
                        Y_DC_Table[i] = temp;
                    }
                }
            }
        }

        private void orderByValues(List<CoeffValue> coefs, List<HuffmanCode> codes, List<HuffmanCode> target)
        {
            int AC_MATRIX_STOP_HEX = 0x7FFF8080;
            for (int i = 0; i < coefs.Count; i++)
            {
                int codesInd = -1;
                //find corresponding index from codes
                for (int j = 0; j < codes.Count; j++)
                {
                    if ((coefs[i].value_to_order == codes[j].value) || (coefs[i].value_to_order == AC_MATRIX_STOP_HEX && codes[j].value == 0x00))
                    {
                        codesInd = j;
                        break;
                    }
                }
                if (codesInd == -1)
                    return; //GENERATE ERROR!!!!!!!!
                target.Add(new HuffmanCode(codes[codesInd].code, codes[codesInd].code_length_bits, codes[codesInd].value, codes[codesInd].value_length_bits, codes[codesInd].frequency));
            }
        }

        //========= memory handlers ===========
        HuffmanMemoryHandler huffmanMemoryHandler = null;
        JpegMemoryHandler memoryHandler = null;
    }

    public class CoeffValue
    {
        public CoeffValue(int value_, int valueLen_, int value_to_order_)
        {
            value = value_;
            value_length_bits = valueLen_;
            value_to_order = value_to_order_;
        }

        public int value { get; set; }
        public int value_length_bits { get; set; }
        public int value_to_order { get; set; }
    }

    public class HuffmanCode
    {
        public HuffmanCode(ulong code_, int codeLenBits_, byte value_, int valueLenBits_, int freq_)
        {
            this.code = code_;
            this.code_length_bits = codeLenBits_;
            this.value = value_;
            this.value_length_bits = valueLenBits_;
            this.frequency = freq_;
        }

        public ulong code { get; set; }
        public int code_length_bits { get; set; }
        public byte value { get; set; }
        public int value_length_bits { get; set; }
        public int frequency { get; set; }
    }

    public class JpegCodesTable
    {
        public JpegCodesTable() { }
        public unsafe void Init(HuffmanMemoryHandler handler_)
        {
            //init Y_DC_Table
            for (int i = 0; i < handler_.codesLenInt_YDc; i++)
            {
                HuffmanCode code = new HuffmanCode(
                                handler_.codesArr_YDc[i],
                                binaryLenHuffCodes(handler_.codesArr_YDc[i]),
                                //binaryLen(handler_.codesArr_YDc[i]),
                                handler_.valuesArr_YDc[i],
                                binaryLen(handler_.valuesArr_YDc[i]),
                                handler_.freqsArr_YDc[i]);
                Y_DC_Table.Add(code);
            }
            //if (Y_DC_Table.Count == 2) //BIG GIANT SUPERIOR COSTIL (do not do like this pls)
            //{
            //    Y_DC_Table[1].code = 3;
            //    Y_DC_Table[1].code_length_bits = 1;
            //    Y_DC_Table[0].code = 6;
            //    Y_DC_Table[0].code_length_bits = 2;
            //}
            //init Y_AC_Table
            for (int i = 0; i < handler_.codesLenInt_YAc; i++)
            {
                HuffmanCode code = new HuffmanCode(
                                handler_.codesArr_YAc[i],
                                binaryLenHuffCodes(handler_.codesArr_YAc[i]),
                                //binaryLen(handler_.codesArr_YAc[i]),
                                handler_.valuesArr_YAc[i],
                                binaryLen(handler_.valuesArr_YAc[i]),
                                handler_.freqsArr_YAc[i]);
                Y_AC_Table.Add(code);
            }
            //if (Y_AC_Table.Count == 2) //BIG GIANT SUPERIOR COSTIL (do not do like this pls)
            //{
            //    Y_AC_Table[1].code = 3;
            //    Y_AC_Table[1].code_length_bits = 1;
            //    Y_AC_Table[0].code = 6;
            //    Y_AC_Table[0].code_length_bits = 2;
            //}
            //init CbCr_DC_Table
            for (int i = 0; i < handler_.codesLenInt_CbCrDc; i++)
            {
                HuffmanCode code = new HuffmanCode(
                                handler_.codesArr_CbCrDc[i],
                                binaryLenHuffCodes(handler_.codesArr_CbCrDc[i]),
                                //binaryLen(handler_.codesArr_CbCrDc[i]),
                                handler_.valuesArr_CbCrDc[i],
                                binaryLen(handler_.valuesArr_CbCrDc[i]),
                                handler_.freqsArr_CbCrDc[i]);
                CbCr_DC_Table.Add(code);
            }
            //if (CbCr_DC_Table.Count == 2) //BIG GIANT SUPERIOR COSTIL (do not do like this pls)
            //{
            //    CbCr_DC_Table[1].code = 3;
            //    CbCr_DC_Table[1].code_length_bits = 1;
            //    CbCr_DC_Table[0].code = 6;
            //    CbCr_DC_Table[0].code_length_bits = 2;
            //}
            //init CbCr_AC_Table
            for (int i = 0; i < handler_.codesLenInt_CbCrAc; i++)
            {
                HuffmanCode code = new HuffmanCode(
                                handler_.codesArr_CbCrAc[i],
                                binaryLenHuffCodes(handler_.codesArr_CbCrAc[i]),
                                //binaryLen(handler_.codesArr_CbCrAc[i]),
                                handler_.valuesArr_CbCrAc[i],
                                binaryLen(handler_.valuesArr_CbCrAc[i]),
                                handler_.freqsArr_CbCrAc[i]);
                CbCr_AC_Table.Add(code);
            }
            //if (CbCr_AC_Table.Count == 2) //BIG GIANT SUPERIOR COSTIL (do not do like this pls)
            //{
            //    CbCr_AC_Table[1].code = 3;
            //    CbCr_AC_Table[1].code_length_bits = 1;
            //    CbCr_AC_Table[0].code = 6;
            //    CbCr_AC_Table[0].code_length_bits = 2;
            //}
        }

        private int binaryLen(ulong msg)
        {
            int i;
            for (i = 0; msg >= (ulong)Math.Pow(2, i); i++) { }
            return i;
        }

        private int binaryLenHuffCodes(ulong msg)
        {
            int i;
            for (i = 0; msg >= (ulong)Math.Pow(2, i); i++) { }
            return --i;
        }

        public List<ulong[]> getCodes()
        {
            List<ulong[]> codesTable = new List<ulong[]>();

            List<ulong> codesYDC = new List<ulong>();
            for (int i = 0; i < Y_DC_Table.Count; i++)
                codesYDC.Add(Y_DC_Table[i].code);
            List<ulong> codesYAC = new List<ulong>();
            for (int i = 0; i < Y_AC_Table.Count; i++)
                codesYAC.Add(Y_AC_Table[i].code);
            List<ulong> codesCbCrDC = new List<ulong>();
            for (int i = 0; i < CbCr_DC_Table.Count; i++)
                codesCbCrDC.Add(CbCr_DC_Table[i].code);
            List<ulong> codesCbCrAC = new List<ulong>();
            for (int i = 0; i < CbCr_AC_Table.Count; i++)
                codesCbCrAC.Add(CbCr_AC_Table[i].code);

            codesTable.Add(codesYDC.ToArray());
            codesTable.Add(codesYAC.ToArray());
            codesTable.Add(codesCbCrDC.ToArray());
            codesTable.Add(codesCbCrAC.ToArray());

            return codesTable;
        }

        public void updateCodes(List<ulong[]> updatedTable)
        {
            for (int i = 0; i < Y_DC_Table.Count; i++)
                Y_DC_Table[i].code = (updatedTable[0])[i];
            for (int i = 0; i < Y_AC_Table.Count; i++)
                Y_AC_Table[i].code = (updatedTable[1])[i];
            for (int i = 0; i < CbCr_DC_Table.Count; i++)
                CbCr_DC_Table[i].code = (updatedTable[2])[i];
            for (int i = 0; i < CbCr_AC_Table.Count; i++)
                CbCr_AC_Table[i].code = (updatedTable[3])[i];
        }

        public List<HuffmanCode> Y_DC_Table = new List<HuffmanCode>();
        public List<HuffmanCode> Y_AC_Table = new List<HuffmanCode>();
        public List<HuffmanCode> CbCr_DC_Table = new List<HuffmanCode>();
        public List<HuffmanCode> CbCr_AC_Table = new List<HuffmanCode>();
    }

    public class JpegMemoryHandler
    {
        public JpegMemoryHandler() 
        {
            Array.Clear(Y_DC_Amounts_of_code_lens, 0, Y_DC_Amounts_of_code_lens.Length);
            Array.Clear(Y_AC_Amounts_of_code_lens, 0, Y_AC_Amounts_of_code_lens.Length);
            Array.Clear(CbCr_DC_Amounts_of_code_lens, 0, CbCr_DC_Amounts_of_code_lens.Length);
            Array.Clear(CbCr_AC_Amounts_of_code_lens, 0, CbCr_AC_Amounts_of_code_lens.Length);
        }

        //========================= DATA ==========================
        //picture data
        public int matrixes_amount = 0;
        public List<byte> serviceFFDA;
        public int channels = 3;
        //structured input for huffman
        public List<int> Y_DC_CodeLens = new List<int>();
        public List<int> Y_AC_CodeLens = new List<int>();
        public List<int> CbCr_DC_CodeLens = new List<int>();
        public List<int> CbCr_AC_CodeLens = new List<int>();
        //coeffs (basically squished matrixes to coeffs arrays) <--------------------HEADER
        public List<CoeffValue> Y_DC_coeffs = new List<CoeffValue>();
        public List<CoeffValue> Y_AC_coeffs = new List<CoeffValue>();
        public List<CoeffValue> CbCr_DC_coeffs = new List<CoeffValue>();
        public List<CoeffValue> CbCr_AC_coeffs = new List<CoeffValue>();
        //output from huffman
        public JpegCodesTable CodesTable = new JpegCodesTable();
        //ordered and appended by matrix values table of huffman codes <-------------- DATA
        public JpegCodesTable JpegCodes = new JpegCodesTable(); //DO NOT INIT!!!!!
        //arrays of 16 vals of code lens of certain len <--------------------HEADER
        public int[] Y_DC_Amounts_of_code_lens = new int[16];
        public int[] Y_AC_Amounts_of_code_lens = new int[16];
        public int[] CbCr_DC_Amounts_of_code_lens = new int[16];
        public int[] CbCr_AC_Amounts_of_code_lens = new int[16];
    }
    public class HuffmanMemoryHandler
    {
        public HuffmanMemoryHandler() { }

        public unsafe void init_input(List<int> Y_DC_CodeLens,
                                      List<int> Y_AC_CodeLens,
                                      List<int> CbCr_DC_CodeLens,
                                      List<int> CbCr_AC_CodeLens)
        {
            this.OPtr_YDc_codelen = Marshal.AllocHGlobal(Y_DC_CodeLens.Count * sizeof(byte));
            this.pYDc_codelen = (byte*)this.OPtr_YDc_codelen;
            this.OPtr_YDc_codelen_Length = Y_DC_CodeLens.Count;
            for (int i = 0; i < Y_DC_CodeLens.Count; i++)
            {
                this.pYDc_codelen[i] = (byte)Y_DC_CodeLens[i];
            }
            this.OPtr_YAc_codelen = Marshal.AllocHGlobal(Y_AC_CodeLens.Count * sizeof(byte));
            this.pYAc_codelen = (byte*)this.OPtr_YAc_codelen;
            this.OPtr_YAc_codelen_Length = Y_AC_CodeLens.Count;
            for (int i = 0; i < Y_AC_CodeLens.Count; i++)
            {
                this.pYAc_codelen[i] = (byte)Y_AC_CodeLens[i];
            }
            this.OPtr_CbCrDc_codelen = Marshal.AllocHGlobal(CbCr_DC_CodeLens.Count * sizeof(byte));
            this.pCbCrDc_codelen = (byte*)this.OPtr_CbCrDc_codelen;
            this.OPtr_CbCrDc_codelen_Length = CbCr_DC_CodeLens.Count;
            for (int i = 0; i < CbCr_DC_CodeLens.Count; i++)
            {
                this.pCbCrDc_codelen[i] = (byte)CbCr_DC_CodeLens[i];
            }
            this.OPtr_CbCrAc_codelen = Marshal.AllocHGlobal(CbCr_AC_CodeLens.Count * sizeof(byte));
            this.pCbCrAc_codelen = (byte*)this.OPtr_CbCrAc_codelen;
            this.OPtr_CbCrAc_codelen_Length = CbCr_AC_CodeLens.Count;
            for (int i = 0; i < CbCr_AC_CodeLens.Count; i++)
            {
                this.pCbCrAc_codelen[i] = (byte)CbCr_AC_CodeLens[i];
            }
        }
        public unsafe void init_output()
        {
            // POINTERS ASSIGN
            //Y DC
            this.codesArr_YDc = (ulong*)Marshal.ReadInt32(codes_YDc);
            this.codesLenInt_YDc = Marshal.ReadInt32(codesLen_YDc);
            this.valuesArr_YDc = (byte*)Marshal.ReadInt32(values_YDc);
            this.valuesLenInt_YDc = Marshal.ReadInt32(valuesLen_YDc);
            this.freqsArr_YDc = (int*)Marshal.ReadInt32(freqs_YDc);
            this.freqsLenInt_YDc = Marshal.ReadInt32(freqsLen_YDc);
            //Y AC
            this.codesArr_YAc = (ulong*)Marshal.ReadInt32(codes_YAc);
            this.codesLenInt_YAc = Marshal.ReadInt32(codesLen_YAc);
            this.valuesArr_YAc = (byte*)Marshal.ReadInt32(values_YAc);
            this.valuesLenInt_YAc = Marshal.ReadInt32(valuesLen_YAc);
            this.freqsArr_YAc = (int*)Marshal.ReadInt32(freqs_YAc);
            this.freqsLenInt_YAc = Marshal.ReadInt32(freqsLen_YAc);
            //CbCr DC
            this.codesArr_CbCrDc = (ulong*)Marshal.ReadInt32(codes_CbCrDc);
            this.codesLenInt_CbCrDc = Marshal.ReadInt32(codesLen_CbCrDc);
            this.valuesArr_CbCrDc = (byte*)Marshal.ReadInt32(values_CbCrDc);
            this.valuesLenInt_CbCrDc = Marshal.ReadInt32(valuesLen_CbCrDc);
            this.freqsArr_CbCrDc = (int*)Marshal.ReadInt32(freqs_CbCrDc);
            this.freqsLenInt_CbCrDc = Marshal.ReadInt32(freqsLen_CbCrDc);
            //CbCr AC
            this.codesArr_CbCrAc = (ulong*)Marshal.ReadInt32(codes_CbCrAc);
            this.codesLenInt_CbCrAc = Marshal.ReadInt32(codesLen_CbCrAc);
            this.valuesArr_CbCrAc = (byte*)Marshal.ReadInt32(values_CbCrAc);
            this.valuesLenInt_CbCrAc = Marshal.ReadInt32(valuesLen_CbCrAc);
            this.freqsArr_CbCrAc = (int*)Marshal.ReadInt32(freqs_CbCrAc);
            this.freqsLenInt_CbCrAc = Marshal.ReadInt32(freqsLen_CbCrAc);
        }

        public unsafe void FreeInputStreams()
        {
            //free source streams
            //FREEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEE
            Marshal.FreeHGlobal(OPtr_YDc_codelen);
            Marshal.FreeHGlobal(OPtr_YAc_codelen);
            Marshal.FreeHGlobal(OPtr_CbCrDc_codelen);
            Marshal.FreeHGlobal(OPtr_CbCrAc_codelen);
            pYDc_codelen = null;
            pYAc_codelen = null;
            pCbCrDc_codelen = null;
            pCbCrAc_codelen = null;
            OPtr_YDc_codelen_Length = 0;
            OPtr_YAc_codelen_Length = 0;
            OPtr_CbCrDc_codelen_Length = 0;
            OPtr_CbCrAc_codelen_Length = 0;
        }

        //========== HUFFMAN INPUT ==============
        public IntPtr OPtr_YDc_codelen = IntPtr.Zero;
        public IntPtr OPtr_YAc_codelen = IntPtr.Zero;
        public IntPtr OPtr_CbCrDc_codelen = IntPtr.Zero;
        public IntPtr OPtr_CbCrAc_codelen = IntPtr.Zero;
        public unsafe byte* pYDc_codelen { get; set; }
        public unsafe byte* pYAc_codelen { get; set; }
        public unsafe byte* pCbCrDc_codelen { get; set; }
        public unsafe byte* pCbCrAc_codelen { get; set; }

        public int OPtr_YDc_codelen_Length = 0;
        public int OPtr_YAc_codelen_Length = 0;
        public int OPtr_CbCrDc_codelen_Length = 0;
        public int OPtr_CbCrAc_codelen_Length = 0;


        //========== HUFFMAN OUTPUT =============
        //Y DC
        public IntPtr codes_YDc = Marshal.AllocHGlobal(sizeof(int)); //this is address of array[uint64_t]
        public IntPtr codesLen_YDc = Marshal.AllocHGlobal(sizeof(int));
        public IntPtr values_YDc = Marshal.AllocHGlobal(sizeof(int)); //this is address of array[char]
        public IntPtr valuesLen_YDc = Marshal.AllocHGlobal(sizeof(int));
        public IntPtr freqs_YDc = Marshal.AllocHGlobal(sizeof(int)); //this is address of array[int]
        public IntPtr freqsLen_YDc = Marshal.AllocHGlobal(sizeof(int));
        //Y AC
        public IntPtr codes_YAc = Marshal.AllocHGlobal(sizeof(int)); //this is address of array[uint64_t]
        public IntPtr codesLen_YAc = Marshal.AllocHGlobal(sizeof(int));
        public IntPtr values_YAc = Marshal.AllocHGlobal(sizeof(int)); //this is address of array[char]
        public IntPtr valuesLen_YAc = Marshal.AllocHGlobal(sizeof(int));
        public IntPtr freqs_YAc = Marshal.AllocHGlobal(sizeof(int)); //this is address of array[int]
        public IntPtr freqsLen_YAc = Marshal.AllocHGlobal(sizeof(int));
        //CbCr DC
        public IntPtr codes_CbCrDc = Marshal.AllocHGlobal(sizeof(int)); //this is address of array[uint64_t]
        public IntPtr codesLen_CbCrDc = Marshal.AllocHGlobal(sizeof(int));
        public IntPtr values_CbCrDc = Marshal.AllocHGlobal(sizeof(int)); //this is address of array[char]
        public IntPtr valuesLen_CbCrDc = Marshal.AllocHGlobal(sizeof(int));
        public IntPtr freqs_CbCrDc = Marshal.AllocHGlobal(sizeof(int)); //this is address of array[int]
        public IntPtr freqsLen_CbCrDc = Marshal.AllocHGlobal(sizeof(int));
        //CbCr AC
        public IntPtr codes_CbCrAc = Marshal.AllocHGlobal(sizeof(int)); //this is address of array[uint64_t]
        public IntPtr codesLen_CbCrAc = Marshal.AllocHGlobal(sizeof(int));
        public IntPtr values_CbCrAc = Marshal.AllocHGlobal(sizeof(int)); //this is address of array[char]
        public IntPtr valuesLen_CbCrAc = Marshal.AllocHGlobal(sizeof(int));
        public IntPtr freqs_CbCrAc = Marshal.AllocHGlobal(sizeof(int)); //this is address of array[int]
        public IntPtr freqsLen_CbCrAc = Marshal.AllocHGlobal(sizeof(int));
        //Y DC
        public int codesLenInt_YDc { get; set; }
        public int valuesLenInt_YDc { get; set; }
        public unsafe ulong* codesArr_YDc { get; set; }
        public unsafe byte* valuesArr_YDc { get; set; }
        public unsafe int* freqsArr_YDc { get; set; }
        public unsafe int freqsLenInt_YDc { get; set; }

        //Y AC
        public int codesLenInt_YAc { get; set; }
        public int valuesLenInt_YAc { get; set; }
        public unsafe ulong* codesArr_YAc { get; set; }
        public unsafe byte* valuesArr_YAc { get; set; }
        public unsafe int* freqsArr_YAc { get; set; }
        public unsafe int freqsLenInt_YAc { get; set; }

        //CbCr DC
        public int codesLenInt_CbCrDc { get; set; }
        public int valuesLenInt_CbCrDc { get; set; }
        public unsafe ulong* codesArr_CbCrDc { get; set; }
        public unsafe byte* valuesArr_CbCrDc { get; set; }
        public unsafe int* freqsArr_CbCrDc { get; set; }
        public unsafe int freqsLenInt_CbCrDc { get; set; }

        //CbCr AC
        public int codesLenInt_CbCrAc { get; set; }
        public int valuesLenInt_CbCrAc { get; set; }
        public unsafe ulong* codesArr_CbCrAc { get; set; }
        public unsafe byte* valuesArr_CbCrAc { get; set; }
        public unsafe int* freqsArr_CbCrAc { get; set; }
        public unsafe int freqsLenInt_CbCrAc { get; set; }
    }
}