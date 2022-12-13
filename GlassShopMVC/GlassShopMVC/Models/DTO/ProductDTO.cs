using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GlassShopMVC.Models.DTO
{
    public class ProductDTO
    {
        public int ProductId { get; set; }
        public string ProductCode { get; set; }
        public string ProductName { get; set; }
        public decimal UnitPrice { get; set; }
        public string Descrip { get; set; }
        public int Amount { get; set; }
        public string Material { get; set; }
        public int Gender { get; set; }
        public string Origin { get; set; }
        public string Thumbnail { get; set; }
        public int Status { get; set; }
        public int CategoryId { get; set; }
        public int BrandId { get; set; }
        public string GenderName { get; set; }
        public string CategoryName { get; set; }
        public string BrandName { get; set; }
        public string StatusName { get; set; }

    }
}