using ProjekatAukcije.Models;
using PagedList;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;

namespace ProjekatAukcije.Controllers
{
    public class TokenController : Controller
    {
        DefaultConnection db = new DefaultConnection();
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(typeof(HomeController));

        public ActionResult OrderToken()
        {
            return View();
        }


        public ActionResult MyOrders(int? page)
        {
            // User user = Session["User"] as User;
            string userId = User.Identity.GetUserId();
            int Id = db.User.Where(u => u.idAspNetUsers == userId).FirstOrDefault().Id;
            int perPage = db.Admin.FirstOrDefault().NumberOfProductsPage;
            var order = db.TokenOrder.Where(t => t.UserId == Id);
            int pageNumber = (page ?? 1);
            return View(PagedListExtensions.ToPagedList(order.ToList(), pageNumber, perPage));
        }


        // POST: Token/Order
        [HttpPost]
        public ActionResult Order(int number)
        {
            TokenOrder order = new TokenOrder();
            //  User user = Session["User"] as User;
            string userId = User.Identity.GetUserId();
            int Id = db.User.Where(u => u.idAspNetUsers == userId).FirstOrDefault().Id;

            Admin admin = db.Admin.FirstOrDefault();

            if (number == 1)
            {
                order.TypeOfTokens = "SYLVER";
                order.NumberOfTokens = admin.SilverPackTokens;
                order.PackagePrice = admin.SilverPackTokens * admin.ValueOfToken;
            }
            if (number == 2)
            {
                order.TypeOfTokens = "GOLD";
                order.NumberOfTokens = admin.GoldPackTokens;
                order.PackagePrice = admin.GoldPackTokens * admin.ValueOfToken;
            }
            if (number == 3)
            {
                order.TypeOfTokens = "PLATINUM";
                order.NumberOfTokens = admin.PlatinumPackTokens;
                order.PackagePrice = admin.PlatinumPackTokens * admin.ValueOfToken;
            }
            order.Id = Guid.NewGuid();
            order.UserId = Id;
            order.Status = "SUBMITTED";

            db.TokenOrder.Add(order);
            db.SaveChanges();

            return Redirect("http://stage.centili.com/payment/widget?apikey=162e68d0383d8eac6835fffac0759ec5&country=rs&reference=" + order.Id + "&returnurl=http://sm150089.azurewebsites.net/Token/CentiliReply");
        }

        public ActionResult CentiliReply(Guid? reference, string status)
        {
            //     using (var transaction = db.Database.BeginTransaction(IsolationLevel.Serializable))
            //    {

            //      try {
            TokenOrder order = db.TokenOrder.Where(t => t.Id == reference).FirstOrDefault();
            if (order != null && order.Status == "SUBMITTED")
            {
                User user = db.User.Where(u => u.Id == order.UserId).FirstOrDefault();// TokenOrder.Find(order.UserId).User;
                if (status != "success")
                {
                    order.Status = "CANCELED";
                    db.Entry(order).State = EntityState.Modified;
                    db.SaveChanges();
                }
                else {
                    user.NumberOfTokens = user.NumberOfTokens + order.NumberOfTokens;
                    order.Status = "COMPLETED";
                    db.Entry(user).State = EntityState.Modified;
                    string subject = $"Your token order";
                    string body = $"Your tokens have been succesfully bought!</p>";
                    SendEmail.Email(body, user.Email, subject);
                    db.Entry(order).State = EntityState.Modified;
                    db.SaveChanges();
                    return RedirectToAction("Index", "Home");
                }

            }

            /*      }
                  catch (Exception e)
                  {
                      transaction.Rollback();
                      log.Error($"Unsucceded transaction on {DateTime.Now}");
                  }
              }
              */
            return RedirectToAction("UserIndex", "Auction");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }




    }
}