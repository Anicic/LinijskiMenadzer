using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebApplication.Models
{
    public class AktivnostViewModel
    {
        public Int16 IzlazID { get; set; }
        public TimeSpan? Vrijeme { get; set; }
        public string IzlazNaziv { get; set; }
        public bool Odlazak { get; set; }
    }
}
