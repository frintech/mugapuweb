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
    
    public partial class Order
    {
        public Order()
        {
            this.OrderDetails = new HashSet<OrderDetail>();
        }
    
        public int OrderId { get; set; }
        public System.DateTime OrderDate { get; set; }
        public string Username { get; set; }
        public string Tracking_REF_ID { get; set; }
        public string Currency { get; set; }
        public string Description { get; set; }
        public string Name { get; set; }
        public decimal Total { get; set; }
    
        public virtual ICollection<OrderDetail> OrderDetails { get; set; }
    }
}