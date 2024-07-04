using API.BO.Models.Enum;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace API.BO.DTOs.Voucher
{
    public class VoucherCreateDTO
    {
        public string VoucherCode { get; set; }
        public DateTime ExpiryDate { get; set; } = DateTime.Now.AddMonths(1);
        public DateTime CreatedDate { get; set; } = DateTime.Now;
        public decimal Percentage { get; set; }
        public bool IsAvailable { get; set; }
    }
}
