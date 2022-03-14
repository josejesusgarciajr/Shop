using System;
using System.IO;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;

namespace Shop.Models
{
    public class FolderAndDirectory
    {
        private readonly IWebHostEnvironment WebHostingEnvironment;

        public FolderAndDirectory(IWebHostEnvironment e)
        {
            WebHostingEnvironment = e;
        }

        public void DeleteImage(int imageID)
        {
            try
            {
                QueryDB queryDB = new QueryDB();
                string imagePath = queryDB.GetImage(imageID).ImagePath;
                string webRootPath = WebHostingEnvironment.WebRootPath + imagePath;

                File.Delete(webRootPath);
            } catch(FileNotFoundException e)
            {
                Console.WriteLine($"File not Found: {e}");
            }
        }

        public void InsertThumbnailImageToFolder(Product product)
        {
            QueryDB queryDB = new QueryDB();
            string companyName = queryDB.GetCompanyName(product.ReferenceID);

            string dir = WebHostingEnvironment.WebRootPath + $"/Images/{companyName}/ProductThumbnails";
            string path = WebHostingEnvironment.WebRootPath
                + $"/Images/{companyName}/ProductThumbnails/{product.UploadThumbnail.FileName}";

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
            QueryDB queryDB = new QueryDB(WebHostingEnvironment);
            string companyName = queryDB.GetCompanyName(product.ReferenceID);

            string dir = WebHostingEnvironment.WebRootPath + $"/Images/{companyName}/CarouselImages";
            string path = "";
            if (product.UploadImageCarousel != null)
            {
                path = WebHostingEnvironment.WebRootPath
                    + $"/Images/{companyName}/CarouselImages/{product.UploadImageCarousel[0].FileName}";
            }
          
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
