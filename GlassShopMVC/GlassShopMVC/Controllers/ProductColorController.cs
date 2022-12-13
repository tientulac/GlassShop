using GlassShopMVC.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace GlassShopMVC.Controllers
{
    public class ProductColorController : Controller
    {
        private LinqDataContext db = new LinqDataContext();

        // GET: ProductColor
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Save(ProductColor req)
        {
            db.ProductColors.InsertOnSubmit(req);
            db.SubmitChanges();
            return Json(new { success = true });
        }

        public ActionResult Delete(int id = 0)
        {
            var prod = db.ProductColors.Where(x => x.ProductColorId == id).FirstOrDefault();
            db.ProductColors.DeleteOnSubmit(prod);
            db.SubmitChanges();
            return Json(new { success = true }, JsonRequestBehavior.AllowGet);
        }
    }
}