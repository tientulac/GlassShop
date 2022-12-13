using GlassShopMVC.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace GlassShopMVC.Controllers
{
    public class DiscountController : Controller
    {
        private LinqDataContext db = new LinqDataContext();

        // GET: Discount
        public ActionResult Index()
        {
            ViewBag.ListDiscount = db.Discounts.ToList();
            return View();
        }

        public ActionResult Save(Discount req)
        {
            db.Discounts.InsertOnSubmit(req);
            db.SubmitChanges();
            return Json(new { success = true });
        }

        public ActionResult Delete(int id = 0)
        {
            var disc = db.Discounts.Where(x => x.DiscountId == id).FirstOrDefault();
            db.Discounts.DeleteOnSubmit(disc);
            db.SubmitChanges();
            return Json(new { success = true }, JsonRequestBehavior.AllowGet);
        }
    }
}