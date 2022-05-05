using System;
namespace Shop.Models
{
    public class Note
    {
        public int ID { get; set; }
        public int CompanyID { get; set; }
        public string Date { get; set; }
        public string Description { get; set; }
        public string Status { get; set; }

        public Note() { }

        public Note(int id, int companyID, string date, string des, string status)
        {
            ID = id;
            CompanyID = companyID;
            Date = date;
            Description = des;
            Status = status;
        }
    }
}
