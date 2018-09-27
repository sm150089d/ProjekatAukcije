using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace ProjekatAukcije.Models
{
    public class DefaultConnection : DbContext
    {
        public virtual DbSet<User> User { get; set; }
        public virtual DbSet<Auction> Auction { get; set; }
        public virtual DbSet<Bid> Bid { get; set; }
        public virtual DbSet<TokenOrder> TokenOrder { get; set; }
        public virtual DbSet<Admin> Admin { get; set; }


        public DefaultConnection() : base("name=DefaultConnection")
        {
        }
    }
}