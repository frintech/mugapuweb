//------------------------------------------------------------------------------
// <auto-generated>
//    This code was generated from a template.
//
//    Manual changes to this file may cause unexpected behavior in your application.
//    Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace MughapuWeb.DAL
{
    using System;
    using System.Collections.Generic;
    
    public partial class SpecialOfferProduct
    {
        public int SeqNo { get; set; }
        public int SpecialOfferID { get; set; }
        public int ProductID { get; set; }
        public int Adduser { get; set; }
        public System.DateTime AddDatetime { get; set; }
        public int ModifiedUser { get; set; }
        public System.DateTime ModifiedDate { get; set; }
        public string PDT_TYPE { get; set; }
        public Nullable<decimal> DiscountPct { get; set; }
        public Nullable<int> OFR_VAL_TYPE { get; set; }
    }
}
