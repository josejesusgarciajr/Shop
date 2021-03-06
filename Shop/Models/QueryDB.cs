using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;

namespace Shop.Models
{
    public class QueryDB
    {
        private string CS = Environment.GetEnvironmentVariable("CONNECTION_STRING");
        // Server=localhost;Database=Arizona;User Id=sa;Password=myPassw0rd;
        private IWebHostEnvironment WebHostEnvironment { get; set; }
        public QueryDB()
        {
        }

        public QueryDB(IWebHostEnvironment e)
        {
            WebHostEnvironment = e;
        }


        private string CleanUpApostrophe(string text)
        {
            string cleanUp = "";

            int index = 0;
            while(index < text.Length)
            {
                if(text[index] == '\'')
                {
                    cleanUp += text[index] + "'";
                } else
                {
                    cleanUp += text[index];
                }
                index++;
            }

            return cleanUp;
        }

        public Note GetNote(int noteID)
        {
            Note note = new Note();

            // establish sql connection
            using(SqlConnection sqlConnection = new SqlConnection(CS))
            {
                // query
                string query = "SELECT * FROM Note"
                    + $" WHERE ID = {noteID}";
                SqlCommand sqlCommand = new SqlCommand(query, sqlConnection);

                //open connection
                sqlConnection.Open();

                // get note from db
                using(SqlDataReader reader = sqlCommand.ExecuteReader())
                {
                    while(reader.Read())
                    {
                        note.ID = (int)reader[0];
                        note.CompanyID = (int)reader[1];
                        note.Date = (string)reader[2];
                        note.Description = (string)reader[3];
                        note.Status = (string)reader[4];
                    }
                }

                // close connection
                sqlConnection.Close();
            }

            return note;
        }

        public void AddNote(Note note)
        {
            note.Description = CleanUpApostrophe(note.Description);

            // establish sql connection
            using(SqlConnection sqlConnection = new SqlConnection(CS))
            {
                // query
                string query = "INSERT INTO Note(CompanyID, Date, Description, Status)"
                    + $" VALUES({note.CompanyID}, '{note.Date}', '{note.Description}', 'Working on It');";
                SqlCommand sqlCommand = new SqlCommand(query, sqlConnection);

                // open connection
                sqlConnection.Open();

                // add note to db
                sqlCommand.ExecuteNonQuery();

                // close connection
                sqlConnection.Close();
            }
        }

        public void UpdateNote(Note note)
        {
            note.Description = CleanUpApostrophe(note.Description);

            // establish sql connection
            using (SqlConnection sqlConnection = new SqlConnection(CS))
            {
                // query
                string query = "UPDATE NOTE"
                    + $" SET Description = '{note.Description}', Status = '{note.Status}'"
                    + $" WHERE ID = {note.ID};";
    
                SqlCommand sqlCommand = new SqlCommand(query, sqlConnection);

                // open connection
                sqlConnection.Open();

                // add note to db
                sqlCommand.ExecuteNonQuery();

                // close connection
                sqlConnection.Close();
            }
        }

        public void DeleteNote(int noteID)
        {
            // establish sql connection
            using (SqlConnection sqlConnection = new SqlConnection(CS))
            {
                // query
                string query = $"DELETE FROM Note WHERE ID = {noteID};";
                SqlCommand sqlCommand = new SqlCommand(query, sqlConnection);

                // open connection
                sqlConnection.Open();

                // add note to db
                sqlCommand.ExecuteNonQuery();

                // close connection
                sqlConnection.Close();
            }
        }

        public List<Product> GetSearchProducts(int companyID, string search)
        {
            List<Product> products = new List<Product>();

            // establish sql connection
            using(SqlConnection sqlConnection = new SqlConnection(CS))
            {
                // query
                string query = "SELECT ID FROM Product" 
                    + $" WHERE (Name LIKE '%{search}%' OR [Description] LIKE '%{search}%')"
                    + $" AND CompanyID = {companyID};";
                SqlCommand sqlCommand = new SqlCommand(query, sqlConnection);

                // open sql connection
                sqlConnection.Open();

                // get searched [products]
                using(SqlDataReader reader = sqlCommand.ExecuteReader())
                {
                    while(reader.Read())
                    {
                        products.Add(GetProduct((int)reader[0]));
                    }
                }

                // close sql connection
                sqlConnection.Close();
            }

            return products;
        }

        public void DeleteImage(int imageID)
        {
            // establish sql connection
            using(SqlConnection sqlConnection = new SqlConnection(CS))
            {
                // query
                string query = "DELETE FROM Image"
                    + $" WHERE ID = {imageID}";
                SqlCommand sqlCommand = new SqlCommand(query, sqlConnection);

                // open sql connection
                sqlConnection.Open();

                // delete image from DB
                sqlCommand.ExecuteNonQuery();

                // close sql connection
                sqlConnection.Close();
            }
        }

