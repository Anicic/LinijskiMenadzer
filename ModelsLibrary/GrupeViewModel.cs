using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebApplication.Models
{
   public class GrupeViewModel
    {
        public Int16 GrupaID { get; set; }
        public string Naziv { get; set; }
        public Int16 TimLiderID { get; set; }

        public List<RadnikViewModel> radnici { get; set; }
    }
}
