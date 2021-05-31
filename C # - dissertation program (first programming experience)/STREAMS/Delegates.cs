using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace STREAMS
{
    
    public static class DelegateList
    {
        public delegate void SaveChange();
        public static SaveChange Save;
        public delegate void UpdateVet(bool ch);
        public static UpdateVet Up_Vet;
        public delegate void UpdateUz(bool ch);
        public static UpdateUz Up_Uz;
        public delegate void PeredNameUz_Marka(int row, int col, string mar, float moshn);
        public static PeredNameUz_Marka EventFormUZ_End;
    }
}
