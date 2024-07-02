using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace API.BO.DTOs.Payment
{
    public class PaymentDTO
    {
        public class PaymentLinkResponse
        {
            public string OrderId { get; set; }
            public string PaymentUrl { get; set; } = string.Empty;
        }

        public class PaymentOrderInfoResponse
        {
            public int ItemAmount { get; set; }
            public double TotalPrice { get; set; }
        }

        public class PaymentReturnResponse
        {
            public string OrderId { get; set; }
            public string? PaymentStatus { get; set; }
            public string? PaymentMessage { get; set; }
            public decimal? Amount { get; set; }
        }
    }
}
