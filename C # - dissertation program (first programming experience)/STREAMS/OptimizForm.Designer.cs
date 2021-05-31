namespace STREAMS
{
    partial class OptimizForm
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
            this.label1 = new System.Windows.Forms.Label();
            this.panel1 = new System.Windows.Forms.Panel();
            this.numericUpDown1 = new System.Windows.Forms.NumericUpDown();
            this.checkBox2 = new System.Windows.Forms.CheckBox();
            this.checkBox1 = new System.Windows.Forms.CheckBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.panel2 = new System.Windows.Forms.Panel();
            this.checkBox6 = new System.Windows.Forms.CheckBox();
            this.checkBox5 = new System.Windows.Forms.CheckBox();
            this.checkBox4 = new System.Windows.Forms.CheckBox();
            this.checkBox3 = new System.Windows.Forms.CheckBox();
            this.label3 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.panel3 = new System.Windows.Forms.Panel();
            this.eff2 = new System.Windows.Forms.Label();
            this.eff1 = new System.Windows.Forms.Label();
            this.panel4 = new System.Windows.Forms.Panel();
            this.eff4 = new System.Windows.Forms.Label();
            this.eff3 = new System.Windows.Forms.Label();
            this.RunOpt = new System.Windows.Forms.Button();
            this.label5 = new System.Windows.Forms.Label();
            this.Apply = new System.Windows.Forms.Button();
            this.ShowLog = new System.Windows.Forms.Button();
            this.LogText = new System.Windows.Forms.RichTextBox();
            this.effS = new System.Windows.Forms.Label();
            this.Vsego = new System.Windows.Forms.Label();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown1)).BeginInit();
            this.panel2.SuspendLayout();
            this.panel3.SuspendLayout();
            this.panel4.SuspendLayout();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label1.Location = new System.Drawing.Point(12, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(413, 16);
            this.label1.TabIndex = 0;
            this.label1.Text = "Выберите мероприятия по снижению потерь электроэнергии:";
            // 
            // panel1
            // 
            this.panel1.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.panel1.Controls.Add(this.numericUpDown1);
            this.panel1.Controls.Add(this.checkBox2);
            this.panel1.Controls.Add(this.checkBox1);
            this.panel1.Controls.Add(this.label2);
            this.panel1.Controls.Add(this.label7);
            this.panel1.Location = new System.Drawing.Point(4, 28);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(466, 64);
            this.panel1.TabIndex = 1;
            // 
            // numericUpDown1
            // 
            this.numericUpDown1.Location = new System.Drawing.Point(314, 15);
            this.numericUpDown1.Maximum = new decimal(new int[] {
            24,
            0,
            0,
            0});
            this.numericUpDown1.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numericUpDown1.Name = "numericUpDown1";
            this.numericUpDown1.Size = new System.Drawing.Size(37, 20);
            this.numericUpDown1.TabIndex = 4;
            this.numericUpDown1.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // checkBox2
            // 
            this.checkBox2.AutoSize = true;
            this.checkBox2.Checked = true;
            this.checkBox2.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBox2.Location = new System.Drawing.Point(20, 39);
            this.checkBox2.Name = "checkBox2";
            this.checkBox2.Size = new System.Drawing.Size(297, 17);
            this.checkBox2.TabIndex = 3;
            this.checkBox2.Text = "оптимизация рабочих напряжений в центрах питания";
            this.checkBox2.UseVisualStyleBackColor = true;
            // 
            // checkBox1
            // 
            this.checkBox1.AutoSize = true;
            this.checkBox1.Checked = true;
            this.checkBox1.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBox1.Location = new System.Drawing.Point(20, 16);
            this.checkBox1.Name = "checkBox1";
            this.checkBox1.Size = new System.Drawing.Size(284, 17);
            this.checkBox1.TabIndex = 2;
            this.checkBox1.Text = "определение оптимальных мест размыкания сети";
            this.checkBox1.UseVisualStyleBackColor = true;
            this.checkBox1.CheckedChanged += new System.EventHandler(this.checkBox1_CheckedChanged);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(0, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(100, 13);
            this.label2.TabIndex = 0;
            this.label2.Text = "Организационные";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(305, 17);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(145, 13);
            this.label7.TabIndex = 8;
            this.label7.Text = "(               перекл. за Трасч)";
            // 
            // panel2
            // 
            this.panel2.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.panel2.Controls.Add(this.checkBox6);
            this.panel2.Controls.Add(this.checkBox5);
            this.panel2.Controls.Add(this.checkBox4);
            this.panel2.Controls.Add(this.checkBox3);
            this.panel2.Controls.Add(this.label3);
            this.panel2.Location = new System.Drawing.Point(4, 94);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(466, 65);
            this.panel2.TabIndex = 2;
            // 
            // checkBox6
            // 
            this.checkBox6.AutoSize = true;
            this.checkBox6.Checked = true;
            this.checkBox6.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBox6.Location = new System.Drawing.Point(20, 85);
            this.checkBox6.Name = "checkBox6";
            this.checkBox6.Size = new System.Drawing.Size(323, 17);
            this.checkBox6.TabIndex = 5;
            this.checkBox6.Text = "Установка устройств компенсации реактивной мощности";
            this.checkBox6.UseVisualStyleBackColor = true;
            this.checkBox6.Visible = false;
            // 
            // checkBox5
            // 
            this.checkBox5.AutoSize = true;
            this.checkBox5.Checked = true;
            this.checkBox5.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBox5.Location = new System.Drawing.Point(20, 62);
            this.checkBox5.Name = "checkBox5";
            this.checkBox5.Size = new System.Drawing.Size(241, 17);
            this.checkBox5.TabIndex = 4;
            this.checkBox5.Text = "Разукрупнение распределительных линий";
            this.checkBox5.UseVisualStyleBackColor = true;
            this.checkBox5.Visible = false;
            // 
            // checkBox4
            // 
            this.checkBox4.AutoSize = true;
            this.checkBox4.Checked = true;
            this.checkBox4.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBox4.Location = new System.Drawing.Point(20, 39);
            this.checkBox4.Name = "checkBox4";
            this.checkBox4.Size = new System.Drawing.Size(325, 17);
            this.checkBox4.TabIndex = 3;
            this.checkBox4.Text = "замена недогруженных/перегруженных трансформаторов";
            this.checkBox4.UseVisualStyleBackColor = true;
            // 
            // checkBox3
            // 
            this.checkBox3.AutoSize = true;
            this.checkBox3.Checked = true;
            this.checkBox3.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBox3.Location = new System.Drawing.Point(20, 16);
            this.checkBox3.Name = "checkBox3";
            this.checkBox3.Size = new System.Drawing.Size(197, 17);
            this.checkBox3.TabIndex = 2;
            this.checkBox3.Text = "замена перегруженных проводов";
            this.checkBox3.UseVisualStyleBackColor = true;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(0, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(72, 13);
            this.label3.TabIndex = 0;
            this.label3.Text = "Технические";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label6.Location = new System.Drawing.Point(472, 9);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(133, 16);
            this.label6.TabIndex = 3;
            this.label6.Text = "Эффект (тыс.кВтч):";
            // 
            // panel3
            // 
            this.panel3.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.panel3.Controls.Add(this.eff2);
            this.panel3.Controls.Add(this.eff1);
            this.panel3.Location = new System.Drawing.Point(476, 28);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(124, 63);
            this.panel3.TabIndex = 4;
            // 
            // eff2
            // 
            this.eff2.Font = new System.Drawing.Font("Gadugi", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.eff2.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(192)))), ((int)(((byte)(0)))));
            this.eff2.Location = new System.Drawing.Point(14, 33);
            this.eff2.Name = "eff2";
            this.eff2.Size = new System.Drawing.Size(95, 27);
            this.eff2.TabIndex = 1;
            this.eff2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // eff1
            // 
            this.eff1.Font = new System.Drawing.Font("Gadugi", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.eff1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(192)))), ((int)(((byte)(0)))));
            this.eff1.Location = new System.Drawing.Point(14, 6);
            this.eff1.Name = "eff1";
            this.eff1.Size = new System.Drawing.Size(95, 27);
            this.eff1.TabIndex = 0;
            this.eff1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // panel4
            // 
            this.panel4.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.panel4.Controls.Add(this.eff4);
            this.panel4.Controls.Add(this.eff3);
            this.panel4.Location = new System.Drawing.Point(476, 94);
            this.panel4.Name = "panel4";
            this.panel4.Size = new System.Drawing.Size(124, 65);
            this.panel4.TabIndex = 5;
            // 
            // eff4
            // 
            this.eff4.Font = new System.Drawing.Font("Gadugi", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.eff4.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(192)))), ((int)(((byte)(0)))));
            this.eff4.Location = new System.Drawing.Point(14, 34);
            this.eff4.Name = "eff4";
            this.eff4.Size = new System.Drawing.Size(95, 27);
            this.eff4.TabIndex = 3;
            this.eff4.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // eff3
            // 
            this.eff3.Font = new System.Drawing.Font("Gadugi", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.eff3.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(192)))), ((int)(((byte)(0)))));
            this.eff3.Location = new System.Drawing.Point(14, 6);
            this.eff3.Name = "eff3";
            this.eff3.Size = new System.Drawing.Size(95, 27);
            this.eff3.TabIndex = 2;
            this.eff3.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // RunOpt
            // 
            this.RunOpt.Location = new System.Drawing.Point(4, 165);
            this.RunOpt.Name = "RunOpt";
            this.RunOpt.Size = new System.Drawing.Size(121, 34);
            this.RunOpt.TabIndex = 6;
            this.RunOpt.Text = "Выполнить";
            this.RunOpt.UseVisualStyleBackColor = true;
            this.RunOpt.Click += new System.EventHandler(this.button1_Click);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label5.Location = new System.Drawing.Point(426, 168);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(53, 18);
            this.label5.TabIndex = 8;
            this.label5.Text = "Всего:";
            // 
            // Apply
            // 
            this.Apply.Enabled = false;
            this.Apply.Location = new System.Drawing.Point(266, 165);
            this.Apply.Name = "Apply";
            this.Apply.Size = new System.Drawing.Size(109, 34);
            this.Apply.TabIndex = 9;
            this.Apply.Text = "Применить к сети";
            this.Apply.UseVisualStyleBackColor = true;
            this.Apply.Click += new System.EventHandler(this.button2_Click);
            // 
            // ShowLog
            // 
            this.ShowLog.Enabled = false;
            this.ShowLog.Location = new System.Drawing.Point(131, 165);
            this.ShowLog.Name = "ShowLog";
            this.ShowLog.Size = new System.Drawing.Size(129, 34);
            this.ShowLog.TabIndex = 10;
            this.ShowLog.Text = "Показать рекомендации";
            this.ShowLog.UseVisualStyleBackColor = true;
            this.ShowLog.Click += new System.EventHandler(this.button3_Click);
            // 
            // LogText
            // 
            this.LogText.Location = new System.Drawing.Point(3, 203);
            this.LogText.Name = "LogText";
            this.LogText.ReadOnly = true;
            this.LogText.Size = new System.Drawing.Size(596, 293);
            this.LogText.TabIndex = 11;
            this.LogText.Text = "";
            // 
            // effS
            // 
            this.effS.Font = new System.Drawing.Font("Gadugi", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.effS.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(192)))), ((int)(((byte)(0)))));
            this.effS.Location = new System.Drawing.Point(480, 166);
            this.effS.Name = "effS";
            this.effS.Size = new System.Drawing.Size(114, 23);
            this.effS.TabIndex = 12;
            this.effS.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // Vsego
            // 
            this.Vsego.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.Vsego.Location = new System.Drawing.Point(476, 164);
            this.Vsego.Name = "Vsego";
            this.Vsego.Size = new System.Drawing.Size(124, 28);
            this.Vsego.TabIndex = 7;
            // 
            // OptimizForm
            // 
            this.AcceptButton = this.RunOpt;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(602, 200);
            this.Controls.Add(this.effS);
            this.Controls.Add(this.LogText);
            this.Controls.Add(this.ShowLog);
            this.Controls.Add(this.Apply);
            this.Controls.Add(this.Vsego);
            this.Controls.Add(this.RunOpt);
            this.Controls.Add(this.panel4);
            this.Controls.Add(this.panel3);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.label5);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "OptimizForm";
            this.Text = "Оптимизация";
            this.TopMost = true;
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown1)).EndInit();
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            this.panel3.ResumeLayout(false);
            this.panel4.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.NumericUpDown numericUpDown1;
        private System.Windows.Forms.CheckBox checkBox2;
        private System.Windows.Forms.CheckBox checkBox1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.CheckBox checkBox6;
        private System.Windows.Forms.CheckBox checkBox5;
        private System.Windows.Forms.CheckBox checkBox4;
        private System.Windows.Forms.CheckBox checkBox3;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.Panel panel4;
        private System.Windows.Forms.Button RunOpt;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Button Apply;
        private System.Windows.Forms.Label eff1;
        private System.Windows.Forms.Button ShowLog;
        private System.Windows.Forms.Label eff2;
        private System.Windows.Forms.RichTextBox LogText;
        private System.Windows.Forms.Label eff4;
        private System.Windows.Forms.Label eff3;
        private System.Windows.Forms.Label effS;
        private System.Windows.Forms.Label Vsego;
    }
}