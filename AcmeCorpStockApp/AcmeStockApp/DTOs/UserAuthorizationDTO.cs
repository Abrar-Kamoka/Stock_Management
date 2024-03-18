using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace AcmeStockApp.DTOs
{
    public class UserAuthorizationDTO
    {
        [Display(Name = "User Type")]
        [Required]
        public bool? UserType { get; set; }

        [NotMapped]
        public string UserTypeString { get; set; }
    }
}
