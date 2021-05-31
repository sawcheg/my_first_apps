namespace STREAMS
{
    partial class GrafForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        public System.ComponentModel.IContainer components = null;

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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(GrafForm));
            this.SetGrafTable = new System.Windows.Forms.Button();
            this.dataU = new System.Windows.Forms.DataGridView();
            this.Ti = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Yi = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.pTypeGr = new System.Windows.Forms.ComboBox();
            this.dataP = new System.Windows.Forms.DataGridView();
            this.dataGridViewTextBoxColumn1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataQ = new System.Windows.Forms.DataGridView();
            this.dataGridViewTextBoxColumn3 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn4 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.textPmax = new System.Windows.Forms.TextBox();
            this.textPmin = new System.Windows.Forms.TextBox();
            this.textWp = new System.Windows.Forms.TextBox();
            this.textUsr = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.textWq = new System.Windows.Forms.TextBox();
            this.textCosFi = new System.Windows.Forms.TextBox();
            this.label8 = new System.Windows.Forms.Label();
            this.label10 = new System.Windows.Forms.Label();
            this.Ok = new System.Windows.Forms.Button();
            this.Cancel = new System.Windows.Forms.Button();
            this.textQmin = new System.Windows.Forms.TextBox();
            this.textQmax = new System.Windows.Forms.TextBox();
            this.label9 = new System.Windows.Forms.Label();
            this.label11 = new System.Windows.Forms.Label();
            this.pictureU = new System.Windows.Forms.PictureBox();
            this.pictureP = new System.Windows.Forms.PictureBox();
            this.pictureQ = new System.Windows.Forms.PictureBox();
            this.button1 = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.dataU)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataP)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataQ)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureU)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureP)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureQ)).BeginInit();
            this.SuspendLayout();
            // 
            // SetGrafTable
            // 
            this.SetGrafTable.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            resources.ApplyResources(this.SetGrafTable, "SetGrafTable");
            this.SetGrafTable.Name = "SetGrafTable";
            this.SetGrafTable.UseVisualStyleBackColor = true;
            this.SetGrafTable.Click += new System.EventHandler(this.SetGraf_Click);
            // 
            // dataU
            // 
            this.dataU.AllowUserToAddRows = false;
            this.dataU.AllowUserToDeleteRows = false;
            this.dataU.AllowUserToResizeColumns = false;
            this.dataU.AllowUserToResizeRows = false;
            this.dataU.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dataU.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.dataU.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataU.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.Ti,
            this.Yi});
            resources.ApplyResources(this.dataU, "dataU");
            this.dataU.Name = "dataU";
            this.dataU.RowHeadersVisible = false;
            this.dataU.TabStop = false;
            this.dataU.CellBeginEdit += new System.Windows.Forms.DataGridViewCellCancelEventHandler(this.dataU_CellBeginEdit);
            this.dataU.CellEndEdit += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataU_CellEndEdit);
            // 
            // Ti
            // 
            resources.ApplyResources(this.Ti, "Ti");
            this.Ti.Name = "Ti";
            this.Ti.ReadOnly = true;
            this.Ti.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.Ti.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // Yi
            // 
            resources.ApplyResources(this.Yi, "Yi");
            this.Yi.Name = "Yi";
            this.Yi.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // pTypeGr
            // 
            this.pTypeGr.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.pTypeGr.FormattingEnabled = true;
            this.pTypeGr.Items.AddRange(new object[] {
            resources.GetString("pTypeGr.Items"),
            resources.GetString("pTypeGr.Items1"),
            resources.GetString("pTypeGr.Items2"),
            resources.GetString("pTypeGr.Items3"),
            resources.GetString("pTypeGr.Items4")});
            resources.ApplyResources(this.pTypeGr, "pTypeGr");
            this.pTypeGr.Name = "pTypeGr";
            this.pTypeGr.SelectedIndexChanged += new System.EventHandler(this.pTypeGr_SelectedIndexChanged);
            // 
            // dataP
            // 
            this.dataP.AllowUserToAddRows = false;
            this.dataP.AllowUserToDeleteRows = false;
            this.dataP.AllowUserToResizeColumns = false;
            this.dataP.AllowUserToResizeRows = false;
            this.dataP.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dataP.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.dataP.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataP.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.dataGridViewTextBoxColumn1,
            this.dataGridViewTextBoxColumn2});
            resources.ApplyResources(this.dataP, "dataP");
            this.dataP.Name = "dataP";
            this.dataP.RowHeadersVisible = false;
            this.dataP.TabStop = false;
            // 
            // dataGridViewTextBoxColumn1
            // 
            resources.ApplyResources(this.dataGridViewTextBoxColumn1, "dataGridViewTextBoxColumn1");
            this.dataGridViewTextBoxColumn1.Name = "dataGridViewTextBoxColumn1";
            this.dataGridViewTextBoxColumn1.ReadOnly = true;
            this.dataGridViewTextBoxColumn1.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // dataGridViewTextBoxColumn2
            // 
            resources.ApplyResources(this.dataGridViewTextBoxColumn2, "dataGridViewTextBoxColumn2");
            this.dataGridViewTextBoxColumn2.Name = "dataGridViewTextBoxColumn2";
            this.dataGridViewTextBoxColumn2.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // dataQ
            // 
            this.dataQ.AllowUserToAddRows = false;
            this.dataQ.AllowUserToDeleteRows = false;
            this.dataQ.AllowUserToResizeColumns = false;
            this.dataQ.AllowUserToResizeRows = false;
            this.dataQ.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dataQ.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.dataQ.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataQ.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.dataGridViewTextBoxColumn3,
            this.dataGridViewTextBoxColumn4});
            resources.ApplyResources(this.dataQ, "dataQ");
            this.dataQ.Name = "dataQ";
            this.dataQ.RowHeadersVisible = false;
            this.dataQ.TabStop = false;
            // 
            // dataGridViewTextBoxColumn3
            // 
            resources.ApplyResources(this.dataGridViewTextBoxColumn3, "dataGridViewTextBoxColumn3");
            this.dataGridViewTextBoxColumn3.Name = "dataGridViewTextBoxColumn3";
            this.dataGridViewTextBoxColumn3.ReadOnly = true;
            this.dataGridViewTextBoxColumn3.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // dataGridViewTextBoxColumn4
            // 
            resources.ApplyResources(this.dataGridViewTextBoxColumn4, "dataGridViewTextBoxColumn4");
            this.dataGridViewTextBoxColumn4.Name = "dataGridViewTextBoxColumn4";
            this.dataGridViewTextBoxColumn4.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // label1
            // 
            resources.ApplyResources(this.label1, "label1");
            this.label1.Name = "label1";
            // 
            // label2
            // 
            resources.ApplyResources(this.label2, "label2");
            this.label2.Name = "label2";
            // 
            // label3
            // 
            resources.ApplyResources(this.label3, "label3");
            this.label3.Name = "label3";
            // 
            // label4
            // 
            resources.ApplyResources(this.label4, "label4");
            this.label4.Name = "label4";
            // 
            // textPmax
            // 
            this.textPmax.Cursor = System.Windows.Forms.Cursors.Default;
            resources.ApplyResources(this.textPmax, "textPmax");
            this.textPmax.Name = "textPmax";
            this.textPmax.ReadOnly = true;
            this.textPmax.TabStop = false;
            // 
            // textPmin
            // 
            this.textPmin.Cursor = System.Windows.Forms.Cursors.Default;
            resources.ApplyResources(this.textPmin, "textPmin");
            this.textPmin.Name = "textPmin";
            this.textPmin.ReadOnly = true;
            this.textPmin.TabStop = false;
            // 
            // textWp
            // 
            this.textWp.Cursor = System.Windows.Forms.Cursors.Default;
            resources.ApplyResources(this.textWp, "textWp");
            this.textWp.Name = "textWp";
            this.textWp.ReadOnly = true;
            this.textWp.TabStop = false;
            // 
            // textUsr
            // 
            this.textUsr.Cursor = System.Windows.Forms.Cursors.Default;
            resources.ApplyResources(this.textUsr, "textUsr");
            this.textUsr.Name = "textUsr";
            this.textUsr.ReadOnly = true;
            this.textUsr.TabStop = false;
            // 
            // label5
            // 
            resources.ApplyResources(this.label5, "label5");
            this.label5.Name = "label5";
            // 
            // label6
            // 
            resources.ApplyResources(this.label6, "label6");
            this.label6.Name = "label6";
            // 
            // label7
            // 
            resources.ApplyResources(this.label7, "label7");
            this.label7.Name = "label7";
            // 
            // textWq
            // 
            this.textWq.Cursor = System.Windows.Forms.Cursors.Default;
            resources.ApplyResources(this.textWq, "textWq");
            this.textWq.Name = "textWq";
            this.textWq.ReadOnly = true;
            this.textWq.TabStop = false;
            // 
            // textCosFi
            // 
            this.textCosFi.Cursor = System.Windows.Forms.Cursors.Default;
            resources.ApplyResources(this.textCosFi, "textCosFi");
            this.textCosFi.Name = "textCosFi";
            this.textCosFi.ReadOnly = true;
            this.textCosFi.ShortcutsEnabled = false;
            this.textCosFi.TabStop = false;
            // 
            // label8
            // 
            resources.ApplyResources(this.label8, "label8");
            this.label8.Name = "label8";
            // 
            // label10
            // 
            resources.ApplyResources(this.label10, "label10");
            this.label10.Name = "label10";
            // 
            // Ok
            // 
            this.Ok.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            resources.ApplyResources(this.Ok, "Ok");
            this.Ok.Name = "Ok";
            this.Ok.UseVisualStyleBackColor = true;
            // 
            // Cancel
            // 
            this.Cancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            resources.ApplyResources(this.Cancel, "Cancel");
            this.Cancel.Name = "Cancel";
            this.Cancel.UseVisualStyleBackColor = true;
            this.Cancel.Click += new System.EventHandler(this.Cancel_Click);
            // 
            // textQmin
            // 
            this.textQmin.Cursor = System.Windows.Forms.Cursors.Default;
            resources.ApplyResources(this.textQmin, "textQmin");
            this.textQmin.Name = "textQmin";
            this.textQmin.ReadOnly = true;
            this.textQmin.TabStop = false;
            // 
            // textQmax
            // 
            this.textQmax.Cursor = System.Windows.Forms.Cursors.Default;
            resources.ApplyResources(this.textQmax, "textQmax");
            this.textQmax.Name = "textQmax";
            this.textQmax.ReadOnly = true;
            this.textQmax.TabStop = false;
            // 
            // label9
            // 
            resources.ApplyResources(this.label9, "label9");
            this.label9.Name = "label9";
            // 
            // label11
            // 
            resources.ApplyResources(this.label11, "label11");
            this.label11.Name = "label11";
            // 
            // pictureU
            // 
            this.pictureU.BackColor = System.Drawing.SystemColors.ActiveCaptionText;
            resources.ApplyResources(this.pictureU, "pictureU");
            this.pictureU.Name = "pictureU";
            this.pictureU.TabStop = false;
            this.pictureU.MouseDown += new System.Windows.Forms.MouseEventHandler(this.pictureU_MouseDown);
            this.pictureU.MouseMove += new System.Windows.Forms.MouseEventHandler(this.pictureU_MouseMove);
            this.pictureU.MouseUp += new System.Windows.Forms.MouseEventHandler(this.pictureU_MouseUp);
            // 
            // pictureP
            // 
            this.pictureP.BackColor = System.Drawing.Color.Black;
            resources.ApplyResources(this.pictureP, "pictureP");
            this.pictureP.Name = "pictureP";
            this.pictureP.TabStop = false;
            this.pictureP.MouseDown += new System.Windows.Forms.MouseEventHandler(this.pictureP_MouseDown);
            this.pictureP.MouseMove += new System.Windows.Forms.MouseEventHandler(this.pictureP_MouseMove);
            this.pictureP.MouseUp += new System.Windows.Forms.MouseEventHandler(this.pictureP_MouseUp);
            // 
            // pictureQ
            // 
            this.pictureQ.BackColor = System.Drawing.Color.Black;
            resources.ApplyResources(this.pictureQ, "pictureQ");
            this.pictureQ.Name = "pictureQ";
            this.pictureQ.TabStop = false;
            this.pictureQ.MouseDown += new System.Windows.Forms.MouseEventHandler(this.pictureQ_MouseDown);
            this.pictureQ.MouseMove += new System.Windows.Forms.MouseEventHandler(this.pictureQ_MouseMove);
            this.pictureQ.MouseUp += new System.Windows.Forms.MouseEventHandler(this.pictureQ_MouseUp);
            // 
            // button1
            // 
            this.button1.Image = global::STREAMS.Properties.Resources.стрелка;
            resources.ApplyResources(this.button1, "button1");
            this.button1.Name = "button1";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // GrafForm
            // 
            this.AccessibleRole = System.Windows.Forms.AccessibleRole.None;
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoValidate = System.Windows.Forms.AutoValidate.Disable;
            this.BackColor = System.Drawing.SystemColors.ButtonFace;
            this.CancelButton = this.Cancel;
            this.ControlBox = false;
            this.Controls.Add(this.button1);
            this.Controls.Add(this.textQmin);
            this.Controls.Add(this.textQmax);
            this.Controls.Add(this.label9);
            this.Controls.Add(this.label11);
            this.Controls.Add(this.Cancel);
            this.Controls.Add(this.Ok);
            this.Controls.Add(this.textWq);
            this.Controls.Add(this.textCosFi);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.label10);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.textUsr);
            this.Controls.Add(this.textWp);
            this.Controls.Add(this.textPmin);
            this.Controls.Add(this.textPmax);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.dataQ);
            this.Controls.Add(this.dataP);
            this.Controls.Add(this.pTypeGr);
            this.Controls.Add(this.pictureQ);
            this.Controls.Add(this.pictureP);
            this.Controls.Add(this.dataU);
            this.Controls.Add(this.SetGrafTable);
            this.Controls.Add(this.pictureU);
            this.Cursor = System.Windows.Forms.Cursors.Default;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "GrafForm";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            ((System.ComponentModel.ISupportInitialize)(this.dataU)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataP)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataQ)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureU)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureP)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureQ)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.PictureBox pictureU;
        private System.Windows.Forms.Button SetGrafTable;
        private System.Windows.Forms.DataGridView dataU;
        private System.Windows.Forms.PictureBox pictureP;
        private System.Windows.Forms.PictureBox pictureQ;
        private System.Windows.Forms.ComboBox pTypeGr;
        private System.Windows.Forms.DataGridView dataP;
        private System.Windows.Forms.DataGridView dataQ;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox textPmax;
        private System.Windows.Forms.TextBox textPmin;
        private System.Windows.Forms.TextBox textWp;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.TextBox textWq;
        private System.Windows.Forms.TextBox textCosFi;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Button Ok;
        private System.Windows.Forms.Button Cancel;
        private System.Windows.Forms.TextBox textUsr;
        private System.Windows.Forms.DataGridViewTextBoxColumn Ti;
        private System.Windows.Forms.DataGridViewTextBoxColumn Yi;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn1;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn2;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn3;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn4;
        private System.Windows.Forms.TextBox textQmin;
        private System.Windows.Forms.TextBox textQmax;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.Button button1;
    }
}