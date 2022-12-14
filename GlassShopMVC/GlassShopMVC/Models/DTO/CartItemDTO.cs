using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GlassShopMVC.Models.DTO
{
    public class CartItemDTO : ProductColor
    {
        public int OrderId { get; set; }
        public int OrderProductId { get; set; }
        public double Total { get; set; }
        public int AccountId { get; set; }
        public int Quantity { get; set; }
        public string ProductName { get; set; }
        public string Thumbnail { get; set; }
        public decimal UnitPrice { get; set; }
    }
}