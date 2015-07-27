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
    
    public partial class PRODUCT
    {
        public PRODUCT()
        {
            this.PDT_IMAGE_PATH = new HashSet<PDT_IMAGE_PATH>();
            this.Value_Specification = new HashSet<Value_Specification>();
            this.ProductCarts = new HashSet<ProductCart>();
            this.ProductOrderDetails = new HashSet<ProductOrderDetail>();
        }
    
        public int Pid { get; set; }
        public string PDT_CODE { get; set; }
        public string PDT_Name { get; set; }
        public decimal PDT_Price { get; set; }
        public string PDT_Description { get; set; }
        public int Category_id { get; set; }
        public int Sub_Category_id { get; set; }
        public int Brand_id { get; set; }
        public int Add_user_Id { get; set; }
        public System.DateTime Add_Datetime { get; set; }
        public int Mod_user_id { get; set; }
        public System.DateTime Mod_Datetime { get; set; }
        public int offer_value_type_id { get; set; }
        public bool IsActive { get; set; }
        public decimal PDT_offer { get; set; }
        public Nullable<decimal> PDT_Discount_price { get; set; }
    
        public virtual ICollection<PDT_IMAGE_PATH> PDT_IMAGE_PATH { get; set; }
        public virtual ICollection<Value_Specification> Value_Specification { get; set; }
        public virtual ICollection<ProductCart> ProductCarts { get; set; }
        public virtual ICollection<ProductOrderDetail> ProductOrderDetails { get; set; }
    }
}
