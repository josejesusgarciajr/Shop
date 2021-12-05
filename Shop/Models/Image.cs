using System;
using Microsoft.AspNetCore.Http;

namespace Shop.Models
{
    public class Image
    {

        public int ID { get; set; }
        public string ImagePath { get; set; }
        public IFormFile ImageFormFile { get; set; }

        public Image() {}

        public Image(int id, string imagePath)
        {
            ID = id;
            ImagePath = imagePath;
        }

    }
}
