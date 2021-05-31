using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace STREAMS
{
    [Serializable]
    public class Vet
    {
       public int tipV;  //     тип ветви 0 - линейный участок, 1 - трансформаторный
       public int k1;    //     коммутационный аппарат в начале участка: отсутсвтвует(-1), выключен(0), включен (1)
       public int k2;    //     коммутационный аппарат в конце участка: отсутсвтвует(-1), выключен(0), включен (1)
       public string nach;
       public string kon;
       public string marka;
       public float dlina_moshnost;


       public Vet(int tipV,int k1, int k2, string nach, string kon, string marka, float dlina_moshnost)
       {
           this.tipV = tipV;
           this.k1 = k1;
           this.k2 = k2;
           this.nach = nach;
           this.kon = kon;
           this.marka = marka;
           this.dlina_moshnost = dlina_moshnost;
       }

    }
}
