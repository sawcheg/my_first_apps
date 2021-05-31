using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace STREAMS
{
    [Serializable]
    public class UZ
    {
        public int tipUz;               // 0- ЦП, 1-БС, 2- Нагрузка, 3-Промежуточный
        public string nameCP;        
        public float nomN;
        public float napr;
        public float cosFi;        
        public float Wp;       
        public float Pmax;
        public float Pmin;
        public List<Graf> graficU;
        public List<Graf> graficP;
        public List<Graf> graficQ;
        public float Qmax;
        public float Qmin;

        public UZ(int tipUz, string nameCP, float nomN, float napr, float cosFi, float Wp, float Pmax, float Pmin, List<Graf> graficU, List<Graf> graficP, List<Graf> graficQ,
            float Qmax,float Qmin)
        {
            this.tipUz = tipUz;
            this.nameCP = nameCP;
            this.nomN = nomN;
            this.napr = napr;
            this.cosFi = cosFi;
            this.Wp = Wp;
            this.Pmax = Pmax;
            this.Pmin = Pmin;
            this.graficU = graficU;
            this.graficQ = graficQ;
            this.graficP = graficP;
            this.Qmax = Qmax;
            this.Qmin = Qmin;

        }
    }
    [Serializable]
    public class Graf               
    {
        public float Xset;
        public float Yset;
        public float Kset;

        public Graf(float x, float y,float k)
        {
            this.Xset = x;
            this.Yset = y;
            this.Kset = k;
        }


    }
}
