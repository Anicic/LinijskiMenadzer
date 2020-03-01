using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebApplication.Models
{
    public class ZahtjeviViewModel
    {
        public short ObavjestenjeID { get; set; }
        public string ObavjestenjeNaziv { get; set; }
        public short PosiljalacID { get; set; }
        public string PosiljalacIme { get; set; }
        public short PrimalacID { get; set; }
        public string PrimalacIme { get; set; }
        public SadrzajViewModel Sadrzaj { get; set; }
        public short TipObavjestenjaID { get; set; }
        public Nullable<bool> Odobreno { get; set; }
        public Nullable<bool> Pregledano { get; set; }
        public DateTime DatumObavjestenja { get; set; }
        public string DatumObavjestenjaString { get; set; }
        public Nullable<DateTime> DatumOdKad { get; set; }
        public Nullable<DateTime> DatumDoKad { get; set; }
        
    }
}
