using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace API.BO.Models
{
    public class Voucher
    {
        public int VoucherId { get; set; }
        public decimal TotalPrice { get; set; }
        public int AccountId { get; set; }
        public int Amount { get; set; }
        public string VoucherCode { get; set; }
        public DateTime ExpiryDate { get; set; }
        public DateTime CreatedDate { get; set; }
        public Enum Type { get; set; }
        public decimal Percentage { get; set; }
        public decimal MoneyThreshold { get; set; }
        public bool IsAvailable { get; set; }
    }
}
