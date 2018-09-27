using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace ProjekatAukcije.Models
{
    [Table("Admin")]
    public class Admin
    {
        public int Id { get; set; }

        public int NumberOfProductsPage { get; set; }

        public int DefaultAuctionDuration { get; set; }

        public int SilverPackTokens { get; set; }

        public int GoldPackTokens { get; set; }

        public int PlatinumPackTokens { get; set; }

        [Required]
        [StringLength(3)]
        public string Currency { get; set; }

        public decimal ValueOfToken { get; set; }
    }

    public class AdminViewModel
    {
        [Required]
        [Display(Name = "Number of products per page")]
        [Range(0, int.MaxValue, ErrorMessage = "Must be a positive number")]
        public int NumberOfProductsPage { get; set; }

        [Required]
        [Display(Name = "Default duration of auction")]
        [Range(0, int.MaxValue, ErrorMessage = "Must be a positive number")]
        public int DefaultAuctionDuration { get; set; }

        [Required]
        [Display(Name = "Silver pack tokens")]
        [Range(0, int.MaxValue, ErrorMessage = "Must be a positive number")]
        public int Silver { get; set; }

        [Required]
        [Display(Name = "Gold pack tokens")]
        [Range(0, int.MaxValue, ErrorMessage = "Must be a positive number")]
        public int Gold { get; set; }

        [Required]
        [Display(Name = "Platinum pack tokens")]
        [Range(0, int.MaxValue, ErrorMessage = "Must be a positive number")]
        public int Platinum { get; set; }

        [Required]
        [StringLength(3, ErrorMessage = "The {0} must be at less than 4 characters long.", MinimumLength = 0)]
        [Display(Name = "Currency")]
        public string Currency { get; set; }

        [Required]
        [Display(Name = "Value of token")]
        [Range(0, int.MaxValue, ErrorMessage = "Must be a positive number")]
        public decimal ValueOfToken { get; set; }

    }

}