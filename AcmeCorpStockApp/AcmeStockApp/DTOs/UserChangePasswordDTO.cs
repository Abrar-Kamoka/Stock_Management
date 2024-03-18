using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace AcmeStockApp.DTOs
{
    public class UserChangePasswordDTO
    {

        [Required, EmailAddress]
        public string Email { get; set; }

        [Required, StringLength(8, MinimumLength = 8), DataType(DataType.Password), Display(Name = "Current Password")]
        public string Password { get; set; }


        [Required, StringLength(8, MinimumLength = 8), DataType(DataType.Password), Display(Name = "New Password")]
        [NotMapped]
        public string NewPassword { get; set; }

        [Required, StringLength(8, MinimumLength = 8), DataType(DataType.Password), Display(Name = "Confirm Password")]
        [NotMapped]
        public string ConfirmPassword { get; set; }

     
    }
}
