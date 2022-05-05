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
         * Vaness's Company
         */
        public static Company Vanessa { get; set; }

        /*
         * Jose's Company
         */
        public static Company Jose { get; set; }

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index(string search = null)
        {
            QueryDB queryDB = new QueryDB();
            List<Company> companies = new List<Company>();

            if (search == null)
            {
                companies = queryDB.GetCompanies();
            } else
            {
                companies = queryDB.GetCompaniesBySearch(search);
            }
            ViewData["search"] = search;
            return View(companies);
        }

        public IActionResult AddNoteView(int companyID)
        {
            ViewData["companyID"] = companyID;
            return View();
        }

        public IActionResult AddNotetoCompany(Note note)
        {
            note.Date = DateTime.Now.ToString("f");

            QueryDB queryDB = new QueryDB();
            queryDB.AddNote(note);

            return RedirectToAction("DisplayCompanyHomePage", "Home", new { companyID = note.CompanyID });
        }

        public IActionResult DeleteNote(int noteID, int companyID)
        {
            QueryDB queryDB = new QueryDB();
            queryDB.DeleteNote(noteID);

            return RedirectToAction("DisplayCompanyHomePage", "Home", new { companyID = companyID });
        }

        public IActionResult DisplayCompanyHomePage(int companyID, string search = null)
        {
            // check if mobile device
            Mobile mobile = new Mobile(HttpContext);
            ViewData["IsMobileDevice"] = mobile.IsMobileDeviceBrowser();

            /*
             * Find CompanyID
             */

            QueryDB queryDB = new QueryDB();
            Company company = queryDB.GetCompany(companyID);

            if(search != null)
            {
                company.Products = queryDB.GetSearchProducts(companyID, search);
            }

            ViewData["search"] = search;
            return View(company);
        }

        public IActionResult DisplayProductView(int productID)
        {
            // get product
            QueryDB queryDB = new QueryDB();
            Product product = queryDB.GetProduct(productID);

            if(product.Flag == true)
            {
                return RedirectToAction("Index", "Home");
            }

            return View(product);
        }

        public IActionResult Privacy()
        {
            HttpContext.Session.Clear();
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
