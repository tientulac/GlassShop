using GlassShopMVC.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace GlassShopMVC.Controllers
{
    public class ProductImageController : Controller
    {
        private LinqDataContext db = new LinqDataContext();

        // GET: ProductImage
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Save(ProductImage req)
        {
            db.ProductImages.InsertOnSubmit(req);
            db.SubmitChanges();
            return Json(new { success = true });
        }

        public ActionResult Delete(int id = 0)
        {
            var prdImage = db.ProductImages.Where(x => x.ProductImageId == id).FirstOrDefault();
            db.ProductImages.DeleteOnSubmit(prdImage);
            db.SubmitChanges();
            return Json(new { success = true }, JsonRequestBehavior.AllowGet);
        }
    }
}