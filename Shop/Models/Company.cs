﻿using System;
using System.Collections.Generic;

namespace Shop.Models
{
    public class Company
    {
        /*
         * Company's Basic Info
         */
        public int ID { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public List<Product> Products { get; set; }

        /*
         * Web Information
         */
        public string HrefAddress { get; set; }

        /*
         * Company's Mission Statement
         */
        public string MissionStatment { get; set; }

        public Company() { }

        public Company(int id, string name, string address, string hrefAddress, List<Product> products,
            string missionStatment)
        {
            ID = id;
            Name = name;
            Address = address;
            HrefAddress = hrefAddress;
            Products = products;
            MissionStatment = missionStatment;
        }
    }
}
