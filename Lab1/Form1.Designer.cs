﻿namespace Lab1
{
    partial class Form1
    {
        /// <summary>
        /// Обязательная переменная конструктора.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Освободить все используемые ресурсы.
        /// </summary>
        /// <param name="disposing">истинно, если управляемый ресурс должен быть удален; иначе ложно.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Код, автоматически созданный конструктором форм Windows

        /// <summary>
        /// Требуемый метод для поддержки конструктора — не изменяйте 
        /// содержимое этого метода с помощью редактора кода.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.name = new System.Windows.Forms.TextBox();
            this.surname = new System.Windows.Forms.TextBox();
            this.addition = new System.Windows.Forms.Button();
            this.result = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.toolStripStatusLabel1 = new System.Windows.Forms.ToolStripStatusLabel();
            this.logBox = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.name_ascii = new System.Windows.Forms.TextBox();
            this.surname_ascii = new System.Windows.Forms.TextBox();
            this.result_ascii = new System.Windows.Forms.TextBox();
            this.tail = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.tail_ascii = new System.Windows.Forms.TextBox();
            this.label9 = new System.Windows.Forms.Label();
            this.справкаToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.менюToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.файлToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.сохранитьЛогToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.загрузитьЛогToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.statusStrip1.SuspendLayout();
            this.menuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // name
            // 
            this.name.Location = new System.Drawing.Point(204, 81);
            this.name.Margin = new System.Windows.Forms.Padding(4);
            this.name.Name = "name";
            this.name.Size = new System.Drawing.Size(132, 22);
            this.name.TabIndex = 0;
            // 
            // surname
            // 
            this.surname.Location = new System.Drawing.Point(204, 176);
            this.surname.Margin = new System.Windows.Forms.Padding(4);
            this.surname.Name = "surname";
            this.surname.Size = new System.Drawing.Size(132, 22);
            this.surname.TabIndex = 1;
            // 
            // addition
            // 
            this.addition.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.addition.Location = new System.Drawing.Point(405, 81);
            this.addition.Margin = new System.Windows.Forms.Padding(4);
            this.addition.Name = "addition";
            this.addition.Size = new System.Drawing.Size(100, 163);
            this.addition.TabIndex = 2;
            this.addition.Text = "сложение";
            this.addition.UseVisualStyleBackColor = true;
            this.addition.Click += new System.EventHandler(this.addition_Click);
            // 
            // result
            // 
            this.result.Location = new System.Drawing.Point(759, 98);
            this.result.Margin = new System.Windows.Forms.Padding(4);
            this.result.Name = "result";
            this.result.Size = new System.Drawing.Size(132, 22);
            this.result.TabIndex = 3;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(141, 81);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(35, 17);
            this.label1.TabIndex = 4;
            this.label1.Text = "Имя";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(127, 176);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(70, 17);
            this.label2.TabIndex = 5;
            this.label2.Text = "Фамилия";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(676, 103);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(76, 17);
            this.label3.TabIndex = 6;
            this.label3.Text = "Результат";
            // 
            // statusStrip1
            // 
            this.statusStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripStatusLabel1});
            this.statusStrip1.Location = new System.Drawing.Point(0, 445);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(909, 26);
            this.statusStrip1.TabIndex = 7;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // toolStripStatusLabel1
            // 
            this.toolStripStatusLabel1.Name = "toolStripStatusLabel1";
            this.toolStripStatusLabel1.Size = new System.Drawing.Size(151, 20);
            this.toolStripStatusLabel1.Text = "toolStripStatusLabel1";
            // 
            // logBox
            // 
            this.logBox.Location = new System.Drawing.Point(12, 285);
            this.logBox.Multiline = true;
            this.logBox.Name = "logBox";
            this.logBox.ReadOnly = true;
            this.logBox.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.logBox.Size = new System.Drawing.Size(879, 151);
            this.logBox.TabIndex = 11;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(18, 265);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(80, 17);
            this.label4.TabIndex = 12;
            this.label4.Text = "Лог сессии";
            // 
            // timer1
            // 
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // name_ascii
            // 
            this.name_ascii.Location = new System.Drawing.Point(21, 124);
            this.name_ascii.Name = "name_ascii";
            this.name_ascii.ReadOnly = true;
            this.name_ascii.Size = new System.Drawing.Size(315, 22);
            this.name_ascii.TabIndex = 14;
            // 
            // surname_ascii
            // 
            this.surname_ascii.Location = new System.Drawing.Point(21, 222);
            this.surname_ascii.Name = "surname_ascii";
            this.surname_ascii.ReadOnly = true;
            this.surname_ascii.Size = new System.Drawing.Size(315, 22);
            this.surname_ascii.TabIndex = 15;
            // 
            // result_ascii
            // 
            this.result_ascii.Location = new System.Drawing.Point(575, 148);
            this.result_ascii.Name = "result_ascii";
            this.result_ascii.ReadOnly = true;
            this.result_ascii.Size = new System.Drawing.Size(316, 22);
            this.result_ascii.TabIndex = 16;
            // 
            // tail
            // 
            this.tail.Location = new System.Drawing.Point(575, 197);
            this.tail.Name = "tail";
            this.tail.ReadOnly = true;
            this.tail.Size = new System.Drawing.Size(316, 22);
            this.tail.TabIndex = 17;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(254, 107);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(88, 17);
            this.label5.TabIndex = 18;
            this.label5.Text = "ASCII Имени";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(235, 202);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(107, 17);
            this.label6.TabIndex = 19;
            this.label6.Text = "ASCII Фамилии";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(770, 127);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(121, 17);
            this.label7.TabIndex = 20;
            this.label7.Text = "ASCII Результата";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(845, 176);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(46, 17);
            this.label8.TabIndex = 21;
            this.label8.Text = "Хвост";
            // 
            // tail_ascii
            // 
            this.tail_ascii.Location = new System.Drawing.Point(575, 244);
            this.tail_ascii.Name = "tail_ascii";
            this.tail_ascii.ReadOnly = true;
            this.tail_ascii.Size = new System.Drawing.Size(316, 22);
            this.tail_ascii.TabIndex = 22;
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(800, 222);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(91, 17);
            this.label9.TabIndex = 23;
            this.label9.Text = "ASCII Хвоста";
            // 
            // справкаToolStripMenuItem
            // 
            this.справкаToolStripMenuItem.Name = "справкаToolStripMenuItem";
            this.справкаToolStripMenuItem.Size = new System.Drawing.Size(81, 24);
            this.справкаToolStripMenuItem.Text = "Справка";
            this.справкаToolStripMenuItem.Click += new System.EventHandler(this.справкаToolStripMenuItem_Click);
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
            this.menuStrip1.Size = new System.Drawing.Size(909, 28);
            this.menuStrip1.TabIndex = 10;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // менюToolStripMenuItem
            // 
            this.менюToolStripMenuItem.Name = "менюToolStripMenuItem";
            this.менюToolStripMenuItem.Size = new System.Drawing.Size(65, 24);
            this.менюToolStripMenuItem.Text = "Меню";
            this.менюToolStripMenuItem.Click += new System.EventHandler(this.менюToolStripMenuItem_Click);
            // 
            // файлToolStripMenuItem
            // 
            this.файлToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.сохранитьЛогToolStripMenuItem,
            this.загрузитьЛогToolStripMenuItem});
            this.файлToolStripMenuItem.Name = "файлToolStripMenuItem";
            this.файлToolStripMenuItem.Size = new System.Drawing.Size(59, 24);
            this.файлToolStripMenuItem.Text = "Файл";
            // 
            // сохранитьЛогToolStripMenuItem
            // 
            this.сохранитьЛогToolStripMenuItem.Name = "сохранитьЛогToolStripMenuItem";
            this.сохранитьЛогToolStripMenuItem.Size = new System.Drawing.Size(193, 26);
            this.сохранитьЛогToolStripMenuItem.Text = "Сохранить лог";
            this.сохранитьЛогToolStripMenuItem.Click += new System.EventHandler(this.сохранитьЛогToolStripMenuItem_Click);
            // 
            // загрузитьЛогToolStripMenuItem
            // 
            this.загрузитьЛогToolStripMenuItem.Name = "загрузитьЛогToolStripMenuItem";
            this.загрузитьЛогToolStripMenuItem.Size = new System.Drawing.Size(193, 26);
            this.загрузитьЛогToolStripMenuItem.Text = "Загрузить лог";
            this.загрузитьЛогToolStripMenuItem.Click += new System.EventHandler(this.загрузитьЛогToolStripMenuItem_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(909, 471);
            this.Controls.Add(this.label9);
            this.Controls.Add(this.tail_ascii);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.tail);
            this.Controls.Add(this.result_ascii);
            this.Controls.Add(this.surname_ascii);
            this.Controls.Add(this.name_ascii);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.logBox);
            this.Controls.Add(this.statusStrip1);
            this.Controls.Add(this.menuStrip1);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.result);
            this.Controls.Add(this.addition);
            this.Controls.Add(this.surname);
            this.Controls.Add(this.name);
            this.MainMenuStrip = this.menuStrip1;
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "Form1";
            this.Text = "Lab1";
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox name;
        private System.Windows.Forms.TextBox surname;
        private System.Windows.Forms.Button addition;
        private System.Windows.Forms.TextBox result;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.TextBox logBox;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel1;
        private System.Windows.Forms.Timer timer1;
        private System.Windows.Forms.TextBox name_ascii;
        private System.Windows.Forms.TextBox surname_ascii;
        private System.Windows.Forms.TextBox result_ascii;
        private System.Windows.Forms.TextBox tail;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.TextBox tail_ascii;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.ToolStripMenuItem справкаToolStripMenuItem;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem файлToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem сохранитьЛогToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem загрузитьЛогToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem менюToolStripMenuItem;
    }
}

