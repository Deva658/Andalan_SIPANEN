using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UCP
{
    public class DataPanen
    {
        public int IdPanen { get; set; }
        public string NamaPetani { get; set; }
        public string NamaTanaman { get; set; }
        public DateTime TanggalPanen { get; set; }
        public double JumlahHasil { get; set; }
        public string SatuanHasil { get; set; }
        public string Kualitas { get; set; }
    }
}
