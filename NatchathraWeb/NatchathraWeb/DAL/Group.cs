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
    
    public partial class Group
    {
        public int GroupId { get; set; }
        public string GroupName { get; set; }
        public Nullable<bool> IsActive { get; set; }
        public Nullable<int> CreatedBy { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
        public Nullable<int> ModifieBy { get; set; }
        public Nullable<System.DateTime> ModifiedDate { get; set; }
    }
}
