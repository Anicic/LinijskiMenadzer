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
    
    public partial class VrijemeZaposlenja
    {
        public short VrijemeZaposlenjaID { get; set; }
        public short RadnikID { get; set; }
        public System.DateTime DatumZaposlenja { get; set; }
        public Nullable<System.DateTime> DatumPrestankaRada { get; set; }
    
        public virtual Radnik Radnik { get; set; }
    }
}
