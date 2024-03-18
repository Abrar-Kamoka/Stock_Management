using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AcmeStockApp.Models.DBEntities
{
    public class RecaptchaKeys
    {
        public string RecaptchaPublickey { get; set; }
        public string RecaptchaPrivatekey { get; set; }
    }
}
