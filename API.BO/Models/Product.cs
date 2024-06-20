using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using MongoDB.Driver;
using MongoDB.Libmongocrypt;
namespace API.BO.Models
{
    [BsonIgnoreExtraElements]
    public class Product
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string ProductId { get; set; }
        public string ProductName { get; set; }
        public string? Description { get; set; }
        public decimal DefaultPrice { get; set; }
        [BsonRepresentation(BsonType.ObjectId)]
        public string CategoryId { get; set; }
        [BsonIgnoreIfNull]
        public Category? Category { get; set; }
        public string Manufacturer { get; set; }
        public int StorageAmount { get; set; }
        public int SaleAmount { get; set; } = 0;
        public decimal CurrentPrice { get; set; }
        public bool IsOnSale { get; set; } = false;
        public DateTime? SaleEndDate { get; set; }
        public string? RelativeUrl { get; set; }
    }
}
