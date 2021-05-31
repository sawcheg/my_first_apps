using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

namespace STREAMS
{
    public partial class FormUz : Form
    {
        List<UZ> uzlist = BaseShema.UZ;
        SprInfo sprInfo;
        int tipV, row, col;
        public FormUz()
        {
            InitializeComponent();
        }
        public void SetKatalog()
        {   
            Encoding enc = Encoding.GetEncoding(1251);
            BinaryFormatter formatter = new BinaryFormatter();
            try
            {
                using (Stream input = File.OpenRead(Application.StartupPath + @"\SprInfo.dll"))             //десериализация файла
                { sprInfo = (SprInfo)formatter.Deserialize(input); }
            }
            catch (Exception ex)
            { MessageBox.Show("Ошибка при чтении справочных данных!" + Environment.NewLine + ex.ToString()); }
        }
        public void SetInfo(int row,int col,int tipV)
        {
            this.tipV = tipV;
            this.row = row;
            this.col = col;
            switch (col)
            {
                case 5:
                    SetKatalog();
                    switch (tipV)
                    {
                        case 0:
                            this.Text = "Список проводов и кабелей";
                            tabControl1.TabPages[0].Text = "Провод";
                            tabControl1.TabPages[1].Text = "Кабель";
                            tabControl1.TabPages.RemoveAt(2);
                            tabControl1.TabPages.RemoveAt(2);
                            listBox1.Items.Clear();
                            listBox2.Items.Clear();
                            foreach (Prov pr in sprInfo.ProvInfo)
                                listBox1.Items.Add(pr.marka.ToString());
                            foreach (Kabel kb in sprInfo.KabelInfo)
                                listBox2.Items.Add(kb.marka.ToString() + " - " + kb.Uiz.ToString() + " кВ");
                            break;
                        default:
                            this.Text = "Список трансформаторов";
                            tabControl1.TabPages[0].Text = "Тр-р.";
                            tabControl1.TabPages[1].Text = "Тр.связи";
                            comboBox1.Items.RemoveAt(0);
                            tabControl1.TabPages.RemoveAt(2);
                            tabControl1.TabPages.RemoveAt(2);
                            listBox1.Items.Clear();
                            listBox2.Items.Clear();
                            foreach (TR tr in sprInfo.TrInfo)
                                listBox1.Items.Add(tr.marka.ToString() + "    " + tr.klassNomNapr.ToString() + "/" + tr.unn.ToString());
                            foreach (TR tr in sprInfo.TrSvInfo)
                                listBox2.Items.Add(tr.marka.ToString() + "    " + tr.klassNomNapr.ToString() + "/" + tr.unn.ToString());
                            break;
                    }
                    break;
                default:
                    this.Text = "Список доступных узлов";
                    tabControl1.TabPages[0].Text = "ЦП";
                    tabControl1.TabPages[1].Text = "БС";
                    tabControl1.TabPages[2].Text = "Нагр";
                    tabControl1.TabPages[3].Text = "Пром";
                    listBox1.Items.Clear();
                    listBox2.Items.Clear();
                    listBox3.Items.Clear();
                    listBox4.Items.Clear();
                    foreach (UZ uz in uzlist)
                        switch (uz.tipUz)
                        {
                            case 0:
                                listBox1.Items.Add(uz.nameCP.ToString());
                                break;
                            case 1:
                                listBox2.Items.Add(uz.nameCP.ToString());
                                break;
                            case 2:
                                listBox3.Items.Add(uz.nameCP.ToString());
                                break;
                            case 3:
                                listBox4.Items.Add(uz.nameCP.ToString());
                                break;
                        }
                    break;
            }
        }
        private void comboBox1_SelectedValueChanged(object sender, EventArgs e)
        {
            float Unom = Convert.ToSingle(comboBox1.SelectedItem);
            switch (col)
            {
                case 5:
                    switch (tipV)
                    {
                        case 0:
                            listBox2.Items.Clear();
                            foreach (Kabel kb in sprInfo.KabelInfo)
                                if (kb.Uiz == Unom)
                                    listBox2.Items.Add(kb.marka.ToString() + " - " + kb.Uiz.ToString() + " кВ");
                            break;
                        default:
                            listBox1.Items.Clear();
                            listBox2.Items.Clear();
                            foreach (TR tr in sprInfo.TrInfo)
                                if (tr.klassNomNapr == Unom)
                                    listBox1.Items.Add(tr.marka.ToString() + "    " + tr.klassNomNapr.ToString() + "/" + tr.unn.ToString());
                            foreach (TR tr in sprInfo.TrSvInfo)
                                if (tr.uvn == Unom)
                                    listBox2.Items.Add(tr.marka.ToString() + "    " + tr.uvn.ToString() + "/" + tr.unn.ToString());
                            break;
                    }
                    break;
                default:
                    listBox1.Items.Clear();
                    listBox2.Items.Clear();
                    listBox3.Items.Clear();
                    listBox4.Items.Clear();
                    foreach (UZ uz in uzlist)
                        switch (uz.tipUz)
                        {
                            case 0:
                                if (uz.nomN == Unom)
                                    listBox1.Items.Add(uz.nameCP.ToString());
                                break;
                            case 1:
                                if (uz.nomN == Unom)
                                    listBox2.Items.Add(uz.nameCP.ToString());
                                break;
                            case 2:
                                if (uz.nomN == Unom)
                                    listBox3.Items.Add(uz.nameCP.ToString());
                                break;
                            case 3:
                                if (uz.nomN == Unom)
                                    listBox4.Items.Add(uz.nameCP.ToString());
                                break;
                        }
                    break;
            }
        }
        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            string search = textBox1.Text;
            switch (tabControl1.SelectedIndex)
            {
                case 0:
                    for (int i = 0; i < listBox1.Items.Count; i++)
                    {
                        string str = listBox1.Items[i].ToString();
                        if (str.StartsWith(search))
                        {
                            listBox1.ClearSelected();
                            listBox1.SelectedItem = listBox1.Items[i];
                            break;
                        }
                    }
                    break;
                case 1:
                    for (int i = 0; i < listBox2.Items.Count; i++)
                    {
                        string str = listBox2.Items[i].ToString();
                        if (str.StartsWith(search))
                        {
                            listBox2.ClearSelected();
                            listBox2.SelectedItem = listBox2.Items[i];
                            break;
                        }
                    }
                    break;
                case 2:
                    for (int i = 0; i < listBox3.Items.Count; i++)
                    {
                        string str = listBox3.Items[i].ToString();
                        if (str.StartsWith(search))
                        {
                            listBox3.ClearSelected();
                            listBox3.SelectedItem = listBox3.Items[i];
                            break;
                        }
                    }
                    break;
                case 3:
                    for (int i = 0; i < listBox4.Items.Count; i++)
                    {
                        string str = listBox4.Items[i].ToString();
                        if (str.StartsWith(search))
                        {
                            listBox4.ClearSelected();
                            listBox4.SelectedItem = listBox4.Items[i];
                            break;
                        }
                    }
                    break;

            }

        }
        private void SetButton_Click(object sender, EventArgs e)
        {
            try
            {
                switch (tabControl1.SelectedIndex)
                {
                    case 0:
                        Vibor(listBox1.SelectedItem.ToString());
                        break;
                    case 1:
                        Vibor(listBox2.SelectedItem.ToString());
                        break;
                    case 2:
                        Vibor(listBox3.SelectedItem.ToString());
                        break;
                    case 3:
                        Vibor(listBox4.SelectedItem.ToString());
                        break;
                }
            }
            catch
            { MessageBox.Show("Сначала выделите в списке", "Ошибка"); }
        }
        private void listBox1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            Vibor(listBox1.SelectedItem.ToString());
        }
        private void listBox2_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            Vibor(listBox2.SelectedItem.ToString());
        }
        private void listBox3_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            Vibor(listBox3.SelectedItem.ToString());
        }
        private void listBox4_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            Vibor(listBox4.SelectedItem.ToString());
        }
        private void Vibor(string param)
        {
            string mar;
            if(col>4)
            {string[] cash = param.Split(new char[] { ' ' });
                 mar= cash[0];}
            else
                mar=param; 
            float mosch=0;
            switch (tipV)
            {
                case 0:
                    DelegateList.EventFormUZ_End(row, col, mar, 0);
                    break;
                case 1:
                    switch (col)
                    {
                        case 5:
                            for (int i = 0; i < sprInfo.TrInfo.Count; i++)
                                if (mar == sprInfo.TrInfo[i].marka)
                                {
                                    mosch = sprInfo.TrInfo[i].snom;
                                    break;
                                }
                            if (mosch == 0)
                                for (int i = 0; i < sprInfo.TrSvInfo.Count; i++)
                                    if (mar == sprInfo.TrSvInfo[i].marka)
                                    {
                                        mosch = sprInfo.TrSvInfo[i].snom;
                                        break;
                                    }
                            break;
                        default:
                            mosch = 0;
                            break;
                    }
                    DelegateList.EventFormUZ_End(row, col, mar, mosch);
                    break;
            }
            this.Dispose();
        }      
    }
}
