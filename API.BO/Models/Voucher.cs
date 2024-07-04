using API.BO.Models.Enum;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace API.BO.Models
{
    [DataContract]
    public class Voucher
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string VoucherId { get; set; }
        public decimal TotalPrice { get; set; }
        [DataMember]

        [BsonRepresentation(BsonType.ObjectId)]
        public string AccountId { get; set; }
        [BsonIgnoreIfNull]
        public Account? Account { get; set; }
        public int Amount { get; set; }
        public string VoucherCode { get; set; }
        public DateTime ExpiryDate { get; set; }
        public DateTime CreatedDate { get; set; }
        public VoucherStatusEnum Status { get; set; }
        public decimal Percentage { get; set; }
        public decimal MoneyThreshold { get; set; }
        public bool IsAvailable { get; set; }
    }
}
