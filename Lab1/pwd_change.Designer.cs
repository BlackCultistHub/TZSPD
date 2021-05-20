namespace Lab1
{
    partial class pwd_change
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(pwd_change));
            this.textBox_prevpwd = new System.Windows.Forms.TextBox();
            this.textBox_newpwd = new System.Windows.Forms.TextBox();
            this.textBox_newPwdCopy = new System.Windows.Forms.TextBox();
            this.button1 = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // textBox_prevpwd
            // 
            this.textBox_prevpwd.Location = new System.Drawing.Point(102, 12);
            this.textBox_prevpwd.Name = "textBox_prevpwd";
            this.textBox_prevpwd.PasswordChar = '*';
            this.textBox_prevpwd.Size = new System.Drawing.Size(210, 20);
            this.textBox_prevpwd.TabIndex = 0;
            // 
            // textBox_newpwd
            // 
            this.textBox_newpwd.Location = new System.Drawing.Point(102, 54);
            this.textBox_newpwd.Name = "textBox_newpwd";
            this.textBox_newpwd.PasswordChar = '*';
            this.textBox_newpwd.Size = new System.Drawing.Size(210, 20);
            this.textBox_newpwd.TabIndex = 1;
            // 
            // textBox_newPwdCopy
            // 
            this.textBox_newPwdCopy.Location = new System.Drawing.Point(102, 95);
            this.textBox_newPwdCopy.Name = "textBox_newPwdCopy";
            this.textBox_newPwdCopy.PasswordChar = '*';
            this.textBox_newPwdCopy.Size = new System.Drawing.Size(210, 20);
            this.textBox_newPwdCopy.TabIndex = 2;
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(12, 138);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(300, 23);
            this.button1.TabIndex = 3;
            this.button1.Text = "Сменить";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(5, 15);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(91, 13);
            this.label1.TabIndex = 4;
            this.label1.Text = "Текущий пароль";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(5, 57);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(80, 13);
            this.label2.TabIndex = 5;
            this.label2.Text = "Новый пароль";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(9, 98);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(68, 13);
            this.label3.TabIndex = 6;
            this.label3.Text = "Повторение";
            // 
            // pwd_change
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(324, 173);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.textBox_newPwdCopy);
            this.Controls.Add(this.textBox_newpwd);
            this.Controls.Add(this.textBox_prevpwd);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "pwd_change";
            this.Text = "Сменить пароль";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox textBox_prevpwd;
        private System.Windows.Forms.TextBox textBox_newpwd;
        private System.Windows.Forms.TextBox textBox_newPwdCopy;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
    }
}