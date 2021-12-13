﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Shop.Models;

namespace Shop.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        /*
         * List of Companies
         */
        public static List<Company> Companies { get; set; }

        /*
         * Vaness's Company
         */
        public static Company Vanessa { get; set; }
        private static List<Product> VanessaProducts = new List<Product>();

        /*
         * Jose's Company
         */
        public static Company Jose { get; set; }
        private static List<Product> JoseProducts = new List<Product>();

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            Companies = new List<Company>();

            QueryDB queryDB = new QueryDB();
            Companies = queryDB.GetCompanies();

            return View(Companies);
        }

        public IActionResult DisplayCompanyHomePage(int companyID)
        {
            Console.WriteLine($"Company: {companyID}");
            /*
             * Find CompanyID
             */
            Company company = new Company();
            foreach(Company c in Companies)
            {
                if(c.ID == companyID)
                {
                    company = c;
                }
            }

            return View(company);
        }

        public IActionResult DisplayProductView(int productID)
        {
            // get product
            QueryDB queryDB = new QueryDB();
            Product product = queryDB.GetProduct(productID);
            Console.WriteLine($"Image Carousel Count: {product.ImageCarousel.Count}");
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
