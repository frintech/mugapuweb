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
    
    public partial class ProductCart
    {
        public int RecordId { get; set; }
        public string CartId { get; set; }
        public int PDT_Id { get; set; }
        public int Count { get; set; }
        public System.DateTime DateCreated { get; set; }
    
        public virtual PRODUCT PRODUCT { get; set; }
    }
}