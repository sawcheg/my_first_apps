using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace STREAMS
{
    public partial class TestForm : Form
    {
        float dP = 0;
        public TestForm()
        {
            InitializeComponent(); 
        }
        public void Clear()
        { dataGridView1.Rows.Clear(); this.Text = "Потери = "; dP = 0; }
        public void SetTable(UZ cp)                                                                     // 
        {
            //dataGridView1.Rows.Add(cp.nameVN, cp.nameNN, cp.nomN, cp.napr,cp.TipTR, cp.cosFi, cp.rsys, cp.xsys);
        }
        public void addvet(L_T_Vetvi l,string nach,string kon)
        {
            string s = "",s1="";
            for (int i = 0; i < l.posle.Count; i++)
                s += l.posle[i].ToString() + ",";
            for (int i = 0; i < l.posle_razom.Count; i++)
                s1 += l.posle_razom[i].ToString() + ",";
            dataGridView1.Rows.Add(l.numRL,l.l_tr? "Лин":"Тр" ,nach,kon,l.ka,l.Unom,l.R,l.X,l.Pn,l.Qn,l.dP,l.dQ,l.Pk,l.Qk,l.Un,l.dU,l.Uk,l.dPxx,l.dQxx,l.I,l.Idop,l.ktr,l.napravlenie? "прямо":"обратно", l.pred,s,s1);
            dP += l.dPxx + l.dP;
        }
        public float Rezultat()
        {
            this.Text = "Потери = " + dP.ToString();
            return dP;
        }
    }
}
