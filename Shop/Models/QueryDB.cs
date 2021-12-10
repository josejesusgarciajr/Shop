using System;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace Shop.Models
{
    public class QueryDB
    {
        private string CS = "Server=localhost;Database=Arizona;User Id=sa;Password=myPassw0rd;";
        public QueryDB()
        {
        }

        public void AddProduct(Product product)
        {

        }

        public Image GetThumbnailImage(int productID)
        {
            Image image = new Image();

            // establish sql connection
            using (SqlConnection sqlConnection = new SqlConnection(CS))
            {
                // query
                string query = "SELECT ID, ImagePath FROM Image"
                        + $" WHERE ProductID = {productID} AND Thumbnail = {1}; ";
                SqlCommand sqlCommand = new SqlCommand(query, sqlConnection);

                // open sql connection
                sqlConnection.Open();

                // get image
                using (SqlDataReader reader = sqlCommand.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        image = new Image((int)reader[0], (string)reader[1]); ;
                    }
                }

                // close sql connection
                sqlConnection.Close();
            }

            return image;
        }

        public Image GetImage(int imageID)
        {
            Image image = new Image();

            // establish sql connection
            using(SqlConnection sqlConnection = new SqlConnection(CS))
            {
                // query
                string query = "SELECT ImagePath FROM Image"
                    + $" WHERE ID = {imageID};";
                SqlCommand sqlCommand = new SqlCommand(query, sqlConnection);

                // open sql connection
                sqlConnection.Open();

                // get image
                using(SqlDataReader reader = sqlCommand.ExecuteReader())
                {
                    while(reader.Read())
                    {
                        image = new Image(imageID, (string)reader[0]); ;
                    }
                }

                // close sql connection
                sqlConnection.Close();
            }

            return image;
        }

        public List<Image> GetCarouselImagesFromProduct(int productID)
        {
            List<Image> images = new List<Image>();

            // establish sql connection
            using(SqlConnection sqlConnection = new SqlConnection(CS))
            {
                // string query
                string query = "SELECT ID, Thumbnail From Image"
                    + $" WHERE ProductID = {productID};";
                SqlCommand sqlCommand = new SqlCommand(query, sqlConnection);

                // open sql connection
                sqlConnection.Open();

                // get images
                using(SqlDataReader reader = sqlCommand.ExecuteReader())
                {
                    while(reader.Read())
                    {
                        int imageID = (int)reader[0];
                        bool thumbnail = (bool)reader[1];

                        if(thumbnail == false)
                        {
                            images.Add(GetImage(imageID));
                        }
                    }
                }

                // close sql connection
                sqlConnection.Close();
            }

            return images;
        }

        public Product GetProduct(int productID)
        {
            Product product = new Product();

            // establish SqlConnection
            using(SqlConnection sqlConnection = new SqlConnection(CS))
            {
                // query
                string query = "SELECT * FROM Product"
                    + $" WHERE ID = {productID};";
                SqlCommand sqlCommand = new SqlCommand(query, sqlConnection);

                Image thumbnail = GetThumbnailImage(productID);
                List<Image> carousel = GetCarouselImagesFromProduct(productID);

                // open sql connection
                sqlConnection.Open();

                // get product
                using(SqlDataReader reader = sqlCommand.ExecuteReader())
                {
                    while(reader.Read())
                    {
                        product = new Product(productID, (int)reader[1], (string)reader[2],
                            (double)reader[3], (string)reader[4], thumbnail, carousel,
                            (bool)reader[5], (double)reader[6], (bool)reader[7]);
                    }
                }

                // close sql connection
                sqlConnection.Close();
            }

            return product;
        }

        public List<Product> GetProductsFromCompany(int companyID)
        {
            List<Product> products = new List<Product>();

            // establish sql connection
            using(SqlConnection sqlConnection = new SqlConnection(CS))
            {
                // query
                string query = "SELECT * FROM Product"
                    + $" WHERE CompanyID = {companyID};";
                SqlCommand sqlCommand = new SqlCommand(query, sqlConnection);

                // open sql connection
                sqlConnection.Open();

                // get products from company
                using(SqlDataReader reader = sqlCommand.ExecuteReader())
                {
                    while(reader.Read())
                    {
                        int productID = (int)reader[0];
                        products.Add(GetProduct(productID));
                    }
                }

                // close sql connection
                sqlConnection.Close();
            }

            return products;
        }

        public Company GetCompany(int companyID)
        {
            Company company = new Company();

            // establish sql connection
            using (SqlConnection sqlConnection = new SqlConnection(CS))
            {
                // query
                string query = "SELECT * FROM Company"
                    + $" WHERE ID = {companyID};";

                List<Product> products = GetProductsFromCompany(companyID);

                SqlCommand sqlCommand = new SqlCommand(query, sqlConnection);

                // open sql connection
                sqlConnection.Open();

                // get company by id from db
                using(SqlDataReader reader = sqlCommand.ExecuteReader())
                {
                    while(reader.Read())
                    {
                        
                        company = new Company(companyID, (string) reader[1], (string) reader[2],
                            (string)reader[3], products, (string) reader[4]);
                    }
                }

                // close sql connection
                sqlConnection.Close();
            }

            return company;
        }

        public Company GetCompanyBasicInfo(int id)
        {
            Company company = new Company();
            // establish sql connection
            using(SqlConnection sqlConnection = new SqlConnection(CS))
            {
                // query
                string query = "SELECT * FROM Company"
                    + $" WHERE ID = {id};";
                SqlCommand sqlCommand = new SqlCommand(query, sqlConnection);

                // open sql connection
                sqlConnection.Open();

                // get basic companies information
                using(SqlDataReader reader = sqlCommand.ExecuteReader())
                {
                    while(reader.Read())
                    {
                        company.ID = (int)reader[0];
                        company.Name = (string)reader[1];
                        company.Address = (string)reader[2];
                        company.HrefAddress = (string)reader[3];
                        company.MissionStatment = (string)reader[4];
                    }
                }

                // close sql connection
                sqlConnection.Close();
            }

            return company;
        }

        public List<Company> GetCompanies()
        {
            List<Company> companies = new List<Company>();
            // establish sql connection
            using (SqlConnection sqlConnection = new SqlConnection(CS))
            {
                // query
                string query = "SELECT * FROM Company";
                SqlCommand sqlCommand = new SqlCommand(query, sqlConnection);

                // open sql connection
                sqlConnection.Open();

                // get basic companies information
                using (SqlDataReader reader = sqlCommand.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        int companyID = (int)reader[0];
                        List<Product> products = GetProductsFromCompany(companyID);

                        Company company = new Company(companyID, (string)reader[1],
                            (string)reader[2], (string)reader[3], products, (string)reader[4]);
                        company.ID = (int)reader[0];
                        company.Name = (string)reader[1];
                        company.Address = (string)reader[2];
                        company.HrefAddress = (string)reader[3];
                        company.MissionStatment = (string)reader[4];
                        companies.Add(company);
                    }
                }

                // close sql connection
                sqlConnection.Close();
            }

            return companies;
        }
    }
}
