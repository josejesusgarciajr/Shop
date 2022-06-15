using System;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Shop.Models;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Shop.Controllers
{
    public class AdminController : Controller
    {
        private readonly IWebHostEnvironment WebHostEnvironment;
        private static int CompanyID = -1;
        public static Authentication Authentication = new Authentication();

        public AdminController(IWebHostEnvironment e)
        {
            WebHostEnvironment = e;
        }

        // GET: /<controller>/
        public IActionResult Index()
        {
            if(HttpContext.Session.GetInt32("key") == null)
            {
                /*
                 * Session has not yet been implemented
                 * Redirect to home page
                 */
                return RedirectToAction("Index", "Home");
            }

            if(HttpContext.Session.GetInt32("key") == Authentication.Key)
            {
                QueryDB queryDB = new QueryDB();
                ViewBag.Companies = queryDB.GetCompanies();
                return View();
            }

            return RedirectToAction("Index", "Home");
        }

        public IActionResult EnterGeneratedKey()
        {
            Random rand = new Random();

            int key = rand.Next(0, 10000);
            Authentication.Key = key;

            string senderEmail = Environment.GetEnvironmentVariable("SENDER_EMAIL");
            string senderPassword = Environment.GetEnvironmentVariable("SENDER_PASSWORD");
            string reciverEmail = senderEmail;

            Email email = new Email(senderEmail, senderPassword, reciverEmail, key);
            email.SendEmail();

            return View((object)key);
        }

        public IActionResult TestAuthentication(int key)
        {
            if(Authentication.Key == key)
            {
                HttpContext.Session.SetInt32("key", key);
                Authentication.LogIn();

                return RedirectToAction("Index", "Admin");
            }

            return RedirectToAction("Index", "Home");
        }

        public IActionResult DisplayCompanyInfo(int companyID)
        {
            if(HttpContext.Session.GetInt32("key") == null)
            {
                return RedirectToAction("Index", "Home");
            }

            if(HttpContext.Session.GetInt32("key") == Authentication.Key)
            {
                QueryDB queryDB = new QueryDB();
                Company company = queryDB.GetCompany(companyID);
                CompanyID = companyID;

                return View(company);
            }

            return RedirectToAction("Index", "Home");
        }

        public IActionResult DeleteProductFromDB(int productID)
        {
            if(HttpContext.Session.GetInt32("key") == null)
            {
                return RedirectToAction("Index", "Home");
            }

            if(HttpContext.Session.GetInt32("key") == Authentication.Key)
            {
                QueryDB queryDB = new QueryDB(WebHostEnvironment);
                queryDB.DeleteProduct(productID);

                return RedirectToAction("DisplayCompanyInfo", new { companyID = CompanyID });
            }

            return RedirectToAction("Index", "Home");
        }

        public IActionResult AddCompanyView()
        {
            if(HttpContext.Session.GetInt32("key") == null)
            {
                return RedirectToAction("Index", "Home");
            }

            if(HttpContext.Session.GetInt32("key") == Authentication.Key)
            {
                return View();
            }

            return RedirectToAction("Index", "Home");
        }

        public IActionResult AddCompanyToDB(Company company)
        {
            if(HttpContext.Session.GetInt32("key") == null)
            {
                return RedirectToAction("Index", "Home");
            }

            if(HttpContext.Session.GetInt32("key") == Authentication.Key)
            {
                QueryDB queryDB = new QueryDB();
                queryDB.AddCompany(company);

                return RedirectToAction("Index", "Home");
            }

            return RedirectToAction("Index", "Home");
        }

        public IActionResult AddProductView(int companyID = -1)
        {
            if(HttpContext.Session.GetInt32("key") == null)
            {
                return RedirectToAction("Index", "Home");
            }

            if(HttpContext.Session.GetInt32("key") == Authentication.Key)
            {
                QueryDB queryDB = new QueryDB();
                ViewBag.Companies = queryDB.GetCompanies();
                ViewData["companyID"] = companyID;

                return View();
            }

            return RedirectToAction("Index", "Home");
        }

        public IActionResult AddProductToDB(Product product)
        {
            if(HttpContext.Session.GetInt32("key") == null)
            {
                return RedirectToAction("Index", "Home");
            }

            if(HttpContext.Session.GetInt32("key") == Authentication.Key)
            {
                // add company to DB
                QueryDB queryDB = new QueryDB(WebHostEnvironment);
                queryDB.AddProduct(product);

                return RedirectToAction("DisplayCompanyInfo", "Admin", new { companyID = product.ReferenceID });
            }

            return RedirectToAction("Index", "Home");
        }

        public IActionResult EditProductView(int productID)
        {
            if(HttpContext.Session.GetInt32("key") == null)
            {
                return RedirectToAction("Index", "Home");
            }

            if(HttpContext.Session.GetInt32("key") == Authentication.Key)
            {
                QueryDB queryDB = new QueryDB();
                Product product = queryDB.GetProduct(productID);

                // get images for carousel
                product.ThumbnailImage = queryDB.GetThumbnailImage(productID);
                product.ImageCarousel = queryDB.GetCarouselImagesFromProduct(productID);

                return View(product);
            }

            return RedirectToAction("Index", "Home");
        }

        public IActionResult EditProductInDB(Product product)
        {

            if(HttpContext.Session.GetInt32("key") == null)
            {
                return RedirectToAction("Index", "Home");
            }

            if(HttpContext.Session.GetInt32("key") == Authentication.Key)
            {

                QueryDB queryDB = new QueryDB(WebHostEnvironment);
                FolderAndDirectory folderAndDirectory = new FolderAndDirectory(WebHostEnvironment);

                /*
                 * Update Products Basic information
                 */
                queryDB.EditProduct(product);

                if(product.UploadThumbnail != null)
                {
                    // delete current thumbnail
                    int thumbnailImageID = queryDB.GetThumbnailImage(product.ID).ID;
                    folderAndDirectory.DeleteImage(thumbnailImageID);
                    queryDB.DeleteImage(thumbnailImageID);

                    folderAndDirectory.InsertThumbnailImageToFolder(product);
                }

                /*
                 * Remove deleted Carousel images from folders
                 * Delete Images Paths from DB
                 */
                if(product.CarouselImagesID != null)
                {
                    foreach(int imageID in product.CarouselImagesID)
                    {
                        folderAndDirectory.DeleteImage(imageID);
                        queryDB.DeleteImage(imageID);
                    }
                }

                // insert carousel images
                folderAndDirectory.InsertCarouselImagesToFolder(product);

                // add all product images to DB
                queryDB.AddProductImages(product);

                return RedirectToAction("DisplayCompanyInfo", new { companyID = product.ReferenceID});
            }

            return RedirectToAction("Index", "Home");
        }
    }
}
