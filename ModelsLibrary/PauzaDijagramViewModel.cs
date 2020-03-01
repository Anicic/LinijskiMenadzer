using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebApplication.Models
{
    public class PauzaDijagramViewModel
    {
        public short PauzeDo { get; set; }
        public short PauzePreko { get; set; }

        public List<string> ListaLabela { get; set; }
        public List<short> ListaMinutaNaPauzi { get; set; }
        public List<string> ListaVremena { get; set; }
    }
}
