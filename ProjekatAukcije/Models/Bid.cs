using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace ProjekatAukcije.Models
{
    [Table("Bid")]
    public class Bid
    {
        public int Id { get; set; }

        [Required]
        public int UserId { get; set; }

        [Required]
        public Guid AuctionId { get; set; }

        [Required]
        public DateTime TimeStarted { get; set; }

        [Required]
        public int NumberOfTokens { get; set; }

        public virtual Auction Auction { get; set; }

        public virtual User User { get; set; }


    }



}