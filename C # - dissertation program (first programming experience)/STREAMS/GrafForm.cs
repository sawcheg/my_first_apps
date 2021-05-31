using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;

namespace STREAMS
{
    public partial class GrafForm : Form
    {
        List<Graf> s = new List<Graf>();                                                  // временный список для коректировки графиков
        UZ GrCP;
        float cashFloat;
        private bool razv = false;                                                        // развернута ли форма
        private int number;                                                               // номер узла
        bool doDraw = false;                                                              // зажата мышь над графиком или нет
        float[,] param = new float[3, 2];                                                 // 3 - три графика , 2-   шаг ,нулевая линия
        public GrafForm()
        {
            InitializeComponent();
        }
        public bool GetGrafInfo(UZ cp, int number)
        {
            try
            {
                float prop;
                this.number = number;
                GrCP = cp;
                if (GrCP.Pmax != 0)                // если поле Pmax будет не заполнено - график не нарисуется вовсе                          
                {
                    if (GrCP.graficP.Count == 0)
                        GrCP.graficP = poluchitGrafic(GrCP.Wp, GrCP.Pmax, GrCP.Pmin, Get_Standart_Gr(2));                 // если новый график
                    else
                        GrCP.graficP = poluchitGrafic(GrCP.Wp, GrCP.Pmax, GrCP.Pmin, Get_cashK_from_List(GrCP.graficP));  // если форма графика уже известна
                    Set_Param(RisovatGrafik(pictureP, GrCP.graficP), 1);                                                  // рисуем
                }
                if (GrCP.cosFi != 0)               // если не ввести косинус - график Q не нарисуется (всегда ли так можно делать??)
                {
                    if (GrCP.graficQ.Count == 0)        // формирование Q-графика по графику P используя косинус Фи
                    {
                        for (int i = 0; i < 24; i++)
                            GrCP.graficQ.Add(new Graf(i + 1, (GrCP.graficP[i].Yset * (float)Math.Tan(Math.Acos(GrCP.cosFi))), 0));
                        float[] k = Koeff_grafika(GrCP.graficQ);
                        for (int i = 0; i < 24; i++)
                            GrCP.graficQ[i].Kset = k[i];
                    }
                    else                                // формируем по уже заданному графику с учетом косинуса,Wp,Qmax,Qmin
                    {
                        if (GrCP.Qmax == 0)
                            GrCP.Qmax = GrCP.Pmax * (float)Math.Tan(Math.Acos(GrCP.cosFi));
                        if (GrCP.Qmin == 0)
                            GrCP.Qmin = GrCP.Pmin * (float)Math.Tan(Math.Acos(GrCP.cosFi));
                        if (((GrCP.Wp * (float)Math.Tan(Math.Acos(GrCP.cosFi)) - GrCP.Qmax) / 23 < GrCP.Qmin) || (GrCP.Wp * (float)Math.Tan(Math.Acos(GrCP.cosFi)) - GrCP.Qmin) / 23 > GrCP.Qmax)
                        {
                            GrCP.Qmin = GrCP.Pmin * (float)Math.Tan(Math.Acos(GrCP.cosFi));
                            GrCP.Qmax = GrCP.Pmax * (float)Math.Tan(Math.Acos(GrCP.cosFi));
                        }
                        GrCP.graficQ = poluchitGrafic(GrCP.Wp * (float)Math.Tan(Math.Acos(GrCP.cosFi)), GrCP.Qmax, GrCP.Qmin, Get_cashK_from_List(GrCP.graficQ));
                    }
                    Set_Param(RisovatGrafik(pictureQ, GrCP.graficQ), 2);
                    textWq.Text = Ras4et_W(GrCP.graficQ).ToString();
                    textQmax.Text = GrCP.Qmax.ToString();
                    textQmin.Text = GrCP.Qmin.ToString();
                }
                float sumkvP = 0, Ui;
                if (GrCP.napr != 0)
                {
                    if (GrCP.graficU.Count == 0)
                    {
                        if ((GrCP.Wp != 0) && (GrCP.Pmax != 0))
                        {
                            for (int i = 0; i < 24; i++)
                                sumkvP += GrCP.graficP[i].Yset;
                            Ui = GrCP.napr * GrCP.napr * GrCP.Wp * GrCP.Wp / 24 / sumkvP;
                        }
                        else
                            for (int i = 1; i <= 24; i++)
                                GrCP.graficU.Add(new Graf(i, GrCP.napr, 0));
                    }
                    else                    // формирование с использованием уже существующего графика
                    {
                        prop = GrCP.napr / Ras4et_Usre(GrCP.graficU, GrCP.graficP);
                        for (int i = 0; i < 24; i++)
                            GrCP.graficU[i].Yset = GrCP.graficU[i].Yset * prop;
                    }
                    Set_Param(RisovatGrafik(pictureU, GrCP.graficU), 0);
                }
                else if (GrCP.tipUz == 0)
                {
                    GrCP.napr = GrCP.nomN;
                    if ((GrCP.Wp != 0) && (GrCP.Pmax != 0))
                    {
                        for (int i = 0; i < 24; i++)
                            sumkvP += GrCP.graficP[i].Yset;
                        Ui = GrCP.napr * GrCP.napr * GrCP.Wp * GrCP.Wp / 24 / sumkvP;
                    }
                    else
                        for (int i = 1; i <= 24; i++)
                            GrCP.graficU.Add(new Graf(i, GrCP.napr, 0));
                    Set_Param(RisovatGrafik(pictureU, GrCP.graficU), 0);
                }
                if (GrCP.graficP.Count != 0)
                {
                    if (GrCP.graficU.Count != 0)
                    {
                        GrCP.napr = Ras4et_Usre(GrCP.graficU, GrCP.graficP);
                        textUsr.Text = GrCP.napr.ToString();
                    }
                    textWp.Text = Ras4et_W(GrCP.graficP).ToString();
                    textPmax.Text = GrCP.Pmax.ToString();
                    textPmin.Text = GrCP.Pmin.ToString();
                    textCosFi.Text = GrCP.cosFi.ToString();
                }
                else
                {
                    if (GrCP.graficU.Count != 0)
                    {
                        GrCP.napr = Ras4et_Usre(GrCP.graficU, GrCP.graficP);
                        textUsr.Text = GrCP.napr.ToString();
                    }
                    if (GrCP.Wp != 0)
                        textWp.Text = GrCP.Wp.ToString();
                    else
                        textWp.Text = "";
                    textPmax.Text = "";
                    textPmin.Text = "";
                    textCosFi.Text = "";
                }
                return true;
            }
            catch
            {
                return false;
            }
        }                                     // передача в  эту форму Узла
        private List<Graf> poluchitGrafic(float w, float max, float min, float[] koef)
        {
            bool ending=false;
            int er,minGr=0,maxGr=0;
            float kzap_bez_min;
            float razn, sum, sumkv, shag;
            if (max != min)
            {
                kzap_bez_min = ((float)w / 24 - min) / (max - min);
                for (int i = 0; i < 24; i++)
                {
                    if (koef[i] == 1)
                        maxGr++;
                    else if (koef[i] == -1)
                        minGr++;
                }
                if ((kzap_bez_min < (1 - (float)minGr / 24)) && (kzap_bez_min > ((float)maxGr / 24)))
                {
                    if (w != 0)
                        while (!ending)
                        {
                            er = 0;
                            sum = 0; sumkv = 0;
                            for (int i = 0; i < 24; i++)
                            {
                                sum += koef[i];
                                sumkv += 1 - (koef[i] * koef[i]);
                            }
                            razn = (sum) - 24 * (2 * kzap_bez_min - 1);
                            shag = razn / sumkv;
                            for (int i = 0; i < 24; i++)
                            {
                                koef[i] -= (1 - koef[i] * koef[i]) * shag;
                                if (koef[i] > 1)
                                {
                                    koef[i] = 1;
                                    er++;
                                }
                                else if (koef[i] < -1)
                                {
                                    koef[i] = -1;
                                    er++;
                                }
                            }
                            if (er == 0)
                                ending = true;
                        }
                    List<Graf> list = new List<Graf>();
                    sum = 0;
                    for (int i = 0; i < 24; i++)
                    {
                        list.Add(new Graf(i + 1, min + (max - min) * ((koef[i] + 1) / 2), koef[i]));
                    }
                    return list;
                }
                else
                {
                    return new List<Graf>();
                }
            }
            else
            {
                List<Graf> list = new List<Graf>();
                for (int i = 0; i < 24; i++)
                {
                    list.Add(new Graf(i + 1, max, 1));
                }
                return list;
            }
        } // моделирование графика по W,P,типу  
        private void SetGraf_Click(object sender, EventArgs e)
        {
            if (!razv)
            {
                
                razv = true;
                SetGrafTable.Text = "<< Скрыть";
                dataU.Rows.Clear();
                dataP.Rows.Clear();
                dataQ.Rows.Clear();
                if(GrCP.graficU.Count!=0)
                for (int i = 0; i < 24; i++)
                     dataU.Rows.Add(GrCP.graficU[i].Xset, Math.Round(GrCP.graficU[i].Yset,2));
                if (GrCP.graficP.Count != 0)
                    for (int i = 0; i < 24; i++)
                        dataP.Rows.Add(GrCP.graficP[i].Xset, Math.Round(GrCP.graficP[i].Yset, 2));
                if (GrCP.graficQ.Count != 0)
                    for (int i = 0; i < 24; i++)
                       dataQ.Rows.Add(GrCP.graficQ[i].Xset, Math.Round(GrCP.graficQ[i].Yset, 2));
                this.Width = 453;
            }
            else
            {
                this.Width = 310;
                razv = false;
                SetGrafTable.Text = "Подробнее >>";
            }
        }                         // разворот параметров графиков
        private float[] RisovatGrafik(PictureBox picture, List<Graf> list)  
        {
            picture.Enabled = true;
            float naib;
            int osXcount = list.Count;
            float maxY = list[0].Yset;
            int osYcount = 0;
            float minY = list[0].Yset;
            foreach (Graf a in list)
            {
                if (maxY < a.Yset)
                    maxY = a.Yset;
                if(minY > a.Yset)
                    minY = a.Yset;
            }
            if (maxY<0&&minY<0)
               naib = -minY;       // поле по У
            else if (minY>0&&maxY>0)
                naib=maxY;
            else
                naib = (maxY- minY);
            int por;
            double shag;                // интервалы с шагом
            if (naib > 1)
            {
                por = (int)Math.Ceiling(Math.Log(naib) / Math.Log(10))-1;
                shag = (Math.Ceiling(naib / Math.Pow(10, por))) * Math.Pow(10, por) / 10;
            }
            else if (naib == 0)
            {
                por = 1;
                shag = 1;
            }
            else
            {
                por = -((int)Math.Ceiling(Math.Log(1 / naib) / Math.Log(10)));
                shag = (Math.Ceiling(naib / Math.Pow(10, por))) * Math.Pow(10, por) / 10;
            }
            if (naib != 0)
                osYcount = (int)Math.Ceiling(naib / shag);
            else
                osYcount = 10;
            //Очистка рисунка
            Bitmap bitmap = new Bitmap(picture.Width, picture.Height);
            Graphics g = Graphics.FromImage(bitmap);
            g.Clear(Color.Black);
            Pen myPen = new Pen(Color.Yellow);
            Pen myPen1 = new Pen(Color.Aqua);
            myPen1.DashStyle = System.Drawing.Drawing2D.DashStyle.Dot;
            myPen1.DashPattern = new float[]{1.0F, 6.9F};
                        
            //рисуем оси
            int osX = 0;
            int nullLine = 0;
            g.DrawLine(myPen, 20, 1, 20, 108); //y
            g.DrawLine(myPen, 20, 1, 17, 5);
            g.DrawLine(myPen, 20, 1, 23, 5);
            if (maxY < 0)
            {
                g.DrawLine(myPen, 15, 15, 208, 15); //x сверху
                g.DrawLine(myPen, 208, 15, 203, 12);
                g.DrawLine(myPen, 208, 15, 203, 18);
                osX = -1;
                nullLine = 15;
            }
            else if (minY > 0)
            {
                g.DrawLine(myPen, 15, 100, 208, 100); //x снизу
                g.DrawLine(myPen, 208, 100, 203, 97);
                g.DrawLine(myPen, 208, 100, 203, 103);
                osX = 1;
                nullLine = 100;
            }
            else
            {
                nullLine = 10 + (int)Math.Ceiling(maxY / shag) * 100 / (osYcount+1);
                g.DrawLine(myPen, 15, nullLine, 208, nullLine); //x снизу
                g.DrawLine(myPen, 208, nullLine, 203, nullLine-3);
                g.DrawLine(myPen, 208, nullLine, 203, nullLine+3);
            }
            //делим оси на интервалы и подписываем их
                    // для оси Y
            Font font = new Font ("Arial",5);
            SolidBrush drawBrush = new SolidBrush(Color.LimeGreen);
            StringFormat drawFormat = new StringFormat();
            drawFormat.Alignment = StringAlignment.Far;
            if (osX == 1)
            {
                for (int i = 1; i <= osYcount; i++)
                {
                    int k = 100 - i * 90 / (osYcount);
                    g.DrawLine(myPen, 17, k, 23, k);
                    g.DrawString((shag * i).ToString(), font, drawBrush, new PointF(15, k - 5), drawFormat);
                    g.DrawLine(myPen1, 27.917f, k, 210, k);
                }
            }
            else if (osX == -1)
            {
                for (int i = 1; i <= osYcount; i++)
                {
                    int k = 15 + i * 90 / (osYcount);
                    g.DrawLine(myPen, 17, k, 23, k);
                    g.DrawString((-shag * i).ToString(), font, drawBrush, new PointF(15, k - 5), drawFormat);
                    g.DrawLine(myPen1, 27.9f, k, 210, k);
                }
            }
            else
            {
                for (int i = 1; i < (int)Math.Ceiling(maxY / shag); i++)
                {
                    int k = nullLine - i * 90 / (osYcount);
                    g.DrawLine(myPen, 17, k, 23, k);
                    g.DrawString((shag * i).ToString(), font, drawBrush, new PointF(15, k - 5), drawFormat);
                    g.DrawLine(myPen1, 27.9f, k, 210, k);
                }
                for (int i = 1; i < (int)Math.Ceiling(-minY / shag); i++)
                {
                    int k = nullLine + i * 90 / (osYcount);
                    g.DrawLine(myPen, 17, k, 23, k);
                    g.DrawString((-shag * i).ToString(), font, drawBrush, new PointF(15, k - 5), drawFormat);
                    g.DrawLine(myPen1, 27.9f, k, 210, k);
                }
            }
            g.DrawString("0", font, drawBrush, new PointF(14, nullLine - 5), drawFormat);
            // ось X
            if (osX == 1)
            {
                for (int i = 1; i < osXcount; i++)
                {
                    int k = 20 + i * 190 / (osXcount);
                    g.DrawLine(myPen, k, nullLine+3, k, nullLine-3);
                    //g.DrawString((list[i].Xset).ToString(), font, drawBrush, new PointF(k-3,nullLine+4));
                    //g.DrawLine(myPen1, k, nullLine-3, k, 0); 
                }
                g.DrawString((list[23].Xset).ToString(), font, drawBrush, new PointF(200, nullLine + 4));
            }
            else if (osX == 0)
            {
                for (int i = 1; i < osXcount; i++)
                {
                    int k = 20 + i * 190 / (osXcount);
                    g.DrawLine(myPen, k, nullLine + 3, k, nullLine - 3);
                    //g.DrawString((list[i].Xset).ToString(), font, drawBrush, new PointF(k - 3, nullLine + 4));
                    //g.DrawLine(myPen1, k, nullLine - 3, k, 0);
                    //g.DrawLine(myPen1, k, nullLine + 15, k, 110);
                }
                g.DrawString((list[23].Xset).ToString(), font, drawBrush, new PointF(200, nullLine + 4));
            }
            else
            {
                for (int i = 1; i < osXcount; i++)
                {
                    int k = 20 + i * 190 / (osXcount);
                    g.DrawLine(myPen, k, nullLine + 3, k, nullLine - 3);
                    //g.DrawString((list[i].Xset).ToString(), font, drawBrush, new PointF(k - 3, 1));
                    //g.DrawLine(myPen1, k, nullLine + 4, k, 110);
                }
                g.DrawString((list[23].Xset).ToString(), font, drawBrush, new PointF(200, 1));
            }
            // сам график
            double mashY = 90.0f / (float)(osYcount)/(float)shag;
            double mashX = 190.0f / (float)(osXcount);
            int num=0;
            int y1;
            Point[] points =  new Point[list.Count] ;
            foreach (Graf gr in list)
            {
                y1 = (int)Math.Round(gr.Yset * mashY);
                points[num].Y = nullLine - y1;
                points[num].X = 20 + (int)Math.Round(mashX * num);
                num++;
            }
            g.DrawCurve(new Pen(Color.Lime,2), points);
            picture.Image = bitmap;
            g.Dispose();
            float[] par = {(float)mashY,(float)nullLine};
            return par;
            
        }           // рисовать график (рисунок, график)
        private float[] Koeff_grafika(List<Graf> grafic)
        {
            float[] koef = new float[24];
            float min, max, mashtab;
            min = grafic[0].Yset;
            max = grafic[0].Yset;
            for (int i = 1; i < 24; i++)
            {
                if (min > grafic[i].Yset)
                    min = grafic[i].Yset;
                if (max < grafic[i].Yset)
                    max = grafic[i].Yset;
            }
            mashtab = (max - min) / 2;
            for (int i = 0; i < 24; i++)
                koef[i] = (grafic[i].Yset - min) / mashtab - 1;
            return koef;
        }        // посчитать коэффициенты из графика 
        public UZ GetCP()
        {
            return GrCP;
        }                                       // для возврата узла в ЦПформу
        public int GetN()
        {
            return number;
        }                                       // для возврата номера узла в ЦПформу
        private float[] Get_Standart_Gr(int tip)
        {
            float[] koef = new float[24];
            switch (tip)
            {
                case 0: // постоянный
                    koef[0] = 1.0f; koef[1] = 1.0f;
                    koef[2] = 1.0f; koef[3] = 1.0f;
                    koef[4] = 1.0f; koef[5] = 1.0f;
                    koef[6] = 1.0f; koef[7] = 1.0f;
                    koef[8] = 1.0f; koef[9] = 1.0f;
                    koef[10] = 1.0f; koef[11] = 1.0f;
                    koef[12] = 1.0f; koef[13] = 1.0f;
                    koef[14] = 1.0f; koef[15] = 1.0f;
                    koef[16] = 1.0f; koef[17] = 1.0f;
                    koef[18] = 1.0f; koef[19] = 1.0f;
                    koef[20] = 1.0f; koef[21] = 1.0f;
                    koef[22] = 1.0f; koef[23] = 1.0f;
                    break;
                case 1: // в одну смену
                    koef[0] = -0.4f; koef[1] = -0.7f;
                    koef[2] = -0.9f; koef[3] = -1.0f;
                    koef[4] = -0.8f; koef[5] = -0.4f;
                    koef[6] = 0.0f; koef[7] = 0.4f;
                    koef[8] = 0.8f; koef[9] = 1.0f;
                    koef[10] = 0.9f; koef[11] = 0.8f;
                    koef[12] = 0.5f; koef[13] = 0.3f;
                    koef[14] = 0.1f; koef[15] = 0.0f;
                    koef[16] = 0.5f; koef[17] = 0.8f;
                    koef[18] = 1.0f; koef[19] = 0.8f;
                    koef[20] = 0.3f; koef[21] = 0.0f;
                    koef[22] = -0.1f; koef[23] = -0.2f;
                    break;
                case 2:     // в две смены
                    koef[0] = -1.0f; koef[1] = -1.0f;
                    koef[2] = -1.0f; koef[3] = -1.0f;
                    koef[4] = -0.85f; koef[5] = -0.69f;
                    koef[6] = -0.54f; koef[7] = -0.29f;
                    koef[8] = -0.08f; koef[9] = 0.23f;
                    koef[10] = 0.38f; koef[11] = 0.23f;
                    koef[12] = -0.23f; koef[13] = -0.23f;
                    koef[14] = 0.38f; koef[15] = 0.85f;
                    koef[16] = 1.0f; koef[17] = 0.85f;
                    koef[18] = 0.38f; koef[19] = -0.23f;
                    koef[20] = -0.38f; koef[21] = -0.54f;
                    koef[22] = -0.85f; koef[23] = -1.0f;
                    break;
                case 3:    // в три смены
                    koef[0] = -0.28f; koef[1] = -0.56f;
                    koef[2] = -0.72f; koef[3] = -1.0f;
                    koef[4] = -0.89f; koef[5] = -1.0f;
                    koef[6] = -0.75f; koef[7] = -0.56f;
                    koef[8] = 0.44f; koef[9] = 1.0f;
                    koef[10] = 0.72f; koef[11] = 0.0f;
                    koef[12] = 0.44f; koef[13] = 0.72f;
                    koef[14] = 0.86f; koef[15] = -0.11f;
                    koef[16] = 0.0f; koef[17] = 0.11f;
                    koef[18] = -0.06f; koef[19] = -0.39f;
                    koef[20] = -0.28f; koef[21] = 0.56f;
                    koef[22] = 0.39f; koef[23] = 0.11f;
                    break;
                case 4: // бытовой
                    koef[0] = -0.58f; koef[1] = -0.58f;
                    koef[2] = -0.58f; koef[3] = -0.58f;
                    koef[4] = -0.58f; koef[5] = -0.37f;
                    koef[6] = -0.16f; koef[7] = -1.0f;
                    koef[8] = -0.26f; koef[9] = -0.47f;
                    koef[10] = -0.47f; koef[11] = -0.37f;
                    koef[12] = -0.26f; koef[13] = -0.47f;
                    koef[14] = -0.47f; koef[15] = -0.47f;
                    koef[16] = -0.47f; koef[17] = -0.26f;
                    koef[18] = -0.05f; koef[19] = 0.37f;
                    koef[20] = 1.0f; koef[21] = -0.68f;
                    koef[22] = -0.05f; koef[23] = -0.47f;
                    break;
            }
            return koef;
        }                // получить массив коэффициентов стандартных графиков
        private float[] Get_cashK_from_List(List<Graf> gr)
        {
            float[] cashK= new float[24];
            for (int i = 0; i < 24; i++)
                cashK[i] = gr[i].Kset;
            return cashK;
        }      // забрать коэффициенты графика из списка
        private float Ras4et_Usre(List<Graf> listU, List<Graf> listP)
        {
            float sum=0;
            if (listP.Count != 0)
            {
                for (int i = 0; i < 24; i++)
                    sum += listP[i].Yset * listP[i].Yset * listU[i].Yset * listU[i].Yset;
                return (float)Math.Round(Math.Sqrt(sum / GrCP.Wp / GrCP.Wp * 24),3);
            }
            else
            {
                for (int i = 0; i < 24; i++)
                    sum += listU[i].Yset * listU[i].Yset/24;
                return (float)Math.Round(Math.Sqrt(sum),3);
            }
        }      // пересчет Usr используя графики напряжения и а.мощности(если он есть)
        private float Ras4et_W(List<Graf> list)
        {
            float sum=0;
            for (int i = 0; i < 24; i++)
                sum += list[i].Yset;
            return sum;                
        }                            // пересчет W используя список
        private float Poisk_pmax_pmin(List<Graf> list,bool max_min)
        {
            float min, max;
            min = list[0].Yset;
            max = list[0].Yset;
            for (int i = 1; i < 24; i++)
            {
                if (min > list[i].Yset)
                    min = list[i].Yset;
                if (max < list[i].Yset)
                    max = list[i].Yset;
            }
            if (max_min)
                return max;
            else
                return min;
        }        // получить максимум(true) или минимум (false) графика из списка
        private void Set_Param(float[] p, int n)
        {
            for (int i = 0; i < 2; i++)
                param[n, i] = p[i];
        }                           // запомнить параметры масштаба и нулевой оси X (после отрисовки графика)
        private void Cancel_Click(object sender, EventArgs e)
        {
            this.Dispose();
        }                 // не годится
        private void pictureU_MouseDown(object sender, MouseEventArgs e)
        {
            if (pictureU.Enabled == true)
                doDraw = true;   
        }   //нажатие мышкой на рисунок U
        private void pictureP_MouseDown(object sender, MouseEventArgs e)
        {
            if (pictureP.Enabled == true)
                doDraw = true;
        }   //нажатие мышкой на рисунок P
        private void pictureQ_MouseDown(object sender, MouseEventArgs e)
        {
            if (pictureQ.Enabled == true)
                doDraw = true;
        }   //нажатие мышкой на рисунок Q
        private void pictureU_MouseMove(object sender, MouseEventArgs e)
        {
            if (doDraw)
                picture_Draw(pictureU, e.X, e.Y, 0);                
        }   //движение курсора мышки над рисунком U
        private void pictureP_MouseMove(object sender, MouseEventArgs e)
        {
            if (doDraw)
                picture_Draw(pictureP, e.X, e.Y, 0);
        }   //движение курсора мышки над рисунком P
        private void pictureQ_MouseMove(object sender, MouseEventArgs e)
        {
            if (doDraw)
                picture_Draw(pictureQ, e.X, e.Y, 0);
        }   //движение курсора мышки над рисунком Q
        private void pictureU_MouseUp(object sender, MouseEventArgs e)
        {
            if (pictureU.Enabled == true && doDraw)
            {
                GrCP.graficU = list_Edit(pictureU, GrCP.graficU, 0);
                Update_U();
                doDraw = false;
                s.Clear();
            }
        }     //действие при отжиме мышки над графиком U
        private void pictureP_MouseUp(object sender, MouseEventArgs e)
        {
            if (pictureP.Enabled == true && doDraw)
            {
                GrCP.graficP = list_Edit(pictureP, GrCP.graficP, 1);
                Update_P();
                doDraw = false;
                s.Clear();
            }
        }     //действие при отжиме мышки над графиком P
        private void pictureQ_MouseUp(object sender, MouseEventArgs e)
        {
            if (pictureQ.Enabled == true && doDraw)
            {
                GrCP.graficQ = list_Edit(pictureQ, GrCP.graficQ, 2);
                Update_Q();
                doDraw = false;
                s.Clear();
            }
        }     //действие при отжиме мышки над графиком Q
        private void picture_Draw(PictureBox pic, int x, int y,int tip)
        {
            if (y > 110)
                y = 110;
            else if (y < 0)
                y = 0;
            if (x >= 20 && x < 203)
                s.Add(new Graf(x, y, 0));           
            Graphics g = pic.CreateGraphics();
            Point[] points = new Point[s.Count];
            int num = 0;
            foreach (Graf gr in s)
            {
                points[num].Y =(int)gr.Yset;
                points[num].X =(int)gr.Xset;
                num++;
            }
            if(s.Count>1)
                g.DrawCurve(new Pen(Color.Aqua, 2), points);
            g.Dispose();            
        }    //отрисовка пользовательской линии на графике
        private List<Graf> list_Edit(PictureBox pic, List<Graf> l, int tip)
        {
            int z;
            for (int i = s.Count - 1; i >= 0; i--)
            {
                s[i].Xset = (int)Math.Round(((float)s[i].Xset - 19.5f) / 190 * 24) + 1;
                s[i].Yset = (param[tip, 1] - s[i].Yset) / param[tip, 0];
                
            }
            for (int i = s.Count - 1; i > 0; i--)
            {
                for (int j = i - 1; j >= 0; j--)
                    if (s[j].Xset == s[i].Xset)
                    {
                        s.RemoveAt(j);
                        i--;
                    }
            }
            for (int i = s.Count - 1; i >= 0; i--)
            {
                z=(int)s[i].Xset-1;
                l[z].Yset = s[i].Yset;
            }
            return l;
        }//округление точек до нужных ординат графика, возврат списка
        private void Update_P()
        {
            Set_Param(RisovatGrafik(pictureP, GrCP.graficP), 1);
            GrCP.Wp = Ras4et_W(GrCP.graficP);
            GrCP.Pmax = Poisk_pmax_pmin(GrCP.graficP, true);
            GrCP.Pmin = Poisk_pmax_pmin(GrCP.graficP, false);
            if (GrCP.graficQ.Count != 0)
                GrCP.cosFi = (float)Math.Round(Math.Cos(Math.Atan(Ras4et_W(GrCP.graficQ) / GrCP.Wp)), 3);
            textWp.Text = Math.Round(GrCP.Wp,2).ToString();
            textPmax.Text = Math.Round(GrCP.Pmax,2).ToString();
            textPmin.Text = Math.Round(GrCP.Pmin,2).ToString();
            textCosFi.Text = Math.Round(GrCP.cosFi,2).ToString();
            float[] k = Koeff_grafika(GrCP.graficP);
            for (int i = 0; i < 24; i++)
                GrCP.graficP[i].Kset = k[i];
        }                                            //обновить все после редактирования списка P
        private void Update_U()
        {
            Set_Param(RisovatGrafik(pictureU, GrCP.graficU), 0);
            GrCP.napr = Ras4et_Usre(GrCP.graficU, GrCP.graficP);
            textUsr.Text = Math.Round(GrCP.napr,3).ToString();
        }                                            //обновить все после редактирования списка U
        private void Update_Q()
        {
            Set_Param(RisovatGrafik(pictureQ, GrCP.graficQ), 2);            
            GrCP.cosFi = (float)Math.Round(Math.Cos(Math.Atan(Ras4et_W(GrCP.graficQ) / GrCP.Wp)), 3);
            textWq.Text = Ras4et_W(GrCP.graficQ).ToString();
            GrCP.Qmax = Poisk_pmax_pmin(GrCP.graficQ, true);
            GrCP.Qmin = Poisk_pmax_pmin(GrCP.graficQ, false);
            textCosFi.Text = Math.Round(GrCP.cosFi,3).ToString();
            textQmax.Text = Math.Round(GrCP.Qmax,2).ToString();
            textQmin.Text = Math.Round(GrCP.Qmin,2).ToString();
            float[] k = Koeff_grafika(GrCP.graficQ);
            for (int i = 0; i < 24; i++)
                GrCP.graficQ[i].Kset = k[i];
        }                                            //обновить все после редактирования списка Q
        private void dataU_CellBeginEdit(object sender, DataGridViewCellCancelEventArgs e)
        {
            cashFloat = Convert.ToSingle(dataU[e.ColumnIndex, e.RowIndex].Value);
        }// начало редактирования ячейки табл. U
        private void dataP_CellBeginEdit(object sender, DataGridViewCellCancelEventArgs e)
        {
            cashFloat = Convert.ToSingle(dataP[e.ColumnIndex, e.RowIndex].Value);
        }// начало редактирования ячейки табл. P
        private void dataQ_CellBeginEdit(object sender, DataGridViewCellCancelEventArgs e)
        {
            cashFloat = Convert.ToSingle(dataQ[e.ColumnIndex, e.RowIndex].Value);
        }// начало редактирования ячейки табл. Q
        private void dataU_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                GrCP.graficU[e.RowIndex].Yset = Convert.ToSingle(dataU[e.ColumnIndex, e.RowIndex].Value);
                Update_U();
            }
            catch
            {
                MessageBox.Show("Введен неверный формат!");
                dataU[e.ColumnIndex, e.RowIndex].Value = cashFloat;
            }
        }  //при завершении редактирования ячейки табл. U
        private void dataP_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                GrCP.graficP[e.RowIndex].Yset = Convert.ToSingle(dataP[e.ColumnIndex, e.RowIndex].Value);
                Update_P();
            }
            catch
            {
                MessageBox.Show("Введен неверный формат!");
                dataP[e.ColumnIndex, e.RowIndex].Value = cashFloat;
            }
        }  //при завершении редактирования ячейки табл. P
        private void dataQ_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                GrCP.graficQ[e.RowIndex].Yset = Convert.ToSingle(dataQ[e.ColumnIndex, e.RowIndex].Value);
                Update_Q();
            }
            catch
            {
                MessageBox.Show("Введен неверный формат!");
                dataQ[e.ColumnIndex, e.RowIndex].Value = cashFloat;
            }
        }  //при завершении редактирования ячейки табл. Q
        private void button1_Click(object sender, EventArgs e)
        {
            if (pictureP.Enabled == true)
            {
                for (int i = 0; i < 24; i++)
                    GrCP.graficQ[i].Yset = GrCP.graficP[i].Yset * (float)Math.Tan(Math.Acos(GrCP.cosFi));
                Update_Q();
            }
        }                      // копирование формы графика P -> Q используя косинус
        private void pTypeGr_SelectedIndexChanged(object sender, EventArgs e)           // выбор типового графика
        {
            int tip = pTypeGr.SelectedIndex;
            if (tip == 0)
            {
                GrCP.Pmax = GrCP.Wp / 24;
                GrCP.Pmin = GrCP.Pmax;
            }
            GrCP.graficP = poluchitGrafic(GrCP.Wp, GrCP.Pmax, GrCP.Pmin, Get_Standart_Gr(tip));
            Update_P();
            this.button1_Click(this,e);
        }
         
    }
}
