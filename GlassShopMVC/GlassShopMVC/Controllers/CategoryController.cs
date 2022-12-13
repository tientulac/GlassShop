using GlassShopMVC.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace GlassShopMVC.Controllers
{
    public class CategoryController : Controller
    {
        private LinqDataContext db = new LinqDataContext(); 

        // GET: Category
        public ActionResult Index()
        {
            ViewBag.ListCategory = db.Categories.ToList();
            return View();
        }

        public ActionResult Save(Category req)
        {
            if (req.CategoryId > 0)
            {
                var cate = db.Categories.Where(x => x.CategoryId == req.CategoryId).FirstOrDefault();
                cate.CategoryCode = req.CategoryCode;
                cate.CategoryName = req.CategoryName;
                db.SubmitChanges();
                return Json(new { success = true });
            }
            db.Categories.InsertOnSubmit(req);
            db.SubmitChanges();
            return Json(new { success = true });
        }

        public ActionResult Delete(int id = 0)
        {
            var cate = db.Categories.Where(x => x.CategoryId == id).FirstOrDefault();
            db.Categories.DeleteOnSubmit(cate);
            db.SubmitChanges();
            return Json(new { success = true }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult FindById(int id = 0)
        {
            var cate = db.Categories.Where(x => x.CategoryId == id).FirstOrDefault();
            return Json(new { success = true, data = cate }, JsonRequestBehavior.AllowGet);
        }
    }
}