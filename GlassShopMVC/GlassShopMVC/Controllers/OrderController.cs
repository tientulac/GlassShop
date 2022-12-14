using GlassShopMVC.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace GlassShopMVC.Controllers
{
    public class OrderController : Controller
    {
        private LinqDataContext db = new LinqDataContext();

        // GET: Order
        public ActionResult Index()
        {
            var orders = (from a in db.Orders
                          select new Models.DTO.OrderDTO { 
                              OrderId = a.OrderId,
                              CreatedAt = a.CreatedAt.GetValueOrDefault(),
                              AccountId = a.AccountId.GetValueOrDefault(),
                              UserName = db.Accounts.Where(x => x.AccountId == a.AccountId).FirstOrDefault().UserName ?? "",
                          }).ToList();
            ViewBag.ListOrder = orders;
            return View();
        }

        public ActionResult Delete(int id = 0)
        {
            var order = db.Orders.Where(x => x.OrderId == id).FirstOrDefault();
            var orderItem = db.OrderProducts.Where(x => x.OrderId == order.OrderId);
            if (orderItem.Any())
            {
                db.OrderProducts.DeleteAllOnSubmit(orderItem);
                db.SubmitChanges();
            }
            db.Orders.DeleteOnSubmit(order);
            db.SubmitChanges();
            return Json(new { success = true }, JsonRequestBehavior.AllowGet);
        }
    }
}