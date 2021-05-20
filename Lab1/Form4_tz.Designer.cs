namespace Lab1
{
    partial class Form4_tz
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form4_tz));
            this.button_send = new System.Windows.Forms.Button();
            this.label14 = new System.Windows.Forms.Label();
            this.logBox = new System.Windows.Forms.TextBox();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.менюToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.файлToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.сохранитьЛогToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.загрузитьЛогToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.сохранитьИсториюToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.загрузитьИториюToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.справкаToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.textBox_msg = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.textBox_fakePort = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.checkBox_fake_payload = new System.Windows.Forms.CheckBox();
            this.label_my_ip = new System.Windows.Forms.Label();
            this.label_packets_sent = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.button_scan_network = new System.Windows.Forms.Button();
            this.textBox_reciever_ip = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.pictureBox_led_recieve = new System.Windows.Forms.PictureBox();
            this.button_end_recieve = new System.Windows.Forms.Button();
            this.label_packets_recieved = new System.Windows.Forms.Label();
            this.button_begin_recieve = new System.Windows.Forms.Button();
            this.label4 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.textBox_recieve = new System.Windows.Forms.TextBox();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.comboBox_adapters = new System.Windows.Forms.ComboBox();
            this.label7 = new System.Windows.Forms.Label();
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.timer_check_message = new System.Windows.Forms.Timer(this.components);
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.richTextBox_history = new System.Windows.Forms.RichTextBox();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.toolStripStatusLabel1 = new System.Windows.Forms.ToolStripStatusLabel();
            this.menuStrip1.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox_led_recieve)).BeginInit();
            this.groupBox4.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.statusStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // button_send
            // 
            this.button_send.Location = new System.Drawing.Point(190, 167);
            this.button_send.Name = "button_send";
            this.button_send.Size = new System.Drawing.Size(174, 37);
            this.button_send.TabIndex = 0;
            this.button_send.Text = "Отправить";
            this.button_send.UseVisualStyleBackColor = true;
            this.button_send.Click += new System.EventHandler(this.button1_Click);
            // 
            // label14
            // 
            this.label14.AutoSize = true;
            this.label14.Location = new System.Drawing.Point(4, 301);
            this.label14.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(65, 13);
            this.label14.TabIndex = 27;
            this.label14.Text = "Лог сессии";
            // 
            // logBox
            // 
            this.logBox.Location = new System.Drawing.Point(7, 316);
            this.logBox.Margin = new System.Windows.Forms.Padding(2);
            this.logBox.Multiline = true;
            this.logBox.Name = "logBox";
            this.logBox.ReadOnly = true;
            this.logBox.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.logBox.Size = new System.Drawing.Size(669, 113);
            this.logBox.TabIndex = 26;
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
            this.menuStrip1.Size = new System.Drawing.Size(1134, 24);
            this.menuStrip1.TabIndex = 25;
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
            this.загрузитьЛогToolStripMenuItem,
            this.сохранитьИсториюToolStripMenuItem,
            this.загрузитьИториюToolStripMenuItem});
            this.файлToolStripMenuItem.Name = "файлToolStripMenuItem";
            this.файлToolStripMenuItem.Size = new System.Drawing.Size(48, 20);
            this.файлToolStripMenuItem.Text = "Файл";
            // 
            // сохранитьЛогToolStripMenuItem
            // 
            this.сохранитьЛогToolStripMenuItem.Name = "сохранитьЛогToolStripMenuItem";
            this.сохранитьЛогToolStripMenuItem.Size = new System.Drawing.Size(185, 22);
            this.сохранитьЛогToolStripMenuItem.Text = "Сохранить лог";
            this.сохранитьЛогToolStripMenuItem.Click += new System.EventHandler(this.сохранитьЛогToolStripMenuItem_Click);
            // 
            // загрузитьЛогToolStripMenuItem
            // 
            this.загрузитьЛогToolStripMenuItem.Name = "загрузитьЛогToolStripMenuItem";
            this.загрузитьЛогToolStripMenuItem.Size = new System.Drawing.Size(185, 22);
            this.загрузитьЛогToolStripMenuItem.Text = "Загрузить лог";
            this.загрузитьЛогToolStripMenuItem.Click += new System.EventHandler(this.загрузитьЛогToolStripMenuItem_Click);
            // 
            // сохранитьИсториюToolStripMenuItem
            // 
            this.сохранитьИсториюToolStripMenuItem.Name = "сохранитьИсториюToolStripMenuItem";
            this.сохранитьИсториюToolStripMenuItem.Size = new System.Drawing.Size(185, 22);
            this.сохранитьИсториюToolStripMenuItem.Text = "Сохранить историю";
            this.сохранитьИсториюToolStripMenuItem.Click += new System.EventHandler(this.сохранитьИсториюToolStripMenuItem_Click);
            // 
            // загрузитьИториюToolStripMenuItem
            // 
            this.загрузитьИториюToolStripMenuItem.Name = "загрузитьИториюToolStripMenuItem";
            this.загрузитьИториюToolStripMenuItem.Size = new System.Drawing.Size(185, 22);
            this.загрузитьИториюToolStripMenuItem.Text = "Загрузить историю";
            this.загрузитьИториюToolStripMenuItem.Click += new System.EventHandler(this.загрузитьИториюToolStripMenuItem_Click);
            // 
            // справкаToolStripMenuItem
            // 
            this.справкаToolStripMenuItem.Name = "справкаToolStripMenuItem";
            this.справкаToolStripMenuItem.Size = new System.Drawing.Size(65, 20);
            this.справкаToolStripMenuItem.Text = "Справка";
            this.справкаToolStripMenuItem.Click += new System.EventHandler(this.справкаToolStripMenuItem_Click);
            // 
            // textBox_msg
            // 
            this.textBox_msg.Location = new System.Drawing.Point(6, 71);
            this.textBox_msg.Multiline = true;
            this.textBox_msg.Name = "textBox_msg";
            this.textBox_msg.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.textBox_msg.Size = new System.Drawing.Size(358, 90);
            this.textBox_msg.TabIndex = 28;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(6, 55);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(65, 13);
            this.label1.TabIndex = 29;
            this.label1.Text = "Сообщение";
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.textBox_fakePort);
            this.groupBox1.Controls.Add(this.label6);
            this.groupBox1.Controls.Add(this.button_send);
            this.groupBox1.Controls.Add(this.checkBox_fake_payload);
            this.groupBox1.Controls.Add(this.label_my_ip);
            this.groupBox1.Controls.Add(this.label_packets_sent);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.label8);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.textBox_msg);
            this.groupBox1.Controls.Add(this.button_scan_network);
            this.groupBox1.Controls.Add(this.textBox_reciever_ip);
            this.groupBox1.Controls.Add(this.label5);
            this.groupBox1.Location = new System.Drawing.Point(7, 88);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(372, 210);
            this.groupBox1.TabIndex = 30;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Отправка";
            // 
            // textBox_fakePort
            // 
            this.textBox_fakePort.Location = new System.Drawing.Point(117, 184);
            this.textBox_fakePort.Name = "textBox_fakePort";
            this.textBox_fakePort.ReadOnly = true;
            this.textBox_fakePort.Size = new System.Drawing.Size(66, 20);
            this.textBox_fakePort.TabIndex = 37;
            this.textBox_fakePort.Text = "80";
            this.textBox_fakePort.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(6, 187);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(112, 13);
            this.label6.TabIndex = 36;
            this.label6.Text = "Фейк-порт (RX = TX)";
            // 
            // checkBox_fake_payload
            // 
            this.checkBox_fake_payload.AutoSize = true;
            this.checkBox_fake_payload.Location = new System.Drawing.Point(5, 167);
            this.checkBox_fake_payload.Name = "checkBox_fake_payload";
            this.checkBox_fake_payload.Size = new System.Drawing.Size(189, 17);
            this.checkBox_fake_payload.TabIndex = 35;
            this.checkBox_fake_payload.Text = "Использовать ложную нагрузку";
            this.checkBox_fake_payload.UseVisualStyleBackColor = true;
            // 
            // label_my_ip
            // 
            this.label_my_ip.AutoSize = true;
            this.label_my_ip.Location = new System.Drawing.Point(275, 16);
            this.label_my_ip.Name = "label_my_ip";
            this.label_my_ip.Size = new System.Drawing.Size(52, 13);
            this.label_my_ip.TabIndex = 34;
            this.label_my_ip.Text = "127.0.0.1";
            // 
            // label_packets_sent
            // 
            this.label_packets_sent.AutoSize = true;
            this.label_packets_sent.Location = new System.Drawing.Point(348, 54);
            this.label_packets_sent.Name = "label_packets_sent";
            this.label_packets_sent.Size = new System.Drawing.Size(13, 13);
            this.label_packets_sent.TabIndex = 2;
            this.label_packets_sent.Text = "0";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(227, 54);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(115, 13);
            this.label3.TabIndex = 0;
            this.label3.Text = "Пакетов отправлено:";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(190, 16);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(44, 13);
            this.label8.TabIndex = 33;
            this.label8.Text = "Мой IP:";
            // 
            // button_scan_network
            // 
            this.button_scan_network.Location = new System.Drawing.Point(190, 30);
            this.button_scan_network.Name = "button_scan_network";
            this.button_scan_network.Size = new System.Drawing.Size(174, 23);
            this.button_scan_network.TabIndex = 32;
            this.button_scan_network.Text = "Сканировать сеть";
            this.button_scan_network.UseVisualStyleBackColor = true;
            this.button_scan_network.Click += new System.EventHandler(this.button4_Click);
            // 
            // textBox_reciever_ip
            // 
            this.textBox_reciever_ip.Location = new System.Drawing.Point(5, 32);
            this.textBox_reciever_ip.Name = "textBox_reciever_ip";
            this.textBox_reciever_ip.Size = new System.Drawing.Size(178, 20);
            this.textBox_reciever_ip.TabIndex = 31;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(5, 16);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(77, 13);
            this.label5.TabIndex = 30;
            this.label5.Text = "IP получателя";
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.pictureBox_led_recieve);
            this.groupBox2.Controls.Add(this.button_end_recieve);
            this.groupBox2.Controls.Add(this.label_packets_recieved);
            this.groupBox2.Controls.Add(this.button_begin_recieve);
            this.groupBox2.Controls.Add(this.label4);
            this.groupBox2.Controls.Add(this.label2);
            this.groupBox2.Controls.Add(this.textBox_recieve);
            this.groupBox2.Location = new System.Drawing.Point(385, 28);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(291, 270);
            this.groupBox2.TabIndex = 31;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Приём";
            // 
            // pictureBox_led_recieve
            // 
            this.pictureBox_led_recieve.Image = global::Lab1.Properties.Resources.LED_red;
            this.pictureBox_led_recieve.Location = new System.Drawing.Point(45, -1);
            this.pictureBox_led_recieve.Name = "pictureBox_led_recieve";
            this.pictureBox_led_recieve.Size = new System.Drawing.Size(19, 16);
            this.pictureBox_led_recieve.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBox_led_recieve.TabIndex = 36;
            this.pictureBox_led_recieve.TabStop = false;
            // 
            // button_end_recieve
            // 
            this.button_end_recieve.Enabled = false;
            this.button_end_recieve.Location = new System.Drawing.Point(150, 15);
            this.button_end_recieve.Name = "button_end_recieve";
            this.button_end_recieve.Size = new System.Drawing.Size(135, 52);
            this.button_end_recieve.TabIndex = 35;
            this.button_end_recieve.Text = "Завершить";
            this.button_end_recieve.UseVisualStyleBackColor = true;
            this.button_end_recieve.Click += new System.EventHandler(this.button_end_recieve_Click);
            // 
            // label_packets_recieved
            // 
            this.label_packets_recieved.AutoSize = true;
            this.label_packets_recieved.Location = new System.Drawing.Point(267, 75);
            this.label_packets_recieved.Name = "label_packets_recieved";
            this.label_packets_recieved.Size = new System.Drawing.Size(13, 13);
            this.label_packets_recieved.TabIndex = 3;
            this.label_packets_recieved.Text = "0";
            // 
            // button_begin_recieve
            // 
            this.button_begin_recieve.Location = new System.Drawing.Point(9, 15);
            this.button_begin_recieve.Name = "button_begin_recieve";
            this.button_begin_recieve.Size = new System.Drawing.Size(135, 52);
            this.button_begin_recieve.TabIndex = 34;
            this.button_begin_recieve.Text = "Начать";
            this.button_begin_recieve.UseVisualStyleBackColor = true;
            this.button_begin_recieve.Click += new System.EventHandler(this.button5_Click);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(156, 74);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(97, 13);
            this.label4.TabIndex = 1;
            this.label4.Text = "Пакетов принято:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(6, 76);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(65, 13);
            this.label2.TabIndex = 29;
            this.label2.Text = "Сообщение";
            // 
            // textBox_recieve
            // 
            this.textBox_recieve.Location = new System.Drawing.Point(9, 92);
            this.textBox_recieve.Multiline = true;
            this.textBox_recieve.Name = "textBox_recieve";
            this.textBox_recieve.ReadOnly = true;
            this.textBox_recieve.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.textBox_recieve.Size = new System.Drawing.Size(274, 172);
            this.textBox_recieve.TabIndex = 28;
            // 
            // timer1
            // 
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // comboBox_adapters
            // 
            this.comboBox_adapters.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBox_adapters.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.comboBox_adapters.FormattingEnabled = true;
            this.comboBox_adapters.Location = new System.Drawing.Point(62, 19);
            this.comboBox_adapters.Name = "comboBox_adapters";
            this.comboBox_adapters.Size = new System.Drawing.Size(304, 21);
            this.comboBox_adapters.TabIndex = 31;
            this.comboBox_adapters.SelectedIndexChanged += new System.EventHandler(this.comboBox_adapters_SelectedIndexChanged);
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(7, 22);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(49, 13);
            this.label7.TabIndex = 32;
            this.label7.Text = "Адаптер";
            // 
            // groupBox4
            // 
            this.groupBox4.Controls.Add(this.label7);
            this.groupBox4.Controls.Add(this.comboBox_adapters);
            this.groupBox4.Location = new System.Drawing.Point(7, 28);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Size = new System.Drawing.Size(372, 54);
            this.groupBox4.TabIndex = 33;
            this.groupBox4.TabStop = false;
            this.groupBox4.Text = "Настройки";
            // 
            // timer_check_message
            // 
            this.timer_check_message.Tick += new System.EventHandler(this.timer_check_message_Tick);
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.richTextBox_history);
            this.groupBox3.Location = new System.Drawing.Point(682, 28);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(445, 401);
            this.groupBox3.TabIndex = 34;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "История";
            // 
            // richTextBox_history
            // 
            this.richTextBox_history.Location = new System.Drawing.Point(6, 15);
            this.richTextBox_history.Name = "richTextBox_history";
            this.richTextBox_history.ReadOnly = true;
            this.richTextBox_history.ScrollBars = System.Windows.Forms.RichTextBoxScrollBars.ForcedVertical;
            this.richTextBox_history.Size = new System.Drawing.Size(427, 380);
            this.richTextBox_history.TabIndex = 0;
            this.richTextBox_history.Text = "";
            // 
            // statusStrip1
            // 
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripStatusLabel1});
            this.statusStrip1.Location = new System.Drawing.Point(0, 443);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(1134, 22);
            this.statusStrip1.TabIndex = 35;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // toolStripStatusLabel1
            // 
            this.toolStripStatusLabel1.Name = "toolStripStatusLabel1";
            this.toolStripStatusLabel1.Size = new System.Drawing.Size(118, 17);
            this.toolStripStatusLabel1.Text = "toolStripStatusLabel1";
            // 
            // Form4_tz
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1134, 465);
            this.Controls.Add(this.statusStrip1);
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.groupBox4);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.label14);
            this.Controls.Add(this.logBox);
            this.Controls.Add(this.menuStrip1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "Form4_tz";
            this.Text = "IP-пакеты";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Form4_tz_FormClosing);
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox_led_recieve)).EndInit();
            this.groupBox4.ResumeLayout(false);
            this.groupBox4.PerformLayout();
            this.groupBox3.ResumeLayout(false);
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button button_send;
        private System.Windows.Forms.Label label14;
        private System.Windows.Forms.TextBox logBox;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem менюToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem файлToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem сохранитьЛогToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem загрузитьЛогToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem справкаToolStripMenuItem;
        private System.Windows.Forms.TextBox textBox_msg;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox textBox_recieve;
        private System.Windows.Forms.Label label_packets_recieved;
        private System.Windows.Forms.Label label_packets_sent;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Timer timer1;
        private System.Windows.Forms.TextBox textBox_reciever_ip;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.ComboBox comboBox_adapters;
        private System.Windows.Forms.GroupBox groupBox4;
        private System.Windows.Forms.Button button_end_recieve;
        private System.Windows.Forms.Button button_begin_recieve;
        private System.Windows.Forms.Button button_scan_network;
        private System.Windows.Forms.Label label_my_ip;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Timer timer_check_message;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.RichTextBox richTextBox_history;
        private System.Windows.Forms.ToolStripMenuItem сохранитьИсториюToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem загрузитьИториюToolStripMenuItem;
        private System.Windows.Forms.PictureBox pictureBox_led_recieve;
        private System.Windows.Forms.CheckBox checkBox_fake_payload;
        private System.Windows.Forms.TextBox textBox_fakePort;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel1;
    }
}