using GlassShopMVC.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace GlassShopMVC.Controllers
{
    public class BrandController : Controller
    {
        private LinqDataContext db = new LinqDataContext();

        // GET: Brand
        public ActionResult Index()
        {
            ViewBag.ListBrand = db.Brands.ToList();
            return View();
        }

        public ActionResult Save(Brand req)
        {
            if (req.BrandId > 0)
            {
                var brand = db.Brands.Where(x => x.BrandId == req.BrandId).FirstOrDefault();
                brand.BrandCode = req.BrandCode;
                brand.BrandName = req.BrandName;
                db.SubmitChanges();
                return Json(new { success = true });
            }
            db.Brands.InsertOnSubmit(req);
            db.SubmitChanges();
            return Json(new { success = true });
        }

        public ActionResult Delete(int id = 0)
        {
            var brand = db.Brands.Where(x => x.BrandId == id).FirstOrDefault();
            db.Brands.DeleteOnSubmit(brand);
            db.SubmitChanges();
            return Json(new { success = true }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult FindById(int id = 0)
        {
            var brand = db.Brands.Where(x => x.BrandId == id).FirstOrDefault();
            return Json(new { success = true, data = brand }, JsonRequestBehavior.AllowGet);
        }
    }
}