using GlassShopMVC.Models;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Threading;
using System.Web;
using System.Web.Mvc;

namespace GlassShopMVC.Controllers
{
    public class AccountController : Controller
    {
        private LinqDataContext db = new LinqDataContext();

        // GET: Account
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Login(Account req)
        {
            var check = db.Accounts.Where(x => x.UserName == req.UserName && x.Password == req.Password);
            if (check.Any())
            {
                var _taikhoan = (from a in db.Accounts.Where(x => x.AccountId == check.FirstOrDefault().AccountId)
                                 select new 
                                 {
                                     AccountId = a.AccountId,
                                     UserName = a.UserName,
                                     Password = a.Password,
                                     Email = a.Email,
                                     Admin = a.Admin.GetValueOrDefault(),
                                     Active = a.Active.GetValueOrDefault()
                                 }).FirstOrDefault();
                return Json(new { success = true, data = _taikhoan, token = createToken(_taikhoan.UserName) }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(new { success = false, data = req }, JsonRequestBehavior.AllowGet);
            }
        }

        public static string createToken(string Username)
        {
            //Set issued at date
            DateTime issuedAt = DateTime.UtcNow;
            //đặt thời gian hết hạn token
            DateTime expires = DateTime.UtcNow.AddDays(10);

            //http://stackoverflow.com/questions/18223868/how-to-encrypt-jwt-security-token
            var tokenHandler = new JwtSecurityTokenHandler();

            //create a identity and add claims to the user which we want to log in

            var userIdentity = new ClaimsIdentity("Identity");
            userIdentity.Label = "Identity";
            userIdentity.AddClaim(new Claim(ClaimTypes.Name, Username));
            userIdentity.AddClaim(new Claim("Username", Username));
            //userIdentity.AddClaim(new Claim("Category", Category));
            //userIdentity.HasClaim(ClaimTypes.Role, Category);
            var claims = new List<Claim>();

            var identity = new ClaimsPrincipal(userIdentity);
            Thread.CurrentPrincipal = identity;
            //string sec = EncryptCode;
            string sec = "088881139703564148785";
            //string sec = "401b09eab3c013d4ca54922bb802bec8fd5318192b0a75f201d8b3727429090fb337591abd3e44453b954555b7a0812e1081c39b740293f765eae731f5a65ed1" + Category;
            var now = DateTime.UtcNow;
            var securityKey = new Microsoft.IdentityModel.Tokens.SymmetricSecurityKey(System.Text.Encoding.Default.GetBytes(sec));
            var signingCredentials = new Microsoft.IdentityModel.Tokens.SigningCredentials(securityKey, Microsoft.IdentityModel.Tokens.SecurityAlgorithms.HmacSha256Signature);

            //create the jwt
            var token =
                (JwtSecurityToken)
                    tokenHandler.CreateJwtSecurityToken(issuer: "http://unisoft.edu.vn/", audience: "http://unisoft.edu.vn/",
                        subject: userIdentity, notBefore: issuedAt, expires: expires, signingCredentials: signingCredentials);
            var tokenString = tokenHandler.WriteToken(token);

            return tokenString;
        }
    }
}