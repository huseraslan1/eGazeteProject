//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace E_GazeteUyg.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class Gazete
    {
        public int Id { get; set; }
        public string Adi { get; set; }
        public System.DateTime Tarih { get; set; }
        public string Pdf { get; set; }
        public string Image { get; set; }
        public Nullable<int> IsimId { get; set; }
    
        public virtual GazeteEk GazeteEk { get; set; }
    }
}
