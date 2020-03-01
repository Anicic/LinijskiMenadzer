using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebApplication.Models
{
   public class SviRadniciViewModel
    {
        public int RadnikID { get; set; }
        public string Ime { get; set; }
        public string Prezime { get; set; }
        public Nullable<int> StatusID { get; set; }
        public string NazivStatusa { get; set; }
        public short? UserID { get; set; }
        public string EmailAdresa { get; set; }
        public string BrojTelefona { get; set; }
    }
}
