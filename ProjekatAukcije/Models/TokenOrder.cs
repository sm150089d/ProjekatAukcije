using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ProjekatAukcije.Models
{
    [Table("TokenOrder")]
    public class TokenOrder
    {
        public Guid Id { get; set; }

        [Required]
        public int UserId { get; set; }

        [Required]
        public string TypeOfTokens { get; set; }

        [Required]
        public int NumberOfTokens { get; set; }

        [Required]
        public decimal PackagePrice { get; set; }

        [Required]
        [StringLength(10)]
        public string Status { get; set; }

        public virtual User User { get; set; }

    }




}