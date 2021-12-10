using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Shop.Models;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Shop.Controllers
{
    public class AdminController : Controller
    {
        // GET: /<controller>/
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult AddProductView()
        {
            QueryDB queryDB = new QueryDB();
            //List<Company> companies = queryDB.GetEveryCompaniesBasicInfo();
            //List<Company> companies = HomeController.Companies;
            ViewBag.Companies = HomeController.Companies;
            return View();
        }

        public IActionResult AddProductToDB(Product product)
        {
            QueryDB queryDB = new QueryDB();

            // add company to DB
            queryDB.AddProduct(product);

            // dispaly company information
            Company company = queryDB.GetCompany(product.ReferenceID);
            return RedirectToAction("ViewCompanyInformation", company);
        }
    }
}
