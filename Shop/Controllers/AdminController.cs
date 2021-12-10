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

        public AdminController(IWebHostEnvironment e)
        {
            WebHostEnvironment = e;
        }

        // GET: /<controller>/
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult AddProductView()
        {
            QueryDB queryDB = new QueryDB();
            ViewBag.Companies = queryDB.GetCompanies();

            return View();
        }

        public IActionResult AddProductToDB(Product product)
        {
            QueryDB queryDB = new QueryDB(WebHostEnvironment);

            // add company to DB
            queryDB.AddProduct(product);
            product.ID = queryDB.GetLastProductID();
            // dispaly company information
            //Company company = queryDB.GetCompany(product.ReferenceID);
            return RedirectToAction("DisplayCompanyHomePage", "Home", product.ReferenceID);
        }
    }
}
