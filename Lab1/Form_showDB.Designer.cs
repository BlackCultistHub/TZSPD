
namespace Lab1
{
    partial class Form_showDB
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form_showDB));
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.err_time = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.lab_id = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.err_text = new System.Windows.Forms.DataGridViewTextBoxColumn();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            this.SuspendLayout();
            // 
            // dataGridView1
            // 
            this.dataGridView1.AllowUserToAddRows = false;
            this.dataGridView1.AllowUserToDeleteRows = false;
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.err_time,
            this.lab_id,
            this.err_text});
            this.dataGridView1.Location = new System.Drawing.Point(12, 12);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.RowHeadersVisible = false;
            this.dataGridView1.Size = new System.Drawing.Size(776, 381);
            this.dataGridView1.TabIndex = 1;
            // 
            // err_time
            // 
            this.err_time.FillWeight = 200F;
            this.err_time.HeaderText = "Время ошибки";
            this.err_time.Name = "err_time";
            this.err_time.ReadOnly = true;
            this.err_time.Width = 200;
            // 
            // lab_id
            // 
            this.lab_id.HeaderText = "Источник";
            this.lab_id.Name = "lab_id";
            this.lab_id.ReadOnly = true;
            // 
            // err_text
            // 
            this.err_text.FillWeight = 473F;
            this.err_text.HeaderText = "Текст ошибки";
            this.err_text.Name = "err_text";
            this.err_text.ReadOnly = true;
            this.err_text.Width = 473;
            // 
            // Form_showDB
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 401);
            this.Controls.Add(this.dataGridView1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "Form_showDB";
            this.Text = "База данных ошибок";
            this.Shown += new System.EventHandler(this.Form_showDB_Shown);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.DataGridView dataGridView1;
        private System.Windows.Forms.DataGridViewTextBoxColumn err_time;
        private System.Windows.Forms.DataGridViewTextBoxColumn lab_id;
        private System.Windows.Forms.DataGridViewTextBoxColumn err_text;
    }
}

