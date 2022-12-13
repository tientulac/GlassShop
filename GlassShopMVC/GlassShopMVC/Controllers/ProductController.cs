using GlassShopMVC.Models;
using GlassShopMVC.Models.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace GlassShopMVC.Controllers
{
    public class ProductController : Controller
    {
        private LinqDataContext db = new LinqDataContext();

        // GET: Product
        public ActionResult Index()
        {
            ViewBag.ListProduct = (from a in db.Products
                                   select new ProductDTO
                                   {
                                       ProductId = a.ProductId,
                                       ProductCode = a.ProductCode,
                                       ProductName = a.ProductName,
                                       UnitPrice = a.UnitPrice.GetValueOrDefault(),
                                       Descrip = a.Descrip,
                                       Amount = a.Amount.GetValueOrDefault(),
                                       Material = a.Material,
                                       Gender = a.Gender.GetValueOrDefault(),
                                       Origin = a.Origin,
                                       Thumbnail = a.Thumbnail,
                                       Status = a.Status.GetValueOrDefault(),
                                       CategoryId = a.CategoryId.GetValueOrDefault(),
                                       BrandId = a.BrandId.GetValueOrDefault(),
                                       GenderName = a.Gender == 1 ? "Male" : a.Gender == 2 ? "Female" : a.Gender == 3 ? "Both" : "",
                                       StatusName = a.Status == 1 ? "New" : "Old",
                                       CategoryName = db.Categories.Where(x => x.CategoryId == a.CategoryId).FirstOrDefault().CategoryName ?? "",
                                       BrandName = db.Brands.Where(x => x.BrandId == a.BrandId).FirstOrDefault().BrandName ?? "",
                                   }).ToList();
            ViewBag.ListBrand = db.Brands.ToList();
            ViewBag.ListCategory = db.Categories.ToList();
            return View();
        }


        public ActionResult Save(Product req)
        {
            if (req.ProductId > 0)
            {
                var prod = db.Products.Where(x => x.ProductId == req.ProductId).FirstOrDefault();
                prod.ProductCode = req.ProductCode;
                prod.ProductName = req.ProductName;
                prod.UnitPrice = req.UnitPrice;
                prod.Descrip = req.Descrip;
                prod.Amount = req.Amount;
                prod.Material = req.Material;
                prod.Gender = req.Gender;
                prod.Origin = req.Origin;
                prod.Thumbnail = req.Thumbnail;
                prod.Status = req.Status;
                prod.CategoryId = req.CategoryId;
                prod.BrandId = req.BrandId;
                db.SubmitChanges();
                return Json(new { success = true });
            }
            req.Status = 1;
            db.Products.InsertOnSubmit(req);
            db.SubmitChanges();
            return Json(new { success = true });
        }

        public ActionResult Delete(int id = 0)
        {
            var prod = db.Products.Where(x => x.ProductId == id).FirstOrDefault();
            db.Products.DeleteOnSubmit(prod);
            db.SubmitChanges();
            return Json(new { success = true }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult DeleteImage(int id = 0)
        {
            var prod = db.ProductImages.Where(x => x.ProductImageId == id).FirstOrDefault();
            db.ProductImages.DeleteOnSubmit(prod);
            db.SubmitChanges();
            return Json(new { success = true }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetListImage(int id = 0)
        {
            var lstImage = (from a in db.ProductImages
                            select new ProductImageDTO {
                                ProductImageId = a.ProductImageId,
                                ProductId = a.ProductId.GetValueOrDefault(),
                                Image = a.Image
                            }).ToList();
            return Json(new { success = true, data = lstImage }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult DeleteColor(int id = 0)
        {
            var color = db.ProductColors.Where(x => x.ProductColorId == id).FirstOrDefault();
            db.ProductColors.DeleteOnSubmit(color);
            db.SubmitChanges();
            return Json(new { success = true }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetListColor(int id = 0)
        {
            var lstImage = (from a in db.ProductColors
                            select new ProductColorDTO
                            {
                                ProductColorId = a.ProductColorId,
                                ProductId = a.ProductId.GetValueOrDefault(),
                                Color = a.Color
                            }).ToList();
            return Json(new { success = true, data = lstImage }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult FindById(int id = 0)
        {
            var prod = (from a in db.Products.Where(x => x.ProductId == id)
                        select new ProductDTO {
                            ProductId = a.ProductId,
                            ProductCode = a.ProductCode,
                            ProductName = a.ProductName,
                            UnitPrice = a.UnitPrice.GetValueOrDefault(),
                            Descrip = a.Descrip,
                            Amount = a.Amount.GetValueOrDefault(),
                            Material = a.Material,
                            Gender = a.Gender.GetValueOrDefault(),
                            Origin = a.Origin,
                            Thumbnail = a.Thumbnail,
                            Status = a.Status.GetValueOrDefault(),
                            CategoryId = a.CategoryId.GetValueOrDefault(),
                            BrandId = a.BrandId.GetValueOrDefault(),
                            GenderName = a.Gender == 1 ? "Male" : a.Gender == 2 ? "Female" : a.Gender == 3 ? "Both" : "",
                            StatusName = a.Status == 1 ? "New" : "Old",
                            CategoryName = db.Categories.Where(x => x.CategoryId == a.CategoryId).FirstOrDefault().CategoryName ?? "",
                            BrandName = db.Brands.Where(x => x.BrandId == a.BrandId).FirstOrDefault().BrandName ?? "",
                        }).FirstOrDefault();
            return Json(new { success = true, data = prod }, JsonRequestBehavior.AllowGet);
        }
    }
}