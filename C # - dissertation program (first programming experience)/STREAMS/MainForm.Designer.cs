namespace STREAMS
{
    partial class MainForm
    {
        /// <summary>
        /// Требуется переменная конструктора.
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
        /// Обязательный метод для поддержки конструктора - не изменяйте
        /// содержимое данного метода при помощи редактора кода.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.файлToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.создатьНовуюToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.открытьСхемуToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.сохранитьToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.сохранитьКакToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.выходToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.данныеToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.UzliMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.VetviMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.katalogLT = new System.Windows.Forms.ToolStripMenuItem();
            this.расчетыToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.Proverka_oshibok = new System.Windows.Forms.ToolStripMenuItem();
            this.Raschet_Poteri = new System.Windows.Forms.ToolStripMenuItem();
            this.Optimization = new System.Windows.Forms.ToolStripMenuItem();
            this.ГрафикаToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.графическоеToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.помощьToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.настройкиToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.справочноеРуководствоКПрограммеToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.оПрограммеToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.saveFileDialog1 = new System.Windows.Forms.SaveFileDialog();
            this.menuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.файлToolStripMenuItem,
            this.данныеToolStripMenuItem,
            this.расчетыToolStripMenuItem,
            this.ГрафикаToolStripMenuItem,
            this.помощьToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(977, 24);
            this.menuStrip1.TabIndex = 1;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // файлToolStripMenuItem
            // 
            this.файлToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.создатьНовуюToolStripMenuItem,
            this.открытьСхемуToolStripMenuItem,
            this.сохранитьToolStripMenuItem,
            this.сохранитьКакToolStripMenuItem,
            this.выходToolStripMenuItem1});
            this.файлToolStripMenuItem.Name = "файлToolStripMenuItem";
            this.файлToolStripMenuItem.Size = new System.Drawing.Size(48, 20);
            this.файлToolStripMenuItem.Text = "Файл";
            // 
            // создатьНовуюToolStripMenuItem
            // 
            this.создатьНовуюToolStripMenuItem.Name = "создатьНовуюToolStripMenuItem";
            this.создатьНовуюToolStripMenuItem.Size = new System.Drawing.Size(194, 22);
            this.создатьНовуюToolStripMenuItem.Text = "Создать новую схему ";
            this.создатьНовуюToolStripMenuItem.Click += new System.EventHandler(this.создатьНовуюToolStripMenuItem_Click);
            // 
            // открытьСхемуToolStripMenuItem
            // 
            this.открытьСхемуToolStripMenuItem.Name = "открытьСхемуToolStripMenuItem";
            this.открытьСхемуToolStripMenuItem.Size = new System.Drawing.Size(194, 22);
            this.открытьСхемуToolStripMenuItem.Text = "Открыть схему";
            this.открытьСхемуToolStripMenuItem.Click += new System.EventHandler(this.открытьСхемуToolStripMenuItem_Click);
            // 
            // сохранитьToolStripMenuItem
            // 
            this.сохранитьToolStripMenuItem.Enabled = false;
            this.сохранитьToolStripMenuItem.Name = "сохранитьToolStripMenuItem";
            this.сохранитьToolStripMenuItem.Size = new System.Drawing.Size(194, 22);
            this.сохранитьToolStripMenuItem.Text = "Сохранить";
            this.сохранитьToolStripMenuItem.Click += new System.EventHandler(this.сохранитьToolStripMenuItem_Click);
            // 
            // сохранитьКакToolStripMenuItem
            // 
            this.сохранитьКакToolStripMenuItem.Enabled = false;
            this.сохранитьКакToolStripMenuItem.Name = "сохранитьКакToolStripMenuItem";
            this.сохранитьКакToolStripMenuItem.Size = new System.Drawing.Size(194, 22);
            this.сохранитьКакToolStripMenuItem.Text = "Сохранить как...";
            this.сохранитьКакToolStripMenuItem.Click += new System.EventHandler(this.сохранитьКакToolStripMenuItem_Click);
            // 
            // выходToolStripMenuItem1
            // 
            this.выходToolStripMenuItem1.Name = "выходToolStripMenuItem1";
            this.выходToolStripMenuItem1.Size = new System.Drawing.Size(194, 22);
            this.выходToolStripMenuItem1.Text = "Выход";
            this.выходToolStripMenuItem1.Click += new System.EventHandler(this.выходToolStripMenuItem1_Click);
            // 
            // данныеToolStripMenuItem
            // 
            this.данныеToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.UzliMenuItem,
            this.VetviMenuItem,
            this.katalogLT});
            this.данныеToolStripMenuItem.Enabled = false;
            this.данныеToolStripMenuItem.Name = "данныеToolStripMenuItem";
            this.данныеToolStripMenuItem.Size = new System.Drawing.Size(62, 20);
            this.данныеToolStripMenuItem.Text = "Данные";
            // 
            // UzliMenuItem
            // 
            this.UzliMenuItem.Name = "UzliMenuItem";
            this.UzliMenuItem.Size = new System.Drawing.Size(269, 22);
            this.UzliMenuItem.Text = "Узлы";
            this.UzliMenuItem.Click += new System.EventHandler(this.UzliMenuItem_Click);
            // 
            // VetviMenuItem
            // 
            this.VetviMenuItem.Name = "VetviMenuItem";
            this.VetviMenuItem.Size = new System.Drawing.Size(269, 22);
            this.VetviMenuItem.Text = "Ветви";
            this.VetviMenuItem.Click += new System.EventHandler(this.VetviMenuItem_Click);
            // 
            // katalogLT
            // 
            this.katalogLT.Name = "katalogLT";
            this.katalogLT.Size = new System.Drawing.Size(269, 22);
            this.katalogLT.Text = "Каталог линий и трансформаторов";
            this.katalogLT.Click += new System.EventHandler(this.katalogLT_Click);
            // 
            // расчетыToolStripMenuItem
            // 
            this.расчетыToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.Proverka_oshibok,
            this.Raschet_Poteri,
            this.Optimization});
            this.расчетыToolStripMenuItem.Enabled = false;
            this.расчетыToolStripMenuItem.Name = "расчетыToolStripMenuItem";
            this.расчетыToolStripMenuItem.Size = new System.Drawing.Size(65, 20);
            this.расчетыToolStripMenuItem.Text = "Расчеты";
            // 
            // Proverka_oshibok
            // 
            this.Proverka_oshibok.Name = "Proverka_oshibok";
            this.Proverka_oshibok.Size = new System.Drawing.Size(253, 22);
            this.Proverka_oshibok.Text = "Проверка ошибок ввода данных";
            this.Proverka_oshibok.Click += new System.EventHandler(this.Proverka_oshibok_Click);
            // 
            // Raschet_Poteri
            // 
            this.Raschet_Poteri.Name = "Raschet_Poteri";
            this.Raschet_Poteri.Size = new System.Drawing.Size(253, 22);
            this.Raschet_Poteri.Text = "Расчет потерь электроэнергии";
            this.Raschet_Poteri.Click += new System.EventHandler(this.Raschet_Poteri_Click);
            // 
            // Optimization
            // 
            this.Optimization.Enabled = false;
            this.Optimization.Name = "Optimization";
            this.Optimization.Size = new System.Drawing.Size(253, 22);
            this.Optimization.Text = "Оптимизация";
            this.Optimization.Click += new System.EventHandler(this.Optimization_Click);
            // 
            // ГрафикаToolStripMenuItem
            // 
            this.ГрафикаToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.графическоеToolStripMenuItem});
            this.ГрафикаToolStripMenuItem.Enabled = false;
            this.ГрафикаToolStripMenuItem.Name = "ГрафикаToolStripMenuItem";
            this.ГрафикаToolStripMenuItem.Size = new System.Drawing.Size(66, 20);
            this.ГрафикаToolStripMenuItem.Text = "Графика";
            // 
            // графическоеToolStripMenuItem
            // 
            this.графическоеToolStripMenuItem.Name = "графическоеToolStripMenuItem";
            this.графическоеToolStripMenuItem.Size = new System.Drawing.Size(262, 22);
            this.графическоеToolStripMenuItem.Text = "Графическое изображение схемы";
            // 
            // помощьToolStripMenuItem
            // 
            this.помощьToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.настройкиToolStripMenuItem,
            this.справочноеРуководствоКПрограммеToolStripMenuItem,
            this.оПрограммеToolStripMenuItem});
            this.помощьToolStripMenuItem.Name = "помощьToolStripMenuItem";
            this.помощьToolStripMenuItem.Size = new System.Drawing.Size(68, 20);
            this.помощьToolStripMenuItem.Text = "Помощь";
            // 
            // настройкиToolStripMenuItem
            // 
            this.настройкиToolStripMenuItem.Name = "настройкиToolStripMenuItem";
            this.настройкиToolStripMenuItem.Size = new System.Drawing.Size(289, 22);
            this.настройкиToolStripMenuItem.Text = "Настройки";
            // 
            // справочноеРуководствоКПрограммеToolStripMenuItem
            // 
            this.справочноеРуководствоКПрограммеToolStripMenuItem.Name = "справочноеРуководствоКПрограммеToolStripMenuItem";
            this.справочноеРуководствоКПрограммеToolStripMenuItem.Size = new System.Drawing.Size(289, 22);
            this.справочноеРуководствоКПрограммеToolStripMenuItem.Text = "Справочное руководство к программе";
            // 
            // оПрограммеToolStripMenuItem
            // 
            this.оПрограммеToolStripMenuItem.Name = "оПрограммеToolStripMenuItem";
            this.оПрограммеToolStripMenuItem.Size = new System.Drawing.Size(289, 22);
            this.оПрограммеToolStripMenuItem.Text = "О программе";
            this.оПрограммеToolStripMenuItem.Click += new System.EventHandler(this.оПрограммеToolStripMenuItem_Click);
            // 
            // openFileDialog1
            // 
            this.openFileDialog1.FileName = "openFileDialog";
            this.openFileDialog1.InitialDirectory = "C:\\\\";
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(977, 447);
            this.Controls.Add(this.menuStrip1);
            this.HelpButton = true;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.IsMdiContainer = true;
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "MainForm";
            this.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.Text = "STREAMS";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem файлToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem расчетыToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem ГрафикаToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem графическоеToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem помощьToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem справочноеРуководствоКПрограммеToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem оПрограммеToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem создатьНовуюToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem Proverka_oshibok;
        private System.Windows.Forms.ToolStripMenuItem Raschet_Poteri;
        private System.Windows.Forms.OpenFileDialog openFileDialog1;
        private System.Windows.Forms.ToolStripMenuItem настройкиToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem данныеToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem UzliMenuItem;
        private System.Windows.Forms.ToolStripMenuItem открытьСхемуToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem сохранитьToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem сохранитьКакToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem выходToolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem VetviMenuItem;
        private System.Windows.Forms.ToolStripMenuItem katalogLT;
        private System.Windows.Forms.SaveFileDialog saveFileDialog1;
        private System.Windows.Forms.ToolStripMenuItem Optimization;
    }
}

