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
        public double DiscountPercentage { get; set; }

        /*
         * Switch to Flag a Product
         */
        public bool Flag { get; set; }

        public Product() { }

        public Product(int id, int referenceID, string name, double price, string desription,
            Image thumbnailImage, List<Image> imageCarousel, bool discountBool, double discount, bool flag)
        {
            ID = id;
            ReferenceID = referenceID;
            Name = name;
            Price = price;
            Description = desription;
            ThumbnailImage = thumbnailImage;
            ImageCarousel = imageCarousel;
            DiscountBool = discountBool;
            DiscountPercentage = discount;
            Flag = flag;
        }
    }
}
