using AcmeStockApp.BLL.Interfaces;
using AcmeStockApp.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.Extensions.Logging;
using SendGrid.Helpers.Errors.Model;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace AcmeStockApp.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IProductService _proService;

        public HomeController(ILogger<HomeController> logger, IProductService context)
        {
            _logger = logger;
            this._proService = context;
        }

        public IActionResult Index()
        {
          //throw new Exception();
            return View();
        }
        
        public IActionResult CloseWindow()
        {
            CloseWindow();
            return View();
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(Product product)
        {
            if (ModelState.IsValid)
            {
                _proService.Add(product);
                return RedirectToAction(nameof(Index));
            }
            return View(product);
        }

        [HttpGet]
        public IActionResult Find()
        {
            ViewBag.ErrorMessage = null;
            return View();
        }

        public IActionResult Find(int id)
        {
            if (id == 0)
            {
                ViewBag.ErrorMessage = "Please provide a valid ID.";
                return View();
            }

            var product = _proService.GetFindProductById(id);
            if (product == null)
            {
                ViewBag.ErrorMessage = "No product found with this ID.";
                return View();
            }
            
            return View(product);
        }

        [HttpGet]
        public IActionResult FindUpdate()
        {
            ViewBag.ErrorMessage = null;
            return View();
        }

        [HttpPost]
        public IActionResult FindUpdate(int id)
        {
            if (id == 0)
            {
                ViewBag.ErrorMessage = "Please provide a valid ID.";
                return View();
            }

            var product = _proService.GetFindProductById(id);
            if (product == null)
            {
                ViewBag.ErrorMessage = "No product found with this ID.";
                return View();
            }

            return View(product);
        }

        [HttpGet]
        public IActionResult SaveUpdate()
        {
            ViewBag.ErrorMessage = null;
            return View();
        }

        [HttpPost]
        public IActionResult SaveUpdate(Product updatedProduct)
        {
            var existingProduct = _proService.GetFindProductById(updatedProduct.Id);

            if (ModelState.IsValid)
            {
                _proService.UpdateProduct(existingProduct, updatedProduct);
                _proService.Save();
            }

            return View(existingProduct);
        }

        [HttpGet]
        public IActionResult Delete()
        {
            ViewBag.ErrorMessage = null;
            return View();
        }

        [HttpPost]
        public IActionResult Delete(int id)
        {
            if (id == 0)
            {
                ViewBag.ErrorMessage = "Please provide a valid ID.";
                return View();
            }

            var product = _proService.GetFindProductById(id);
            if (product == null)
            {
                ViewBag.ErrorMessage = "Error: No product found.";
                return View();
            }

            _proService.DeleteProduct(product);
            _proService.Save();

            return View(product);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
