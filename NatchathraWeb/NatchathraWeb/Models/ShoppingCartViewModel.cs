using NatchathraWeb.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace NatchathraWeb.Models
{
    public class ShoppingCartViewModel
    {
        public List<ProductCart> CartItems { get; set; }
        public decimal CartTotal { get; set; }
    }
    public class ShoppingCartRemoveViewModel
    {
        public string Message { get; set; }
        public decimal CartTotal { get; set; }
        public int CartCount { get; set; }
        public int ItemCount { get; set; }
        public int DeleteId { get; set; }
    }
}