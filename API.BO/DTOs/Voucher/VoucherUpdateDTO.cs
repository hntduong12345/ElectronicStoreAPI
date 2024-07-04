using API.BO.Models.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace API.BO.DTOs.Voucher
{
    public class VoucherUpdateDTO
    {
        public decimal TotalPrice { get; set; }

        public int Amount { get; set; }
        public string VoucherCode { get; set; }
        public DateTime ExpiryDate { get; set; }
        public DateTime CreatedDate { get; set; }
        public decimal Percentage { get; set; }
        public decimal MoneyThreshold { get; set; }
        public bool IsAvailable { get; set; }
    }
}
