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
    public partial class VetviForm : Form
    {
        bool changed;                                        // были ли изменения в таблице
        Excel.Worksheet xlSheet;
        Excel.Range xlSheetRange;
        public VetviForm()                                                                                 // +
        {
            InitializeComponent();
            DelegateList.EventFormUZ_End = new DelegateList.PeredNameUz_Marka(dataGridVetv_CellDoubleClickEnd);
        }
        private void Vetvi_FormClosing(object sender, FormClosingEventArgs e)                          // +
        {
            if (e.CloseReason == CloseReason.UserClosing)
            {
                e.Cancel = true;
                CloseV_Click(this.CloseV,e);         // пока скроем
            }
        }
        public void ClearTable()                                                                        // +
        {
            dataGridVetv.Rows.Clear();
        }
        public void Changed(bool ch)                // изменилась  ли таблица                           // +
        {
            if (ch)
            {
                changed = true;
                saveV.Enabled = true;
            }
            else
            {
                changed = false;
                saveV.Enabled = false;
            }
        }
        public void SetTable1(int numLine)          //создать и заполнить новую строку в таблице        // +
        {
            Vet vet = BaseShema.Vet[numLine];
            dataGridVetv.Rows.Add((dataGridVetv.Columns[0] as DataGridViewComboBoxColumn).Items[vet.tipV], "", "", vet.nach, vet.kon, vet.marka, vet.dlina_moshnost);
            switch (vet.tipV)
            {
                case 0:
                    for(int i=3;i<=6;i++)
                    dataGridVetv.Rows[numLine].Cells[i].Style.BackColor = Color.Khaki;
                    break;
                case 1:
                    for(int i=3;i<=6;i++)
                    dataGridVetv.Rows[numLine].Cells[i].Style.BackColor = Color.PaleTurquoise;
                    break;
            }
            switch (vet.k1)
            {
                case -1:
                    dataGridVetv[1, numLine].Style.BackColor = Color.White;
                    break;
                case 0:
                    dataGridVetv[1, numLine].Style.BackColor = Color.Tomato;
                    break;
                case 1:
                    dataGridVetv[1, numLine].Style.BackColor = Color.LightGreen;
                    break;
            }
            switch (vet.k2)
            {
                case -1:
                    dataGridVetv[2, numLine].Style.BackColor = Color.White;
                    break;
                case 0:
                    dataGridVetv[2, numLine].Style.BackColor = Color.Tomato;
                    break;
                case 1:
                    dataGridVetv[2, numLine].Style.BackColor = Color.LightGreen;
                    break;
            }
            if (Convert.ToSingle(dataGridVetv.Rows[numLine].Cells[6].Value) == 0)
                dataGridVetv.Rows[numLine].Cells[6].Value = "";
        }
        public void SetLineTable(int numLine)                                                           // +
        {
            Vet vet = BaseShema.Vet[numLine];
            switch (vet.tipV)
            {
                case 0:
                    for (int i =3; i <= 6; i++)
                        dataGridVetv.Rows[numLine].Cells[i].Style.BackColor = Color.Khaki;
                    dataGridVetv.Rows[numLine].Cells[0].Value = (dataGridVetv.Columns[0] as DataGridViewComboBoxColumn).Items[0];
                    break;
                case 1:
                    for (int i = 3; i <= 6; i++)
                        dataGridVetv.Rows[numLine].Cells[i].Style.BackColor = Color.PaleTurquoise;
                    dataGridVetv.Rows[numLine].Cells[0].Value = (dataGridVetv.Columns[0] as DataGridViewComboBoxColumn).Items[1];
                    break;
            }
            switch (vet.k1)
            {
                case -1:
                    dataGridVetv[1, numLine].Style.BackColor = Color.White;
                    break;
                case 0:
                    dataGridVetv[1, numLine].Style.BackColor = Color.Tomato;
                    break;
                case 1:
                    dataGridVetv[1, numLine].Style.BackColor = Color.LightGreen;
                    break;
            }
            switch (vet.k2)
            {
                case -1:
                    dataGridVetv[2, numLine].Style.BackColor = Color.White;
                    break;
                case 0:
                    dataGridVetv[2, numLine].Style.BackColor = Color.Tomato;
                    break;
                case 1:
                    dataGridVetv[2, numLine].Style.BackColor = Color.LightGreen;
                    break;
            }
            dataGridVetv.Rows[numLine].Cells[3].Value = vet.nach;
            dataGridVetv.Rows[numLine].Cells[4].Value = vet.kon;
            dataGridVetv.Rows[numLine].Cells[5].Value = vet.marka;
            if (vet.dlina_moshnost != 0)
                dataGridVetv.Rows[numLine].Cells[6].Value = vet.dlina_moshnost;
            else
                dataGridVetv.Rows[numLine].Cells[6].Value = "";
        }
        private void dataGridVet_Changed(object sender, EventArgs e)     // изменилась таблица          // +
        {
            Changed(true);
        }
        private void saveV_Click(object sender, EventArgs e)       // сохранение изменений в файл       // +
        {
            Changed(false);
            DelegateList.Save();
        }
        private void CloseV_Click(object sender, EventArgs e)      // кнопка закрыть с запросом на подтв. // +
        {
            if (changed)
                if (MessageBox.Show("В таблице редактировались данные. Сохранить изменения?", "Подтверждение о сохранении", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                { this.saveV_Click(this, e); }
            this.Hide();
        }
        private void Excel_Click(object sender, EventArgs e)       //пока не будет ее                     
        {
            
            //contextMenuStrip1.Show(ExcelButton, new Point(30, 0));
            openEx_Click(sender,e);
        }
        private void openEx_Click(object sender, EventArgs e)      //та же фигня                         
        {
            int RowCount = 0;
            string nameRl = "";
            float Snom = 0;
            float cos = 0.85f, zagr = 0.8f, koef = 0.3f;
            int t=24;

            Excel.Application App = new Excel.Application();
            App.FindFile();
            if (App.ActiveSheet != null)
            {
                Excel.Worksheet workSheet = App.ActiveSheet;
                try
                {
                    RowCount = workSheet.UsedRange.Cells.Rows.Count;
                    BaseShema.UZ.Clear();
                    BaseShema.Vet.Clear();
                    for (int row = 1; row <= workSheet.UsedRange.Cells.Rows.Count; row++)
                    {
                        if (workSheet.UsedRange.Cells[row, 2].Value != null)
                        {
                            nameRl = workSheet.UsedRange.Cells[row, 2].Value.ToString();
                            if (workSheet.UsedRange.Cells[row, 3].Value != null)
                            {
                                BaseShema.UZ.Add(new UZ(0, workSheet.UsedRange.Cells[row, 3].Value.ToString() + " (" + nameRl + ")", 10,10.5f, 0, 0, 0, 0, new List<Graf>(), new List<Graf>(), new List<Graf>(), 0, 0));
                                BaseShema.UZ.Add(new UZ(3, workSheet.UsedRange.Cells[row, 4].Value.ToString() + " (" + nameRl + ")", 10,0, 0, 0, 0, 0, new List<Graf>(), new List<Graf>(), new List<Graf>(), 0, 0));
                                BaseShema.Vet.Add(new Vet(0, -1, -1, BaseShema.UZ[BaseShema.UZ.Count - 2].nameCP, BaseShema.UZ[BaseShema.UZ.Count - 1].nameCP, workSheet.UsedRange.Cells[row, 5].Value.ToString(), (float)workSheet.UsedRange.Cells[row, 6].Value));
                                if (workSheet.UsedRange.Cells[row, 7].Value != null)
                                {
                                    Snom = Convert.ToSingle(workSheet.UsedRange.Cells[row, 7].Value.Remove(0, workSheet.UsedRange.Cells[row, 7].Value.IndexOf('-') + 1));
                                    if (workSheet.UsedRange.Cells[row, 8].Value != null)
                                        BaseShema.UZ.Add(new UZ(2, workSheet.UsedRange.Cells[row, 4].Value.ToString() + "-НН1 (" + nameRl + ")", 0.38f, 0, cos, Snom * zagr * zagr * zagr * t / cos, Snom * zagr / cos, Snom * koef * zagr / cos, new List<Graf>(), new List<Graf>(), new List<Graf>(), 0, 0));
                                    else
                                        BaseShema.UZ.Add(new UZ(2, workSheet.UsedRange.Cells[row, 4].Value.ToString() + "-НН (" + nameRl + ")", 0.38f, 0, cos, Snom * zagr * zagr * zagr * t / cos, Snom * zagr / cos, Snom * koef * zagr / cos, new List<Graf>(), new List<Graf>(), new List<Graf>(), 0, 0));
                                    BaseShema.Vet.Add(new Vet(1, -1, -1, BaseShema.UZ[BaseShema.UZ.Count - 2].nameCP, BaseShema.UZ[BaseShema.UZ.Count - 1].nameCP, workSheet.UsedRange.Cells[row, 7].Value.ToString(), Snom));
                                    if (workSheet.UsedRange.Cells[row, 8].Value != null)
                                    {
                                        Snom = Convert.ToSingle(workSheet.UsedRange.Cells[row, 8].Value.Remove(0, workSheet.UsedRange.Cells[row, 8].Value.IndexOf('-') + 1));
                                        BaseShema.UZ.Add(new UZ(2, workSheet.UsedRange.Cells[row, 4].Value + "-НН2 (" + nameRl + ")", 0.38f, 0, cos, Snom * zagr * zagr * zagr * t / cos, Snom * zagr / cos, Snom * koef * zagr / cos, new List<Graf>(), new List<Graf>(), new List<Graf>(), 0, 0));
                                        BaseShema.Vet.Add(new Vet(1, -1, -1, BaseShema.UZ[BaseShema.UZ.Count - 3].nameCP, BaseShema.UZ[BaseShema.UZ.Count - 1].nameCP, workSheet.UsedRange.Cells[row, 8].Value.ToString(), Snom));
                                        if (workSheet.UsedRange.Cells[row, 9].Value != null)
                                        {
                                            Snom = Convert.ToSingle(workSheet.UsedRange.Cells[row, 9].Value.Remove(0, workSheet.UsedRange.Cells[row, 9].Value.IndexOf('-') + 1));
                                            BaseShema.UZ.Add(new UZ(2, workSheet.UsedRange.Cells[row, 4].Value + "-НН3 (" + nameRl + ")", 0.38f, 0, cos, Snom * zagr * zagr * zagr * t / cos, Snom * zagr / cos, Snom * koef * zagr / cos, new List<Graf>(), new List<Graf>(), new List<Graf>(), 0, 0));
                                            BaseShema.Vet.Add(new Vet(1, -1, -1, BaseShema.UZ[BaseShema.UZ.Count - 4].nameCP, BaseShema.UZ[BaseShema.UZ.Count - 1].nameCP, workSheet.UsedRange.Cells[row, 9].Value.ToString(), Snom));
                                        }
                                    }
                                }
                            }
                        }
                        else if (workSheet.UsedRange.Cells[row, 3].Value != null)
                        {
                            BaseShema.UZ.Add(new UZ(3, workSheet.UsedRange.Cells[row, 3].Value.ToString() + " (" + nameRl + ")", 10, 0, 0, 0, 0, 0, new List<Graf>(), new List<Graf>(), new List<Graf>(), 0, 0));
                            BaseShema.UZ.Add(new UZ(3, workSheet.UsedRange.Cells[row, 4].Value.ToString() + " (" + nameRl + ")", 10, 0, 0, 0, 0, 0, new List<Graf>(), new List<Graf>(), new List<Graf>(), 0, 0));
                            BaseShema.Vet.Add(new Vet(0, -1, -1, BaseShema.UZ[BaseShema.UZ.Count - 2].nameCP, BaseShema.UZ[BaseShema.UZ.Count - 1].nameCP, workSheet.UsedRange.Cells[row, 5].Value.ToString(), (float)workSheet.UsedRange.Cells[row, 6].Value));
                            if (workSheet.UsedRange.Cells[row, 7].Value != null)
                            {
                                Snom = Convert.ToSingle(workSheet.UsedRange.Cells[row, 7].Value.Remove(0, workSheet.UsedRange.Cells[row, 7].Value.IndexOf('-') + 1));
                                if (workSheet.UsedRange.Cells[row, 8].Value != null)
                                    BaseShema.UZ.Add(new UZ(2, workSheet.UsedRange.Cells[row, 4].Value.ToString() + "-НН1 (" + nameRl + ")", 0.38f, 0, cos, Snom * zagr * zagr * zagr * t / cos, Snom * zagr / cos, Snom * koef * zagr / cos, new List<Graf>(), new List<Graf>(), new List<Graf>(), 0, 0));
                                else
                                    BaseShema.UZ.Add(new UZ(2, workSheet.UsedRange.Cells[row, 4].Value.ToString() + "-НН (" + nameRl + ")", 0.38f, 0, cos, Snom * zagr * zagr * zagr * t / cos, Snom * zagr / cos, Snom * koef * zagr / cos, new List<Graf>(), new List<Graf>(), new List<Graf>(), 0, 0));
                                BaseShema.Vet.Add(new Vet(1, -1, -1, BaseShema.UZ[BaseShema.UZ.Count - 2].nameCP, BaseShema.UZ[BaseShema.UZ.Count - 1].nameCP, workSheet.UsedRange.Cells[row, 7].Value.ToString(), Snom));
                                if (workSheet.UsedRange.Cells[row, 8].Value != null)
                                {
                                    Snom = Convert.ToSingle(workSheet.UsedRange.Cells[row, 8].Value.Remove(0, workSheet.UsedRange.Cells[row, 8].Value.IndexOf('-') + 1));
                                    BaseShema.UZ.Add(new UZ(2, workSheet.UsedRange.Cells[row, 4].Value + "-НН2 (" + nameRl + ")", 0.38f, 0, cos, Snom * zagr * zagr * zagr * t / cos, Snom * zagr / cos, Snom * koef * zagr / cos, new List<Graf>(), new List<Graf>(), new List<Graf>(), 0, 0));
                                    BaseShema.Vet.Add(new Vet(1, -1, -1, BaseShema.UZ[BaseShema.UZ.Count - 3].nameCP, BaseShema.UZ[BaseShema.UZ.Count - 1].nameCP, workSheet.UsedRange.Cells[row, 8].Value.ToString(), Snom));
                                    if (workSheet.UsedRange.Cells[row, 9].Value != null)
                                    {
                                        Snom = Convert.ToSingle(workSheet.UsedRange.Cells[row, 9].Value.Remove(0, workSheet.UsedRange.Cells[row, 9].Value.IndexOf('-') + 1));
                                        BaseShema.UZ.Add(new UZ(2, workSheet.UsedRange.Cells[row, 4].Value + "-НН3 (" + nameRl + ")", 0.38f, 0, cos, Snom * zagr * zagr * zagr * t / cos, Snom * zagr / cos, Snom * koef * zagr / cos, new List<Graf>(), new List<Graf>(), new List<Graf>(), 0, 0));
                                        BaseShema.Vet.Add(new Vet(1, -1, -1, BaseShema.UZ[BaseShema.UZ.Count - 4].nameCP, BaseShema.UZ[BaseShema.UZ.Count - 1].nameCP, workSheet.UsedRange.Cells[row, 9].Value.ToString(), Snom));
                                    }
                                }
                            }
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
                    DelegateList.Up_Vet(true);      
                    DelegateList.Up_Uz(true);  
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
                int numRow = dataGridVetv.Rows.Count + 1;
                for (int row = 3; row <= numRow; row++)
                {
                    for (int col = 1; col <= 10; col++)
                    {
                        if (col < 3)
                            xlSheet.Cells[row, col].Value = dataGridVetv.Rows[row - 3].Cells[col - 1].Value;
                        else
                            xlSheet.Cells[row, col].Value = Convert.ToDecimal(dataGridVetv.Rows[row - 3].Cells[col - 1].Value);
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
        private void dataGridV_CellEndEdit(object sender, DataGridViewCellEventArgs e)                 // +
        {
                DataGridView grid = (sender as DataGridView);
                int Stroka = e.RowIndex;
                int Stolbec = e.ColumnIndex;
                if (BaseShema.Vet.Count < dataGridVetv.Rows.Count-1)
                    BaseShema.Vet.Add(new Vet(0,-1,-1,"","","",0));
                try
                {
                    switch(Stolbec)
                    {
                        case 0:
                            switch (grid[0, Stroka].Value.ToString())
                            {
                                case "Лин":
                                    if(BaseShema.Vet[Stroka].tipV!=0)
                                    {   BaseShema.Vet[Stroka].marka= "";
                                    BaseShema.Vet[Stroka].dlina_moshnost= 0;  }
                                    BaseShema.Vet[Stroka].tipV = 0;
                                    SetLineTable(Stroka);
                                    for(int i=3;i<=6;i++)
                                        dataGridVetv.Rows[Stroka].Cells[i].Style.BackColor = Color.Khaki;
                                    break;
                                case "ТР":
                                    if (BaseShema.Vet[Stroka].tipV != 1)
                                    {
                                        BaseShema.Vet[Stroka].marka = "";
                                        BaseShema.Vet[Stroka].dlina_moshnost = 0;
                                    }
                                    BaseShema.Vet[Stroka].tipV = 1;
                                    SetLineTable(Stroka);
                                    for(int i=3;i<=6;i++)
                                        dataGridVetv.Rows[Stroka].Cells[i].Style.BackColor = Color.PaleTurquoise;
                                    break;
                                default:
                                    BaseShema.Vet[Stroka].tipV = 0;
                                    SetLineTable(Stroka);
                                    for(int i=3;i<=6;i++)
                                        dataGridVetv.Rows[Stroka].Cells[i].Style.BackColor = Color.PaleTurquoise;
                                    break;
                            }
                            break;
                        case 6:
                        BaseShema.Vet[Stroka].dlina_moshnost = Convert.ToSingle(grid.Rows[Stroka].Cells[6].Value);
                        SetLineTable(Stroka);
                        break;                        
                    }
                    Changed(true);
                }
                catch (Exception ex)
                {
                    grid.Rows[Stroka].Cells[Stolbec].Value = "";
                    MessageBox.Show("Ячейка (строка:" + (Stroka+1).ToString() + ", столбец:" + (Stolbec+1).ToString() + ") заполнена неправильно!" + Environment.NewLine + Environment.NewLine + "Описание ошибки: " + ex.Message, "Ошибка чтения", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        private void dataGridV_UserDeletedRow(object sender, DataGridViewRowEventArgs e)
        {
            BaseShema.Vet.RemoveAt(dataGridVetv.CurrentRow.Index);
            Changed(true);
        }
        private void dataGridVetv_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex!=BaseShema.Vet.Count)
            switch (e.ColumnIndex)
            {
                case 3:
                    FormUzCreate(e.RowIndex,3);
                    break;
                case 4:
                    FormUzCreate(e.RowIndex, 4);
                    break;
                case 5:
                    FormUzCreate(e.RowIndex, 5);
                    break;
            }
        }
        private void dataGridVetv_CellDoubleClickEnd(int row, int col, string mar, float moshn)
        {
            dataGridVetv.Rows[row].Cells[col].Value = mar;
            if (moshn != 0)
                dataGridVetv.Rows[row].Cells[6].Value = moshn;
            switch (col)
            {
                case 3:
                    BaseShema.Vet[row].nach = mar;
                    break;
                case 4:
                    BaseShema.Vet[row].kon = mar;
                    break;
                case 5:
                    BaseShema.Vet[row].marka = mar;
                    if (moshn != 0)
                        BaseShema.Vet[row].dlina_moshnost = Convert.ToSingle(dataGridVetv.Rows[row].Cells[6].Value);
                    break;
            }
            Changed(true);
        }
        private void FormUzCreate(int row, int col)
        {
            Point aPoint = dataGridVetv.GetCellDisplayRectangle(col, row, false).Location;
            aPoint.X += dataGridVetv.Location.X + dataGridVetv.Columns[col].Width;
            aPoint.Y += dataGridVetv.Location.Y;
            aPoint = PointToScreen(aPoint);
            FormUz aForm = new FormUz();
            aForm.TopMost = true;
            aForm.StartPosition = FormStartPosition.Manual;
            aForm.Location = aPoint;
            aForm.SetInfo(row, col, BaseShema.Vet[row].tipV);
            aForm.Show();
        }
        private void dataGridVetv_CellMouseDown(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {  // клик по комм. аппарат
                this.Text = "Ветви - строка №" + (e.RowIndex+1).ToString();
                if ((e.ColumnIndex == 1) && (BaseShema.Vet.Count != e.RowIndex))
                {
                    if (e.RowIndex < dataGridVetv.Rows.Count - 1)
                    {
                        switch (BaseShema.Vet[e.RowIndex].k1)
                        {
                            case -1:
                                BaseShema.Vet[e.RowIndex].k1 = 0;
                                dataGridVetv[e.ColumnIndex, e.RowIndex].Style.BackColor = Color.Tomato;
                                dataGridVetv[e.ColumnIndex, e.RowIndex].Style.SelectionBackColor = Color.Tomato;
                                break;
                            case 0:
                                BaseShema.Vet[e.RowIndex].k1 = 1;
                                dataGridVetv[e.ColumnIndex, e.RowIndex].Style.BackColor = Color.LightGreen;
                                dataGridVetv[e.ColumnIndex, e.RowIndex].Style.SelectionBackColor = Color.LightGreen;
                                break;
                            case 1:
                                BaseShema.Vet[e.RowIndex].k1 = -1;
                                dataGridVetv[e.ColumnIndex, e.RowIndex].Style.BackColor = Color.White;
                                dataGridVetv[e.ColumnIndex, e.RowIndex].Style.SelectionBackColor = Color.White;
                                break;
                        }
                        Changed(true);
                    }
                    else
                        dataGridVetv[e.ColumnIndex, e.RowIndex].Style.SelectionBackColor = Color.White;
                }
                else if ((e.ColumnIndex == 2) && (BaseShema.Vet.Count != e.RowIndex))
                {
                    if (e.RowIndex < dataGridVetv.Rows.Count - 1)
                    {
                        switch (BaseShema.Vet[e.RowIndex].k2)
                        {
                            case -1:
                                BaseShema.Vet[e.RowIndex].k2 = 0;
                                dataGridVetv[e.ColumnIndex, e.RowIndex].Style.BackColor = Color.Tomato;
                                dataGridVetv[e.ColumnIndex, e.RowIndex].Style.SelectionBackColor = Color.Tomato;
                                break;
                            case 0:
                                BaseShema.Vet[e.RowIndex].k2 = 1;
                                dataGridVetv[e.ColumnIndex, e.RowIndex].Style.BackColor = Color.LightGreen;
                                dataGridVetv[e.ColumnIndex, e.RowIndex].Style.SelectionBackColor = Color.LightGreen;
                                break;
                            case 1:
                                BaseShema.Vet[e.RowIndex].k2 = -1;
                                dataGridVetv[e.ColumnIndex, e.RowIndex].Style.BackColor = Color.White;
                                dataGridVetv[e.ColumnIndex, e.RowIndex].Style.SelectionBackColor = Color.White;
                                break;
                        }
                        Changed(true);
                    }
                    else
                        dataGridVetv[e.ColumnIndex, e.RowIndex].Style.SelectionBackColor = Color.White;
                }
            }

            else // контекстное меню
            {
                if (BaseShema.Vet.Count > 0)
                {
                    dataGridVetv.ClearSelection();
                    dataGridVetv.Rows[e.RowIndex].Selected = true;
                    contextMenuStrip2.Show(MousePosition);
                }
            }
        }

        private void toolStripMenuItem2_Click(object sender, EventArgs e)
        {
           BaseShema.Vet.RemoveAt(dataGridVetv.SelectedRows[0].Index);           
           dataGridVetv.Rows.RemoveAt(dataGridVetv.SelectedRows[0].Index);
           this.Changed(true);            
        }

        private void OpenGORSR_Click(object sender, EventArgs e)
        {

        }

       

    }
}
