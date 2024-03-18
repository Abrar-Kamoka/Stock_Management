using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AcmeStockApp.Controllers
{
    public class GlobalErrorController : Controller
    {

        [Route("/404")]
        public IActionResult Error404()
        {
            return View();
        }

        [Route("/Error401")]
        public IActionResult Error401()
        {
            return View();
        }

        [Route("/Error500")]
        public IActionResult Error500()
        {
            return View();
        }

    }
}
