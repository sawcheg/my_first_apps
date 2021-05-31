using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Drawing.Printing;

namespace STREAMS
{
    public partial class Rezultat : Form    {
        Bitmap memoryImage;
        public Rezultat(int col_lin, int col_lin_v, float dlina_lin, float dlina_lin_v, int col_tr, int col_tr_v
            , float ust_m_tr, float ust_m_tr_v, int col_Cp, int col_Bs, float time)
        {            
            InitializeComponent();
            if (col_lin != col_lin_v)
                this.colLin.Text = col_lin.ToString() + " (" + col_lin_v.ToString() + ")";
            else
                this.colLin.Text = col_lin.ToString();
            if (col_tr != col_tr_v)
                this.colTR.Text = col_tr.ToString() + " (" + col_tr_v.ToString() + ")";
            else
                this.colTR.Text = col_tr.ToString();
            if (dlina_lin != dlina_lin_v)
                this.sumDlin.Text = Math.Round(dlina_lin, 3).ToString() + " (" + Math.Round(dlina_lin_v, 3).ToString() + ")";
            else
                this.sumDlin.Text = Math.Round(dlina_lin, 3).ToString();

            if (ust_m_tr != ust_m_tr_v)
                this.sumUst.Text = (ust_m_tr/1000).ToString() + " (" + (ust_m_tr_v/1000).ToString()+")";
            else
                this.sumUst.Text = (ust_m_tr / 1000).ToString();
            this.colCP.Text = col_Cp.ToString();
            this.colBS.Text = col_Bs.ToString();
            this.time.Text = time.ToString();
        }
        public void set_rezult(float o1, float o2, float o3,float o4,float po, float p1, float p2, float p3, float p4)
        {
            this.otpVsego.Text = Math.Round(o1, 3).ToString();
            this.otpCp.Text = Math.Round(o2, 3).ToString();
            this.otpBS.Text = Math.Round(o3, 3).ToString();
            this.otp_iz_seti.Text = Math.Round(o4, 3).ToString();
            this.potAll.Text = Math.Round(p1, 3).ToString();
            this.potAllpr.Text = Math.Round(p1 / o1 * 100, 2).ToString();
            this.potL.Text = Math.Round(p2, 3).ToString();
            this.potLpr.Text = Math.Round(p2 / o1 * 100, 2).ToString();
            this.potNTR.Text = Math.Round(p3, 3).ToString();
            this.potNTRpr.Text = Math.Round(p3 / o1 * 100, 2).ToString();
            this.potXX.Text = Math.Round(p4, 3).ToString();
            this.potXXpr.Text = Math.Round(p4 / o1 * 100, 2).ToString();
            this.Po.Text = Math.Round(po, 3).ToString();
        }
        public void add_diagr_2(float pr, int xx)   // 0 - хх, 1- нагруз
        {
            this.vrem_diagr.Series[xx].Points.AddY(pr);
        }
        public void add_diagr_1(float pr, int xx,string name)   // 0 - хх, 1- нагруз
        {
            this.RL_diagr.Series[xx].Points.AddXY(name,pr);
        }
        private void button3_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            Graphics myGraphics = panel1.CreateGraphics();
            Size s = this.panel1.Size;
            memoryImage = new Bitmap(s.Width, s.Height, myGraphics);
            Bitmap MyB = new Bitmap(panel1.Width, panel1.Height, myGraphics);
            panel1.DrawToBitmap(MyB, new Rectangle(0, 0, panel1.Width, panel1.Height));

            PrintDoc TP = new PrintDoc(MyB, new PrinterSettings());
            TP.Print();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            OptimizForm op_form = new OptimizForm();
            op_form.MdiParent = this.MdiParent;
            op_form.Show();
        }
    }
    class PrintDoc : PrintDocument
    {
        private Bitmap _picture;

        public PrintDoc(Bitmap _Image, PrinterSettings _PrinterSettings)
        {
            _picture = _Image;//new Bitmap(_Image);
            this.PrinterSettings = _PrinterSettings;

            this.DefaultPageSettings.Landscape = false;
        }

        protected override void OnPrintPage(PrintPageEventArgs e)
        {
            e.Graphics.DrawImage(_picture, 0, 10);

            e.HasMorePages = false;
        }
    }
}
