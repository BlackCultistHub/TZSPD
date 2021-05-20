namespace Lab1
{
    partial class Form_parse
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form_parse));
            this.richTextBox1 = new System.Windows.Forms.RichTextBox();
            this.button1 = new System.Windows.Forms.Button();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.openToParseToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.comboBox_parsePattern = new System.Windows.Forms.ComboBox();
            this.groupBox_timepick = new System.Windows.Forms.GroupBox();
            this.checkBox_timePick = new System.Windows.Forms.CheckBox();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.dateTimePicker_begin = new System.Windows.Forms.DateTimePicker();
            this.dateTimePicker_end = new System.Windows.Forms.DateTimePicker();
            this.menuStrip1.SuspendLayout();
            this.groupBox_timepick.SuspendLayout();
            this.SuspendLayout();
            // 
            // richTextBox1
            // 
            this.richTextBox1.Location = new System.Drawing.Point(13, 25);
            this.richTextBox1.Name = "richTextBox1";
            this.richTextBox1.Size = new System.Drawing.Size(510, 248);
            this.richTextBox1.TabIndex = 0;
            this.richTextBox1.Text = "";
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(365, 285);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(158, 23);
            this.button1.TabIndex = 2;
            this.button1.Text = "Выделить включения";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.openToParseToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(767, 24);
            this.menuStrip1.TabIndex = 3;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // openToParseToolStripMenuItem
            // 
            this.openToParseToolStripMenuItem.Name = "openToParseToolStripMenuItem";
            this.openToParseToolStripMenuItem.Size = new System.Drawing.Size(98, 20);
            this.openToParseToolStripMenuItem.Text = "Открыть файл";
            this.openToParseToolStripMenuItem.Click += new System.EventHandler(this.openToParseToolStripMenuItem_Click);
            // 
            // comboBox_parsePattern
            // 
            this.comboBox_parsePattern.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBox_parsePattern.FormattingEnabled = true;
            this.comboBox_parsePattern.Items.AddRange(new object[] {
            "Вариант 4",
            "LAB-паттерн"});
            this.comboBox_parsePattern.Location = new System.Drawing.Point(13, 287);
            this.comboBox_parsePattern.Name = "comboBox_parsePattern";
            this.comboBox_parsePattern.Size = new System.Drawing.Size(200, 21);
            this.comboBox_parsePattern.TabIndex = 4;
            // 
            // groupBox_timepick
            // 
            this.groupBox_timepick.Controls.Add(this.dateTimePicker_end);
            this.groupBox_timepick.Controls.Add(this.dateTimePicker_begin);
            this.groupBox_timepick.Controls.Add(this.label2);
            this.groupBox_timepick.Controls.Add(this.label1);
            this.groupBox_timepick.Enabled = false;
            this.groupBox_timepick.Location = new System.Drawing.Point(530, 50);
            this.groupBox_timepick.Name = "groupBox_timepick";
            this.groupBox_timepick.Size = new System.Drawing.Size(229, 69);
            this.groupBox_timepick.TabIndex = 5;
            this.groupBox_timepick.TabStop = false;
            this.groupBox_timepick.Text = "Выбор времени";
            this.toolTip1.SetToolTip(this.groupBox_timepick, "Включает парсинг логов, соответствующим указанному времени в формате hh:mm:ss");
            // 
            // checkBox_timePick
            // 
            this.checkBox_timePick.AutoSize = true;
            this.checkBox_timePick.Location = new System.Drawing.Point(529, 27);
            this.checkBox_timePick.Name = "checkBox_timePick";
            this.checkBox_timePick.Size = new System.Drawing.Size(75, 17);
            this.checkBox_timePick.TabIndex = 0;
            this.checkBox_timePick.Text = "Включить";
            this.checkBox_timePick.UseVisualStyleBackColor = true;
            this.checkBox_timePick.CheckedChanged += new System.EventHandler(this.checkBox1_CheckedChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(6, 42);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(92, 13);
            this.label1.TabIndex = 3;
            this.label1.Text = "С этого времени";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(115, 42);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(76, 13);
            this.label2.TabIndex = 4;
            this.label2.Text = "По это время";
            // 
            // dateTimePicker_begin
            // 
            this.dateTimePicker_begin.Format = System.Windows.Forms.DateTimePickerFormat.Time;
            this.dateTimePicker_begin.Location = new System.Drawing.Point(6, 19);
            this.dateTimePicker_begin.Name = "dateTimePicker_begin";
            this.dateTimePicker_begin.ShowUpDown = true;
            this.dateTimePicker_begin.Size = new System.Drawing.Size(89, 20);
            this.dateTimePicker_begin.TabIndex = 5;
            // 
            // dateTimePicker_end
            // 
            this.dateTimePicker_end.Format = System.Windows.Forms.DateTimePickerFormat.Time;
            this.dateTimePicker_end.Location = new System.Drawing.Point(118, 19);
            this.dateTimePicker_end.Name = "dateTimePicker_end";
            this.dateTimePicker_end.ShowUpDown = true;
            this.dateTimePicker_end.Size = new System.Drawing.Size(89, 20);
            this.dateTimePicker_end.TabIndex = 6;
            // 
            // Form_parse
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(767, 321);
            this.Controls.Add(this.groupBox_timepick);
            this.Controls.Add(this.comboBox_parsePattern);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.richTextBox1);
            this.Controls.Add(this.checkBox_timePick);
            this.Controls.Add(this.menuStrip1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "Form_parse";
            this.Text = "REGEX парсер";
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.groupBox_timepick.ResumeLayout(false);
            this.groupBox_timepick.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.RichTextBox richTextBox1;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem openToParseToolStripMenuItem;
        private System.Windows.Forms.ComboBox comboBox_parsePattern;
        private System.Windows.Forms.GroupBox groupBox_timepick;
        private System.Windows.Forms.CheckBox checkBox_timePick;
        private System.Windows.Forms.ToolTip toolTip1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.DateTimePicker dateTimePicker_end;
        private System.Windows.Forms.DateTimePicker dateTimePicker_begin;
    }
}