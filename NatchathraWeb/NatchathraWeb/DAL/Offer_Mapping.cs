//------------------------------------------------------------------------------
// <auto-generated>
//    This code was generated from a template.
//
//    Manual changes to this file may cause unexpected behavior in your application.
//    Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace NatchathraWeb.DAL
{
    using System;
    using System.Collections.Generic;
    
    public partial class Offer_Mapping
    {
        public int OMP_id { get; set; }
        public string Type { get; set; }
        public int Mappid { get; set; }
        public int Offer_id { get; set; }
        public int offer_type_id { get; set; }
        public int Add_User_id { get; set; }
        public System.DateTime Add_Datetime { get; set; }
        public int Mod_User_Id { get; set; }
        public System.DateTime Modify_Datetime { get; set; }
    
        public virtual offer offer { get; set; }
        public virtual Offer_type Offer_type { get; set; }
    }
}
