//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace WebApplication.Controllers
{
    using System;
    using System.Collections.Generic;
    
    public partial class Obavjestenje
    {
        public short ObavjestenjeID { get; set; }
        public short PosiljalacID { get; set; }
        public short PrimalacID { get; set; }
        public short SadrzajID { get; set; }
        public Nullable<bool> Odobreno { get; set; }
        public Nullable<bool> Pregledano { get; set; }
        public short TipObavjestenjaID { get; set; }
        public Nullable<System.DateTime> DatumObavjestenja { get; set; }
        public string Odgovor { get; set; }
        public Nullable<short> GrupaID { get; set; }
    
        public virtual Radnik Posiljalac { get; set; }
        public virtual Radnik Primalac { get; set; }
        public virtual SadrzajObavjestenja SadrzajObavjestenja { get; set; }
        public virtual TipObavjestenja TipObavjestenja { get; set; }
    }
}
