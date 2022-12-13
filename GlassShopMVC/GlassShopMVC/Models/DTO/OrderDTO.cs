using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GlassShopMVC.Models.DTO
{
    public class OrderDTO
    {
        public int OrderId { get; set; }
        public int AccountId { get; set; }
        public DateTime CreatedAt { get; set; }
        public string UserName { get; set; }
        public decimal TotalPrice { get; set; }
    }
}