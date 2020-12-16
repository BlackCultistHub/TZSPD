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

        byte[] container;
        byte[] cvz;
        byte[] steganoContainer;
        byte[] out_cvz;
        int container_capacity;
        int cvz_length;

        const int N = 50;
        readonly int[] C1 = { 3, 6 };
        readonly int C1_ind = 30;
        readonly int[] C2 = { 5, 3 };
        readonly int C2_ind = 23;

        public Form3_tz()
        {
            InitializeComponent();
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
                    if (p1 == 0xFF && p2 == 0xC0)
                    {
                        container_pointer++;
                        goto FFC0;
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
        FFC0:
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
                //read codes
                huff_tables_codes.Add(new int[block_len - 19]);
                for (int i = 0; i < block_len-19; i++)
                {
                    (huff_tables_codes[huff_tables_codes.Count - 1])[i] = container[container_pointer];
                    container_pointer++;
                }
                huff_table_ind++;
            }
        FFDA:
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

            container_pointer+=4; //skip 00 3F 00

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
            //int[] codeLens = new int[16]; //массив количеств кодов из 16 элементов.
            //int[] codes = { };
            //int[] codesVals = { };
            //int code = 0;
            //for (int j = 0; j < 16; j++)
            //{
            //    for (int k = 0; k < codeLens[j]; k++)
            //    {
            //        codesVals.Append(code);
            //        code++;
            //    }
            //    code <<= 1;
            //}

            //filling matrixes
            //match channels--tables
            int[] matched_dc = new int[6]; //contains table inds YYYCbCr
            int[] matched_ac = new int[6]; //contains table inds YYYCbCr
            //channel Y
            //DC search
            int target_index = channels_table_match[0];
            target_index >>= 4;
            int found_dc_index = 0;
            int found_ac_index = 0;
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
            target_index = channels_table_match[1];
            target_index >>= 4;
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
            //AC
            matched_ac[4] = found_ac_index;
            //channel Cr
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
            //AC
            matched_ac[5] = found_ac_index;

            int current_table_ind = 0;
            bool use_dc = true;

            int pointerDataStart = container_pointer;
            int bits_counter = 0;
            int prevBitsCounter = bits_counter / 8;
            int buffer = 1;
            buffer <<= 1;
            while (!(p1 == 0xFF && p2 == 0xD9))
            {
                buffer += getBit(container[pointerDataStart + (bits_counter / 8)], 7 - (bits_counter % 8));
                //var TRACKER_buffer = ToBinaryString((uint)buffer); //DEBUG=====================================================
                bits_counter++;

                //for eof
                if (prevBitsCounter != bits_counter / 8)
                {
                    prevBitsCounter = bits_counter / 8;
                    p1 = container[container_pointer];
                    p2 = container[container_pointer + 1];
                    container_pointer++;
                }
                //==for eof

                int code = 0;
                if (use_dc)
                    code = checkCode(buffer, huff_tables_codes[matched_dc[current_table_ind % 6]], huff_tables_codes_binary[matched_dc[current_table_ind % 6]]);
                else
                    code = checkCode(buffer, huff_tables_codes[matched_ac[current_table_ind % 6]], huff_tables_codes_binary[matched_ac[current_table_ind % 6]]);
                if (code != -1)
                {
                    if (code == 0x00 && !use_dc)
                    {
                        current_table_ind++;
                        use_dc = true;
                        int border = picture_matrixes[picture_matrixes.Count - 1].Count;
                        for (int i = 0; i < 64 - border; i++)
                            picture_matrixes[picture_matrixes.Count - 1].Add(0);
                    }
                    else
                    {
                        if (use_dc) //write as dc coeff
                        {
                            use_dc = false;
                            picture_matrixes.Add(new List<int>());
                            int binaryVal = 0;
                            int binaryValBits = 0;
                            for (int i = 0; i < code; i++)
                            {
                                binaryVal <<= 1;
                                binaryVal += getBit(container[pointerDataStart + ((bits_counter + i) / 8)], 7 - ((bits_counter + i) % 8));
                                //var TRACKER_binaryValDC = ToBinaryString((uint)binaryVal); //DEBUG=====================================================
                                binaryValBits++;
                            }
                            if (getBit(binaryVal, binaryValBits-1) == 0)
                            {
                                picture_matrixes[picture_matrixes.Count - 1].Add((binaryVal - (int)Math.Pow(2, binaryValBits) + 1));
                            }
                            else
                                picture_matrixes[picture_matrixes.Count - 1].Add(binaryVal);
                            bits_counter += binaryValBits;
                        }
                        else
                        {
                            int top = code & 0xF0;
                            top >>= 4;
                            int bottom = code & 0x0F;
                            //write 'top' zeros
                            for (int i = 0; i < top; i++)
                            {
                                picture_matrixes[picture_matrixes.Count - 1].Add(0);
                            }
                            //read 'bottom' next bits
                            int binaryVal = 0;
                            int binaryValBits = 0;
                            for (int i = 0; i < bottom; i++)
                            {
                                binaryVal <<= 1;
                                binaryVal += getBit(container[pointerDataStart + ((bits_counter + i) / 8)], 7 - ((bits_counter + i) % 8));
                                //var TRACKER_binaryValAC = ToBinaryString((uint)binaryVal); //DEBUG=====================================================
                                binaryValBits++;
                            }
                            if (getBit(binaryVal, binaryValBits-1) == 0)
                            {
                                picture_matrixes[picture_matrixes.Count - 1].Add((binaryVal - (int)Math.Pow(2, binaryValBits) + 1));
                            }
                            else
                                picture_matrixes[picture_matrixes.Count - 1].Add(binaryVal);
                            bits_counter += binaryValBits;
                        }
                        
                    }
                    buffer = 1;
                }
                buffer <<= 1;
            }

            //cvz max len define
            int cvz_max_len = picture_matrixes.Count/8;

            //quant
            for (int i = 0; i < picture_matrixes.Count; i++)
            {
                if ((i+1)%5 == 0 || (i+1)%6 == 6) //Cb Cr
                {
                    (picture_matrixes[i])[C1_ind] *= (quant_matrixes[1])[1];
                    (picture_matrixes[i])[C2_ind] *= (quant_matrixes[1])[1];
                }
                else //Y
                {
                    (picture_matrixes[i])[C1_ind] *= (quant_matrixes[0])[1];
                    (picture_matrixes[i])[C2_ind] *= (quant_matrixes[0])[1];
                }
            }

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
            (picture_matrixes[0])[30] = -120;
            (picture_matrixes[1])[30] = 120;
            (picture_matrixes[2])[30] = -120;

            //back quant
            for (int i = 0; i < picture_matrixes.Count; i++)
            {
                if ((i + 1) % 5 == 0 || (i + 1) % 6 == 6) //Cb Cr
                {
                    (picture_matrixes[i])[C1_ind] /= (quant_matrixes[1])[1];
                    (picture_matrixes[i])[C2_ind] /= (quant_matrixes[1])[1];
                }
                else //Y
                {
                    (picture_matrixes[i])[C1_ind] /= (quant_matrixes[0])[1];
                    (picture_matrixes[i])[C2_ind] /= (quant_matrixes[0])[1];
                }
            }

            //DC reveal and hide not needed

            byte[] changedBlock = packWithHuffman(picture_matrixes);

        }

        private unsafe byte[] packWithHuffman(List<List<int>> matrixes) //returns from first FFC4 to FFD9
        {
            try
            {
                /*
                 * 1. [prepare data]
                 */
                //count Y and Cb/Cr matrixes
                int Y_matrixes_count = 0;
                int CbCr_matrixes_count = 0;
                for (int i = 0; i < matrixes.Count; i++)
                {
                    if ((i + 1) % 5 == 0 || (i + 1) % 6 == 0) //Cb Cr
                    {
                        CbCr_matrixes_count++;
                    }
                    else //Y
                    {
                        Y_matrixes_count++;
                    }
                }

                /*
                 * [coding lengths]
                 */
                IntPtr OPtr_YDc_codelen = Marshal.AllocHGlobal(Y_matrixes_count * sizeof(byte));
                byte* pYDc_codelen = (byte*)OPtr_YDc_codelen;
                IntPtr OPtr_CbCrDc_codelen = Marshal.AllocHGlobal(CbCr_matrixes_count * sizeof(byte));
                byte* pCbCrDc_codelen = (byte*)OPtr_CbCrDc_codelen;
                //count how much memory needed for AC coeffs of Y and CbCr matrix
                int AC_slots_for_Y = 1; //0x00 val
                int AC_slots_for_CrCb = 1; //0x00 val
                for (int i = 0; i < matrixes.Count; i++)
                {
                    if ((i + 1) % 5 == 0 || (i + 1) % 6 == 0) //Cb Cr
                    {
                        for (int j = 1; j < (matrixes[i]).Count; j++)
                        {
                            if ((matrixes[i])[j] != 0) //count coeff
                                AC_slots_for_CrCb++;
                        }
                    }
                    else //Y
                    {
                        for (int j = 1; j < (matrixes[i]).Count; j++)
                        {
                            if ((matrixes[i])[j] != 0) //count coeff
                                AC_slots_for_Y++;
                        }
                    }
                }

                IntPtr OPtr_YAc_codelen = Marshal.AllocHGlobal(AC_slots_for_Y * sizeof(byte));
                byte* pYAc_codelen = (byte*)OPtr_YAc_codelen;
                IntPtr OPtr_CbCrAc_codelen = Marshal.AllocHGlobal(AC_slots_for_CrCb * sizeof(byte));
                byte* pCbCrAc_codelen = (byte*)OPtr_CbCrAc_codelen;

                //Y DC len
                int memory_counter = 0; //universal
                for (int i = 0; i < matrixes.Count; i++)
                {
                    if ((i + 1) % 5 == 0 || (i + 1) % 6 == 0) //Cb Cr
                    { }
                    else //Y
                    {
                        int current_dc = (matrixes[i])[0];
                        byte dc_binary_len = (byte)binaryLen((ulong)Math.Abs(current_dc));
                        pYDc_codelen[memory_counter] = dc_binary_len;
                        memory_counter++;
                    }
                }
                //Cb/Cr DC len
                memory_counter = 0;
                for (int i = 0; i < matrixes.Count; i++)
                {
                    if ((i + 1) % 5 == 0 || (i + 1) % 6 == 0) //Cb Cr
                    {
                        int current_dc = (matrixes[i])[0];
                        byte dc_binary_len = (byte)binaryLen((ulong)Math.Abs(current_dc));
                        pCbCrDc_codelen[memory_counter] = dc_binary_len;
                        memory_counter++;
                    }
                    else //Y
                    {
                    }
                }
                //Y AC zeros+len
                memory_counter = 0;
                for (int i = 0; i < matrixes.Count; i++)
                {
                    if ((i + 1) % 5 == 0 || (i + 1) % 6 == 0) //Cb Cr
                    {
                    }
                    else //Y
                    {
                        byte zerosBefore = 0;
                        for (int j = 1; j < (matrixes[i]).Count; j++)
                        {
                            if ((matrixes[i])[j] == 0) //count zeros before coeff
                                zerosBefore++;
                            else //save val
                            {
                                int current_ac = (matrixes[i])[j];
                                byte ac_binary_len = (byte)binaryLen((ulong)Math.Abs(current_ac));
                                byte ac_val = (byte)(((byte)0xF0 & (zerosBefore<<4)) + ((byte)0x0F & ac_binary_len));
                                zerosBefore = 0;
                                pYAc_codelen[memory_counter] = ac_val;
                                memory_counter++;
                            }
                        }
                    }
                }
                pYAc_codelen[memory_counter] = 0x00;
                //Cb/Cr AC zeros+len
                memory_counter = 0;
                for (int i = 0; i < matrixes.Count; i++)
                {
                    if ((i + 1) % 5 == 0 || (i + 1) % 6 == 0) //Cb Cr
                    {
                        byte zerosBefore = 0;
                        for (int j = 1; j < (matrixes[i]).Count; j++)
                        {
                            if ((matrixes[i])[j] == 0) //count zeros before coeff
                                zerosBefore++;
                            else //save val
                            {
                                int current_ac = (matrixes[i])[j];
                                byte ac_binary_len = (byte)binaryLen((ulong)Math.Abs(current_ac));
                                byte ac_val = (byte)(((byte)0xF0 & (zerosBefore<<4)) + ((byte)0x0F & ac_binary_len));
                                zerosBefore = 0;
                                pCbCrAc_codelen[memory_counter] = ac_val;
                                memory_counter++;
                            }
                        }
                    }
                    else //Y
                    {
                    }
                }
                pCbCrAc_codelen[memory_counter] = 0x00;
                memory_counter = 0;
                /*
                 * [getting coeffs itselfs]
                 */
                List<int> YDc = new List<int>();
                List<int> CbCrDc = new List<int>();
                List<int> YAc = new List<int>();
                List<int> CbCrAc = new List<int>();
                //lens in bits
                List<int> YDc_coeff_len = new List<int>();
                List<int> YAc_coeff_len = new List<int>();
                List<int> CbCrDc_coeff_len = new List<int>();
                List<int> CbCrAc_coeff_len = new List<int>();

                //Y DC
                for (int i = 0; i < matrixes.Count; i++)
                {
                    if ((i + 1) % 5 == 0 || (i + 1) % 6 == 0) //Cb Cr
                    { }
                    else //Y
                    {
                        int current_dc = (matrixes[i])[0];
                        byte dc_binary_len = (byte)binaryLen((ulong)Math.Abs(current_dc));
                        YDc_coeff_len.Add(dc_binary_len);
                        if (current_dc < 0)
                            current_dc = (byte)((int)Math.Pow(2, dc_binary_len) - (int)Math.Abs(current_dc) - 1);
                        YDc.Add(current_dc);
                    }
                }
                //CbCr DC
                for (int i = 0; i < matrixes.Count; i++)
                {
                    if ((i + 1) % 5 == 0 || (i + 1) % 6 == 0) //Cb Cr
                    {
                        int current_dc = (matrixes[i])[0];
                        byte dc_binary_len = (byte)binaryLen((ulong)Math.Abs(current_dc));
                        CbCrDc_coeff_len.Add(dc_binary_len);
                        if (current_dc < 0)
                            current_dc = (byte)((int)Math.Pow(2, dc_binary_len) - (int)Math.Abs(current_dc) - 1);
                        CbCrDc.Add(current_dc);
                    }
                    else //Y
                    {}
                }
                //Y AC
                for (int i = 0; i < matrixes.Count; i++)
                {
                    if ((i + 1) % 5 == 0 || (i + 1) % 6 == 0) //Cb Cr
                    {
                    }
                    else //Y
                    {
                        for (int j = 1; j < (matrixes[i]).Count; j++)
                        {
                            //NON-ZEROS
                            if ((matrixes[i])[j] != 0)
                            {
                                int current_ac = (matrixes[i])[j];
                                byte ac_binary_len = (byte)binaryLen((ulong)Math.Abs(current_ac));
                                YAc_coeff_len.Add(ac_binary_len);
                                if (current_ac < 0)
                                    current_ac = (byte)((int)Math.Pow(2, ac_binary_len) - (int)Math.Abs(current_ac) - 1);
                                YAc.Add(current_ac);
                            }
                        }
                        YAc_coeff_len.Add(0);
                        YAc.Add(0x7FFF8080);
                    }
                }
                //CbCr AC
                for (int i = 0; i < matrixes.Count; i++)
                {
                    if ((i + 1) % 5 == 0 || (i + 1) % 6 == 0) //Cb Cr
                    {
                        for (int j = 1; j < (matrixes[i]).Count; j++)
                        {
                            //NON-ZEROS
                            if ((matrixes[i])[j] != 0)
                            {
                                int current_ac = (matrixes[i])[j];
                                byte ac_binary_len = (byte)binaryLen((ulong)Math.Abs(current_ac));
                                CbCrAc_coeff_len.Add(ac_binary_len);
                                if (current_ac < 0)
                                    current_ac = (byte)((int)Math.Pow(2, ac_binary_len) - (int)Math.Abs(current_ac) - 1);
                                CbCrAc.Add(current_ac);
                            }
                        }
                        CbCrAc_coeff_len.Add(0);
                        CbCrAc.Add(0x7FFF8080);
                    }
                    else //Y
                    {
                    }
                }

                /*
                 * 2. [making huffman codes]
                 */
                /*
                 * Allocating memory
                 */
                //Y DC
                IntPtr codes_YDc = Marshal.AllocHGlobal(sizeof(int)); //this is address of array[uint64_t]
                IntPtr codesLen_YDc = Marshal.AllocHGlobal(sizeof(int));
                IntPtr values_YDc = Marshal.AllocHGlobal(sizeof(int)); //this is address of array[char]
                IntPtr valuesLen_YDc = Marshal.AllocHGlobal(sizeof(int));
                //Y AC
                IntPtr codes_YAc = Marshal.AllocHGlobal(sizeof(int)); //this is address of array[uint64_t]
                IntPtr codesLen_YAc = Marshal.AllocHGlobal(sizeof(int));
                IntPtr values_YAc = Marshal.AllocHGlobal(sizeof(int)); //this is address of array[char]
                IntPtr valuesLen_YAc = Marshal.AllocHGlobal(sizeof(int));
                //CbCr DC
                IntPtr codes_CbCrDc = Marshal.AllocHGlobal(sizeof(int)); //this is address of array[uint64_t]
                IntPtr codesLen_CbCrDc = Marshal.AllocHGlobal(sizeof(int));
                IntPtr values_CbCrDc = Marshal.AllocHGlobal(sizeof(int)); //this is address of array[char]
                IntPtr valuesLen_CbCrDc = Marshal.AllocHGlobal(sizeof(int));
                //CbCr AC
                IntPtr codes_CbCrAc = Marshal.AllocHGlobal(sizeof(int)); //this is address of array[uint64_t]
                IntPtr codesLen_CbCrAc = Marshal.AllocHGlobal(sizeof(int));
                IntPtr values_CbCrAc = Marshal.AllocHGlobal(sizeof(int)); //this is address of array[char]
                IntPtr valuesLen_CbCrAc = Marshal.AllocHGlobal(sizeof(int));

                /*
                 * Running huffman
                 */
                Encode_getCodes_Wrapper(OPtr_YDc_codelen.ToInt32(), Y_matrixes_count, codes_YDc.ToInt32(), codesLen_YDc.ToInt32(), values_YDc.ToInt32(), valuesLen_YDc.ToInt32());
                Encode_getCodes_Wrapper(OPtr_YAc_codelen.ToInt32(), AC_slots_for_Y, codes_YAc.ToInt32(), codesLen_YAc.ToInt32(), values_YAc.ToInt32(), valuesLen_YAc.ToInt32());
                Encode_getCodes_Wrapper(OPtr_CbCrDc_codelen.ToInt32(), CbCr_matrixes_count, codes_CbCrDc.ToInt32(), codesLen_CbCrDc.ToInt32(), values_CbCrDc.ToInt32(), valuesLen_CbCrDc.ToInt32());
                Encode_getCodes_Wrapper(OPtr_CbCrAc_codelen.ToInt32(), AC_slots_for_CrCb, codes_CbCrAc.ToInt32(), codesLen_CbCrAc.ToInt32(), values_CbCrAc.ToInt32(), valuesLen_CbCrAc.ToInt32());
                //free source streams
                Marshal.FreeHGlobal(OPtr_YDc_codelen);
                Marshal.FreeHGlobal(OPtr_YAc_codelen);
                Marshal.FreeHGlobal(OPtr_CbCrDc_codelen);
                Marshal.FreeHGlobal(OPtr_CbCrAc_codelen);

                //save
                var huffman_handler

                /*
                 * Saving outputData
                 */
                // Y DC
                int codesLenInt_YDc = Marshal.ReadInt32(codesLen_YDc);
                int valuesLenInt_YDc = Marshal.ReadInt32(valuesLen_YDc);
                ulong* codesArr_YDc = (ulong*)Marshal.ReadInt32(codes_YDc);
                byte* valuesArr_YDc = (byte*)Marshal.ReadInt32(values_YDc);
                // Y AC
                int codesLenInt_YAc = Marshal.ReadInt32(codesLen_YAc);
                int valuesLenInt_YAc = Marshal.ReadInt32(valuesLen_YAc);
                ulong* codesArr_YAc = (ulong*)Marshal.ReadInt32(codes_YAc);
                byte* valuesArr_YAc = (byte*)Marshal.ReadInt32(values_YAc);
                // CbCr DC
                int codesLenInt_CbCrDc = Marshal.ReadInt32(codesLen_CbCrDc);
                int valuesLenInt_CbCrDc = Marshal.ReadInt32(valuesLen_CbCrDc);
                ulong* codesArr_CbCrDc = (ulong*)Marshal.ReadInt32(codes_CbCrDc);
                byte* valuesArr_CbCrDc = (byte*)Marshal.ReadInt32(values_CbCrDc);
                // CbCr AC
                int codesLenInt_CbCrAc = Marshal.ReadInt32(codesLen_CbCrAc);
                int valuesLenInt_CbCrAc = Marshal.ReadInt32(valuesLen_CbCrAc);
                ulong* codesArr_CbCrAc = (ulong*)Marshal.ReadInt32(codes_CbCrAc);
                byte* valuesArr_CbCrAc = (byte*)Marshal.ReadInt32(values_CbCrAc);
                /*
                 * counting amount of codes of certain len
                 */
                int[] code_lens_YDc = new int[16];
                Array.Clear(code_lens_YDc, 0, code_lens_YDc.Length);
                int[] code_lens_YAc = new int[16];
                Array.Clear(code_lens_YAc, 0, code_lens_YAc.Length);
                int[] code_lens_CbCrDc = new int[16];
                Array.Clear(code_lens_CbCrDc, 0, code_lens_CbCrDc.Length);
                int[] code_lens_CbCrAc = new int[16];
                Array.Clear(code_lens_CbCrAc, 0, code_lens_CbCrAc.Length);
                //Y DC
                for (int i = 0; i < codesLenInt_YDc; i++) //run through codes
                {
                    //int temp = (int)codesArr_YDc[i];
                    code_lens_YDc[binaryLen(codesArr_YDc[i])-1]++;
                }
                //Y AC
                for (int i = 0; i < codesLenInt_YAc; i++) //run through codes
                {
                    code_lens_YAc[binaryLen(codesArr_YAc[i]) -1]++;
                }
                //CbCr DC
                for (int i = 0; i < codesLenInt_CbCrDc; i++) //run through codes
                {
                    code_lens_CbCrDc[binaryLen(codesArr_CbCrDc[i]) -1]++;
                }
                //CbCr AC
                for (int i = 0; i < codesLenInt_CbCrAc; i++) //run through codes
                {
                    code_lens_CbCrAc[binaryLen(codesArr_CbCrDc[i]) -1]++;
                }
                /*
                 * Sorting arrays
                 */
                //bubble sort
                byte[] sorted_vals_YDc = bubbleSort(codesArr_YDc, codesLenInt_YDc, valuesArr_YDc, valuesLenInt_YDc);
                byte[] sorted_vals_YAc = bubbleSort(codesArr_YAc, codesLenInt_YAc, valuesArr_YAc, valuesLenInt_YAc);
                byte[] sorted_vals_CbCrDc = bubbleSort(codesArr_CbCrDc, codesLenInt_CbCrDc, valuesArr_CbCrDc, valuesLenInt_CbCrDc);
                byte[] sorted_vals_CbCrAc = bubbleSort(codesArr_CbCrAc, codesLenInt_CbCrAc, valuesArr_CbCrAc, valuesLenInt_CbCrAc);

                /*
                 * Pack back
                 */
                //Forming FFC4 blocks
            //Y DC=============================================================
                byte[] headerFFC4_YDc = new byte[19 + sorted_vals_YDc.Length];
                //length 2b
                if (19 + sorted_vals_YDc.Length < 255)
                {
                    headerFFC4_YDc[0] = (byte)0x00;
                    headerFFC4_YDc[1] = (byte)(19 + sorted_vals_YDc.Length);
                }
                else
                {
                    int diByteLen = 19 + sorted_vals_YDc.Length;
                    headerFFC4_YDc[0] = (byte)(diByteLen >> 8);
                    headerFFC4_YDc[1] = (byte)(diByteLen & 0x00FF);
                }
                //type 1b
                headerFFC4_YDc[2] = 0x00;
                //code lens 16b
                for (int i = 0; i < 16; i++)
                    headerFFC4_YDc[i + 3] = (byte)code_lens_YDc[i];
                //values Nb
                for (int i = 0; i < sorted_vals_YDc.Length; i++)
                    headerFFC4_YDc[i + 19] = sorted_vals_YDc[i];
            //Y AC=============================================================
                byte[] headerFFC4_YAc = new byte[19 + sorted_vals_YAc.Length];
                //length 2b
                if (19 + sorted_vals_YAc.Length < 255)
                {
                    headerFFC4_YAc[0] = (byte)0x00;
                    headerFFC4_YAc[1] = (byte)(19 + sorted_vals_YAc.Length);
                }
                else
                {
                    int diByteLen = 19 + sorted_vals_YAc.Length;
                    headerFFC4_YAc[0] = (byte)(diByteLen >> 8);
                    headerFFC4_YAc[1] = (byte)(diByteLen & 0x00FF);
                }
                //type 1b
                headerFFC4_YAc[2] = 0x10;
                //code lens 16b
                for (int i = 0; i < 16; i++)
                    headerFFC4_YAc[i + 3] = (byte)code_lens_YAc[i];
                //values Nb
                for (int i = 0; i < sorted_vals_YAc.Length; i++)
                    headerFFC4_YAc[i + 19] = sorted_vals_YAc[i];
            //CbCr DC=============================================================
                byte[] headerFFC4_CbCrDc = new byte[19 + sorted_vals_CbCrDc.Length];
                //length 2b
                if (19 + sorted_vals_CbCrDc.Length < 255)
                {
                    headerFFC4_CbCrDc[0] = (byte)0x00;
                    headerFFC4_CbCrDc[1] = (byte)(19 + sorted_vals_CbCrDc.Length);
                }
                else
                {
                    int diByteLen = 19 + sorted_vals_CbCrDc.Length;
                    headerFFC4_CbCrDc[0] = (byte)(diByteLen >> 8);
                    headerFFC4_CbCrDc[1] = (byte)(diByteLen & 0x00FF);
                }
                //type 1b
                headerFFC4_CbCrDc[2] = 0x01;
                //code lens 16b
                for (int i = 0; i < 16; i++)
                    headerFFC4_CbCrDc[i + 3] = (byte)code_lens_CbCrDc[i];
                //values Nb
                for (int i = 0; i < sorted_vals_CbCrDc.Length; i++)
                    headerFFC4_CbCrDc[i + 19] = sorted_vals_CbCrDc[i];
            //CbCr AC=============================================================
                byte[] headerFFC4_CbCrAc = new byte[19 + sorted_vals_CbCrAc.Length];
                //length 2b
                if (19 + sorted_vals_YDc.Length < 255)
                {
                    headerFFC4_CbCrAc[0] = (byte)0x00;
                    headerFFC4_CbCrAc[1] = (byte)(19 + sorted_vals_CbCrAc.Length);
                }
                else
                {
                    int diByteLen = 19 + sorted_vals_CbCrAc.Length;
                    headerFFC4_CbCrAc[0] = (byte)(diByteLen >> 8);
                    headerFFC4_CbCrAc[1] = (byte)(diByteLen & 0x00FF);
                }
                //type 1b
                headerFFC4_CbCrAc[2] = 0x11;
                //code lens 16b
                for (int i = 0; i < 16; i++)
                    headerFFC4_CbCrAc[i + 3] = (byte)code_lens_CbCrAc[i];
                //values Nb
                for (int i = 0; i < sorted_vals_CbCrAc.Length; i++)
                    headerFFC4_CbCrAc[i + 19] = sorted_vals_CbCrAc[i];

                //United FFC4 blocks
                byte[] FFC4Union = new byte[8 + headerFFC4_YDc.Length + headerFFC4_YAc.Length + headerFFC4_CbCrDc.Length + headerFFC4_CbCrAc.Length];
                FFC4Union[0] = 0xFF;
                FFC4Union[1] = 0xC4;
                headerFFC4_YDc.CopyTo(FFC4Union, 2);
                FFC4Union[2 + headerFFC4_YDc.Length] = 0xFF;
                FFC4Union[2 + headerFFC4_YDc.Length + 1] = 0xC4;
                headerFFC4_YAc.CopyTo(FFC4Union, 2 + headerFFC4_YDc.Length + 2);
                FFC4Union[4 + headerFFC4_YDc.Length + headerFFC4_YAc.Length] = 0xFF;
                FFC4Union[4 + headerFFC4_YDc.Length + headerFFC4_YAc.Length + 1] = 0xC4;
                headerFFC4_YDc.CopyTo(FFC4Union, 4 + headerFFC4_YDc.Length + headerFFC4_YAc.Length + 2);
                FFC4Union[6 + headerFFC4_YDc.Length + headerFFC4_YAc.Length + headerFFC4_CbCrDc.Length] = 0xFF;
                FFC4Union[6 + headerFFC4_YDc.Length + headerFFC4_YAc.Length + headerFFC4_CbCrDc.Length + 1] = 0xC4;
                headerFFC4_YDc.CopyTo(FFC4Union, 6 + headerFFC4_YDc.Length + headerFFC4_YAc.Length + headerFFC4_CbCrDc.Length + 2);
                //FFDA block=================================================
                byte[] headerFFDA = new byte[12];
                headerFFDA[0] = 0x00;
                headerFFDA[1] = 0x0C;
                headerFFDA[2] = 0x03;
                headerFFDA[3] = 0x01;
                headerFFDA[4] = 0x00; //CHECK
                headerFFDA[3] = 0x02;
                headerFFDA[4] = 0x11; //CHECK
                headerFFDA[3] = 0x03;
                headerFFDA[4] = 0x11; //CHECK
                headerFFDA[1] = 0x00;
                headerFFDA[2] = 0x3F;
                headerFFDA[3] = 0x00;
                //FFDA DATA===================================================
                //len of stream
                int length_of_FFDA_data = YDc.Count + CbCrDc.Count + YAc.Count + CbCrAc.Count + codesLenInt_YDc + codesLenInt_YAc + codesLenInt_CbCrDc + codesLenInt_CbCrAc;
                //byte[] FFDA_data_stream = new byte[length_of_FFDA_data];
                IntPtr IntPtrFFDA_data_stream = Marshal.AllocHGlobal(length_of_FFDA_data * sizeof(byte));
                byte* FFDA_data_stream = (byte*)IntPtrFFDA_data_stream;
                //List<int> YDc = new List<int>();
                //List<int> CbCrDc = new List<int>();
                //List<int> YAc = new List<int>();
                //List<int> CbCrAc = new List<int>();
                //int codesLenInt_YDc = Marshal.ReadInt32(codesLen_YDc);
                //int codesLenInt_YAc = Marshal.ReadInt32(codesLen_YAc);
                //int codesLenInt_CbCrDc = Marshal.ReadInt32(codesLen_CbCrDc);
                //int codesLenInt_CbCrAc = Marshal.ReadInt32(codesLen_CbCrAc);

                //million super counters (very impotrant please do not delete)
                int write_Dc_matrix_counter = 0;
                int write_Ac_matrix_counter = 0;

                int Y_set_counter = 0;
                int CbCr_set_counter = 0;
                for (int i = 0; i < matrixes.Count/6; i++) // Cycles of (YYYYCbCr)-N times
                {
                    //YYYY-set
                    for (int j = 0; j < 4; j++)
                    {
                        //int huffman_code_len = binaryLenHuffCodes(codesArr_YDc[write_Y_matrix_counter]);
                        //write codesArr_YDc[write_Y_matrix_counter] with len of huffman_code_len
                        //write YDc[write_Y_matrix_counter] with len YDc_coeff_len


                    }
                }


                ulong magic_counter = 0;
                for (int i = 0; i < length_of_FFDA_data; i++)
                {
                    editByte(FFDA_data_stream + (magic_counter / 8), (ulong)YDc[0], (int)(magic_counter % 8), 0, 8);
                    editByte(FFDA_data_stream + (magic_counter / 8) + 1, (ulong)YDc[0], 0, (int)(magic_counter % 8), 8);
                    editByte(FFDA_data_stream + (magic_counter / 8) + 2, (ulong)YDc[0], 0, (int)(magic_counter % 8) + 1, 8);
                }

                /*
                 * Free memory
                 */
                //free ints
                Marshal.FreeHGlobal(codesLen_YDc);
                Marshal.FreeHGlobal(valuesLen_YDc);
                Marshal.FreeHGlobal(codesLen_YAc);
                Marshal.FreeHGlobal(valuesLen_YAc);
                Marshal.FreeHGlobal(codesLen_CbCrDc);
                Marshal.FreeHGlobal(valuesLen_CbCrDc);
                Marshal.FreeHGlobal(codesLen_CbCrAc);
                Marshal.FreeHGlobal(valuesLen_CbCrAc);
                //free pointers
                Free_Encode_getCodes_Wrapper(Marshal.ReadInt32(codes_YDc), Marshal.ReadInt32(values_YDc));
                Free_Encode_getCodes_Wrapper(Marshal.ReadInt32(codes_YAc), Marshal.ReadInt32(values_YAc));
                Free_Encode_getCodes_Wrapper(codes_CbCrDc.ToInt32(), values_CbCrDc.ToInt32()); //fix
                Free_Encode_getCodes_Wrapper(codes_CbCrAc.ToInt32(), values_CbCrAc.ToInt32()); //fix
                return null;
            }
            catch
            {
                return null;
            }
        }

        private unsafe void doStuff()
        {
            //byte[] input = { 0, 1 };
            IntPtr Ptr_input = Marshal.AllocHGlobal(2 * sizeof(byte));
            byte* input = (byte*)Ptr_input;
            input[0] = 0;
            input[1] = 1;

            IntPtr codesLen = Marshal.AllocHGlobal(sizeof(int));
            IntPtr valuesLen = Marshal.AllocHGlobal(sizeof(int));

            IntPtr codes = Marshal.AllocHGlobal(sizeof(int)); //this is address of array[uint16_t]
            IntPtr values = Marshal.AllocHGlobal(sizeof(int)); //this is address of array[char]

            Encode_getCodes_Wrapper(Ptr_input.ToInt32(), 2, codes.ToInt32(), codesLen.ToInt32(), values.ToInt32(), valuesLen.ToInt32());
            int codesLenInt = Marshal.ReadInt32(codesLen);
            int valuesLenInt = Marshal.ReadInt32(valuesLen);
            Console.Write("CodesLen: ");
            Console.WriteLine(codesLenInt);
            Console.Write("ValuesLen: ");
            Console.WriteLine(valuesLenInt);
            ulong* codesArr = (ulong*)Marshal.ReadInt32(codes);
            byte* valuesArr = (byte*)Marshal.ReadInt32(values);

            Console.WriteLine("Code table:");
            Console.WriteLine("__|Value|------------|Code|__");
            for (int i = 0; i < codesLenInt; i++)
            {
                Console.Write("|    ");
                Console.Write(valuesArr[i]);
                Console.Write("  >----------->  ");
                Console.Write(codesArr[i]);
                Console.WriteLine("    |");
            }
            Console.WriteLine("-----------------------------");
        }

        private unsafe void editByte(byte* targetToEdit, ulong value, int offsetByte, int offsetValue, int valueLen)
        {
            ulong temp = value<<(64-valueLen+offsetValue); //maybe Uint64
            temp >>= 56 + offsetByte;
            *targetToEdit += (byte)temp;
        }

        private unsafe byte[] bubbleSort(ulong* codes, int codesLen, byte* values, int valuesLen)
        {
            ulong[] sorted = new ulong[codesLen];
            //copy codes
            for (int i = 0; i < codesLen; i++)
            {
                sorted[i] = (ulong)codes[i];
            }
            byte[] sortedVals = new byte[codesLen];
            //copy vals
            for (int i = 0; i < codesLen; i++)
            {
                sortedVals[i] = values[i];
            }


            ulong temp;
            byte temp_val;
            for (int j = 0; j <= sorted.Length - 2; j++)
            {
                for (int i = 0; i <= sorted.Length - 2; i++)
                {
                    if (sorted[i] > sorted[i + 1])
                    {
                        temp = sorted[i + 1];
                        sorted[i + 1] = sorted[i];
                        sorted[i] = temp;

                        temp_val = sortedVals[i + 1];
                        sortedVals[i + 1] = sortedVals[i];
                        sortedVals[i] = temp_val;
                    }
                }
            }

            return sortedVals;
        }

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
            return Convert.ToString(num, 2).PadLeft(32, '0');
        }

        public class JpegShell
        {
            public unsafe JpegShell(ref List<List<int>> matrixes) 
            {
                //count Y and Cb/Cr matrixes
                int Y_matrixes_count = 0;
                int CbCr_matrixes_count = 0;
                for (int i = 0; i < matrixes.Count; i++)
                {
                    if ((i + 1) % 5 == 0 || (i + 1) % 6 == 0) //Cb Cr
                    {
                        CbCr_matrixes_count++;
                    }
                    else //Y
                    {
                        Y_matrixes_count++;
                    }
                }

                OPtr_YDc_codelen = Marshal.AllocHGlobal(Y_matrixes_count * sizeof(byte));
                byte* pYDc_codelen = (byte*)OPtr_YDc_codelen;
                OPtr_CbCrDc_codelen = Marshal.AllocHGlobal(CbCr_matrixes_count * sizeof(byte));
                byte* pCbCrDc_codelen = (byte*)OPtr_CbCrDc_codelen;
                //count how much memory needed for AC coeffs of Y and CbCr matrix
                int AC_slots_for_Y = 1; //0x00 val
                int AC_slots_for_CrCb = 1; //0x00 val
                for (int i = 0; i < matrixes.Count; i++)
                {
                    if ((i + 1) % 5 == 0 || (i + 1) % 6 == 0) //Cb Cr
                    {
                        for (int j = 1; j < (matrixes[i]).Count; j++)
                        {
                            if ((matrixes[i])[j] != 0) //count coeff
                                AC_slots_for_CrCb++;
                        }
                    }
                    else //Y
                    {
                        for (int j = 1; j < (matrixes[i]).Count; j++)
                        {
                            if ((matrixes[i])[j] != 0) //count coeff
                                AC_slots_for_Y++;
                        }
                    }
                }

                OPtr_YAc_codelen = Marshal.AllocHGlobal(AC_slots_for_Y * sizeof(byte));
                byte* pYAc_codelen = (byte*)OPtr_YAc_codelen;
                OPtr_CbCrAc_codelen = Marshal.AllocHGlobal(AC_slots_for_CrCb * sizeof(byte));
                byte* pCbCrAc_codelen = (byte*)OPtr_CbCrAc_codelen;
            }


            private IntPtr OPtr_YDc_codelen;
            private IntPtr OPtr_CbCrDc_codelen;
            private IntPtr OPtr_YAc_codelen;
            private IntPtr OPtr_CbCrAc_codelen;

        }

        public class HuffmanCode
        {
            public HuffmanCode(ulong code_, int codeLenBits_, byte value_, int valueLenBits_)
            {
                this.code = code_;
                this.code_length_bits = codeLenBits_;
                this.value = value_;
                this.value_length_bits = valueLenBits_;
            }

            public ulong code { get; set; }
            public int code_length_bits { get; set; }
            public byte value { get; set; }
            public int value_length_bits { get; set; }

        }

        public class JpegCodesTable
        {
            public unsafe JpegCodesTable(HuffmanHandler handler_)
            {
                //init Y_DC_Table
                for (int i = 0; i < handler_.codesLenInt_YDc; i++)
                {
                    HuffmanCode code = new HuffmanCode(
                                    handler_.codesArr_YDc[i], 
                                    binaryLenHuffCodes(handler_.codesArr_YDc[i]),
                                    handler_.valuesArr_YDc[i],
                                    binaryLen(handler_.valuesArr_YDc[i]));
                    Y_DC_Table.Add(code);
                }
                //init Y_AC_Table
                for (int i = 0; i < handler_.codesLenInt_YAc; i++)
                {
                    HuffmanCode code = new HuffmanCode(
                                    handler_.codesArr_YAc[i],
                                    binaryLenHuffCodes(handler_.codesArr_YAc[i]),
                                    handler_.valuesArr_YAc[i],
                                    binaryLen(handler_.valuesArr_YAc[i]));
                    Y_AC_Table.Add(code);
                }
                //init CbCr_DC_Table
                for (int i = 0; i < handler_.codesLenInt_CbCrDc; i++)
                {
                    HuffmanCode code = new HuffmanCode(
                                    handler_.codesArr_CbCrDc[i],
                                    binaryLenHuffCodes(handler_.codesArr_CbCrDc[i]),
                                    handler_.valuesArr_CbCrDc[i],
                                    binaryLen(handler_.valuesArr_CbCrDc[i]));
                    CbCr_DC_Table.Add(code);
                }
                //init CbCr_AC_Table
                for (int i = 0; i < handler_.codesLenInt_CbCrAc; i++)
                {
                    HuffmanCode code = new HuffmanCode(
                                    handler_.codesArr_CbCrAc[i],
                                    binaryLenHuffCodes(handler_.codesArr_CbCrAc[i]),
                                    handler_.valuesArr_CbCrAc[i],
                                    binaryLen(handler_.valuesArr_CbCrAc[i]));
                    CbCr_AC_Table.Add(code);
                }
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

            public List<HuffmanCode> Y_DC_Table = new List<HuffmanCode>();
            public List<HuffmanCode> Y_AC_Table = new List<HuffmanCode>();
            public List<HuffmanCode> CbCr_DC_Table = new List<HuffmanCode>();
            public List<HuffmanCode> CbCr_AC_Table = new List<HuffmanCode>();
        }

        public class JpegMemoryHandler
        {
            public JpegMemoryHandler() { }

            public HuffmanMemoryHandler huffman_handler;

        }

        public class HuffmanMemoryHandler
        {
            public HuffmanMemoryHandler() { }

            public unsafe void init_input()
            {

            }
            public unsafe void init_output(IntPtr codes_YDc, IntPtr codesLen_YDc, IntPtr values_YDc, IntPtr valuesLen_YDc,
                                            IntPtr codes_YAc, IntPtr codesLen_YAc, IntPtr values_YAc, IntPtr valuesLen_YAc,
                                            IntPtr codes_CbCrDc, IntPtr codesLen_CbCrDc, IntPtr values_CbCrDc, IntPtr valuesLen_CbCrDc,
                                            IntPtr codes_CbCrAc, IntPtr codesLen_CbCrAc, IntPtr values_CbCrAc, IntPtr valuesLen_CbCrAc)
            {
                // POINTERS ASSIGN
                //Y DC
                this.codesArr_YDc = (ulong*)Marshal.ReadInt32(codes_YDc);
                this.codesLenInt_YDc = Marshal.ReadInt32(codesLen_YDc);
                this.valuesArr_YDc = (byte*)Marshal.ReadInt32(values_YDc);
                this.valuesLenInt_YDc = Marshal.ReadInt32(valuesLen_YDc);
                //Y AC
                this.codesArr_YAc = (ulong*)Marshal.ReadInt32(codes_YAc);
                this.codesLenInt_YAc = Marshal.ReadInt32(codesLen_YAc);
                this.valuesArr_YAc = (byte*)Marshal.ReadInt32(values_YAc);
                this.valuesLenInt_YAc = Marshal.ReadInt32(valuesLen_YAc);
                //CbCr DC
                this.codesArr_CbCrDc = (ulong*)Marshal.ReadInt32(codes_CbCrDc);
                this.codesLenInt_CbCrDc = Marshal.ReadInt32(codesLen_CbCrDc);
                this.valuesArr_CbCrDc = (byte*)Marshal.ReadInt32(values_CbCrDc);
                this.valuesLenInt_CbCrDc = Marshal.ReadInt32(valuesLen_CbCrDc);
                //CbCr AC
                this.codesArr_CbCrAc = (ulong*)Marshal.ReadInt32(codes_CbCrAc);
                this.codesLenInt_CbCrAc = Marshal.ReadInt32(codesLen_CbCrAc);
                this.valuesArr_CbCrAc = (byte*)Marshal.ReadInt32(values_CbCrAc);
                this.valuesLenInt_CbCrAc = Marshal.ReadInt32(valuesLen_CbCrAc);
            }

            //========== HUFFMAN INPUT ==============



            //========== HUFFMAN OUTPUT =============
            //Y DC
            public int codesLenInt_YDc { get; set; }
            public int valuesLenInt_YDc { get; set; }
            public unsafe ulong* codesArr_YDc { get; set; }
            public unsafe byte* valuesArr_YDc { get; set; }

            //Y AC
            public int codesLenInt_YAc { get; set; }
            public int valuesLenInt_YAc { get; set; }
            public unsafe ulong* codesArr_YAc { get; set; }
            public unsafe byte* valuesArr_YAc { get; set; }

            //CbCr DC
            public int codesLenInt_CbCrDc { get; set; }
            public int valuesLenInt_CbCrDc { get; set; }
            public unsafe ulong* codesArr_CbCrDc { get; set; }
            public unsafe byte* valuesArr_CbCrDc { get; set; }

            //CbCr AC
            public int codesLenInt_CbCrAc { get; set; }
            public int valuesLenInt_CbCrAc { get; set; }
            public unsafe ulong* codesArr_CbCrAc { get; set; }
            public unsafe byte* valuesArr_CbCrAc { get; set; }
        }
    }

    public class HuffmanDLLHandler
    {

    }

    public class JpegBuilder
    {
        public JpegBuilder() { }

        public void makeCode_lens(ref List<List<int>> matrixes)
        {
            Y_AC_CodeLens.Add(0x00); //Stop-code val
            CbCr_AC_CodeLens.Add(0x00); //Stop-code val
            for (int i = 0; i < matrixes.Count; i++)
            {
                if ((i + 1) % 5 == 0 || (i + 1) % 6 == 0) //Cb Cr
                {
                    //DC
                    {
                        int current_dc = (matrixes[i])[0];
                        byte dc_binary_len = (byte)binaryLen((ulong)Math.Abs(current_dc));
                        CbCr_DC_CodeLens.Add(dc_binary_len);
                    }
                    //AC
                    {
                        byte zerosBefore = 0;
                        for (int j = 1; j < (matrixes[i]).Count; j++)
                        {
                            if ((matrixes[i])[j] == 0) //count zeros before coeff
                                zerosBefore++;
                            else //save val
                            {
                                int current_ac = (matrixes[i])[j];
                                byte ac_binary_len = (byte)binaryLen((ulong)Math.Abs(current_ac));
                                byte ac_val = (byte)(((byte)0xF0 & (zerosBefore << 4)) + ((byte)0x0F & ac_binary_len));
                                zerosBefore = 0;
                                CbCr_AC_CodeLens.Add(ac_val);
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
                        Y_AC_CodeLens.Add(dc_binary_len);
                    }
                    //AC
                    {
                        byte zerosBefore = 0;
                        for (int j = 1; j < (matrixes[i]).Count; j++)
                        {
                            if ((matrixes[i])[j] == 0) //count zeros before coeff
                                zerosBefore++;
                            else //save val
                            {
                                int current_ac = (matrixes[i])[j];
                                byte ac_binary_len = (byte)binaryLen((ulong)Math.Abs(current_ac));
                                byte ac_val = (byte)(((byte)0xF0 & (zerosBefore << 4)) + ((byte)0x0F & ac_binary_len));
                                zerosBefore = 0;
                                Y_AC_CodeLens.Add(ac_val);
                            }
                        }
                    }
                }
            }
        }

        private int binaryLen(ulong msg)
        {
            int i;
            for (i = 0; msg >= (ulong)Math.Pow(2, i); i++) { }
            return i;
        }

        //========================= DATA ==========================
        public List<byte> Y_DC_CodeLens = new List<byte>();
        public List<byte> Y_AC_CodeLens = new List<byte>();
        public List<byte> CbCr_DC_CodeLens = new List<byte>();
        public List<byte> CbCr_AC_CodeLens = new List<byte>();
    }
}
