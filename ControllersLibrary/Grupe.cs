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
    
    public partial class Grupe
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Grupe()
        {
            this.GrupaRadniks = new HashSet<GrupaRadnik>();
        }
    
        public short GrupaID { get; set; }
        public string Naziv { get; set; }
        public Nullable<short> TipGrupeID { get; set; }
        public Nullable<System.DateTime> DatumOd { get; set; }
        public Nullable<System.DateTime> DatumDo { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<GrupaRadnik> GrupaRadniks { get; set; }
        public virtual TipGrupe TipGrupe { get; set; }
    }
}
