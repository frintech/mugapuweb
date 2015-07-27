﻿//------------------------------------------------------------------------------
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
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    using System.Data.Objects;
    using System.Data.Objects.DataClasses;
    using System.Linq;
    
    public partial class MughapuEntities : DbContext
    {
        public MughapuEntities()
            : base("name=MughapuEntities")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public DbSet<Brand> Brands { get; set; }
        public DbSet<KEY_Specifications> KEY_Specifications { get; set; }
        public DbSet<Menu> Menus { get; set; }
        public DbSet<menuTree> menuTrees { get; set; }
        public DbSet<offer> offers { get; set; }
        public DbSet<Offer_Mapping> Offer_Mapping { get; set; }
        public DbSet<Offer_type> Offer_type { get; set; }
        public DbSet<OFFER_VALUE_TYPE> OFFER_VALUE_TYPE { get; set; }
        public DbSet<PDT_IMAGE_PATH> PDT_IMAGE_PATH { get; set; }
        public DbSet<PRODUCT> PRODUCTs { get; set; }
        public DbSet<Spec_category> Spec_category { get; set; }
        public DbSet<Value_Specification> Value_Specification { get; set; }
        public DbSet<ProductCart> ProductCarts { get; set; }
        public DbSet<ProductOrder> ProductOrders { get; set; }
        public DbSet<ProductOrderDetail> ProductOrderDetails { get; set; }
        public DbSet<Coupon> Coupons { get; set; }
        public DbSet<SpecialOffer> SpecialOffers { get; set; }
        public DbSet<SpecialOfferProduct> SpecialOfferProducts { get; set; }
        public DbSet<TreeviewList> TreeviewLists { get; set; }
        public DbSet<Coupon_Code> Coupon_Code { get; set; }
        public DbSet<CouponProduct> CouponProducts { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<User_Info> User_Info { get; set; }
        public DbSet<Banner_Image_Dtl> Banner_Image_Dtl { get; set; }
        public DbSet<ProductOfferValue> ProductOfferValues { get; set; }
        public DbSet<ProductCouponValue> ProductCouponValues { get; set; }
    
        public virtual int GetTreeView()
        {
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("GetTreeView");
        }
    
        public virtual int Sp_GetOffer()
        {
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("Sp_GetOffer");
        }
    
        public virtual int Sp_GetCoupon(string cupnCode)
        {
            var cupnCodeParameter = cupnCode != null ?
                new ObjectParameter("cupnCode", cupnCode) :
                new ObjectParameter("cupnCode", typeof(string));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("Sp_GetCoupon", cupnCodeParameter);
        }
    }
}