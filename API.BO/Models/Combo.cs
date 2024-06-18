using API.BO.Models.Documents;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace API.BO.Models
{
    public class Combo
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        [BsonElement("_id")]
        public string ComboId { get; set; }
        public string Name { get; set; }
        public List<ComboProducts> Products { get; set; }
        public decimal Price { get; set; }
        public bool IsAvailable {  get; set; }
    }
}
