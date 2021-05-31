using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Excel = Microsoft.Office.Interop.Excel;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace STREAMS
{
    public partial class Katalog : Form
    {
        BinaryFormatter formatter = new BinaryFormatter();
        public SprInfo sprInfo = new SprInfo();
        int k = 1;
        bool changed = false;
        public Katalog()
        {        
            InitializeComponent();            
        } 
        private void ExcelButton_Click(object sender, EventArgs e)
        {
            contextMenuStrip1.Show(ExcelButton, new Point(30, 0));
        }
        private void Changed(bool ch)
        {
            if (ch)
            {
                this.savebt.Enabled = true;
                changed = true;
            }
            else
            {
                this.savebt.Enabled = false;
                changed = false;
            }
        }
        private void загрузитьИзToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Excel.Application App = new Excel.Application();
            App.FindFile();
            if (App.ActiveSheet != null)
            {
                Excel.Worksheet workSheet = App.ActiveSheet;
                try
                {
                    if (k == 1)
                    {
                        katTR.RowCount = workSheet.UsedRange.Cells.Rows.Count - 1;
                        for (int row = 3; row <= workSheet.UsedRange.Cells.Rows.Count; row++)
                        {
                            for (int col = 2; col <= 19; col++)
                            {
                                if (workSheet.UsedRange.Cells[row, col].Value != null)
                                    katTR.Rows[row - 3].Cells[col - 2].Value = workSheet.UsedRange.Cells[row, col].Value;
                                else
                                    katTR.Rows[row - 3].Cells[col - 2].Value = "";
                            }
                        }
                    }
                    else if (k == 2)
                    {
                        katTrSv.RowCount = workSheet.UsedRange.Cells.Rows.Count - 1;
                        for (int row = 3; row <= workSheet.UsedRange.Cells.Rows.Count; row++)
                        {
                            for (int col = 2; col <= 15; col++)
                            {
                                if (workSheet.UsedRange.Cells[row, col].Value != null)
                                    katTrSv.Rows[row - 3].Cells[col - 2].Value = workSheet.UsedRange.Cells[row, col].Value;
                                else
                                    katTrSv.Rows[row - 3].Cells[col - 2].Value = "";
                            }
                        }
                    }
                    else if (k == 3) 
                    {
                        ProvodaTb.RowCount = workSheet.UsedRange.Cells.Rows.Count - 2;
                        for (int row = 4; row <= workSheet.UsedRange.Cells.Rows.Count; row++)
                        {
                            for (int col = 2; col <= 10; col++)
                            {
                                if (workSheet.UsedRange.Cells[row, col].Value != null)
                                    ProvodaTb.Rows[row - 4].Cells[col - 2].Value = workSheet.UsedRange.Cells[row, col].Value;
                                else
                                    ProvodaTb.Rows[row - 4].Cells[col - 2].Value = "";
                            }
                        }
                    }
                    else if (k == 4)
                    {
                        KabelTb.RowCount = workSheet.UsedRange.Cells.Rows.Count - 2;
                        for (int row = 4; row <= workSheet.UsedRange.Cells.Rows.Count; row++)
                        {
                            for (int col = 2; col <= 15; col++)
                            {
                                if (workSheet.UsedRange.Cells[row, col].Value != null)
                                    KabelTb.Rows[row - 4].Cells[col - 2].Value = workSheet.UsedRange.Cells[row, col].Value;
                                else
                                    KabelTb.Rows[row - 4].Cells[col - 2].Value = "";
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
                    Changed(true);
                    App.Quit();
                    releaseObject(workSheet);
                    //Changed();                    
                }
            }
            releaseObject(App);

        }
        private void releaseObject(object obj)                                                                // +
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
        public void SetTableTR()                                                                   // +
        {
            katTR.Rows.Clear();
            foreach (TR tr in sprInfo.TrInfo)
              katTR.Rows.Add(tr.marka,tr.klassNomNapr,tr.shSoedVn,tr.shSoedNn,tr.uvn,tr.unn,tr.snom,tr.dpxx,tr.dpkz,tr.ukz
                                ,tr.ixx,tr.kye,tr.prizn,tr.shagReg,tr.nnul,tr.nmin,tr.nmax,tr.primech);
            katTrSv.Rows.Clear();
            foreach (TR tr in sprInfo.TrSvInfo)
              katTrSv.Rows.Add(tr.marka, tr.uvn, tr.unn, tr.snom, tr.dpxx, tr.dpkz, tr.ukz
                                , tr.ixx, tr.kye, tr.shagReg, tr.nnul, tr.nmin, tr.nmax, tr.primech);
            ProvodaTb.Rows.Clear();
            foreach (Prov pr in sprInfo.ProvInfo)
                ProvodaTb.Rows.Add(pr.marka, pr.R0, pr.R070, pr.X0, pr.sechenie, pr.idop, pr.B0, pr.B0emk, pr.primech);
            KabelTb.Rows.Clear();
            foreach (Kabel kab in sprInfo.KabelInfo)
                KabelTb.Rows.Add(kab.marka, kab.Uiz, kab.fR0, kab.fR070, kab.fX0, kab.fsechenie, kab.nR0, kab.nR070, kab.nX0, kab.nsechenie,
                kab.idop, kab.B0, kab.B0emk, kab.primech);
            Changed(false);
        }       
        private void savebt_Click(object sender, EventArgs e)
        {
            SprInfo sprInfo = new SprInfo();
            sprInfo.TrInfo = new List<TR>();
            sprInfo.TrSvInfo = new List<TR>();
            sprInfo.ProvInfo = new List<Prov>();
            sprInfo.KabelInfo = new List<Kabel>();
            BinaryFormatter formatter = new BinaryFormatter();
            sprInfo.TrInfo.Clear();
            sprInfo.TrSvInfo.Clear();
            sprInfo.ProvInfo.Clear();
            sprInfo.KabelInfo.Clear();
            for (int i = 0; i < katTR.RowCount-1; i++)
            {
                sprInfo.TrInfo.Add(new TR(katTR.Rows[i].Cells[0].Value.ToString(), Convert.ToInt32(katTR.Rows[i].Cells[1].Value), katTR.Rows[i].Cells[2].Value.ToString(),
                       katTR.Rows[i].Cells[3].Value.ToString(), Convert.ToSingle(katTR.Rows[i].Cells[4].Value), Convert.ToSingle(katTR.Rows[i].Cells[5].Value), Convert.ToInt32(katTR.Rows[i].Cells[6].Value)
                       , Convert.ToSingle(katTR.Rows[i].Cells[7].Value), Convert.ToSingle(katTR.Rows[i].Cells[8].Value), Convert.ToSingle(katTR.Rows[i].Cells[9].Value), Convert.ToSingle(katTR.Rows[i].Cells[10].Value),
                       katTR.Rows[i].Cells[11].Value.ToString(), katTR.Rows[i].Cells[12].Value.ToString(), katTR.Rows[i].Cells[13].Value.ToString(), katTR.Rows[i].Cells[14].Value.ToString(),
                       katTR.Rows[i].Cells[15].Value.ToString(), katTR.Rows[i].Cells[16].Value.ToString(), katTR.Rows[i].Cells[17].Value.ToString()));
              
            }
            for (int i = 0; i < katTrSv.RowCount-1; i++)
            {
                sprInfo.TrSvInfo.Add(new TR(katTrSv.Rows[i].Cells[0].Value.ToString(), Convert.ToSingle(katTrSv.Rows[i].Cells[1].Value), Convert.ToSingle(katTrSv.Rows[i].Cells[2].Value), Convert.ToInt32(katTrSv.Rows[i].Cells[3].Value)
                       , Convert.ToSingle(katTrSv.Rows[i].Cells[4].Value), Convert.ToSingle(katTrSv.Rows[i].Cells[5].Value), Convert.ToSingle(katTrSv.Rows[i].Cells[6].Value), Convert.ToSingle(katTrSv.Rows[i].Cells[7].Value),
                       katTrSv.Rows[i].Cells[8].Value.ToString(), katTrSv.Rows[i].Cells[9].Value.ToString(), katTrSv.Rows[i].Cells[10].Value.ToString(),
                       katTrSv.Rows[i].Cells[11].Value.ToString(), katTrSv.Rows[i].Cells[12].Value.ToString(), katTrSv.Rows[i].Cells[13].Value.ToString()));

            }
            for (int i = 0; i < ProvodaTb.RowCount - 1; i++)
            {
                sprInfo.ProvInfo.Add(new Prov(ProvodaTb.Rows[i].Cells[0].Value.ToString(), Convert.ToSingle(ProvodaTb.Rows[i].Cells[1].Value), Convert.ToSingle(ProvodaTb.Rows[i].Cells[2].Value), Convert.ToSingle(ProvodaTb.Rows[i].Cells[3].Value)
                    , Convert.ToSingle(ProvodaTb.Rows[i].Cells[4].Value), Convert.ToSingle(ProvodaTb.Rows[i].Cells[5].Value), Convert.ToSingle(ProvodaTb.Rows[i].Cells[6].Value), Convert.ToSingle(ProvodaTb.Rows[i].Cells[7].Value)
                    , ProvodaTb.Rows[i].Cells[8].Value.ToString()));
            }
            for (int i = 0; i < KabelTb.RowCount - 1; i++)
            {
                sprInfo.KabelInfo.Add(new Kabel(KabelTb.Rows[i].Cells[0].Value.ToString(), Convert.ToSingle(KabelTb.Rows[i].Cells[1].Value),Convert.ToSingle(KabelTb.Rows[i].Cells[2].Value),
                    Convert.ToSingle(KabelTb.Rows[i].Cells[3].Value),Convert.ToSingle(KabelTb.Rows[i].Cells[4].Value),Convert.ToSingle(KabelTb.Rows[i].Cells[5].Value),Convert.ToSingle(KabelTb.Rows[i].Cells[6].Value),
                    Convert.ToSingle(KabelTb.Rows[i].Cells[7].Value),Convert.ToSingle(KabelTb.Rows[i].Cells[8].Value),Convert.ToSingle(KabelTb.Rows[i].Cells[9].Value),Convert.ToSingle(KabelTb.Rows[i].Cells[10].Value),
                    Convert.ToSingle(KabelTb.Rows[i].Cells[11].Value),Convert.ToSingle(KabelTb.Rows[i].Cells[12].Value),KabelTb.Rows[i].Cells[13].Value.ToString()));
            }

            using (Stream cr = File.OpenWrite(Application.StartupPath+@"\SprInfo.dll"))             //сохранить 
                { formatter.Serialize(cr, sprInfo); }
            Changed(false);

        }
        private void tabControl1_SelectedIndexChanged(object sender, EventArgs e)
        {
            k=tabControl1.SelectedIndex +1;
        }
        private void Katalog_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (e.CloseReason == CloseReason.UserClosing)
            {
                e.Cancel = true;
                CloseBt_Click(this.CloseBt, e);         
            }
        }
        private void CloseBt_Click(object sender, EventArgs e)
        {
            if (changed)
                if (MessageBox.Show("В таблицах редактировались данные. Сохранить изменения?", "Подтверждение о сохранении", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                { this.savebt_Click(this, e); }
            this.Dispose();
        }
        private void katTR_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            Changed(true);
        }

        private void KabelTb_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            Changed(true);
        }

        private void ProvodaTb_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            Changed(true);
        }

        private void katTrSv_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            Changed(true);
        }

       

    }
    [Serializable]
    public class SprInfo
    {
        public List<TR> TrInfo;
        public List<TR> TrSvInfo;
        public List<Prov> ProvInfo;
        public List<Kabel> KabelInfo;
    }
    [Serializable]
    public class TR
    {
        public string marka ;
        public int klassNomNapr;        
        public string shSoedVn;
        public string shSoedNn;
        public float uvn;
        public float unn;
        public int snom ;        
        public float dpxx ;
        public float dpkz ;
        public float ukz ;
        public float ixx ;
        public string kye ;
        public string prizn;
        public string shagReg;
        public string nnul ;
        public string nmin;
        public string nmax;
        public string primech ;

        public TR(string mar, int klassN, string ShSoedVn, string ShSoedNn, float Uvn, float Unn,
            int Snom, float Dpxx, float Dpkz, float Ukz, float Ixx, string Kye, string Prizn, string ShagReg
            , string Nnul, string Nmin, string Nmax, string Primech)
        {
            this.marka = mar;
            this.klassNomNapr = klassN;
            this.shSoedVn = ShSoedVn;
            this.shSoedNn = ShSoedNn;
            this.uvn = Uvn;
            this.unn = Unn;
            this.snom = Snom;
            this.dpxx = Dpxx;
            this.dpkz = Dpkz;
            this.ukz = Ukz;
            this.ixx = Ixx;
            this.kye = Kye;
            this.prizn = Prizn;
            this.shagReg = ShagReg;
            this.nnul = Nnul;
            this.nmin = Nmin;
            this.nmax = Nmax;
            this.primech = Primech;
        }
        public TR(string mar, float Uvn, float Unn, int Snom, float Dpxx, float Dpkz, float Ukz, float Ixx, string Kye, string ShagReg
           , string Nnul, string Nmin, string Nmax, string Primech)
        {
            this.marka = mar;
            this.uvn = Uvn;
            this.unn = Unn;
            this.snom = Snom;
            this.dpxx = Dpxx;
            this.dpkz = Dpkz;
            this.ukz = Ukz;
            this.ixx = Ixx;
            this.kye = Kye;
            this.shagReg = ShagReg;
            this.nnul = Nnul;
            this.nmin = Nmin;
            this.nmax = Nmax;
            this.primech = Primech;
        }

    }
    [Serializable]
    public class Prov
    {
        public string marka;
        public float R0;
        public float R070;
        public float X0;
        public float sechenie;
        public float idop;
        public float B0;
        public float B0emk;
        public string primech;

        public Prov(string marka, float R0,float R070, float X0,float sechenie,float idop,
        float B0,float B0emk,string primech)
        {
            this.marka = marka;
            this.R0=R0;
            this.R070=R070;
            this.X0=X0;
            this.sechenie=sechenie;
            this.idop=idop;
            this.B0=B0;
            this.B0emk=B0emk;
            this.primech = primech;
        }       
    }
    [Serializable]
    public class Kabel
    {
        public string marka;
        public float Uiz;
        public float fR0;
        public float fR070;
        public float fX0;
        public float fsechenie;
        public float nR0;
        public float nR070;
        public float nX0;
        public float nsechenie;
        public float idop;
        public float B0;
        public float B0emk;
        public string primech;

        public Kabel(string marka, float Uiz, float fR0, float fR070, float fX0, float fsechenie, float nR0, float nR070, float nX0, float nsechenie,
        float idop, float B0, float B0emk, string primech)
        {
            this.marka = marka;
            this.Uiz = Uiz;
            this.fR0 = fR0;
            this.fR070 = fR070;
            this.fX0 = fX0;
            this.fsechenie = fsechenie;
            this.nR0 = nR0;
            this.nR070 = nR070;
            this.nX0 = nX0;
            this.nsechenie = nsechenie;
            this.idop = idop;
            this.B0 = B0;
            this.B0emk = B0emk;
            this.primech = primech;
        }
    }

}