        public void EditProduct(Product product)
        {
            // establish sql connection
            using(SqlConnection sqlConnection = new SqlConnection(CS))
            {
                int discountBool = -1;
                if (product.DiscountBool == false)
                {
                    discountBool = 0;
                }
                else
                {
                    discountBool = 1;
                }

                int flag = -1;
                if (product.Flag == false)
                {
                    flag = 0;
                }
                else
                {
                    flag = 1;
                }

                string nameOfProduct = CleanUpApostrophe(product.Name);
                string description = CleanUpApostrophe(product.Description);
                // query
                string query = "UPDATE Product" 
                    + $" SET Name = '{nameOfProduct}', Price = {product.Price}, " 
                    + $"[Description] = '{description}', DiscountBool = {discountBool},"
                    + $" DiscountPercentage = {product.DiscountPercentage}, Flag = {flag}"
                    + $" WHERE ID = {product.ID};";
                SqlCommand sqlCommand = new SqlCommand(query, sqlConnection);

                // open sql connection
                sqlConnection.Open();

                // edit product in database
                sqlCommand.ExecuteNonQuery();

                // close sql connection
                sqlConnection.Close();
            }
        }

        private void DeleteProductCarouselImages(int productID)
        {
            // establish sql connection
            using(SqlConnection sqlConnection = new SqlConnection(CS))
            {
                // query
                string query = "DELETE FROM Image"
                    + $" WHERE ProductID = {productID};";
                SqlCommand sqlCommand = new SqlCommand(query, sqlConnection);

                // open sql connection
                sqlConnection.Open();

                // delete carousel
                sqlCommand.ExecuteNonQuery();

                // close sql connection
                sqlConnection.Close();
            }
        }

        public void DeleteProduct(int productID)
        {
            /*
             *  delete all images associated with image
             */
            FolderAndDirectory folderAndDirectory = new FolderAndDirectory(WebHostEnvironment);

            List<int> imageIDList = new List<int>();
            using(SqlConnection sqlConnection = new SqlConnection(CS))
            {
                // query
                string query = "SELECT ID FROM Image"
                    + $" WHERE ProductID = '{productID}';";
                SqlCommand sqlCommand = new SqlCommand(query, sqlConnection);

                // open sql connection
                sqlConnection.Open();

                using(SqlDataReader reader = sqlCommand.ExecuteReader())
                {
                    while(reader.Read())
                    {
                        imageIDList.Add((int)reader[0]);
                    }
                }

                // close slq connection
                sqlConnection.Close();
            }

            foreach(int imageID in imageIDList)
            {
                folderAndDirectory.DeleteImage(imageID);
            }

            /*
             * Now delete the product carousel images from database
             * they depend on product
             */
            DeleteProductCarouselImages(productID);

            // establish sql connection
            using (SqlConnection sqlConnection2 = new SqlConnection(CS))
            {
                // query
                string query = "DELETE FROM Product"
                    + $" WHERE ID = {productID};";
                SqlCommand sqlCommand = new SqlCommand(query, sqlConnection2);

                // open sql connection
                sqlConnection2.Open();

                // delete product from database
                sqlCommand.ExecuteNonQuery();

                // close sql connectino
                sqlConnection2.Close();
            }
        }

        public void AddCompany(Company company)
        {
            // establish sq connection
            using(SqlConnection sqlConnection = new SqlConnection(CS))
            {
                // query
                string query = "INSERT INTO Company"
                    + $" VALUES ('{company.Name}', '{company.Address}',"
                    + $" '{company.HrefAddress}', '{company.MissionStatment}');";
                SqlCommand sqlCommand = new SqlCommand(query, sqlConnection);

                // open sql connection
                sqlConnection.Open();

                // add company to database
                sqlCommand.ExecuteNonQuery();

                // close sql connection
                sqlConnection.Close();
            }
        }

        public string GetCompanyName(int companyID)
        {
            string companyName = "";
            // establish sql connection
            using(SqlConnection sqlConnection = new SqlConnection(CS))
            {
                // query
                string query = "SELECT Name FROM Company" +
                    $" WHERE ID = {companyID};";
                SqlCommand sqlCommand = new SqlCommand(query, sqlConnection);

                // open sql connection
                sqlConnection.Open();

                companyName += sqlCommand.ExecuteScalar();

                // close sql connection
                sqlConnection.Close();
            }

            return companyName;
        }

        public void AddProduct(Product product)
        {
            // Insert Product Info to Database
            using (SqlConnection sqlConnection = new SqlConnection(CS))
            {
                int discountBool = -1;
                if(product.DiscountBool == false)
                {
                    discountBool = 0;
                } else
                {
                    discountBool = 1;
                }

                int flag = -1;
                if(product.Flag == false)
                {
                    flag = 0;
                } else
                {
                    flag = 1;
                }

                string nameOfProduct = CleanUpApostrophe(product.Name);
                string description = CleanUpApostrophe(product.Description);
                // query
                string query = "INSERT INTO Product(CompanyID, Name, Price, Description,"
                    + " DiscountBool, DiscountPercentage, Flag)"
                    + $" VALUES({product.ReferenceID}, '{nameOfProduct}', {product.Price}, '{description}',"
                    + $" {discountBool}, {product.DiscountPercentage}, {flag});";
                SqlCommand sqlCommand = new SqlCommand(query, sqlConnection);
                // open sql connection
                sqlConnection.Open();

                sqlCommand.ExecuteNonQuery();

                // close sql connection
                sqlConnection.Close();
            }

            // set product id
            product.ID = GetLastProductID();

            // Add Product Images to DB
            AddProductImages(product);

            // Insert Product Images to Folders
            FolderAndDirectory folderAndDirectory = new FolderAndDirectory(WebHostEnvironment);
            folderAndDirectory.InsertThumbnailImageToFolder(product);
            folderAndDirectory.InsertCarouselImagesToFolder(product);

        }

