using ProjekatAukcije.Models;
using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.Entity;

namespace ProjekatAukcije.Controllers
{
    public class AuctionController : Controller
    {

        private DefaultConnection db = new DefaultConnection();

        //
        // GET: /Auction/UserIndex
        public ActionResult Close(Guid? id)
        {
            Auction auction = db.Auction.Where(a => a.Id == id).FirstOrDefault();
            auction.DateTimeClosed = DateTime.Now;
            auction.Status = "COMPLETED";

            db.Entry(auction).State = EntityState.Modified;
            db.SaveChanges();
            return RedirectToAction("UserIndex", "Auction");
        }

        public ActionResult CloseAbout(Guid? id)
        {
            Auction auction = db.Auction.Where(a => a.Id == id).FirstOrDefault();
            auction.DateTimeClosed = DateTime.Now;
            auction.Status = "COMPLETED";

            db.Entry(auction).State = EntityState.Modified;
            db.SaveChanges();
            return RedirectToAction("AboutAuction", "Auction");
        }

        [AllowAnonymous]
        public ActionResult UserIndex(string currentFilter, string search, int? lowPrice, int? highPrice, int? NumberOfProductsPage, string status, int? page)
        {

            IList<Auction> auctions = db.Auction.Where(a => a.Status == "OPENED").ToList();

            if (search != null)
                page = 1;
            else
                search = currentFilter;

            ViewBag.CurrentFilter = search;
            foreach (var a in auctions)
            {
                if ((DateTime.Now - a.DateTimeOpened.Value).TotalSeconds > a.Duration)
                {
                    Bid bid = a.Bid.LastOrDefault();
                    if (bid != null)
                    {
                        a.User = bid.User;
                    }
                    a.DateTimeClosed = DateTime.Now;
                    a.Status = "COMPLETED";

                    db.Entry(a).State = EntityState.Modified;
                    db.SaveChanges();
                }
            }
        //    List<Auction> aukcije = new List<Auction>();
            if ((status == null || status == "") && NumberOfProductsPage == null && (search == null || search == "") && highPrice == null && lowPrice == null)
            {
                var auction = from a in db.Auction select a;
                List<Auction> aukcije = new List<Auction>();
                aukcije = auction.ToList();

                ViewBag.NumberOfProductsPage = NumberOfProductsPage != null ? NumberOfProductsPage : db.Admin.FirstOrDefault().NumberOfProductsPage;

                int pageNumber = (page ?? 1);

                return View(PagedListExtensions.ToPagedList(aukcije.Distinct(), pageNumber, ViewBag.NumberOfProductsPage));
            }
                    //  aukcije = db.Auction.ToList();
            else {
                var auction = from a in db.Auction select a;
                if (status != null && status != "")
                {
                    auction = auction.Where(a => a.Status == status);
                   
                }
                ViewBag.Status = status;
                if (highPrice != null)
                {
                    auction = auction.Where(a => a.CurrentPrice < highPrice);
                }
                ViewBag.High = highPrice;

                if (lowPrice != null)
                {
                    auction = auction.Where(a => a.CurrentPrice > lowPrice);
                }
                ViewBag.Low = lowPrice;
                List<Auction> aukcije = new List<Auction>();
                if (search != null && search != "")
                {
                    var words = search.Split(' ');

                    foreach (var a in auction)
                    {
                        foreach (var w in words)
                        {
                            if (a.Name.ToLower().Contains(w.ToLower()))
                                aukcije.Add(a);
                        }
                    }
                }
                else
                {
                    aukcije = auction.ToList();
                }
                ViewBag.NumberOfProductsPage = NumberOfProductsPage != null ? NumberOfProductsPage : db.Admin.FirstOrDefault().NumberOfProductsPage;

                int pageNumber = (page ?? 1);

                return View(PagedListExtensions.ToPagedList(aukcije.Distinct(), pageNumber, ViewBag.NumberOfProductsPage));
            }
           
        }

        // GET: /Auction/AboutAuction
        [AllowAnonymous]
        public ActionResult AboutAuction(Guid id)
        {
            //  Guid guid = Guid.Parse(id);
            Auction auction = db.Auction.Where(a => a.Id == id).FirstOrDefault();
            return View("AboutAuction", auction);
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