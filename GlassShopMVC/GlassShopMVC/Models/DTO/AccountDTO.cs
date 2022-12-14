using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GlassShopMVC.Models.DTO
{
    public class AccountDTO: Account
    {
        public string OldPassword { get; set; }
        public string NewPassword { get; set; }
    }
}