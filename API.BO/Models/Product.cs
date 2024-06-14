using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using MongoDB.Driver;
using MongoDB.Bson;
using MongoDB.Libmongocrypt;
using MongoDB.Bson.Serialization.Attributes;
namespace API.BO.Models
{
    public class Product
    {
        [BsonId()]
        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public string Description { get; set; }
        public decimal DefaultPrice { get; set; }
        public int CategoryId { get; set; }
        public string Manufacturer { get; set; }
        public int StorageAmount { get; set; }
        public int SaleAmount { get; set; }
        public decimal CurrentPrice { get; set; }
        public bool IsOnSale { get; set; }
        public DateTime? SaleEndDate { get; set; }
        public string RelativeUrl { get; set; }
    }
}
