using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace STREAMS
{
    [Serializable]
    public class Sheme
    {
        public string path;
        public List<UZ> UZ;
        public List<Vet> Vet;
        public Sheme(List<UZ> uz, List<Vet> vet)
        {
            this.UZ= uz;
            this.Vet = vet;
        }
    }
    public class BaseShema
    {
        public static List<UZ> UZ;
        public static List<Vet> Vet;
        public static string path;
    }
}
