using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

#nullable disable

namespace AcmeStockApp.Models
{
    public partial class StockAppUser
    {
        public int Id { get; set; }

        [Required, EmailAddress]
        public string Email { get; set; }

        [Required]
        public string Name { get; set; }

        [Required, StringLength(8, MinimumLength = 8), DataType(DataType.Password)]
        public string Password { get; set; }

        [Required, StringLength(8, MinimumLength = 8), DataType(DataType.Password)]
        public string ConfirmPassword { get; set; }

        [Display(Name = "User Type")]
        [Required]
        public bool? UserType { get; set; }

        [NotMapped]
        public string UserTypeString { get; set; }
    }
}
