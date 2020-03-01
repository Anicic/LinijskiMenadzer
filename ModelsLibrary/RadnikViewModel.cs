using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebApplication.Models
{
    public class RadnikViewModel
    {
        public short RadnikID { get; set; }
        public string Ime { get; set; }
        public string Prezime { get; set; }
        public byte? SektorID { get; set; }
        public string DomenskoIme { get; set; }
        public string EmailAdresa { get; set; }
        public string Lozinka { get; set; }
        public string LinkSlike { get; set; }
        public string BrojTelefona { get; set; }

        public bool Lider { get; set; }
        public string PrekovremenoVrijemeProvedenoNaPoslu { get; set; }
        public int ProcenatProvedenogVremenaNaPoslu { get; set; }
        public int ProcenatPrekovremenogProvedenogVremenaNaPoslu { get; set; }
        public string VrijemeProvedenoNaPoslu { get; set; }
        public TimeSpan? DolazakNaPosao { get; set; }
        public TimeSpan? OdlazakSaPosla { get; set; }
        public TimeSpan? OtisaoSaPosla { get; set; }
        public int ProcenatPrekovremenogRada { get; set; }
    }
}
