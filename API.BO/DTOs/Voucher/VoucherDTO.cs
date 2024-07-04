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
        public decimal TotalPrice { get; set; }

        public AccountDTO? Account { get; set; }
        public int Amount { get; set; }
        public string VoucherCode { get; set; }
        public DateTime ExpiryDate { get; set; }
        public DateTime CreatedDate { get; set; }
        public decimal Percentage { get; set; }
        public decimal MoneyThreshold { get; set; }
        public bool IsAvailable { get; set; }
    }
}
