using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace WebApplication.Models
{
    public class SadrzajViewModel
    {
        public short PrimalacID { get; set; }
        public short TipID { get; set; }
       
        public string TextObavjestenja { get; set; }
        public Nullable<short> RadnikID { get; set; }
        public Nullable<System.DateTime> DatumOd { get; set; }
        public Nullable<System.DateTime> DatumDo { get; set; }
    }
}
