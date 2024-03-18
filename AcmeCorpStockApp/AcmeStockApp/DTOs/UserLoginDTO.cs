using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace AcmeStockApp.DTOs
{
    public class UserLoginDTO
    {
        public int Id { get; set; }

        [Required, EmailAddress]
        public string Email { get; set; }

        [Required, StringLength(8, MinimumLength = 8), DataType(DataType.Password)]
        public string Password { get; set; }
        
        [NotMapped]
        public bool RememberMe { get; set; }
    }
}
