using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Shop.Models;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Shop.Controllers
{
    public class AdminController : Controller
    {
        private readonly IWebHostEnvironment WebHostEnvironment;
        private static int CompanyID = -1;
        public AdminController(IWebHostEnvironment e)
        {
            WebHostEnvironment = e;
        }

        // GET: /<controller>/
        public IActionResult Index()
        {
            QueryDB queryDB = new QueryDB();
            ViewBag.Companies = queryDB.GetCompanies();
            return View();
        }

        public IActionResult DisplayCompanyInfo(int companyID)
        {
            QueryDB queryDB = new QueryDB();
            Company company = queryDB.GetCompany(companyID);
            CompanyID = companyID;

            return View(company);
        }

        public IActionResult DeleteProduct(int productID)
        {
            return RedirectToAction("DisplayCompanyInfo", new { companyID = CompanyID });
        }

        public IActionResult AddCompanyView()
        {
            return View();
        }

        public IActionResult AddCompanyToDB(Company company)
        {
            QueryDB queryDB = new QueryDB();
            queryDB.AddCompany(company);

            return RedirectToAction("Index", "Home");
        }

        public IActionResult AddProductView()
        {
            QueryDB queryDB = new QueryDB();
            ViewBag.Companies = queryDB.GetCompanies();

            return View();
        }

        public IActionResult AddProductToDB(Product product)
        {
            Console.WriteLine($"Name of product: {product.Name}");
            // add company to DB
            QueryDB queryDB = new QueryDB(WebHostEnvironment);
            queryDB.AddProduct(product);

            return RedirectToAction("DisplayCompanyHomePage", "Home", new { companyID = product.ReferenceID });
        }
    }
}
