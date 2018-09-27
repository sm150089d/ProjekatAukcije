using Microsoft.AspNet.Identity;
using ProjekatAukcije.Models;
using ProjekatAukcije.Hubs;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.SignalR;

namespace ProjekatAukcije.Controllers
{
    
    public class BidController : Controller
    {
        DefaultConnection db = new DefaultConnection();
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(typeof(HomeController));

        // Post: Bid
        [HttpPost]
        public ActionResult Pay(Guid auctionId)
        {
            Auction auction = db.Auction.Where(a => a.Id == auctionId).FirstOrDefault();
            return View("Payment", auction);
        }


        // POST: Bid
        
        [HttpPost]
        public ActionResult Payment(Guid auctionId, int amount)
        {
            using (var transaction = db.Database.BeginTransaction(IsolationLevel.Serializable))
            {
                string userId = User.Identity.GetUserId();
                User user = db.User.Where(u => u.idAspNetUsers == userId).FirstOrDefault();
                int Id = db.User.Where(u => u.idAspNetUsers == userId).FirstOrDefault().Id;
                try {
                    int NumberOfTokensOfUser = db.User.Where(u => u.Id == Id).FirstOrDefault().NumberOfTokens;
                    decimal HighestNumberOfTokensInAuction = db.Auction.Where(a => a.Id == auctionId).FirstOrDefault().CurrentPrice;

                    if (NumberOfTokensOfUser <= HighestNumberOfTokensInAuction || amount > NumberOfTokensOfUser)
                    {
                        Auction auction = db.Auction.Where(a => a.Id == auctionId).FirstOrDefault();
                        return View("Payment", auction);
                    }
                    else
                    {

                        Bid bid = new Bid();
                        bid.UserId = Id;
                        bid.AuctionId = auctionId;
                        bid.TimeStarted = DateTime.Now;
                        bid.NumberOfTokens = amount;

                        db.User.Where(u => u.Id == Id).FirstOrDefault().NumberOfTokens = db.User.Where(u => u.Id == Id).FirstOrDefault().NumberOfTokens - amount;
                        db.Auction.Where(a => a.Id == auctionId).FirstOrDefault().PriceInTokens = amount;
                        db.Auction.Where(a => a.Id == auctionId).FirstOrDefault().CurrentPrice += amount * db.Auction.Where(a => a.Id == auctionId).FirstOrDefault().TokenValue;

                        db.Bid.Add(bid);
                        db.SaveChanges();
                        transaction.Commit();

                        Auction auction = db.Auction.Where(a => a.Id == auctionId).FirstOrDefault();

                        var hubContext = GlobalHost.ConnectionManager.GetHubContext<ProjekatHub>();
                        hubContext.Clients.All.userIndex(auctionId, auction.CurrentPrice, auction.Currency, user.FirstName, user.LastName);
                        hubContext.Clients.All.newBid(user.Email, System.DateTime.Now.ToString("dd MMM yyyy hh:mm:ss"), auction.PriceInTokens, auction.CurrentPrice);

                    }
                }
                catch (Exception e)
                {
                    transaction.Rollback();
                    log.Error($"Failed transaction! Bid placed on auction {auctionId} failed.");
                }

            }


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