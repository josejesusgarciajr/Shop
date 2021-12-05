using System;
using System.Collections.Generic;

namespace Shop.Models
{
    public class Product
    {
        /*
         * Product Information
         */
        public int ID { get; set; }
        public int ReferenceID { get; set; }
        public string Name { get; set; }
        public double Price { get; set; }
        public string Description { get; set; }

        /*
         * Images Information
         */
        public Image ThumbnailImage { get; set; }
        public List<Image> ImageCarousel { get; set; }

        /*
         * Discount Information
         */
        public bool DiscountBool { get; set; }
        public double Discount { get; set; }

        /*
         * Switch to Flag a Product
         */
        public bool Flag { get; set; }

        public Product()
        {
        }
    }
}
