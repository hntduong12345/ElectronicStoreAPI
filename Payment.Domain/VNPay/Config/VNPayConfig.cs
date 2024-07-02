using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Payment.Domain.VNPay.Config
{
    public class VNPayConfig
    {
        public string PaymentUrl { get; set; }
        public string ReturnUrl { get; set; }
        public string TmnCode { get; set; }
        public string HashSecret { get; set; }
        public string Version { get; set; }
        public string CurrencyCode { get; set; }
        public string Locale { get; set; }
    }
}
