using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebApplication.Models
{
    public class RadnikCheckBoxViewModel
    {
        public int RadnikID { get; set; }
        public string ImeIPrezime { get; set; }
        public bool DaLiJeUTojGrupi { get; set; }
        public bool? Vodja { get; set; }
    }
}
