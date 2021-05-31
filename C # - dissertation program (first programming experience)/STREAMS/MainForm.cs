using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Text;
using System.IO;
using System.Windows.Forms;
using System.Runtime.Serialization.Formatters.Binary;



namespace STREAMS
{
    public interface IForm1
    {
        void createNewSheme(String s);
    }    
    public partial class MainForm : Form, IForm1
    { 
        public Encoding enc = Encoding.GetEncoding(1251);
        BinaryFormatter formatter = new BinaryFormatter();        
        CPForm cpForm = new CPForm();
        VetviForm vetviForm = new VetviForm();        
        public MainForm()
        {
                InitializeComponent();
                cpForm.MdiParent = this;
                vetviForm.MdiParent = this;
                DelegateList.Save = new DelegateList.SaveChange(save_change);
                DelegateList.Up_Vet = new DelegateList.UpdateVet(VetForm_Create);
                DelegateList.Up_Uz = new DelegateList.UpdateUz(CpForm_Create);
        }
        private void EnableSh()                                                                     // доделать
        {
            this.сохранитьToolStripMenuItem.Enabled = true;
            this.сохранитьКакToolStripMenuItem.Enabled = true;
            this.данныеToolStripMenuItem.Enabled = true;
            this.ГрафикаToolStripMenuItem.Enabled = true;
            this.расчетыToolStripMenuItem.Enabled = true;
        }
        public void createNewSheme(String s)                                                        // +
        {
            BaseShema.path = Application.StartupPath + @"\" + s + ".sh";
            BaseShema.UZ = new List<UZ>();
            BaseShema.Vet = new List<Vet>();
            Sheme sh = new Sheme(BaseShema.UZ, BaseShema.Vet);
            if (File.Exists(BaseShema.path))
            {
                if (MessageBox.Show("Файл с таким именем уже существует. Перезаписать?", "Внимание", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    using (Stream cr = File.Create(BaseShema.path))
                    { formatter.Serialize(cr, sh); }
                    openSh(BaseShema.path, true);
                    EnableSh();
                }
            }
            else
            {
                using (Stream cr = File.Create(BaseShema.path))
                { formatter.Serialize(cr, sh);}
                openSh(BaseShema.path, true);
                EnableSh();
            }
        }
        private void SaveShemeInFile(bool saveAs)                                                   // +
        {
            Sheme sh = new Sheme(BaseShema.UZ, BaseShema.Vet);
            if (!saveAs)
            {
                using (Stream cr = File.OpenWrite(BaseShema.path))             //сохранить 
                { formatter.Serialize(cr, sh); }
            }
            else
            {
                saveFileDialog1.InitialDirectory = Application.StartupPath;     //сохранить в файл..
                saveFileDialog1.Filter = "Схема сети (*.sh)|*.sh";
                saveFileDialog1.FileName = "";
                if (saveFileDialog1.ShowDialog() == DialogResult.OK)
                {
                    BaseShema.path = saveFileDialog1.FileName;
                    using (Stream cr = File.Create(BaseShema.path))             
                    { formatter.Serialize(cr, sh);}
                    this.Text = "STREAMS - Файл данных: " + BaseShema.path;
                }
            }
        }
        private void создатьНовуюToolStripMenuItem_Click(object sender, EventArgs e)                // +
        {
            newNameSheme vvodname;
            vvodname = new newNameSheme(this);
            vvodname.MdiParent = this;
            vvodname.Show();  
        }           
        public void openSh(string n, bool newFile)                                                  // +
        {
            this.Text = "STREAMS - Файл данных: " + n;
            if (newFile == false)
            {
                Sheme sh;
                using (Stream input = File.OpenRead(n))             //десериализация файла
                {sh = (Sheme)formatter.Deserialize(input); }
                BaseShema.path = n;
                BaseShema.UZ = sh.UZ;
                BaseShema.Vet = sh.Vet;               
            }
            VetForm_Create(false);
            CpForm_Create(false);            
        }
        public void save_change()                                                                        // +
        {
            SaveShemeInFile(false);
        }
        private void открытьСхемуToolStripMenuItem_Click(object sender, EventArgs e)                // +
        {
            openFileDialog1.InitialDirectory = Application.StartupPath;
            openFileDialog1.Filter = "Схема сети (*.sh)|*.sh";
            openFileDialog1.FileName = "";
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                openSh(openFileDialog1.FileName, false);
                EnableSh();
            }  
        }
        private void сохранитьToolStripMenuItem_Click(object sender, EventArgs e)                   // +
        {
            SaveShemeInFile(false);
        }
        private void сохранитьКакToolStripMenuItem_Click(object sender, EventArgs e)                // +
        {
            SaveShemeInFile(true);
        }
        private void выходToolStripMenuItem1_Click(object sender, EventArgs e)                      // добавить сохранение
        {
            if (MessageBox.Show("Выйти из программы?", "Выход",  MessageBoxButtons.YesNo,MessageBoxIcon.Question) == DialogResult.Yes)
            {
                Application.Exit();
            }
        }
        private void UzliMenuItem_Click(object sender, EventArgs e) //+
        {
            if(cpForm.Visible ==false)
            CpForm_Create(false);
        }
        private void CpForm_Create(bool change)
        {
                cpForm.ClearTable();
                if (BaseShema.UZ.Count != 0)
                {
                    for(int num=0;num<BaseShema.UZ.Count;num++)                      //перенос списка в таблицы
                        cpForm.SetTable1(num);
                }
                else
                {
                    cpForm.ClearTable();
                }
                if (change)
                    cpForm.Changed(true);
                else
                    cpForm.Changed(false);
                cpForm.Show();
        }
        public void VetForm_Create(bool change)
        {
            vetviForm.ClearTable();
            if (BaseShema.Vet.Count != 0)
            {
                for (int num = 0; num < BaseShema.Vet.Count;num++)                       //перенос списка в таблицы
                    vetviForm.SetTable1(num);
            }
            else
            {
                vetviForm.ClearTable();
            }
            if (change)
                vetviForm.Changed(true);
            else
                vetviForm.Changed(false);
            vetviForm.Show();
        }             //создание формы узлов
        private void katalogLT_Click(object sender, EventArgs e)
        {
            Katalog katForm = new Katalog();
            katForm.MdiParent = this;
            try
            {
                using (Stream input = File.OpenRead(Application.StartupPath + @"\SprInfo.dll"))             //десериализация файла
                { katForm.sprInfo = (SprInfo)formatter.Deserialize(input); }
            }
            catch (Exception ex)
            { MessageBox.Show("Ошибка при чтении справочных данных!" + Environment.NewLine + ex.ToString()); }
            katForm.SetTableTR();
            katForm.Show();
        }
        private void VetviMenuItem_Click(object sender, EventArgs e)
        {
            if(vetviForm.Visible ==false)
            VetForm_Create(false);
        }
        private void Proverka_oshibok_Click(object sender, EventArgs e)
        {
            raschet r = new raschet();           
            r.Run();
            r.Show_dialog();
        }
        private void Raschet_Poteri_Click(object sender, EventArgs e)
        {
            bool prod = false;
            raschet r = new raschet();
            r.Run();
            if (r.sum - r.zamechania != 0)
                r.Show_dialog();
            else if (r.zamechania != 0)
            {
                r.change_name_button();
                prod = r.Show_dialog();
            } 
            else
                prod = true;
            if (prod)
            {                             
                int start = Environment.TickCount;
                r.Regim();
                int end = Environment.TickCount;         
                r.in_result((float)(end-start)/1000,this);
                //for(int i=0;i<24;i++)
                //r.in_test(10);
                this.Optimization.Enabled = true;
            }

        }
        private void Optimization_Click(object sender, EventArgs e)
        {
            OptimizForm op_form = new OptimizForm();
            op_form.MdiParent = this;
            op_form.Show();

        }

        private void оПрограммеToolStripMenuItem_Click(object sender, EventArgs e)
        {
            About a = new About();
            //a.MdiParent = this;
            a.ShowDialog();
        }  
    }
}


