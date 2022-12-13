using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GlassShopMVC.Models.DTO
{
    public class ProductColorDTO
    {
        public int ProductColorId { get; set; }
        public int ProductId { get; set; }
        public string Color { get; set; }
    }
}