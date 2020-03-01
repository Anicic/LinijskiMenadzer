using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebApplication.Models
{
    public class ObavjestenjeViewModel
    {
        public short ObavjestenjeID { get; set; }
        public string ObavjestenjeNaziv { get; set; }
        public short PosiljalacID { get; set; }
        public string PosiljalacIme { get; set; }
        public short PrimalacID { get; set; }
        public string SadrzajObavjestenja { get; set; }
        public string PrimalacIme { get; set; }
        public DateTime DatumObavjestenja { get; set; }
        public string DatumObavjestenjaString { get; set; }
        public short SadrzajID { get; set; }
        public Nullable<bool> Odobreno { get; set; }
        public Nullable<bool> Pregledano { get; set; }
        public short TipObavjestenjaID { get; set; }
        public Nullable<DateTime> DatumOdKad { get; set; }
        public Nullable<DateTime> DatumDoKad { get; set; }
        public string OpisObavjestenja { get; set; }
        public string ImeIPrezimeRadnika { get; set; }
        public string Email { get; set; }
        public string BrojTelefona { get; set; }
    }
}
