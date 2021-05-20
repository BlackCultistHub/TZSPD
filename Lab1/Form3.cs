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

using System.Threading;
using System.Diagnostics;

namespace Lab1
{
    public partial class Form3 : Form
    {
        readonly List<int> arrayP = new List<int>();
        List<int> arrayM = new List<int>();
        bool arrayM_available = true;

        public ArrayList log = new ArrayList();
        string prevousMode = "";
        bool genTimerWasStarted = false;

        public delegate void UpdateLogBoxDelegate();
        public delegate void UpdateInfoDelegate();
        public void InvokeUpdateLogBox()
        {
            logBox.Text = "";
            foreach (string line in log)
            {
                logBox.Text += line + Environment.NewLine;
            }
        }
        public void InvokeUpdateInfo()
        {
            if (comboBox_inputMode.Text != prevousMode)
            {
                switch (comboBox_inputMode.Text)
                {
                    case "Вручную":
                        textBox_randomFrequency.Clear();
                        textBox_randomFrequency.ReadOnly = true;
                        button_generate.Enabled = false;
                        button_calculate.Enabled = true;
                        checkBox_startGenTimer.Enabled = false;
                        dataGridView1.Rows.Clear();
                        dataGridView1.Rows.Add();
                        break;
                    case "Случайно":
                        textBox_randomFrequency.Clear();
                        textBox_randomFrequency.ReadOnly = true;
                        button_generate.Enabled = true;
                        button_calculate.Enabled = true;
                        checkBox_startGenTimer.Enabled = false;
                        dataGridView1.Rows.Clear();
                        break;
                    case "Случайно с частотой":
                        textBox_randomFrequency.ReadOnly = false;
                        textBox_randomFrequency.Text = "500";
                        button_generate.Enabled = false;
                        button_calculate.Enabled = false;
                        checkBox_startGenTimer.Enabled = true;
                        dataGridView1.Rows.Clear();
                        break;
                }
                textBox_randomFrequency.Refresh();
                button_generate.Refresh();
                button_calculate.Refresh();
                checkBox_startGenTimer.Refresh();
                prevousMode = comboBox_inputMode.Text;
            }
            if (checkBox_startGenTimer.Checked && !genTimerWasStarted)
            {
                try
                {
                    timer_valid_check();
                    timer_genCount.Interval = int.Parse(textBox_randomFrequency.Text);
                }
                catch (Exception ex)
                {
                    var logLine = DateTime.Now.ToString() + ": " + ex.Message;
                    log.Add(logLine);
                    File.AppendAllText(Directory.GetCurrentDirectory() + "\\global_log.log", DateTime.Now.ToString() + ": LAB3: " + ex.Message + Environment.NewLine);
                    try
                    {
                        var time_ms = MSSQL_logging.log_error_onTimer(DateTime.Now, "LAB3", "LAB3: " + ex.Message);
                        toolStripStatusLabel1.Text = "Логирование выполнено за " + time_ms + "мс";
                    }
                    catch { }
                    Invoke(new UpdateLogBoxDelegate(InvokeUpdateLogBox));
                }
                finally
                {
                    timer_genCount.Start();
                    genTimerWasStarted = true;
                }
            }
            else if (!checkBox_startGenTimer.Checked && genTimerWasStarted)
            {
                timer_genCount.Stop();
                genTimerWasStarted = false;
            }
        }
        public Form3()
        {
            InitializeComponent();
            comboBox_inputMode.SelectedIndex = 0;
            this.FormBorderStyle = FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.CenterToScreen();

            timer_updInfo.Start();
            prevousMode = comboBox_inputMode.Text;
            for (int i = 0; i < int.Parse(textBox_elementsOfA.Text); i++)
                dataGridView1.Columns.Add("Element_" + i.ToString(), "Element_" + i.ToString());
            dataGridView1.Rows.Add();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            Invoke(new UpdateInfoDelegate(InvokeUpdateInfo));
        }

        private void менюToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Hide();
            var form = new Form_start();
            form.Closed += (s, args) => this.Close();
            form.Show();
        }

        private void сохранитьToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SaveFileDialog saveFileDialog1 = new SaveFileDialog();

            saveFileDialog1.FileName = "lab2.log";
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

        private void загрузитьToolStripMenuItem_Click(object sender, EventArgs e)
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