        public int GetLastProductID()
        {
            int id = -1;
            // establish sql connection
            using(SqlConnection sqlConnection = new SqlConnection(CS))
            {
                // query
                string query = "SELECT TOP 1 ID FROM Product ORDER BY ID DESC;";
                SqlCommand sqlCommand = new SqlCommand(query, sqlConnection);

                // open sql connection
                sqlConnection.Open();

                id = (int)sqlCommand.ExecuteScalar();

                // close sql connection
                sqlConnection.Close();
            }

            return id;
        }

        public void AddProductImages(Product product)
        {
            // insert product images to database
            using(SqlConnection sqlConnection = new SqlConnection(CS))
            {
                // open sql connection
                sqlConnection.Open();

                // get compnay name
                string companyName = GetCompanyName(product.ReferenceID);
                string query;
                SqlCommand sqlCommand;
                if (product.UploadThumbnail != null)
                {
                    /*
                     * THUMBNAIL IMAGE
                     */
                    query = "INSERT INTO Image(CompanyID, ProductID, Thumbnail, ImagePath)"
                            + $" VALUES({product.ReferenceID}, {product.ID}, {1}, '/images/{companyName}/ProductThumbnails/{product.UploadThumbnail.FileName}');";
                    sqlCommand = new SqlCommand(query, sqlConnection);
                    sqlCommand.ExecuteNonQuery();
                }

                /*
                 * CAROUSEL IMAGES
                 */
                 try
                {
                    foreach (IFormFile imageFile in product.UploadImageCarousel)
                    {
                        // query
                        query = "INSERT INTO Image(CompanyID, ProductID, Thumbnail, ImagePath)"
                            + $" VALUES({product.ReferenceID}, {product.ID}, {0}, '/images/{companyName}/CarouselImages/{imageFile.FileName}');";
                        sqlCommand = new SqlCommand(query, sqlConnection);
                        sqlCommand.ExecuteNonQuery();
                    }
                } catch(Exception e)
                {
                    // no images to upload
                    Console.WriteLine($"Error: {e}");
                }

                // close sql connection
                sqlConnection.Close();
            }
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

        public List<Note> GetNotesFromCompany(int companyID)
        {
            List<Note> notes = new List<Note>();

            // establish sql connection
            using(SqlConnection sqlConnection = new SqlConnection(CS))
            {
                // query
                string query = "SELECT * FROM Note"
                    + $" WHERE CompanyID = {companyID};";
                SqlCommand sqlCommand = new SqlCommand(query, sqlConnection);

                // open connection
                sqlConnection.Open();

                // get notes from each company
                using(SqlDataReader reader = sqlCommand.ExecuteReader())
                {
                    while(reader.Read())
                    {
                        notes.Add(new Note((int)reader[0], (int)reader[1],
                            (string)reader[2], (string)reader[3], (string)reader[4]));
                    }
                }

                // close connection
                sqlConnection.Close();
            }

            return notes;
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
                List<Note> toDoList = GetNotesFromCompany(companyID);

                SqlCommand sqlCommand = new SqlCommand(query, sqlConnection);

                // open sql connection
                sqlConnection.Open();

                // get company by id from db
                using(SqlDataReader reader = sqlCommand.ExecuteReader())
                {
                    while(reader.Read())
                    {
                        
                        company = new Company(companyID, (string) reader[1], (string) reader[2],
                            (string)reader[3], products, toDoList, (string) reader[4]);
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
                        List<Note> toDoList = GetNotesFromCompany(companyID);

                        Company company = new Company(companyID, (string)reader[1],
                            (string)reader[2], (string)reader[3], products, toDoList, (string)reader[4]);
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

        public List<Company> GetCompaniesBySearch(string search)
        {
            List<Company> companies = new List<Company>();

            // establish sql connection
            using(SqlConnection sqlConnection = new SqlConnection(CS))
            {
                // query
                string query = "SELECT * FROM Company "
                    + $" WHERE Name LIKE '%{search}%';";
                SqlCommand sqlCommand = new SqlCommand(query, sqlConnection);

                sqlConnection.Open();

                // get companies that match the search
                using(SqlDataReader reader = sqlCommand.ExecuteReader())
                {
                    while(reader.Read())
                    {
                        int companyID = (int)reader[0];
                        List<Product> products = GetProductsFromCompany(companyID);
                        List<Note> toDoList = GetNotesFromCompany(companyID);

                        companies.Add(new Company(companyID, (string)reader[1],
                            (string)reader[2], (string)reader[3], products, toDoList, (string)reader[4]));
                    }
                }

                sqlConnection.Close();
            }

            return companies;
        }
    }
}
