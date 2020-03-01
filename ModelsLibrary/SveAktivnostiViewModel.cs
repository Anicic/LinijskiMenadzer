using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebApplication.Models
{
    public class SveAktivnostiViewModel
    {
        public TimeSpan? Pocetak { get; set; }
        public TimeSpan? Kraj { get; set; }
        public Int16 IzlazID { get; set; }
        public string IzlazNaziv { get; set; }
    }
}
