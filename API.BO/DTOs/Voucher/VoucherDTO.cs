using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using API.BO.DTOs.Account;
using API.BO.Models.Enum;

namespace API.BO.DTOs.Voucher
{
    public class VoucherDTO
    {
        public string VoucherId { get; set; }
        public AccountDTO? Account { get; set; }
        public string VoucherCode { get; set; }
        public string ExpiryDate { get; set; }
        public string CreatedDate { get; set; }
        public decimal Percentage { get; set; }
        public bool IsAvailable { get; set; }
    }
}
