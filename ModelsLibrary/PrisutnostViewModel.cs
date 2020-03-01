using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebApplication.Models
{
    public class PrisutnostViewModel
    {
        public int? RadnikID { get; set; }
        public short NaPoslu { get; set; }
        public short PrivatnoOdsutan { get; set; }
        public short VisednevnoOdsutan { get; set; }
        public short Nepoznato { get; set; }
        public short SluzbenoOdsutan { get; set; }
        public short PreostaloDana { get; set; }
    }
}
