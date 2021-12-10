using System;
using System.IO;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;

namespace Shop.Models
{
    public class FolderAndDirectory
    {
        public IWebHostEnvironment WebHostingEnvironment { get; set; }

        public FolderAndDirectory(IWebHostEnvironment e)
        {
            WebHostingEnvironment = e;
        }

        public void InsertThumbnailImageToFolder(Product product)
        {
            QueryDB queryDB = new QueryDB();
            string companyName = queryDB.GetCompanyName(product.ReferenceID);

            string dir = WebHostingEnvironment.WebRootPath + $"/Images/{companyName}/ProductThumbnails";
            string path = WebHostingEnvironment.WebRootPath
                + $"/Images/{companyName}/ProductThumbnails/{product.UploadThumbnail.FileName}";
            //Console.WriteLine($"PATH FOR THUMBNAIL: {path}");

            try
            {
                if(Directory.Exists(dir))
                {
                    // insert image onto directory
                    product.UploadThumbnail.CopyTo(new FileStream(path, FileMode.Create));
                } else
                {
                    // create directory
                    Directory.CreateDirectory(dir);
                    // insert image onto directory
                    product.UploadThumbnail.CopyTo(new FileStream(path, FileMode.Create));
                }
            } catch(Exception e)
            {
                Console.WriteLine($"Error trying to insert image to path: {path}");
                Console.WriteLine(e);
            }
        }

        public void InsertCarouselImagesToFolder(Product product)
        {
            QueryDB queryDB = new QueryDB();
            string companyName = queryDB.GetCompanyName(product.ReferenceID);

            string dir = WebHostingEnvironment.WebRootPath + $"/Images/{companyName}/CarouselImages";
            string path = WebHostingEnvironment.WebRootPath
                + $"/Images/{companyName}/CarouselImages/{product.UploadImageCarousel[0].FileName}";
            //Console.WriteLine($"PATH FOR Carousel: {path}");
            try
            {
                if (Directory.Exists(dir))
                {
                    // insert carousel images onto directory
                    foreach(IFormFile imageFile in product.UploadImageCarousel)
                    {
                        path = WebHostingEnvironment.WebRootPath
                            + $"/Images/{companyName}/CarouselImages/{imageFile.FileName}";
                        imageFile.CopyTo(new FileStream(path, FileMode.Create));
                    }
                }
                else
                {
                    // create directory
                    Directory.CreateDirectory(dir);
  
                    // insert carousel images onto directory
                    foreach (IFormFile imageFile in product.UploadImageCarousel)
                    {
                        path = WebHostingEnvironment.WebRootPath
                            + $"/Images/{companyName}/CarouselImages/{imageFile.FileName}";
                        imageFile.CopyTo(new FileStream(path, FileMode.Create));
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine($"Error trying to insert Carousel image to path: {path}");
                Console.WriteLine(e);
            }
        }
    }
}
