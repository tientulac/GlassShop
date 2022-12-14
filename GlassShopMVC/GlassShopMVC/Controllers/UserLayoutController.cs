using GlassShopMVC.Models;
using GlassShopMVC.Models.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace GlassShopMVC.Controllers
{
    public class UserLayoutController : Controller
    {
        private LinqDataContext db = new LinqDataContext();

        // GET: UserLayout
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
            ViewBag.ListColor = db.ProductColors.ToList();
            ViewBag.ListImage = db.ProductImages.ToList();
            return View();
        }

        [HttpPost]
        public ActionResult Register(Account req)
        {
            try
            {
                var _user = db.Accounts.Where(x => x.UserName == req.UserName);
                var _userEmail = db.Accounts.Where(x => x.Email == req.Email);
                if (_user.Any())
                {
                    return Json(new { success = false, data = "The user name was exist !" }, JsonRequestBehavior.AllowGet);
                }
                if (_userEmail.Any())
                {
                    return Json(new { success = false, data = "The email was exist !" }, JsonRequestBehavior.AllowGet);
                }
                var _userInsert = new Account();
                _userInsert.UserName = req.UserName;
                _userInsert.Email = req.Email;
                _userInsert.Password = req.Password;
                _userInsert.Admin = false;
                _userInsert.Active = true;
                _userInsert.FullName = req.FullName;

                db.Accounts.InsertOnSubmit(_userInsert);
                db.SubmitChanges();

                return Json(new { success = true, data = "Register successfully !" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { success = false, data = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public ActionResult ChangePass(AccountDTO req)
        {
            try
            {
                var _user = db.Accounts.Where(x => x.UserName == req.UserName && x.Email == req.Email && x.Password == req.OldPassword && x.Email == req.Email);
                var _userEmail = db.Accounts.Where(x => x.Email == req.Email);
                var _userPassword = db.Accounts.Where(x => x.Password == req.OldPassword);
                if (!_userEmail.Any())
                {
                    return Json(new { success = false, data = "The email does not exist !" }, JsonRequestBehavior.AllowGet);
                }

                if (!_userPassword.Any())
                {
                    return Json(new { success = false, data = "The password does not match !" }, JsonRequestBehavior.AllowGet);
                }

                if (_user.Any())
                {
                    _user.FirstOrDefault().Password = req.NewPassword;
                    db.SubmitChanges();
                    return Json(new { success = true, data = "Successfully !" }, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                return Json(new { success = false, data = ex.Message }, JsonRequestBehavior.AllowGet);
            }
            return Json(new { success = false, data = "ERROR !" }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult InsertCartItem(int accountId, int productColorId)
        {
            var cart = db.Orders.Where(x => x.AccountId == accountId && x.Status == 1);
            if (cart.Any())
            {
                var cartId = cart.FirstOrDefault().OrderId;
                var checkItem = db.OrderProducts.Where(x => x.OrderId == cartId && x.ProductColorId == productColorId);
                if (!checkItem.Any())
                {
                    var cartItem = new OrderProduct();
                    cartItem.ProductColorId = productColorId;
                    cartItem.OrderId = cartId;
                    cartItem.Quantity = 1;
                    db.OrderProducts.InsertOnSubmit(cartItem);
                    db.SubmitChanges();
                }
                else
                {
                    var item = checkItem.FirstOrDefault();
                    item.OrderId = cartId;
                    item.ProductColorId = productColorId;
                    item.Quantity++;
                    db.SubmitChanges();
                }
            }
            else
            {
                var cart_insert = new Order();
                var cartItem = new OrderProduct();
                cart_insert.Status = 1;
                cart_insert.CreatedAt = DateTime.Now;
                cart_insert.AccountId = accountId;
                db.Orders.InsertOnSubmit(cart_insert);
                db.SubmitChanges();
                cartItem.ProductColorId = productColorId;
                cartItem.OrderId = cart_insert.OrderId;
                cartItem.Quantity = 1;
                db.OrderProducts.InsertOnSubmit(cartItem);
                db.SubmitChanges();
            }
            return Json(new { success = true }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult DeleteCartItem(int orderProductId)
        {
            var _cartItem = db.OrderProducts.Where(M => M.OrderProductId == orderProductId).FirstOrDefault();
            db.OrderProducts.DeleteOnSubmit(_cartItem);
            db.SubmitChanges();
            return Json(new { success = true }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult UpdateCountCart(int orderProductId, int count)
        {
            var _cartItem = db.OrderProducts.Where(x => x.OrderProductId == orderProductId);
            if (_cartItem.Any())
            {
                _cartItem.FirstOrDefault().Quantity = count;
                db.SubmitChanges();
                return Json(new { success = true }, JsonRequestBehavior.AllowGet);
            }
            return Json(new { success = false, data = "" }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetCartItem(int accountId)
        {
            var cartByUser = db.Orders.Where(x => x.AccountId == accountId && x.Status == 1);
            if (cartByUser.Any())
            {
                var listCartItem = (from a in db.OrderProducts.Where(x => x.OrderId == cartByUser.FirstOrDefault().OrderId)
                                    select new CartItemDTO
                                    {
                                        AccountId = cartByUser.FirstOrDefault().AccountId.GetValueOrDefault(),
                                        OrderProductId = a.OrderProductId,
                                        OrderId = a.OrderId.GetValueOrDefault(),
                                        ProductColorId = a.ProductColorId.GetValueOrDefault(),
                                        ProductId = db.ProductColors.Where(x => x.ProductColorId == a.ProductColorId).FirstOrDefault().ProductId ?? 0,
                                        ProductName = db.Products.Where(x => x.ProductId == (db.ProductColors.Where(p => p.ProductColorId == a.ProductColorId).FirstOrDefault().ProductId ?? 0)).FirstOrDefault().ProductName ?? "",
                                        Thumbnail = db.Products.Where(x => x.ProductId == (db.ProductColors.Where(p => p.ProductColorId == a.ProductColorId).FirstOrDefault().ProductId ?? 0)).FirstOrDefault().Thumbnail ?? "",
                                        Quantity = a.Quantity.GetValueOrDefault(),
                                        UnitPrice = db.Products.Where(x => x.ProductId == (db.ProductColors.Where(p => p.ProductColorId == a.ProductColorId).FirstOrDefault().ProductId ?? 0)).FirstOrDefault().UnitPrice ?? 0,
                                        Color = db.ProductColors.Where(x => x.ProductColorId == a.ProductColorId).FirstOrDefault().Color ?? ""
                                    });
                return Json(new { success = true, data = listCartItem }, JsonRequestBehavior.AllowGet);
            }
            return Json(new { success = true, data = "" }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetOrderByUserId(int accountId)
        {
            var customerId = db.Accounts.Where(x => x.AccountId == accountId)?.FirstOrDefault()?.AccountId ?? 0;
            if (customerId > 0)
            {
                var listOrder = (from a in db.Orders.Where(o => o.AccountId == customerId)
                                 select new OrderDTO
                                 {
                                     OrderId = a.OrderId,
                                     AccountId = a.AccountId.GetValueOrDefault(),
                                     StatusName = (a.Status == 1) ? "Open" : (a.Status == 2) ? "Delivering" : "Cancle",
                                     Status = a.Status,
                                     TotalPrice = GetTotalPrice(a.OrderId)
                                 }).ToList();
                return Json(new { success = true, data = listOrder }, JsonRequestBehavior.AllowGet);
            }
            return Json(new { success = true, data = "" }, JsonRequestBehavior.AllowGet);
        }

        public int GetCountCart(int accountId)
        {
            var countCart = 0;
            var cart = db.Orders.Where(x => x.AccountId == accountId && x.Status == 1);
            if (cart.Any())
            {
                countCart = db.OrderProducts.Where(x => x.OrderId == cart.FirstOrDefault().OrderId).Sum(c => c.Quantity) ?? 0;
            }
            return countCart;
        }

        public ActionResult PlaceOrder(int accountId)
        {
            var order = db.Orders.Where(x => x.AccountId == accountId && x.Status == 1).FirstOrDefault();
            if (order.OrderId > 0)
            {
                order.Status = 2;
                db.SubmitChanges();
                return Json(new { success = true }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(new { success = false }, JsonRequestBehavior.AllowGet);
            }
        }

        public int GetCountOrder(int accountId)
        {
            var countOrder = 0;
            var customerId = db.Accounts.Where(x => x.AccountId == accountId)?.FirstOrDefault()?.AccountId ?? 0;
            var order = db.Orders.Where(x => x.AccountId == customerId);
            if (order.Any())
            {
                countOrder = db.Orders.Where(x => x.AccountId == customerId && x.Status == 2).Count();
            }
            return countOrder;
        }

        public decimal GetTotalPrice(int orderId)
        {
            var listItem = db.OrderProducts.Where(x => x.OrderId == orderId);
            decimal total = 0;
            if (listItem.Any())
            {
                foreach (var item in listItem)
                {
                    var pId = db.ProductColors.Where(x => x.ProductColorId == item.ProductColorId).FirstOrDefault().ProductId ?? 0;
                    var unitPrice = db.Products.Where(x => x.ProductId == pId).FirstOrDefault().UnitPrice ?? 0;
                    total += unitPrice * item.Quantity.GetValueOrDefault();
                }
            }
            return total;
        }
    }
}