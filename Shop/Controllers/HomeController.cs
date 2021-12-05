using System;
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
        public List<Company> Companies = new List<Company>();

        /*
         * Vaness's Company
         */
        public static Company Vanessa { get; set; }
        private static List<Product> VanessaProducts { get; set; }

        /*
         * Jose's Company
         */
        public static Company Jose { get; set; }
        private static List<Product> JoseProducts { get; set; }
 
        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            Vanessa = new Company(0, "Vanessa", "DisneyWorld, Orlando Florida", VanessaProducts,
                "https://www.google.com/maps/dir//Walt+Disney+World®+Resort,+Orlando,+FL/@28.3771857,-81.5729287,17z/data=!4m9!4m8!1m0!1m5!1m1!1s0x88dd7ee634caa5f7:0xa71e391fd01cf1a0!2m2!1d-81.57074!2d28.3771857!3e0",
                "To make the world a better place!");

            Jose = new Company(1, "Jose", "Maui, Hawaii", JoseProducts,
                "https://www.google.com/maps/dir//Maui,+Hawaii/@20.7983484,-156.4019666,12z/data=!4m9!4m8!1m0!1m5!1m1!1s0x79552b4acc4c61dd:0xcc43e741dc113e7f!2m2!1d-156.3319253!2d20.7983626!3e0",
                "To take back earth. To unfuck it!");

            Companies.Add(Vanessa);
            Companies.Add(Jose);

            return View(Companies);
        }

        public IActionResult DisplayCompanyHomePage(int companyID)
        {
            Console.WriteLine($"Company: {companyID}");
            /*
             * Find CompanyID
             */

            return View();
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
