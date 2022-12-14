using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GlassShopMVC.Models.DTO
{
    public class OrderDTO: Order
    {
        public string UserName { get; set; }
        public decimal TotalPrice { get; set; }
        public string StatusName { get; set; }
    }
}