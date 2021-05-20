namespace Lab1
{
    partial class Form3
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form3));
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.менюToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.логToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.сохранитьToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.загрузитьToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.справкаToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.comboBox_inputMode = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.textBox_randomFrequency = new System.Windows.Forms.TextBox();
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.dataSetBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.label4 = new System.Windows.Forms.Label();
            this.logBox = new System.Windows.Forms.TextBox();
            this.timer_updInfo = new System.Windows.Forms.Timer(this.components);
            this.button_generate = new System.Windows.Forms.Button();
            this.button_calculate = new System.Windows.Forms.Button();
            this.timer_genCount = new System.Windows.Forms.Timer(this.components);
            this.checkBox_startGenTimer = new System.Windows.Forms.CheckBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.textBox_gen_max = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.textBox_gen_min = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.textBox_elementsOfA = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.textBox_time_linear = new System.Windows.Forms.TextBox();
            this.textBox_time_parallel = new System.Windows.Forms.TextBox();
            this.label9 = new System.Windows.Forms.Label();
            this.label10 = new System.Windows.Forms.Label();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.toolStripStatusLabel1 = new System.Windows.Forms.ToolStripStatusLabel();
            this.menuStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataSetBindingSource)).BeginInit();
            this.groupBox1.SuspendLayout();
            this.statusStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // menuStrip1
            // 
            this.menuStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.менюToolStripMenuItem,
            this.логToolStripMenuItem,
            this.справкаToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Padding = new System.Windows.Forms.Padding(4, 2, 0, 2);
            this.menuStrip1.Size = new System.Drawing.Size(701, 24);
            this.menuStrip1.TabIndex = 0;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // менюToolStripMenuItem
            // 
            this.менюToolStripMenuItem.Name = "менюToolStripMenuItem";
            this.менюToolStripMenuItem.Size = new System.Drawing.Size(53, 20);
            this.менюToolStripMenuItem.Text = "Меню";
            this.менюToolStripMenuItem.Click += new System.EventHandler(this.менюToolStripMenuItem_Click);
            // 
            // логToolStripMenuItem
            // 
            this.логToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.сохранитьToolStripMenuItem,
            this.загрузитьToolStripMenuItem});
            this.логToolStripMenuItem.Name = "логToolStripMenuItem";
            this.логToolStripMenuItem.Size = new System.Drawing.Size(39, 20);
            this.логToolStripMenuItem.Text = "Лог";
            // 
            // сохранитьToolStripMenuItem
            // 
            this.сохранитьToolStripMenuItem.Name = "сохранитьToolStripMenuItem";
            this.сохранитьToolStripMenuItem.Size = new System.Drawing.Size(133, 22);
            this.сохранитьToolStripMenuItem.Text = "Сохранить";
            this.сохранитьToolStripMenuItem.Click += new System.EventHandler(this.сохранитьToolStripMenuItem_Click);
            // 
            // загрузитьToolStripMenuItem
            // 
            this.загрузитьToolStripMenuItem.Name = "загрузитьToolStripMenuItem";
            this.загрузитьToolStripMenuItem.Size = new System.Drawing.Size(133, 22);
            this.загрузитьToolStripMenuItem.Text = "Загрузить";
            this.загрузитьToolStripMenuItem.Click += new System.EventHandler(this.загрузитьToolStripMenuItem_Click);
            // 
            // справкаToolStripMenuItem
            // 
            this.справкаToolStripMenuItem.Name = "справкаToolStripMenuItem";
            this.справкаToolStripMenuItem.Size = new System.Drawing.Size(65, 20);
            this.справкаToolStripMenuItem.Text = "Справка";
            this.справкаToolStripMenuItem.Click += new System.EventHandler(this.справкаToolStripMenuItem_Click);
            // 
            // comboBox_inputMode
            // 
            this.comboBox_inputMode.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBox_inputMode.FormattingEnabled = true;
            this.comboBox_inputMode.Items.AddRange(new object[] {
            "Вручную",
            "Случайно",
            "Случайно с частотой"});
            this.comboBox_inputMode.Location = new System.Drawing.Point(20, 66);
            this.comboBox_inputMode.Margin = new System.Windows.Forms.Padding(2);
            this.comboBox_inputMode.Name = "comboBox_inputMode";
            this.comboBox_inputMode.Size = new System.Drawing.Size(146, 21);
            this.comboBox_inputMode.TabIndex = 1;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(20, 47);
            this.label1.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(82, 13);
            this.label1.TabIndex = 2;
            this.label1.Text = "Вариант ввода";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(174, 47);
            this.label2.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(69, 13);
            this.label2.TabIndex = 4;
            this.label2.Text = "Частота, мс";
            // 
            // textBox_randomFrequency
            // 
            this.textBox_randomFrequency.Location = new System.Drawing.Point(176, 67);
            this.textBox_randomFrequency.Margin = new System.Windows.Forms.Padding(2);
            this.textBox_randomFrequency.Name = "textBox_randomFrequency";
            this.textBox_randomFrequency.ReadOnly = true;
            this.textBox_randomFrequency.Size = new System.Drawing.Size(146, 20);
            this.textBox_randomFrequency.TabIndex = 5;
            // 
            // dataGridView1
            // 
            this.dataGridView1.AllowUserToAddRows = false;
            this.dataGridView1.AllowUserToDeleteRows = false;
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Location = new System.Drawing.Point(20, 90);
            this.dataGridView1.Margin = new System.Windows.Forms.Padding(2);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.RowHeadersVisible = false;
            this.dataGridView1.RowHeadersWidth = 51;
            this.dataGridView1.RowTemplate.Height = 24;
            this.dataGridView1.Size = new System.Drawing.Size(484, 135);
            this.dataGridView1.TabIndex = 6;
            this.dataGridView1.CellValueChanged += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridView1_CellValueChanged);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(24, 230);
            this.label4.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(65, 13);
            this.label4.TabIndex = 14;
            this.label4.Text = "Лог сессии";
            // 
            // logBox
            // 
            this.logBox.Location = new System.Drawing.Point(20, 246);
            this.logBox.Margin = new System.Windows.Forms.Padding(2);
            this.logBox.Multiline = true;
            this.logBox.Name = "logBox";
            this.logBox.ReadOnly = true;
            this.logBox.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.logBox.Size = new System.Drawing.Size(485, 128);
            this.logBox.TabIndex = 13;
            // 
            // timer_updInfo
            // 
            this.timer_updInfo.Interval = 250;
            this.timer_updInfo.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // button_generate
            // 
            this.button_generate.Enabled = false;
            this.button_generate.Location = new System.Drawing.Point(327, 47);
            this.button_generate.Margin = new System.Windows.Forms.Padding(2);
            this.button_generate.Name = "button_generate";
            this.button_generate.Size = new System.Drawing.Size(92, 38);
            this.button_generate.TabIndex = 15;
            this.button_generate.Text = "Сгенерировать";
            this.button_generate.UseVisualStyleBackColor = true;
            this.button_generate.Click += new System.EventHandler(this.button_generate_Click);
            // 
            // button_calculate
            // 
            this.button_calculate.Location = new System.Drawing.Point(423, 47);
            this.button_calculate.Margin = new System.Windows.Forms.Padding(2);
            this.button_calculate.Name = "button_calculate";
            this.button_calculate.Size = new System.Drawing.Size(80, 38);
            this.button_calculate.TabIndex = 16;
            this.button_calculate.Text = "Вычислить";
            this.button_calculate.UseVisualStyleBackColor = true;
            this.button_calculate.Click += new System.EventHandler(this.button_calculate_Click);
            // 
            // timer_genCount
            // 
            this.timer_genCount.Tick += new System.EventHandler(this.timer_genCount_Tick);
            // 
            // checkBox_startGenTimer
            // 
            this.checkBox_startGenTimer.AutoSize = true;
            this.checkBox_startGenTimer.Enabled = false;
            this.checkBox_startGenTimer.Location = new System.Drawing.Point(270, 46);
            this.checkBox_startGenTimer.Margin = new System.Windows.Forms.Padding(2);
            this.checkBox_startGenTimer.Name = "checkBox_startGenTimer";
            this.checkBox_startGenTimer.Size = new System.Drawing.Size(55, 17);
            this.checkBox_startGenTimer.TabIndex = 17;
            this.checkBox_startGenTimer.Text = "Старт";
            this.checkBox_startGenTimer.UseVisualStyleBackColor = true;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.textBox_gen_max);
            this.groupBox1.Controls.Add(this.label6);
            this.groupBox1.Controls.Add(this.textBox_gen_min);
            this.groupBox1.Controls.Add(this.label5);
            this.groupBox1.Controls.Add(this.textBox_elementsOfA);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Location = new System.Drawing.Point(508, 47);
            this.groupBox1.Margin = new System.Windows.Forms.Padding(2);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Padding = new System.Windows.Forms.Padding(2);
            this.groupBox1.Size = new System.Drawing.Size(183, 178);
            this.groupBox1.TabIndex = 18;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Настройки";
            // 
            // textBox_gen_max
            // 
            this.textBox_gen_max.Location = new System.Drawing.Point(8, 108);
            this.textBox_gen_max.Margin = new System.Windows.Forms.Padding(2);
            this.textBox_gen_max.Name = "textBox_gen_max";
            this.textBox_gen_max.Size = new System.Drawing.Size(172, 20);
            this.textBox_gen_max.TabIndex = 22;
            this.textBox_gen_max.Text = "100";
            this.textBox_gen_max.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(5, 92);
            this.label6.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(117, 13);
            this.label6.TabIndex = 23;
            this.label6.Text = "Максимум генерации";
            // 
            // textBox_gen_min
            // 
            this.textBox_gen_min.Location = new System.Drawing.Point(8, 72);
            this.textBox_gen_min.Margin = new System.Windows.Forms.Padding(2);
            this.textBox_gen_min.Name = "textBox_gen_min";
            this.textBox_gen_min.Size = new System.Drawing.Size(172, 20);
            this.textBox_gen_min.TabIndex = 20;
            this.textBox_gen_min.Text = "-100";
            this.textBox_gen_min.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(5, 55);
            this.label5.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(111, 13);
            this.label5.TabIndex = 21;
            this.label5.Text = "Минимум генерации";
            // 
            // textBox_elementsOfA
            // 
            this.textBox_elementsOfA.Location = new System.Drawing.Point(7, 35);
            this.textBox_elementsOfA.Margin = new System.Windows.Forms.Padding(2);
            this.textBox_elementsOfA.Name = "textBox_elementsOfA";
            this.textBox_elementsOfA.Size = new System.Drawing.Size(172, 20);
            this.textBox_elementsOfA.TabIndex = 19;
            this.textBox_elementsOfA.Text = "20";
            this.textBox_elementsOfA.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(4, 19);
            this.label3.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(181, 13);
            this.label3.TabIndex = 19;
            this.label3.Text = "Количество элементов массива P";
            // 
            // textBox_time_linear
            // 
            this.textBox_time_linear.Location = new System.Drawing.Point(512, 279);
            this.textBox_time_linear.Margin = new System.Windows.Forms.Padding(2);
            this.textBox_time_linear.Multiline = true;
            this.textBox_time_linear.Name = "textBox_time_linear";
            this.textBox_time_linear.ReadOnly = true;
            this.textBox_time_linear.Size = new System.Drawing.Size(172, 25);
            this.textBox_time_linear.TabIndex = 19;
            // 
            // textBox_time_parallel
            // 
            this.textBox_time_parallel.Location = new System.Drawing.Point(512, 322);
            this.textBox_time_parallel.Margin = new System.Windows.Forms.Padding(2);
            this.textBox_time_parallel.Multiline = true;
            this.textBox_time_parallel.Name = "textBox_time_parallel";
            this.textBox_time_parallel.ReadOnly = true;
            this.textBox_time_parallel.Size = new System.Drawing.Size(172, 25);
            this.textBox_time_parallel.TabIndex = 29;
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(510, 262);
            this.label9.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(161, 13);
            this.label9.TabIndex = 30;
            this.label9.Text = "Время линейного выполнения";
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(510, 305);
            this.label10.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(185, 13);
            this.label10.TabIndex = 31;
            this.label10.Text = "Время параллельного выполнения";
            // 
            // statusStrip1
            // 
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripStatusLabel1});
            this.statusStrip1.Location = new System.Drawing.Point(0, 384);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(701, 22);
            this.statusStrip1.TabIndex = 32;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // toolStripStatusLabel1
            // 
            this.toolStripStatusLabel1.Name = "toolStripStatusLabel1";
            this.toolStripStatusLabel1.Size = new System.Drawing.Size(118, 17);
            this.toolStripStatusLabel1.Text = "toolStripStatusLabel1";
            // 
            // Form3
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(701, 406);
            this.Controls.Add(this.statusStrip1);
            this.Controls.Add(this.label10);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.label9);
            this.Controls.Add(this.textBox_time_parallel);
            this.Controls.Add(this.checkBox_startGenTimer);
            this.Controls.Add(this.textBox_time_linear);
            this.Controls.Add(this.button_calculate);
            this.Controls.Add(this.button_generate);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.logBox);
            this.Controls.Add(this.dataGridView1);
            this.Controls.Add(this.textBox_randomFrequency);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.comboBox_inputMode);
            this.Controls.Add(this.menuStrip1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MainMenuStrip = this.menuStrip1;
            this.Margin = new System.Windows.Forms.Padding(2);
            this.Name = "Form3";
            this.Text = "Потоки";
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataSetBindingSource)).EndInit();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem менюToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem логToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem сохранитьToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem загрузитьToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem справкаToolStripMenuItem;
        private System.Windows.Forms.ComboBox comboBox_inputMode;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox textBox_randomFrequency;
        private System.Windows.Forms.DataGridView dataGridView1;
        private System.Windows.Forms.BindingSource dataSetBindingSource;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox logBox;
        private System.Windows.Forms.Timer timer_updInfo;
        private System.Windows.Forms.Button button_generate;
        private System.Windows.Forms.Button button_calculate;
        private System.Windows.Forms.Timer timer_genCount;
        private System.Windows.Forms.CheckBox checkBox_startGenTimer;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.TextBox textBox_gen_max;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox textBox_gen_min;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox textBox_elementsOfA;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox textBox_time_linear;
        private System.Windows.Forms.TextBox textBox_time_parallel;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel1;
    }
}