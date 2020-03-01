using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebApplication.Models
{
    public class GrupeUKojimaJeLiderViewModel
    {
        public int GrupaID { get; set; }
        public string NazivGrupe { get; set; }
        public List<SviRadniciViewModel>  radnici { get; set; }
    }
}