        private void button_generate_Click(object sender, EventArgs e)
        {
            try
            {
                //validation check
                if (textBox_elementsOfA.TextLength == 0)
                {
                    string message = "Размер массива должен быть указан!";
                    MessageBox.Show(message, "Error.", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    var logLine = DateTime.Now.ToString() + ": " + message;
                    log.Add(logLine);
                    File.AppendAllText(Directory.GetCurrentDirectory() + "\\global_log.log", DateTime.Now.ToString() + ": LAB3: " + message + Environment.NewLine);
                    try
                    {
                        var time_ms = MSSQL_logging.log_error_onTimer(DateTime.Now, "LAB3", "LAB3: " + message);
                        toolStripStatusLabel1.Text = "Логирование выполнено за " + time_ms + "мс";
                    }
                    catch { }
                    Invoke(new UpdateLogBoxDelegate(InvokeUpdateLogBox));
                    return;
                }
                if (textBox_gen_max.TextLength == 0)
                {
                    string message = "Максимум генерации должен быть указан!";
                    MessageBox.Show(message, "Error.", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    var logLine = DateTime.Now.ToString() + ": " + message;
                    log.Add(logLine);
                    File.AppendAllText(Directory.GetCurrentDirectory() + "\\global_log.log", DateTime.Now.ToString() + ": LAB3: " + message + Environment.NewLine);
                    try
                    {
                        var time_ms = MSSQL_logging.log_error_onTimer(DateTime.Now, "LAB3", "LAB3: " + message);
                        toolStripStatusLabel1.Text = "Логирование выполнено за " + time_ms + "мс";
                    }
                    catch { }
                    Invoke(new UpdateLogBoxDelegate(InvokeUpdateLogBox));
                    return;
                }
                if (textBox_gen_min.TextLength == 0)
                {
                    string message = "Минимум генерации должен быть указан!";
                    MessageBox.Show(message, "Error.", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    var logLine = DateTime.Now.ToString() + ": " + message;
                    log.Add(logLine);
                    File.AppendAllText(Directory.GetCurrentDirectory() + "\\global_log.log", DateTime.Now.ToString() + ": LAB3: " + message + Environment.NewLine);
                    try
                    {
                        var time_ms = MSSQL_logging.log_error_onTimer(DateTime.Now, "LAB3", "LAB3: " + message);
                        toolStripStatusLabel1.Text = "Логирование выполнено за " + time_ms + "мс";
                    }
                    catch { }
                    Invoke(new UpdateLogBoxDelegate(InvokeUpdateLogBox));
                    return;
                }
                //logic
                button_calculate.Enabled = true;
                button_calculate.Refresh();
                //defines
                int cells_per_row = Math.Abs(int.Parse(textBox_elementsOfA.Text));
                int gen_min = int.Parse(textBox_gen_min.Text);
                int gen_max = int.Parse(textBox_gen_max.Text);
                //0. clear
                dataGridView1.Rows.Clear();
                arrayP.Clear();
                arrayM.Clear();
                //1. generate new
                dataGridView1.Rows.Add();
                var rnd = new Random();
                for (int i = 0; i < cells_per_row; i++)
                {
                    var newNumb = rnd.Next(gen_min, gen_max);
                    arrayP.Add(newNumb);
                    arrayM.Add(newNumb);
                    dataGridView1.Rows[0].Cells[i].Value = newNumb;
                }
            }
            catch (Exception ex)
            {
                var logLine = DateTime.Now.ToString() + ": " + ex.Message;
                log.Add(logLine);
                File.AppendAllText(Directory.GetCurrentDirectory() + "\\global_log.log", DateTime.Now.ToString() + ": LAB3: " + ex.Message + Environment.NewLine);
                try
                {
                    var time_ms = MSSQL_logging.log_error_onTimer(DateTime.Now, "LAB3", "LAB3: " + ex.Message);
                    toolStripStatusLabel1.Text = "Логирование выполнено за " + time_ms + "мс";
                }
                catch { }
                Invoke(new UpdateLogBoxDelegate(InvokeUpdateLogBox));
            }
        }

        private void timer_valid_check()
        {
            //validation check
            if (textBox_elementsOfA.TextLength == 0)
            {
                string message = "Размер массива должен быть указан!";
                var logLine = DateTime.Now.ToString() + ": " + message;
                log.Add(logLine);
                File.AppendAllText(Directory.GetCurrentDirectory() + "\\global_log.log", DateTime.Now.ToString() + ": LAB3: " + message + Environment.NewLine);
                try
                {
                    var time_ms = MSSQL_logging.log_error_onTimer(DateTime.Now, "LAB3", "LAB3: " + message);
                    toolStripStatusLabel1.Text = "Логирование выполнено за " + time_ms + "мс";
                }
                catch { }
                Invoke(new UpdateLogBoxDelegate(InvokeUpdateLogBox));
                checkBox_startGenTimer.Checked = false;
                checkBox_startGenTimer.Refresh();
                textBox_elementsOfA.Text = "20";
                textBox_elementsOfA.Refresh();
                MessageBox.Show(message, "Error.", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            if (textBox_gen_max.TextLength == 0)
            {
                string message = "Максимум генерации должен быть указан!";
                var logLine = DateTime.Now.ToString() + ": " + message;
                log.Add(logLine);
                File.AppendAllText(Directory.GetCurrentDirectory() + "\\global_log.log", DateTime.Now.ToString() + ": LAB3: " + message + Environment.NewLine);
                try
                {
                    var time_ms = MSSQL_logging.log_error_onTimer(DateTime.Now, "LAB3", "LAB3: " + message);
                    toolStripStatusLabel1.Text = "Логирование выполнено за " + time_ms + "мс";
                }
                catch { }
                Invoke(new UpdateLogBoxDelegate(InvokeUpdateLogBox));
                checkBox_startGenTimer.Checked = false;
                checkBox_startGenTimer.Refresh();
                textBox_gen_max.Text = "100";
                textBox_gen_max.Refresh();
                MessageBox.Show(message, "Error.", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            if (textBox_gen_min.TextLength == 0)
            {
                string message = "Минимум генерации должен быть указан!";
                var logLine = DateTime.Now.ToString() + ": " + message;
                log.Add(logLine);
                File.AppendAllText(Directory.GetCurrentDirectory() + "\\global_log.log", DateTime.Now.ToString() + ": LAB3: " + message + Environment.NewLine);
                try
                {
                    var time_ms = MSSQL_logging.log_error_onTimer(DateTime.Now, "LAB3", "LAB3: " + message);
                    toolStripStatusLabel1.Text = "Логирование выполнено за " + time_ms + "мс";
                }
                catch { }
                Invoke(new UpdateLogBoxDelegate(InvokeUpdateLogBox));
                checkBox_startGenTimer.Checked = false;
                checkBox_startGenTimer.Refresh();
                textBox_gen_min.Text = "-100";
                textBox_gen_min.Refresh();
                MessageBox.Show(message, "Error.", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            if (textBox_randomFrequency.TextLength == 0)
            {
                string message = "Частота должна быть указана!";
                var logLine = DateTime.Now.ToString() + ": " + message;
                log.Add(logLine);
                File.AppendAllText(Directory.GetCurrentDirectory() + "\\global_log.log", DateTime.Now.ToString() + ": LAB3: " + message + Environment.NewLine);
                try
                {
                    var time_ms = MSSQL_logging.log_error_onTimer(DateTime.Now, "LAB3", "LAB3: " + message);
                    toolStripStatusLabel1.Text = "Логирование выполнено за " + time_ms + "мс";
                }
                catch { }
                Invoke(new UpdateLogBoxDelegate(InvokeUpdateLogBox));
                checkBox_startGenTimer.Checked = false;
                checkBox_startGenTimer.Refresh();
                textBox_randomFrequency.Text = "500";
                textBox_randomFrequency.Refresh();
                MessageBox.Show(message, "Error.", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
        }
        private void timer_genCount_Tick(object sender, EventArgs e)
        {
            try
            {
                var timerLinear = new Stopwatch();
                var timerParallel = new Stopwatch();
                //logic
                //defines
                int cells_per_row = Math.Abs(int.Parse(textBox_elementsOfA.Text));
                int gen_min = int.Parse(textBox_gen_min.Text);
                int gen_max = int.Parse(textBox_gen_max.Text);
                //0. clear
                dataGridView1.Rows.Clear();
                //1. generate new
                dataGridView1.Rows.Add();
                var rnd = new Random();
                for (int i = 0; i < cells_per_row; i++)
                {
                    var newNumb = rnd.Next(gen_min, gen_max);
                    arrayP.Add(newNumb);
                    arrayM.Add(newNumb);
                    dataGridView1.Rows[0].Cells[i].Value = newNumb;
                }
                //count linear
                timerLinear.Start();
                countPart1();
                countPart2();
                countPart3();
                countPart4();
                timerLinear.Stop();
                textBox_time_linear.Text = timerLinear.ElapsedTicks.ToString() + " тиков";
                textBox_time_linear.Refresh();
                //count parallel
                arrayM.Clear();
                arrayM.AddRange(arrayP.ToArray());
                timerParallel.Start();
                dataGridView1.Rows.Add();
                Thread f1 = new Thread(new ThreadStart(countPart1));
                Thread f2 = new Thread(new ThreadStart(countPart2));
                f1.Start();
                f2.Start();
                f1.Join();
                f2.Join();
                Thread f21 = new Thread(new ThreadStart(countPart3));
                Thread f22 = new Thread(new ThreadStart(countPart4));
                f21.Start();
                f21.Join();
                f22.Start();
                f22.Join();
                timerParallel.Stop();
                textBox_time_parallel.Text = timerParallel.ElapsedTicks.ToString() + " тиков";
                textBox_time_parallel.Refresh();

                //cut
                for (int i = 0; i < arrayM.Count; i++)
                {
                    dataGridView1.Rows[1].Cells[i].Value = arrayM[i];
                }
                arrayP.Clear();
                arrayM.Clear();
                Invoke(new UpdateLogBoxDelegate(InvokeUpdateLogBox));
            }
            catch (Exception ex)
            {
                var logLine = DateTime.Now.ToString() + ": " + ex.Message;
                log.Add(logLine);
                File.AppendAllText(Directory.GetCurrentDirectory() + "\\global_log.log", DateTime.Now.ToString() + ": LAB3: " + ex.Message + Environment.NewLine);
                try
                {
                    var time_ms = MSSQL_logging.log_error_onTimer(DateTime.Now, "LAB3", "LAB3: " + ex.Message);
                    toolStripStatusLabel1.Text = "Логирование выполнено за " + time_ms + "мс";
                }
                catch { }
                Invoke(new UpdateLogBoxDelegate(InvokeUpdateLogBox));
            }
        }

        private void button_calculate_Click(object sender, EventArgs e)
        {
            try
            {
                if (comboBox_inputMode.SelectedIndex == 0)
                    foreach (DataGridViewCell cell in dataGridView1.Rows[0].Cells){
                        if (cell.Value == null)
                            throw new Exception("Входной массив заполнен неверно!");
                        arrayP.Add(Int32.Parse(cell.Value.ToString()));
                        arrayM.Add(Int32.Parse(cell.Value.ToString()));
                    }
                if (arrayP.Count == 0 || arrayM.Count == 0)
                    throw new Exception("Необходимо заполнить или сгенерировать исходный массив!");
                //logic
                var timerLinear = new Stopwatch();
                var timerParallel = new Stopwatch();
                button_calculate.Enabled = false;
                button_calculate.Refresh();
                //count linear
                //save temp
                timerLinear.Start();
                countPart1();
                countPart2();
                countPart3();
                countPart4();
                timerLinear.Stop();
                textBox_time_linear.Text = timerLinear.ElapsedTicks.ToString() + " тиков";
                textBox_time_linear.Refresh();
                //count parallel
                arrayM.Clear();
                arrayM.AddRange(arrayP.ToArray());
                timerParallel.Start();
                dataGridView1.Rows.Add();
                Thread f1 = new Thread(new ThreadStart(countPart1));
                Thread f2 = new Thread(new ThreadStart(countPart2));
                f1.Start();
                f2.Start();
                f1.Join();
                f2.Join();
                Thread f21 = new Thread(new ThreadStart(countPart3));
                Thread f22 = new Thread(new ThreadStart(countPart4));
                f21.Start();
                f21.Join();
                f22.Start();
                f22.Join();
                timerParallel.Stop();
                textBox_time_parallel.Text = timerParallel.ElapsedTicks.ToString() + " тиков";
                textBox_time_parallel.Refresh();

                //cut
                for (int i = 0; i < arrayM.Count; i++)
                {
                    dataGridView1.Rows[1].Cells[i].Value = arrayM[i];
                }
                arrayP.Clear();
                arrayM.Clear();
                Invoke(new UpdateLogBoxDelegate(InvokeUpdateLogBox));
            }
            catch (Exception ex)
            {
                dataGridView1.Rows.Clear();
                dataGridView1.Rows.Add();
                var logLine = DateTime.Now.ToString() + ": " + ex.Message;
                log.Add(logLine);
                File.AppendAllText(Directory.GetCurrentDirectory() + "\\global_log.log", DateTime.Now.ToString() + ": LAB3: " + ex.Message + Environment.NewLine);
                try
                {
                    var time_ms = MSSQL_logging.log_error_onTimer(DateTime.Now, "LAB3", "LAB3: " + ex.Message);
                    toolStripStatusLabel1.Text = "Логирование выполнено за " + time_ms + "мс";
                }
                catch { }
                Invoke(new UpdateLogBoxDelegate(InvokeUpdateLogBox));
            }
        }

        private void countPart1()
        {
            try
            {
                for (int i = 0; i < arrayP.Count; i += 2)
                {
                    while (!arrayM_available) { }
                    arrayM_available = false; //LOCK
                    arrayM[i] = i * arrayP[i];
                    arrayM_available = true; //UNLOCK
                }
                return;
            }
            catch (Exception ex)
            {
                var logLine = DateTime.Now.ToString() + ": " + ex.Message;
                log.Add(logLine);
                File.AppendAllText(Directory.GetCurrentDirectory() + "\\global_log.log", DateTime.Now.ToString() + ": LAB3: " + ex.Message + Environment.NewLine);
                if ((new Form_Params()).dataBaseEndabled())
                {
                    if (!DatabaseOperations.log_error("LAB3", ex.Message))
                    {
                        var logLineDB = DateTime.Now.ToString() + "Не удалось выполнить операцию логирования в БД.";
                        log.Add(logLineDB);
                        File.AppendAllText(Directory.GetCurrentDirectory() + "\\global_log.log", DateTime.Now.ToString() + ": LAB3: Не удалось выполнить операцию логирования в БД." + Environment.NewLine);
                    }
                }
                arrayM_available = true;
                return;
            }
        }

        private void countPart2()
        {
            try
            {
                for (int i = 1; i < arrayP.Count; i += 2)
                {
                    while (!arrayM_available) { }
                    arrayM_available = false; //LOCK
                    arrayM[i] = 0 - arrayP[i];
                    arrayM_available = true; //UNLOCK
                }
                return;
            }
            catch (Exception ex)
            {
                var logLine = DateTime.Now.ToString() + ": " + ex.Message;
                log.Add(logLine);
                File.AppendAllText(Directory.GetCurrentDirectory() + "\\global_log.log", DateTime.Now.ToString() + ": LAB3: " + ex.Message + Environment.NewLine);
                try
                {
                    var time_ms = MSSQL_logging.log_error_onTimer(DateTime.Now, "LAB3", "LAB3: " + ex.Message);
                    toolStripStatusLabel1.Text = "Логирование выполнено за " + time_ms + "мс";
                }
                catch { }
                arrayM_available = true;
                return;
            }
        }

        private void countPart3()
        {
            try
            {
                while (!arrayM_available) { }
                arrayM_available = false; //LOCK
                //find first <0 elem
                int index = 0;
                for (int i = 0; i < arrayM.Count; i++)
                    if (arrayM[i] < 0){ 
                        index = i;
                        break;
                    }
                arrayM[index] = 0;
                arrayM_available = true; //UNLOCK
                return;
            }
            catch (Exception ex)
            {
                var logLine = DateTime.Now.ToString() + ": " + ex.Message;
                log.Add(logLine);
                File.AppendAllText(Directory.GetCurrentDirectory() + "\\global_log.log", DateTime.Now.ToString() + ": LAB3: " + ex.Message + Environment.NewLine);
                try
                {
                    var time_ms = MSSQL_logging.log_error_onTimer(DateTime.Now, "LAB3", "LAB3: " + ex.Message);
                    toolStripStatusLabel1.Text = "Логирование выполнено за " + time_ms + "мс";
                }
                catch { }
                arrayM_available = true;
                return;
            }
        }

        private void countPart4()
        {
            try
            {
                while (!arrayM_available) { }
                arrayM_available = false; //LOCK
                //every M[i] mod 3 == 0 --> M[i] = M[i] * M[2]
                for (int i = 0; i < arrayM.Count; i++)
                    if (arrayM[i] % 3 == 0)
                        arrayM[i] *= arrayM[2];
                arrayM_available = true; //UNLOCK
                return;
            }
            catch (Exception ex)
            {
                var logLine = DateTime.Now.ToString() + ": " + ex.Message;
                log.Add(logLine);
                File.AppendAllText(Directory.GetCurrentDirectory() + "\\global_log.log", DateTime.Now.ToString() + ": LAB3: " + ex.Message + Environment.NewLine);
                try
                {
                    var time_ms = MSSQL_logging.log_error_onTimer(DateTime.Now, "LAB3", "LAB3: " + ex.Message);
                    toolStripStatusLabel1.Text = "Логирование выполнено за " + time_ms + "мс";
                }
                catch { }
                arrayM_available = true;
                return;
            }
        }

        private void dataGridView1_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            if (comboBox_inputMode.Text == "Вручную")
            {
                button_calculate.Enabled = true;
                button_calculate.Refresh();
            }
        }
    }
}
