using API.BO.Models;
using Payment.Domain.VNPay.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static API.BO.DTOs.Payment.PaymentDTO;

namespace API.Service.Interfaces
{
    public interface IPaymentService
    {
        public Task<PaymentLinkResponse> CreatePayment(Order order);
        public Task<PaymentReturnResponse> CheckPaymentResponse(VNPayResponse response);
    }
}
