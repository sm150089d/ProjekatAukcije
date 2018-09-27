using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace ProjekatAukcije.Models
{
    [Table("User")]
    public class User
    {
        public int Id { get; set; }

        [Required]
        [StringLength(20)]
        public string FirstName { get; set; }

        [Required]
        [StringLength(20)]
        public string LastName { get; set; }

        [Required]
        [StringLength(200)]
        public string Email { get; set; }

        [Required]
        public string Password { get; set; }

        public int NumberOfTokens { get; set; }

        public string idAspNetUsers { get; set; }

        [Required]
        [StringLength(5)]
        public string Role { get; set; }

        public virtual ICollection<Auction> Auction { get; set; }

        public virtual ICollection<Bid> Bid { get; set; }

        public virtual ICollection<TokenOrder> TokenOrder { get; set; }

    }
}