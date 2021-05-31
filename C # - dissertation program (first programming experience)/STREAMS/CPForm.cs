using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.IO;
using Excel = Microsoft.Office.Interop.Excel;

namespace STREAMS
{
    public partial class CPForm : Form
    {
        public static Encoding enc = Encoding.GetEncoding(1251);    //"для русских символов"
        bool changed;                                        // были ли изменения в таблице
        Excel.Worksheet xlSheet;
        Excel.Range xlSheetRange;
        float cash;                                         // временная переменная, используемая при редактировании некоторых ячеек
        public CPForm()                                                                                 // +
        {
            InitializeComponent();
        }
        private void CPForm_FormClosing(object sender, FormClosingEventArgs e)                          // +
        {
            if (e.CloseReason == CloseReason.UserClosing)
            {
                e.Cancel = true;
                CloseCP_Click(this.CloseCP,e);         // пока скроем
            }
        }
        public void ClearTable()                                                                        // +
        {
            dataGridCP.Rows.Clear();
        }
        public void Changed(bool ch)              // изменилась  таблица                                      // +
        {
            if (ch)
            {
                changed = true;
                saveCP.Enabled = true;
            }
            else
            {
                changed = false;
                saveCP.Enabled = false;
            }
        }
        public void SetTable1(int numLine)   //создать и заполнить новую строку в таблице               // +
        {
            UZ cp = BaseShema.UZ[numLine];
            dataGridCP.Rows.Add((dataGridCP.Columns[0] as DataGridViewComboBoxColumn).Items[cp.tipUz], cp.nameCP, cp.nomN.ToString(), cp.napr, cp.cosFi, cp.Wp, cp.Pmax, cp.Pmin);
            if ((cp.graficP.Count!=0)||(cp.graficU.Count!=0))
                dataGridCP.Rows[numLine].Cells[8].Style.BackColor = Color.LightGreen;
            else
                dataGridCP.Rows[numLine].Cells[8].Style.BackColor = Color.Orange;
            switch (cp.tipUz)
            {
                case 0:
                    dataGridCP.Rows[numLine].Cells[1].Style.BackColor = Color.LightGreen;
                    dataGridCP.Rows[numLine].Cells[3].Style.BackColor = Color.LightGreen;
                    dataGridCP.Rows[numLine].Cells[4].Style.BackColor = Color.White;
                    dataGridCP.Rows[numLine].Cells[5].Style.BackColor = Color.LightBlue;
                    dataGridCP.Rows[numLine].Cells[6].Style.BackColor = Color.White;
                    dataGridCP.Rows[numLine].Cells[7].Style.BackColor = Color.White;
                    break;
                case 1:
                    dataGridCP.Rows[numLine].Cells[1].Style.BackColor = Color.LightGreen;
                    dataGridCP.Rows[numLine].Cells[3].Style.BackColor = Color.White;
                    dataGridCP.Rows[numLine].Cells[4].Style.BackColor = Color.LightGreen;
                    dataGridCP.Rows[numLine].Cells[5].Style.BackColor = Color.LightBlue;
                    dataGridCP.Rows[numLine].Cells[6].Style.BackColor = Color.LightGreen;
                    dataGridCP.Rows[numLine].Cells[7].Style.BackColor = Color.LightBlue;
                    break;
                case 2:
                    dataGridCP.Rows[numLine].Cells[1].Style.BackColor = Color.LightGreen;
                    dataGridCP.Rows[numLine].Cells[3].Style.BackColor = Color.White;
                    dataGridCP.Rows[numLine].Cells[4].Style.BackColor = Color.LightGreen;
                    dataGridCP.Rows[numLine].Cells[5].Style.BackColor = Color.LightBlue;
                    dataGridCP.Rows[numLine].Cells[6].Style.BackColor = Color.LightGreen;
                    dataGridCP.Rows[numLine].Cells[7].Style.BackColor = Color.LightBlue;
                    break;
                case 3:
                    dataGridCP.Rows[numLine].Cells[1].Style.BackColor = Color.LightGreen;
                    dataGridCP.Rows[numLine].Cells[3].Style.BackColor = Color.White;
                    dataGridCP.Rows[numLine].Cells[4].Style.BackColor = Color.White;
                    dataGridCP.Rows[numLine].Cells[5].Style.BackColor = Color.White;
                    dataGridCP.Rows[numLine].Cells[6].Style.BackColor = Color.White;
                    dataGridCP.Rows[numLine].Cells[7].Style.BackColor = Color.White;
                    dataGridCP.Rows[numLine].Cells[8].Style.BackColor = Color.LightGreen;
                    break;
            }
            for (int i = 3; i < 8; i++)
                if (dataGridCP.Rows[numLine].Cells[i].Value.ToString() == "0")
                    dataGridCP.Rows[numLine].Cells[i].Value = "";

        }
        private void dataGridCP_Changed(object sender, EventArgs e)     // изменилась таблица           // +
        {
            Changed(true);
        }
        private void saveCP_Click(object sender, EventArgs e)    // сохранение изменений в файл         // +
        {
            Changed(false);
            DelegateList.Save();
        }
        private void CloseCP_Click(object sender, EventArgs e)   // кнопка закрыть с запросом на подтв. // +
        {
            if (changed)
                if (MessageBox.Show("В таблице редактировались данные. Сохранить изменения в файле?", "Подтверждение о сохранении", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                { this.saveCP_Click(this, e); }
            this.Hide();
        }
        private void Excel_Click(object sender, EventArgs e)     //пока не будет ее                     
        {
            contextMenuStrip1.Show(ExcelButton, new Point(30, 0));
        }
        private void openEx_Click(object sender, EventArgs e)        //та же фигня                      
        {
            Excel.Application App = new Excel.Application();
            App.FindFile();
            if (App.ActiveSheet != null)
            {
                Excel.Worksheet workSheet = App.ActiveSheet;
                try
                {
                    dataGridCP.RowCount = workSheet.UsedRange.Cells.Rows.Count - 2;
                    for (int row = 3; row <= workSheet.UsedRange.Cells.Rows.Count; row++)
                    {
                        for (int col = 1; col <= 10; col++)
                        {
                            dataGridCP.Rows[row - 3].Cells[col - 1].Value = workSheet.UsedRange
                                         .Cells[row, col].Value;
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Описание ошибки: Exception catched " +
                        "with message: '{0}'",
                        ex.Message);
                }
                finally
                {   // обязательное завершение
                    // работы приложения 
                    App.Quit();
                    releaseObject(workSheet);
                    Changed(true);
                    //TableToListNoGr();
                }
            }
            releaseObject(App);

        }
        private void releaseObject(object obj)                     // чето для уничтожения Экселя       // +
        {
            try
            {
                System.Runtime.InteropServices.Marshal.ReleaseComObject(obj);
                obj = null;
            }
            catch (Exception ex)
            {
                obj = null;
                MessageBox.Show("Unable to release the Object " + ex.ToString());
            }
            finally
            {
                GC.Collect();
            }
        }
        private void SaveEx_Click(object sender, EventArgs e)      // в эксель, тоже не пашет           
        {
            Excel.Application App = new Excel.Application();
            try
            {
                App.Workbooks.Add(Type.Missing);
                //делаем временно неактивным документ
                App.Interactive = false;
                App.EnableEvents = false;
                //выбираем лист на котором будем работать (Лист 1)
                xlSheet = (Excel.Worksheet)App.Sheets[1];
                //Название листа
                xlSheet.Name = "Данные по ЦП";
                //Выгрузка данных
                //называем колонки
                xlSheet.Cells[1, 1].Value = "Наименование шин ВН ЦП";
                xlSheet.Cells[1, 2].Value = "Наименование шин НН ЦП";
                xlSheet.Cells[1, 3].Value = "Ном. напряжение шин НН, кВ";
                xlSheet.Cells[1, 4].Value = "Напряжение шин НН ЦП, кВ";
                xlSheet.Cells[1, 5].Value = "cos φ," + Environment.NewLine + " о.е.";
                xlSheet.Cells[1, 6].Value = "R сист.," + Environment.NewLine + " Ом";
                xlSheet.Cells[1, 7].Value = "X сист.," + Environment.NewLine + " Ом";
                xlSheet.Cells[1, 8].Value = "Параметры трансф-в ЦП";
                xlSheet.Cells[2, 8].Value = "R тр., Ом";
                xlSheet.Cells[2, 9].Value = "X тр., Ом";
                xlSheet.Cells[2, 10].Value = "I доп, А";
                xlSheet.Range[xlSheet.Cells[1, 1], xlSheet.Cells[2, 1]].MergeCells = true;
                xlSheet.Range[xlSheet.Cells[1, 2], xlSheet.Cells[2, 2]].MergeCells = true;
                xlSheet.Range[xlSheet.Cells[1, 3], xlSheet.Cells[2, 3]].MergeCells = true;
                xlSheet.Range[xlSheet.Cells[1, 4], xlSheet.Cells[2, 4]].MergeCells = true;
                xlSheet.Range[xlSheet.Cells[1, 5], xlSheet.Cells[2, 5]].MergeCells = true;
                xlSheet.Range[xlSheet.Cells[1, 6], xlSheet.Cells[2, 6]].MergeCells = true;
                xlSheet.Range[xlSheet.Cells[1, 7], xlSheet.Cells[2, 7]].MergeCells = true;
                xlSheet.Range[xlSheet.Cells[1, 8], xlSheet.Cells[1, 10]].MergeCells = true;
                //выделяем первую строку
                xlSheetRange = xlSheet.get_Range("A1:J2", Type.Missing);
                //делаем полужирный текст и перенос слов
                xlSheetRange.WrapText = true;
                xlSheetRange.Font.Bold = true;
                xlSheetRange.HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter;
                //заполняем строки
                int numRow = dataGridCP.Rows.Count + 1;
                for (int row = 3; row <= numRow; row++)
                {
                    for (int col = 1; col <= 10; col++)
                    {
                        if (col < 3)
                            xlSheet.Cells[row, col].Value = dataGridCP.Rows[row - 3].Cells[col - 1].Value;
                        else
                            xlSheet.Cells[row, col].Value = Convert.ToDecimal(dataGridCP.Rows[row - 3].Cells[col - 1].Value);
                    }
                }
                //выбираем всю область данных
                xlSheetRange = xlSheet.UsedRange;
                //выравниваем строки и колонки по их содержимому
                xlSheetRange.Columns.AutoFit();
                xlSheetRange.Rows.AutoFit();
                xlSheet.Columns[1, Type.Missing].EntireColumn.ColumnWidth = 15;
                xlSheet.Columns[2, Type.Missing].EntireColumn.ColumnWidth = 15;
                xlSheet.Columns[3, Type.Missing].EntireColumn.ColumnWidth = 17;
                xlSheet.Columns[4, Type.Missing].EntireColumn.ColumnWidth = 14;
                xlSheetRange.Borders[Excel.XlBordersIndex.xlEdgeBottom].LineStyle = Excel.XlLineStyle.xlDouble;
                xlSheetRange.Borders[Excel.XlBordersIndex.xlEdgeLeft].LineStyle = Excel.XlLineStyle.xlDouble;
                xlSheetRange.Borders[Excel.XlBordersIndex.xlEdgeRight].LineStyle = Excel.XlLineStyle.xlDouble;
                xlSheetRange.Borders[Excel.XlBordersIndex.xlEdgeTop].LineStyle = Excel.XlLineStyle.xlDouble;
                xlSheetRange.Borders[Excel.XlBordersIndex.xlInsideHorizontal].LineStyle = Excel.XlLineStyle.xlContinuous;
                xlSheetRange.Borders[Excel.XlBordersIndex.xlInsideVertical].LineStyle = Excel.XlLineStyle.xlContinuous;

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
            finally
            {
                //Показываем ексель
                App.Visible = true;
                App.Interactive = true;
                App.ScreenUpdating = true;
                App.UserControl = true;
                //Отсоединяемся от Excel
                releaseObject(xlSheetRange);
                releaseObject(xlSheet);
                releaseObject(App);
            }
        }
        private void dataGridCP_CellContentClick(object sender, DataGridViewCellEventArgs e) //график   // +
        {
            DataGridView grid = (sender as DataGridView);
            if (e.RowIndex < BaseShema.UZ.Count)
                if (e.ColumnIndex == 8)
                {
                    if(BaseShema.UZ[e.RowIndex].tipUz!=3)
                    if (Proverka_Osh(e.RowIndex)||BaseShema.UZ[e.RowIndex].tipUz==0)
                    {
                        if (grid.SelectedCells.Count != 0)
                        {
                            grid.ClearSelection();
                            grid.Rows[e.RowIndex].Cells[e.ColumnIndex].Selected = true;
                        }
                        Point aPoint = grid.GetCellDisplayRectangle(e.ColumnIndex, e.RowIndex, false).Location;
                        aPoint.X += grid.Location.X + grid.Columns[e.ColumnIndex].Width;
                        aPoint.Y += grid.Location.Y;
                        aPoint = PointToScreen(aPoint);

                        GrafForm aForm = new GrafForm();
                        aForm.Size = new Size(310, 440);
                        //aForm.ControlBox = true;
                        //aForm.FormBorderStyle = FormBorderStyle.None;
                        //aForm.BackColor = Color.Black;
                        aForm.TopMost = true;
                        aForm.Deactivate += new EventHandler(aForm_Deactivate);
                        //aForm.AutoSizeMode = AutoSizeMode.GrowAndShrink;
                        aForm.StartPosition = FormStartPosition.Manual;
                        aForm.Location = aPoint;
                        aForm.Show();
                        aForm.Update();
                        if (!aForm.GetGrafInfo(BaseShema.UZ[e.RowIndex], e.RowIndex))
                        {
                            aForm.Dispose();
                            MessageBox.Show("Введенные данные не позволяют сформировать график. Проверьте правильность информации в узле (Wp, Pmax, Pmin).",
                          "Ошибка в исх.данных", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                    else
                        MessageBox.Show("Введенные данные не позволяют сформировать график. Проверьте правильность информации в узле (Wp, Pmax, Pmin).",
                            "Ошибка в исх.данных", MessageBoxButtons.OK, MessageBoxIcon.Error);

                }
        }
        private void aForm_Deactivate(object sender, EventArgs e) // для закрытия всплывающей формы     // +
        {
            int num = (sender as GrafForm).GetN();
             BaseShema.UZ[num] = (sender as GrafForm).GetCP();
            dataGridCP[3, num].Value = BaseShema.UZ[num].napr;
            dataGridCP[4, num].Value = BaseShema.UZ[num].cosFi;
            dataGridCP[5, num].Value = BaseShema.UZ[num].Wp;
            dataGridCP[6, num].Value = BaseShema.UZ[num].Pmax;
            dataGridCP[7, num].Value = BaseShema.UZ[num].Pmin;
            for (int i = 3; i < 8; i++)
                if (dataGridCP.Rows[num].Cells[i].Value.ToString() == "0")
                    dataGridCP.Rows[num].Cells[i].Value = "";
            if((BaseShema.UZ[num].graficP.Count!=0)||(BaseShema.UZ[num].graficU.Count!=0))
                dataGridCP.Rows[num].Cells[8].Style.BackColor = Color.LightGreen;
            (sender as Form).Close();
            Changed(true);
        }
        private bool Proverka_Osh(int row)
        {
            int minGr = 0, maxGr = 0;
            if (BaseShema.UZ[row].graficP.Count != 0)
                for (int i = 0; i < 24; i++)
                {
                    if (BaseShema.UZ[row].graficP[i].Kset == 1)
                        maxGr++;
                    else if (BaseShema.UZ[row].graficP[i].Kset == -1)
                        minGr++;
                }
            else
            { maxGr = 2; minGr = 1; }
            if (BaseShema.UZ[row].Pmin != BaseShema.UZ[row].Pmax)
            {
                float kzap_bez_min = ((float)BaseShema.UZ[row].Wp / 24 - BaseShema.UZ[row].Pmin) / (BaseShema.UZ[row].Pmax - BaseShema.UZ[row].Pmin);
                if ((kzap_bez_min < (1 - (float)minGr / 24)) && (kzap_bez_min > ((float)maxGr / 24)))
                    return true;
                else
                    return false;
            }
            else 
                return true;
        }
        private void dataGridCP_CellEndEdit(object sender, DataGridViewCellEventArgs e)                 // +
        {
            {
                DataGridView grid = (sender as DataGridView);
                int Stroka = e.RowIndex;
                int Stolbec = e.ColumnIndex;
                if (BaseShema.UZ.Count < dataGridCP.Rows.Count - 1)
                {
                    BaseShema.UZ.Add(new UZ(0, "", 10, 0, 0, 0, 0, 0, new List<Graf>(), new List<Graf>(), new List<Graf>(),0,0));
                    if(Stolbec!=1)
                        grid.Rows[Stroka].Cells[1].Value = "";
                    if(Stolbec!=2)
                        grid.Rows[Stroka].Cells[2].Value = "10";
                }
                try
                {
                    switch(Stolbec)
                    {
                        case 0:
                            switch (grid[0, Stroka].Value.ToString())
                            {
                                case "ЦП":
                                    BaseShema.UZ[Stroka].tipUz = 0;
                                    grid.Rows[Stroka].Cells[1].Style.BackColor = Color.LightGreen;
                                    grid.Rows[Stroka].Cells[3].Style.BackColor = Color.LightGreen;
                                    grid.Rows[Stroka].Cells[4].Style.BackColor = Color.White;
                                    grid.Rows[Stroka].Cells[5].Style.BackColor = Color.LightBlue;
                                    grid.Rows[Stroka].Cells[6].Style.BackColor = Color.White;
                                    grid.Rows[Stroka].Cells[7].Style.BackColor = Color.White;
                                    break;
                                case "БС":
                                    BaseShema.UZ[Stroka].tipUz = 1;
                                    grid.Rows[Stroka].Cells[1].Style.BackColor = Color.LightGreen;
                                    grid.Rows[Stroka].Cells[3].Style.BackColor = Color.White;
                                    grid.Rows[Stroka].Cells[4].Style.BackColor = Color.LightGreen;
                                    grid.Rows[Stroka].Cells[5].Style.BackColor = Color.LightBlue;
                                    grid.Rows[Stroka].Cells[6].Style.BackColor = Color.LightGreen;
                                    grid.Rows[Stroka].Cells[7].Style.BackColor = Color.LightBlue;
                                    break;
                                case "Нагр":
                                    BaseShema.UZ[Stroka].tipUz = 2;
                                    grid.Rows[Stroka].Cells[1].Style.BackColor = Color.LightGreen;
                                    grid.Rows[Stroka].Cells[3].Style.BackColor = Color.White;
                                    grid.Rows[Stroka].Cells[4].Style.BackColor = Color.LightGreen;
                                    grid.Rows[Stroka].Cells[5].Style.BackColor = Color.LightBlue;
                                    grid.Rows[Stroka].Cells[6].Style.BackColor = Color.LightGreen;
                                    grid.Rows[Stroka].Cells[7].Style.BackColor = Color.LightBlue;
                                    break;
                                case "Пром":
                                    grid.Rows[Stroka].Cells[1].Style.BackColor = Color.LightGreen;
                                    grid.Rows[Stroka].Cells[3].Style.BackColor = Color.White;  
                                    grid.Rows[Stroka].Cells[4].Style.BackColor = Color.White;
                                    grid.Rows[Stroka].Cells[5].Style.BackColor = Color.White;
                                    grid.Rows[Stroka].Cells[6].Style.BackColor = Color.White;
                                    grid.Rows[Stroka].Cells[7].Style.BackColor = Color.White;
                                    grid.Rows[Stroka].Cells[3].Value = "";
                                    grid.Rows[Stroka].Cells[4].Value = "";
                                    grid.Rows[Stroka].Cells[5].Value = "";
                                    grid.Rows[Stroka].Cells[6].Value = "";
                                    grid.Rows[Stroka].Cells[7].Value = "";
                                    if (cash != 3)
                                    {
                                        BaseShema.UZ[Stroka] = new UZ(3, grid.Rows[Stroka].Cells[1].Value.ToString(), Convert.ToSingle(grid.Rows[Stroka].Cells[2].Value),0,  0, 0, 0, 0, new List<Graf>(), new List<Graf>(), new List<Graf>(),0,0);
                                        grid.Rows[Stroka].Cells[8].Style.BackColor = Color.LightGreen;
                                    }
                                    break;
                            }
                            break;
                        case 1:
                        BaseShema.UZ[Stroka].nameCP = grid.Rows[Stroka].Cells[1].Value.ToString();
                            break;
                        case 2:
                        BaseShema.UZ[Stroka].nomN = Convert.ToSingle(grid.Rows[Stroka].Cells[2].Value);
                            break;
                        case 3:
                        BaseShema.UZ[Stroka].napr = Convert.ToSingle(grid.Rows[Stroka].Cells[3].Value);
                        if (BaseShema.UZ[Stroka].napr != cash)
                        {
                            grid.Rows[Stroka].Cells[8].Style.BackColor = Color.Orange;
                        }
                            break;
                        case 4:
                        BaseShema.UZ[Stroka].cosFi = Convert.ToSingle(grid.Rows[Stroka].Cells[4].Value);
                        if (BaseShema.UZ[Stroka].cosFi != cash)
                        {
                            grid.Rows[Stroka].Cells[8].Style.BackColor = Color.Orange;
                        }
                            break;                            
                        case 5:
                        BaseShema.UZ[Stroka].Wp = Convert.ToSingle(grid.Rows[Stroka].Cells[5].Value);
                        if (BaseShema.UZ[Stroka].Wp != cash)
                        {
                            grid.Rows[Stroka].Cells[8].Style.BackColor = Color.Orange;
                        }
                            break;
                        case 6:
                        BaseShema.UZ[Stroka].Pmax = Convert.ToSingle(grid.Rows[Stroka].Cells[6].Value);
                        if (BaseShema.UZ[Stroka].Pmax != cash)
                        {
                            grid.Rows[Stroka].Cells[8].Style.BackColor = Color.Orange;
                        }
                            break;
                        case 7:
                        BaseShema.UZ[Stroka].Pmin = Convert.ToSingle(grid.Rows[Stroka].Cells[7].Value);
                        if (BaseShema.UZ[Stroka].Pmin != cash)
                        {
                            grid.Rows[Stroka].Cells[8].Style.BackColor = Color.Orange;
                        }
                            break;
                    }
                }
                catch (Exception ex)
                {
                    grid.Rows[Stroka].Cells[Stolbec].Value = "";
                    MessageBox.Show("Ячейка (строка:" + (Stroka+1).ToString() + ", столбец:" + (Stolbec+1).ToString() + ") заполнена неправильно!" + Environment.NewLine + Environment.NewLine + "Описание ошибки: " + ex.Message, "Ошибка чтения", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }

        }
        public List<UZ> printCPlist()
        {
            return BaseShema.UZ;
        }               // вернуть форме1 список с данными по ЦП
        private void dataGridCP_CellBeginEdit(object sender, DataGridViewCellCancelEventArgs e)
        {
            DataGridView grid = (sender as DataGridView);
            if (e.RowIndex < BaseShema.UZ.Count)
                switch (e.ColumnIndex)
                {
                    case 0:
                        cash = BaseShema.UZ[e.RowIndex].tipUz;
                        break;
                    case 3:
                        cash = BaseShema.UZ[e.RowIndex].napr;
                        break;
                    case 4:
                        cash = BaseShema.UZ[e.RowIndex].cosFi;
                        break;
                    case 5:
                        cash = BaseShema.UZ[e.RowIndex].Wp;
                        break;
                    case 6:
                        cash = BaseShema.UZ[e.RowIndex].Pmax;
                        break;
                    case 7:
                        cash = BaseShema.UZ[e.RowIndex].Pmin;
                        break;
                }
        }
        private void dataGridCP_UserDeletingRow(object sender, DataGridViewRowCancelEventArgs e)
        {
            BaseShema.UZ.RemoveAt(dataGridCP.CurrentRow.Index);
            Changed(true);
        }
        private void dataGridCP_CellMouseDown(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {  // клик по комм. аппарат
                
                if (BaseShema.UZ.Count > 0)
                {
                    dataGridCP.ClearSelection();
                    dataGridCP.Rows[e.RowIndex].Selected = true;
                    contextMenuStrip2.Show(MousePosition);
                }
            }
            else if (e.Button == MouseButtons.Left)
                this.Text = "Узлы - строка №" + (e.RowIndex + 1).ToString();
        }
        private void toolStripMenuItem1_Click(object sender, EventArgs e)
        {
            BaseShema.UZ[dataGridCP.SelectedRows[0].Index].graficU.Clear();
            BaseShema.UZ[dataGridCP.SelectedRows[0].Index].graficP.Clear();
            BaseShema.UZ[dataGridCP.SelectedRows[0].Index].graficQ.Clear();
            dataGridCP.Rows[dataGridCP.SelectedRows[0].Index].Cells[8].Style.BackColor = Color.Orange;
            Changed(true);
            dataGridCP.ClearSelection();
        }
        private void toolStripMenuItem2_Click(object sender, EventArgs e)
        {
            BaseShema.UZ.RemoveAt(dataGridCP.SelectedRows[0].Index);
            dataGridCP.Rows.RemoveAt(dataGridCP.SelectedRows[0].Index);
            this.Changed(true);   
        }
    }
}
