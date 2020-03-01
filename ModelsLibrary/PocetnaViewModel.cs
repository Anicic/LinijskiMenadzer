using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebApplication.Models
{
    public class PocetnaViewModel
    {
        public int RadnikID { get; set; }
        public string Ime { get; set; }
        public string Prezime { get; set; }
        public string DomenskoIme { get; set; }
        public string EmailAdresa { get; set; }
        public string Lozinka { get; set; }
        public Int32 SlikaID { get; set; }
        public Int16 SektorID { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public Int32 UserID { get; set; }
    }
}
