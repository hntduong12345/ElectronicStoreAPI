using API.BO.Models.Documents;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using API.BO.Models.Enum;

namespace API.BO.Models
{
    public class Account
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string AccountId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string Address { get; set; }
        public string PhoneNumber { get; set; }
        public AccountStatusEnum Status { get; set; }
        public AccountRoleEnum Role { get; set; }
        [BsonRepresentation(BsonType.ObjectId)]
        public List<string?> OrdersId { get; set; }
        [BsonIgnoreIfNull]
        public List<Order?> Orders { get; set; }
    }
}
