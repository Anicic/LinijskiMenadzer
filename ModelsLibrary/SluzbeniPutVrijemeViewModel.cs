using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebApplication.Models
{
    public class SluzbeniPutVrijemeViewModel
    {
        public List<string> ListaLabela { get; set; }
        public List<short> ListaMinutaNaSluzbenomPutu { get; set; }
        public List<string> ListaVremena { get; set; }
    }
}
