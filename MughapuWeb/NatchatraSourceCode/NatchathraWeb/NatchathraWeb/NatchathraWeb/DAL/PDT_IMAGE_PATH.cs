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
    
    public partial class PDT_IMAGE_PATH
    {
        public int img_id { get; set; }
        public int PDT_ID { get; set; }
        public string Image_Path { get; set; }
        public bool Home_Flag { get; set; }
        public bool Banner_Flag { get; set; }
        public bool First_Flag { get; set; }
        public int User_id { get; set; }
        public System.DateTime His_Datetime { get; set; }
        public string Img_type { get; set; }
    
        public virtual PRODUCT PRODUCT { get; set; }
    }
}
