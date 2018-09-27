using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ProjekatAukcije.Models
{
    [Table("Auction")]
    public class Auction
    {
        public Guid Id { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public byte[] Picture { get; set; }


        public int Duration { get; set; }


        public decimal StartingPrice { get; set; }


        public decimal CurrentPrice { get; set; }

        public int PriceInTokens { get; set; }

        public decimal TokenValue { get; set; }


        public DateTime DateTimeCreated { get; set; }


        public DateTime? DateTimeOpened { get; set; }


        public DateTime? DateTimeClosed { get; set; }

        [Required]
        [StringLength(10)]
        public string Status { get; set; }

        [Required]
        [StringLength(3)]
        public string Currency { get; set; }
        public int? IdOfUser { get; set; }
        public virtual User User { get; set; }

        public virtual ICollection<Bid> Bid { get; set; }

    }

    public class UserIndexViewModel
    {

        [Display(Name = "Product")]
        public string Search { get; set; }

        [Display(Name = "Low value of the price")]
        public string LowPrice { get; set; }

        [Display(Name = "High value of the price")]
        public string HighPrice { get; set; }


        [Display(Name = "Status")]
        public string Status { get; set; }

        [Display(Name = "Number Of Products Page")]
        public int NumberOfProductsPage { get; set; }


    }


}