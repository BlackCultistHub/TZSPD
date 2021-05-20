namespace Lab1
{
    partial class Form2_tz
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form2_tz));
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.менюToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.файлToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.сохранитьЛогToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.загрузитьЛогToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.справкаToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.label14 = new System.Windows.Forms.Label();
            this.logBox = new System.Windows.Forms.TextBox();
            this.button1 = new System.Windows.Forms.Button();
            this.Container = new System.Windows.Forms.PictureBox();
            this.label1 = new System.Windows.Forms.Label();
            this.SteganoContainer = new System.Windows.Forms.PictureBox();
            this.button2 = new System.Windows.Forms.Button();
            this.button3 = new System.Windows.Forms.Button();
            this.button4 = new System.Windows.Forms.Button();
            this.button5 = new System.Windows.Forms.Button();
            this.textBox_message = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.textBox_maxMsgLen = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.label_loading_container = new System.Windows.Forms.Label();
            this.label_loading_stegano = new System.Windows.Forms.Label();
            this.progress_steg = new System.Windows.Forms.ProgressBar();
            this.label_progress = new System.Windows.Forms.Label();
            this.label_task = new System.Windows.Forms.Label();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.toolStripStatusLabel1 = new System.Windows.Forms.ToolStripStatusLabel();
            this.menuStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.Container)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.SteganoContainer)).BeginInit();
            this.statusStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // menuStrip1
            // 
            this.menuStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.менюToolStripMenuItem,
            this.файлToolStripMenuItem,
            this.справкаToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Padding = new System.Windows.Forms.Padding(4, 2, 0, 2);
            this.menuStrip1.Size = new System.Drawing.Size(950, 24);
            this.menuStrip1.TabIndex = 14;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // менюToolStripMenuItem
            // 
            this.менюToolStripMenuItem.Name = "менюToolStripMenuItem";
            this.менюToolStripMenuItem.Size = new System.Drawing.Size(53, 20);
            this.менюToolStripMenuItem.Text = "Меню";
            this.менюToolStripMenuItem.Click += new System.EventHandler(this.менюToolStripMenuItem_Click);
            // 
            // файлToolStripMenuItem
            // 
            this.файлToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.сохранитьЛогToolStripMenuItem,
            this.загрузитьЛогToolStripMenuItem});
            this.файлToolStripMenuItem.Name = "файлToolStripMenuItem";
            this.файлToolStripMenuItem.Size = new System.Drawing.Size(48, 20);
            this.файлToolStripMenuItem.Text = "Файл";
            // 
            // сохранитьЛогToolStripMenuItem
            // 
            this.сохранитьЛогToolStripMenuItem.Name = "сохранитьЛогToolStripMenuItem";
            this.сохранитьЛогToolStripMenuItem.Size = new System.Drawing.Size(155, 22);
            this.сохранитьЛогToolStripMenuItem.Text = "Сохранить лог";
            this.сохранитьЛогToolStripMenuItem.Click += new System.EventHandler(this.сохранитьЛогToolStripMenuItem_Click);
            // 
            // загрузитьЛогToolStripMenuItem
            // 
            this.загрузитьЛогToolStripMenuItem.Name = "загрузитьЛогToolStripMenuItem";
            this.загрузитьЛогToolStripMenuItem.Size = new System.Drawing.Size(155, 22);
            this.загрузитьЛогToolStripMenuItem.Text = "Загрузить лог";
            // 
            // справкаToolStripMenuItem
            // 
            this.справкаToolStripMenuItem.Name = "справкаToolStripMenuItem";
            this.справкаToolStripMenuItem.Size = new System.Drawing.Size(65, 20);
            this.справкаToolStripMenuItem.Text = "Справка";
            this.справкаToolStripMenuItem.Click += new System.EventHandler(this.справкаToolStripMenuItem_Click_1);
            // 
            // label14
            // 
            this.label14.AutoSize = true;
            this.label14.Location = new System.Drawing.Point(11, 299);
            this.label14.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(65, 13);
            this.label14.TabIndex = 22;
            this.label14.Text = "Лог сессии";
            // 
            // logBox
            // 
            this.logBox.Location = new System.Drawing.Point(6, 315);
            this.logBox.Margin = new System.Windows.Forms.Padding(2);
            this.logBox.Multiline = true;
            this.logBox.Name = "logBox";
            this.logBox.ReadOnly = true;
            this.logBox.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.logBox.Size = new System.Drawing.Size(928, 123);
            this.logBox.TabIndex = 21;
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(106, 240);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(133, 23);
            this.button1.TabIndex = 23;
            this.button1.Text = "Выбрать контейнер";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // Container
            // 
            this.Container.Location = new System.Drawing.Point(12, 48);
            this.Container.Name = "Container";
            this.Container.Size = new System.Drawing.Size(338, 186);
            this.Container.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.Container.TabIndex = 24;
            this.Container.TabStop = false;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 30);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(61, 13);
            this.label1.TabIndex = 25;
            this.label1.Text = "Контейнер";
            // 
            // SteganoContainer
            // 
            this.SteganoContainer.Location = new System.Drawing.Point(356, 48);
            this.SteganoContainer.Name = "SteganoContainer";
            this.SteganoContainer.Size = new System.Drawing.Size(338, 186);
            this.SteganoContainer.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.SteganoContainer.TabIndex = 26;
            this.SteganoContainer.TabStop = false;
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(527, 240);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(167, 43);
            this.button2.TabIndex = 27;
            this.button2.Text = "Сохранить стеганоконтейнер";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // button3
            // 
            this.button3.Location = new System.Drawing.Point(356, 240);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(165, 43);
            this.button3.TabIndex = 28;
            this.button3.Text = "Выбрать стеганоконтейнер";
            this.button3.UseVisualStyleBackColor = true;
            this.button3.Click += new System.EventHandler(this.button3_Click);
            // 
            // button4
            // 
            this.button4.Location = new System.Drawing.Point(824, 201);
            this.button4.Margin = new System.Windows.Forms.Padding(2);
            this.button4.Name = "button4";
            this.button4.Size = new System.Drawing.Size(110, 76);
            this.button4.TabIndex = 35;
            this.button4.Text = "Показать";
            this.button4.UseVisualStyleBackColor = true;
            this.button4.Click += new System.EventHandler(this.button4_Click);
            // 
            // button5
            // 
            this.button5.Location = new System.Drawing.Point(703, 201);
            this.button5.Margin = new System.Windows.Forms.Padding(2);
            this.button5.Name = "button5";
            this.button5.Size = new System.Drawing.Size(107, 76);
            this.button5.TabIndex = 34;
            this.button5.Text = "Скрыть";
            this.button5.UseVisualStyleBackColor = true;
            this.button5.Click += new System.EventHandler(this.button5_Click);
            // 
            // textBox_message
            // 
            this.textBox_message.Location = new System.Drawing.Point(703, 101);
            this.textBox_message.Margin = new System.Windows.Forms.Padding(2);
            this.textBox_message.Multiline = true;
            this.textBox_message.Name = "textBox_message";
            this.textBox_message.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.textBox_message.Size = new System.Drawing.Size(231, 96);
            this.textBox_message.TabIndex = 32;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(700, 84);
            this.label2.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(65, 13);
            this.label2.TabIndex = 29;
            this.label2.Text = "Сообщение";
            // 
            // textBox_maxMsgLen
            // 
            this.textBox_maxMsgLen.Location = new System.Drawing.Point(703, 62);
            this.textBox_maxMsgLen.Margin = new System.Windows.Forms.Padding(2);
            this.textBox_maxMsgLen.Name = "textBox_maxMsgLen";
            this.textBox_maxMsgLen.ReadOnly = true;
            this.textBox_maxMsgLen.Size = new System.Drawing.Size(147, 20);
            this.textBox_maxMsgLen.TabIndex = 33;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(700, 45);
            this.label5.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(125, 13);
            this.label5.TabIndex = 31;
            this.label5.Text = "сообщения в символах";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(699, 30);
            this.label4.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(117, 13);
            this.label4.TabIndex = 30;
            this.label4.Text = "Максимальная длина";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(747, 255);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(22, 13);
            this.label3.TabIndex = 36;
            this.label3.Text = "<---";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(865, 255);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(22, 13);
            this.label6.TabIndex = 37;
            this.label6.Text = "--->";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(353, 30);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(101, 13);
            this.label7.TabIndex = 38;
            this.label7.Text = "Стеганоконтейнер";
            // 
            // label_loading_container
            // 
            this.label_loading_container.AutoSize = true;
            this.label_loading_container.Location = new System.Drawing.Point(103, 122);
            this.label_loading_container.Name = "label_loading_container";
            this.label_loading_container.Size = new System.Drawing.Size(139, 26);
            this.label_loading_container.TabIndex = 39;
            this.label_loading_container.Text = "              Загрузка.\r\nПожалуйста, подождите...\r\n";
            this.label_loading_container.Visible = false;
            // 
            // label_loading_stegano
            // 
            this.label_loading_stegano.AutoSize = true;
            this.label_loading_stegano.Location = new System.Drawing.Point(452, 122);
            this.label_loading_stegano.Name = "label_loading_stegano";
            this.label_loading_stegano.Size = new System.Drawing.Size(139, 26);
            this.label_loading_stegano.TabIndex = 40;
            this.label_loading_stegano.Text = "              Загрузка.\r\nПожалуйста, подождите...\r\n";
            this.label_loading_stegano.Visible = false;
            // 
            // progress_steg
            // 
            this.progress_steg.Location = new System.Drawing.Point(356, 201);
            this.progress_steg.Name = "progress_steg";
            this.progress_steg.Size = new System.Drawing.Size(338, 23);
            this.progress_steg.Step = 1;
            this.progress_steg.Style = System.Windows.Forms.ProgressBarStyle.Continuous;
            this.progress_steg.TabIndex = 41;
            this.progress_steg.Visible = false;
            // 
            // label_progress
            // 
            this.label_progress.AutoSize = true;
            this.label_progress.Location = new System.Drawing.Point(534, 182);
            this.label_progress.Name = "label_progress";
            this.label_progress.Size = new System.Drawing.Size(15, 13);
            this.label_progress.TabIndex = 42;
            this.label_progress.Text = "%";
            this.label_progress.Visible = false;
            // 
            // label_task
            // 
            this.label_task.AutoSize = true;
            this.label_task.Location = new System.Drawing.Point(368, 182);
            this.label_task.Name = "label_task";
            this.label_task.Size = new System.Drawing.Size(43, 13);
            this.label_task.TabIndex = 43;
            this.label_task.Text = "Задача";
            this.label_task.Visible = false;
            // 
            // statusStrip1
            // 
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripStatusLabel1});
            this.statusStrip1.Location = new System.Drawing.Point(0, 447);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(950, 22);
            this.statusStrip1.TabIndex = 44;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // toolStripStatusLabel1
            // 
            this.toolStripStatusLabel1.Name = "toolStripStatusLabel1";
            this.toolStripStatusLabel1.Size = new System.Drawing.Size(118, 17);
            this.toolStripStatusLabel1.Text = "toolStripStatusLabel1";
            // 
            // Form2_tz
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(950, 469);
            this.Controls.Add(this.statusStrip1);
            this.Controls.Add(this.label_progress);
            this.Controls.Add(this.label_task);
            this.Controls.Add(this.progress_steg);
            this.Controls.Add(this.label_loading_stegano);
            this.Controls.Add(this.label_loading_container);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.button4);
            this.Controls.Add(this.button5);
            this.Controls.Add(this.textBox_message);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.textBox_maxMsgLen);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.button3);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.SteganoContainer);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.Container);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.label14);
            this.Controls.Add(this.logBox);
            this.Controls.Add(this.menuStrip1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(2);
            this.Name = "Form2_tz";
            this.Text = "BMP";
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.Container)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.SteganoContainer)).EndInit();
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem справкаToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem менюToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem файлToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem сохранитьЛогToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem загрузитьЛогToolStripMenuItem;
        private System.Windows.Forms.Label label14;
        private System.Windows.Forms.TextBox logBox;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.PictureBox Container;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.PictureBox SteganoContainer;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Button button3;
        private System.Windows.Forms.Button button4;
        private System.Windows.Forms.Button button5;
        private System.Windows.Forms.TextBox textBox_message;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox textBox_maxMsgLen;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label_loading_container;
        private System.Windows.Forms.Label label_loading_stegano;
        private System.Windows.Forms.ProgressBar progress_steg;
        private System.Windows.Forms.Label label_progress;
        private System.Windows.Forms.Label label_task;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel1;
    }
}