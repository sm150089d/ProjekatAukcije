using PagedList;
using ProjekatAukcije.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ProjekatAukcije.Controllers
{
    public class AdminController : Controller
    {

        DefaultConnection db = new DefaultConnection();

        [AdminAuthorize(Roles = "Admin")]
        public ActionResult Index()
        {
            return View();
        }


        [AdminAuthorize(Roles = "Admin")]
        public ActionResult Admin()
        {
            return View();
        }
        // Post: Admin
        [HttpPost]
        [AdminAuthorize(Roles = "Admin")]
        public ActionResult Index(AdminViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return RedirectToAction("Admin", "Admin", model);
            }
            Admin admin = db.Admin.FirstOrDefault();
            admin.Currency = model.Currency;
            admin.NumberOfProductsPage = model.NumberOfProductsPage;
            admin.DefaultAuctionDuration = model.DefaultAuctionDuration;
            admin.SilverPackTokens = model.Silver;
            admin.GoldPackTokens = model.Gold;
            admin.PlatinumPackTokens = model.Platinum;
            admin.ValueOfToken = model.ValueOfToken;

            db.Entry(admin).State = EntityState.Modified;
            db.SaveChanges();

            return RedirectToAction("Index", "Admin");
        }

        [AdminAuthorize(Roles = "Admin")]
        public ActionResult Set(int? page)
        {
            var auctions = db.Auction.Where(a => a.Status == "READY");
            int perPage = db.Admin.FirstOrDefault().NumberOfProductsPage;
            int pageNumber = (page ?? 1);

            return View(PagedListExtensions.ToPagedList(auctions.ToList(), pageNumber, perPage));
        }

        [HttpPost]
        [AdminAuthorize(Roles = "Admin")]
        public ActionResult SetOpened(Guid id)
        {
            Auction auction = db.Auction.Where(a => a.Id == id).FirstOrDefault();
            auction.Status = "OPENED";
            auction.DateTimeOpened = DateTime.Now;

            db.Entry(auction).State = EntityState.Modified;

            db.SaveChanges();

            return RedirectToAction("Index", "Admin");
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