namespace STREAMS
{
    partial class CPForm
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
            if (disposing && (components.Components.Count!=0))
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle7 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle9 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle8 = new System.Windows.Forms.DataGridViewCellStyle();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(CPForm));
            this.panel1 = new System.Windows.Forms.Panel();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.ExcelButton = new System.Windows.Forms.Button();
            this.OpenGORSR = new System.Windows.Forms.Button();
            this.CloseCP = new System.Windows.Forms.Button();
            this.saveCP = new System.Windows.Forms.Button();
            this.dataGridCP = new System.Windows.Forms.DataGridView();
            this.TipUz = new System.Windows.Forms.DataGridViewComboBoxColumn();
            this.naimShinVN = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.nomNapr = new System.Windows.Forms.DataGridViewComboBoxColumn();
            this.naprShinNN = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.cosFi = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Wp = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Pmax = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Pmin = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.grafik = new System.Windows.Forms.DataGridViewButtonColumn();
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.openEx = new System.Windows.Forms.ToolStripMenuItem();
            this.SaveEx = new System.Windows.Forms.ToolStripMenuItem();
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.contextMenuStrip2 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.toolStripMenuItem2 = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridCP)).BeginInit();
            this.contextMenuStrip1.SuspendLayout();
            this.contextMenuStrip2.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.label4);
            this.panel1.Controls.Add(this.label3);
            this.panel1.Controls.Add(this.label2);
            this.panel1.Controls.Add(this.label1);
            this.panel1.Controls.Add(this.ExcelButton);
            this.panel1.Controls.Add(this.OpenGORSR);
            this.panel1.Controls.Add(this.CloseCP);
            this.panel1.Controls.Add(this.saveCP);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel1.Location = new System.Drawing.Point(0, 318);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(731, 37);
            this.panel1.TabIndex = 0;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(205, 19);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(175, 13);
            this.label4.TabIndex = 11;
            this.label4.Text = "- рекомендуется для заполнения";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(205, 3);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(197, 13);
            this.label3.TabIndex = 10;
            this.label3.Text = "- обязательные для заполнения поля";
            // 
            // label2
            // 
            this.label2.BackColor = System.Drawing.Color.LightBlue;
            this.label2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label2.Location = new System.Drawing.Point(186, 19);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(13, 13);
            this.label2.TabIndex = 9;
            // 
            // label1
            // 
            this.label1.BackColor = System.Drawing.Color.LightGreen;
            this.label1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label1.Location = new System.Drawing.Point(186, 3);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(13, 13);
            this.label1.TabIndex = 8;
            // 
            // ExcelButton
            // 
            this.ExcelButton.Enabled = false;
            this.ExcelButton.Image = global::STREAMS.Properties.Resources.MicrosoftExcel;
            this.ExcelButton.Location = new System.Drawing.Point(140, 2);
            this.ExcelButton.Name = "ExcelButton";
            this.ExcelButton.Size = new System.Drawing.Size(30, 30);
            this.ExcelButton.TabIndex = 7;
            this.ExcelButton.UseVisualStyleBackColor = true;
            this.ExcelButton.Click += new System.EventHandler(this.Excel_Click);
            // 
            // OpenGORSR
            // 
            this.OpenGORSR.Enabled = false;
            this.OpenGORSR.Location = new System.Drawing.Point(3, 6);
            this.OpenGORSR.Name = "OpenGORSR";
            this.OpenGORSR.Size = new System.Drawing.Size(131, 23);
            this.OpenGORSR.TabIndex = 3;
            this.OpenGORSR.Text = "Открыть файл GORSR";
            this.OpenGORSR.UseVisualStyleBackColor = true;
            // 
            // CloseCP
            // 
            this.CloseCP.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.CloseCP.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.CloseCP.Location = new System.Drawing.Point(629, 7);
            this.CloseCP.Name = "CloseCP";
            this.CloseCP.Size = new System.Drawing.Size(99, 23);
            this.CloseCP.TabIndex = 1;
            this.CloseCP.Text = "Закрыть";
            this.CloseCP.UseVisualStyleBackColor = true;
            this.CloseCP.Click += new System.EventHandler(this.CloseCP_Click);
            // 
            // saveCP
            // 
            this.saveCP.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.saveCP.Enabled = false;
            this.saveCP.Location = new System.Drawing.Point(539, 7);
            this.saveCP.Name = "saveCP";
            this.saveCP.Size = new System.Drawing.Size(84, 23);
            this.saveCP.TabIndex = 0;
            this.saveCP.Text = "Сохранить";
            this.saveCP.UseVisualStyleBackColor = true;
            this.saveCP.Click += new System.EventHandler(this.saveCP_Click);
            // 
            // dataGridCP
            // 
            this.dataGridCP.AllowUserToResizeRows = false;
            this.dataGridCP.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dataGridCP.BackgroundColor = System.Drawing.Color.LightGray;
            dataGridViewCellStyle7.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle7.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle7.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            dataGridViewCellStyle7.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle7.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle7.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle7.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dataGridCP.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle7;
            this.dataGridCP.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridCP.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.TipUz,
            this.naimShinVN,
            this.nomNapr,
            this.naprShinNN,
            this.cosFi,
            this.Wp,
            this.Pmax,
            this.Pmin,
            this.grafik});
            this.dataGridCP.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataGridCP.Location = new System.Drawing.Point(0, 0);
            this.dataGridCP.Name = "dataGridCP";
            this.dataGridCP.RightToLeft = System.Windows.Forms.RightToLeft.No;
            dataGridViewCellStyle9.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle9.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle9.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            dataGridViewCellStyle9.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle9.NullValue = "1";
            dataGridViewCellStyle9.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle9.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle9.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dataGridCP.RowHeadersDefaultCellStyle = dataGridViewCellStyle9;
            this.dataGridCP.RowHeadersWidth = 30;
            this.dataGridCP.Size = new System.Drawing.Size(731, 318);
            this.dataGridCP.TabIndex = 1;
            this.dataGridCP.CellBeginEdit += new System.Windows.Forms.DataGridViewCellCancelEventHandler(this.dataGridCP_CellBeginEdit);
            this.dataGridCP.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridCP_CellContentClick);
            this.dataGridCP.CellEndEdit += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridCP_CellEndEdit);
            this.dataGridCP.CellMouseDown += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.dataGridCP_CellMouseDown);
            this.dataGridCP.CurrentCellDirtyStateChanged += new System.EventHandler(this.dataGridCP_Changed);
            this.dataGridCP.UserDeletingRow += new System.Windows.Forms.DataGridViewRowCancelEventHandler(this.dataGridCP_UserDeletingRow);
            // 
            // TipUz
            // 
            this.TipUz.HeaderText = "Тип узла";
            this.TipUz.Items.AddRange(new object[] {
            "ЦП",
            "БС",
            "Нагр",
            "Пром"});
            this.TipUz.MinimumWidth = 40;
            this.TipUz.Name = "TipUz";
            // 
            // naimShinVN
            // 
            this.naimShinVN.FillWeight = 140F;
            this.naimShinVN.HeaderText = "Наименование узла";
            this.naimShinVN.MaxInputLength = 30;
            this.naimShinVN.MinimumWidth = 100;
            this.naimShinVN.Name = "naimShinVN";
            this.naimShinVN.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // nomNapr
            // 
            this.nomNapr.HeaderText = "Ном. напряж., кВ";
            this.nomNapr.Items.AddRange(new object[] {
            "0,38",
            "6",
            "10",
            "35"});
            this.nomNapr.MaxDropDownItems = 4;
            this.nomNapr.MinimumWidth = 30;
            this.nomNapr.Name = "nomNapr";
            this.nomNapr.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            // 
            // naprShinNN
            // 
            this.naprShinNN.HeaderText = "Ср.экспл. напряж., кВ";
            this.naprShinNN.MaxInputLength = 5;
            this.naprShinNN.MinimumWidth = 50;
            this.naprShinNN.Name = "naprShinNN";
            this.naprShinNN.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // cosFi
            // 
            this.cosFi.HeaderText = "cos φ";
            this.cosFi.MaxInputLength = 10;
            this.cosFi.MinimumWidth = 40;
            this.cosFi.Name = "cosFi";
            this.cosFi.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // Wp
            // 
            this.Wp.HeaderText = "Wp, кВт*ч";
            this.Wp.MinimumWidth = 60;
            this.Wp.Name = "Wp";
            this.Wp.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // Pmax
            // 
            this.Pmax.HeaderText = "Pmax, кВт";
            this.Pmax.MinimumWidth = 40;
            this.Pmax.Name = "Pmax";
            this.Pmax.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // Pmin
            // 
            this.Pmin.HeaderText = "Pmin, кВт";
            this.Pmin.MinimumWidth = 40;
            this.Pmin.Name = "Pmin";
            this.Pmin.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // grafik
            // 
            dataGridViewCellStyle8.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle8.SelectionBackColor = System.Drawing.Color.Gray;
            this.grafik.DefaultCellStyle = dataGridViewCellStyle8;
            this.grafik.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.grafik.HeaderText = "Графики";
            this.grafik.MinimumWidth = 30;
            this.grafik.Name = "grafik";
            this.grafik.Text = "_--_-_";
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.AutoSize = false;
            this.contextMenuStrip1.BackColor = System.Drawing.SystemColors.MenuBar;
            this.contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.openEx,
            this.SaveEx});
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.ShowImageMargin = false;
            this.contextMenuStrip1.Size = new System.Drawing.Size(82, 48);
            // 
            // openEx
            // 
            this.openEx.AutoSize = false;
            this.openEx.Name = "openEx";
            this.openEx.Size = new System.Drawing.Size(81, 22);
            this.openEx.Text = "Открыть";
            this.openEx.Click += new System.EventHandler(this.openEx_Click);
            // 
            // SaveEx
            // 
            this.SaveEx.AutoSize = false;
            this.SaveEx.Name = "SaveEx";
            this.SaveEx.Size = new System.Drawing.Size(81, 22);
            this.SaveEx.Text = "Сохранить";
            this.SaveEx.Click += new System.EventHandler(this.SaveEx_Click);
            // 
            // openFileDialog1
            // 
            this.openFileDialog1.FileName = "openFileDialog1";
            // 
            // contextMenuStrip2
            // 
            this.contextMenuStrip2.BackColor = System.Drawing.SystemColors.MenuBar;
            this.contextMenuStrip2.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripMenuItem1,
            this.toolStripMenuItem2});
            this.contextMenuStrip2.Name = "contextMenuStrip1";
            this.contextMenuStrip2.ShowImageMargin = false;
            this.contextMenuStrip2.Size = new System.Drawing.Size(187, 70);
            // 
            // toolStripMenuItem2
            // 
            this.toolStripMenuItem2.Name = "toolStripMenuItem2";
            this.toolStripMenuItem2.Size = new System.Drawing.Size(186, 22);
            this.toolStripMenuItem2.Text = "Удалить строку";
            this.toolStripMenuItem2.Click += new System.EventHandler(this.toolStripMenuItem2_Click);
            // 
            // toolStripMenuItem1
            // 
            this.toolStripMenuItem1.Name = "toolStripMenuItem1";
            this.toolStripMenuItem1.Size = new System.Drawing.Size(186, 22);
            this.toolStripMenuItem1.Text = "Удалить данные графика";
            this.toolStripMenuItem1.Click += new System.EventHandler(this.toolStripMenuItem1_Click);
            // 
            // CPForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(731, 355);
            this.Controls.Add(this.dataGridCP);
            this.Controls.Add(this.panel1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MinimumSize = new System.Drawing.Size(570, 160);
            this.Name = "CPForm";
            this.Text = "Узлы";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.CPForm_FormClosing);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridCP)).EndInit();
            this.contextMenuStrip1.ResumeLayout(false);
            this.contextMenuStrip2.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.DataGridView dataGridCP;
        private System.Windows.Forms.Button CloseCP;
        private System.Windows.Forms.Button saveCP;
        private System.Windows.Forms.Button OpenGORSR;
        private System.Windows.Forms.OpenFileDialog openFileDialog1;
        private System.Windows.Forms.Button ExcelButton;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private System.Windows.Forms.ToolStripMenuItem openEx;
        private System.Windows.Forms.ToolStripMenuItem SaveEx;
        private System.Windows.Forms.DataGridViewComboBoxColumn TipUz;
        private System.Windows.Forms.DataGridViewTextBoxColumn naimShinVN;
        private System.Windows.Forms.DataGridViewComboBoxColumn nomNapr;
        private System.Windows.Forms.DataGridViewTextBoxColumn naprShinNN;
        private System.Windows.Forms.DataGridViewTextBoxColumn cosFi;
        private System.Windows.Forms.DataGridViewTextBoxColumn Wp;
        private System.Windows.Forms.DataGridViewTextBoxColumn Pmax;
        private System.Windows.Forms.DataGridViewTextBoxColumn Pmin;
        private System.Windows.Forms.DataGridViewButtonColumn grafik;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip2;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem2;
    }
}