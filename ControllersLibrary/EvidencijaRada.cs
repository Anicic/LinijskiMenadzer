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
    
    public partial class EvidencijaRada
    {
        public short EvidencijaRadaID { get; set; }
        public byte TipID { get; set; }
        public short RadnikID { get; set; }
        public System.DateTime Pocetak { get; set; }
        public System.DateTime Kraj { get; set; }
    
        public virtual Tip Tip { get; set; }
        public virtual Radnik Radnik { get; set; }
    }
}
