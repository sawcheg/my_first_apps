using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace STREAMS
{
    public partial class OptimizForm : Form
    {
        Opt_Razm opt;
        bool razv;
        public OptimizForm()
        {
            InitializeComponent();
            opt = new Opt_Razm();
            razv = false;
            this.Height = 224;
        }
        private void button2_Click(object sender, EventArgs e)
        {
            opt.Apply_KA();
            opt.Apply_napr();
            opt.Apply_prov();
            opt.Apply_tr();
            Apply.Enabled = false;
            DelegateList.Up_Vet(true);      
            DelegateList.Up_Uz(true);  
        }
        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox1.Checked == true)
                numericUpDown1.Enabled = true;
            else
                numericUpDown1.Enabled = false;

        }
        private void button1_Click(object sender, EventArgs e)
        {            
            int[] a = new int[24];
            List<float> n = new List<float>();
            opt.Setting(checkBox1.Checked ? (int)numericUpDown1.Value : 0, checkBox2.Checked, checkBox3.Checked, checkBox4.Checked, checkBox5.Checked, checkBox6.Checked);
            n = opt.Run();
            //if (a != null)
            eff1.Text = Math.Round(n[0],3).ToString();
            eff2.Text = Math.Round(n[1], 3).ToString();
            eff3.Text = Math.Round(n[2], 3).ToString();
            eff4.Text = Math.Round(n[3], 3).ToString();
            effS.Text = Math.Round(n[0] + n[1] + n[2] + n[3], 3).ToString();
            RunOpt.Enabled = false;
            ShowLog.Enabled = true;
            Apply.Enabled = true;
            this.AcceptButton = this.Apply;
        }
        private void button3_Click(object sender, EventArgs e)
        {
            if (razv)
            {
                ShowLog.Text = "Показать рекомендации";
                this.Height = 224;
                razv = false;
            }
            else
            {
                LogText.Text = opt.Retutn_Log();
                ShowLog.Text = "Скрыть рекомендации";
                this.Height = 520;
                razv = true;
            }
        }
    }
}
