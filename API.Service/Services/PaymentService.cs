using API.BO.DTOs.Payment;
using API.BO.Models;
using API.BO.Models.Documents;
using API.Service.Interfaces;
using Payment.Domain.VNPay.Config;
using Payment.Domain.VNPay.Request;
using Payment.Domain.VNPay.Response;
using Payment.Service.Helper;
using Payment.Service.VNPay;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static API.BO.DTOs.Payment.PaymentDTO;

namespace API.Service.Services
{
    public class PaymentService : IPaymentService
    {
        private readonly IVNPayService _vNPayService;
        private readonly IOrderService _orderService;
        private readonly IProductServices _productService;

        public PaymentService(IVNPayService vNPayService, IProductServices productServices, IOrderService orderService)
        {
            if (_vNPayService == null)
                _vNPayService = vNPayService;
            if (_productService == null)
                _productService = productServices;
            if (_orderService == null)
                _orderService = orderService;
        }

        public async Task<PaymentLinkResponse> CreatePayment(Order order)
        {
            VNPayConfig vnPayConfig = VNPayHelper.GetConfigData();

            var orderInfo = await _orderService.GetOrderById(order.OrderId);

            VNPayRequest request = new VNPayRequest()
            {
                vnp_Version = vnPayConfig.Version,
                vnp_TmnCode = vnPayConfig.TmnCode,
                vnp_CreateDate = DateTime.Now.ToString("yyyyMMddHHmmss"),
                vnp_IpAddr = IPAddressHelper.GetLocalIPAddress(),
                vnp_Amount = /*(int)Math.Ceiling(orderInfo.TotalPrice) * 100*/ orderInfo.TotalPrice * 100,
                vnp_CurrCode = vnPayConfig.CurrencyCode,
                vnp_OrderType = "other",
                vnp_OrderInfo = $"Ngày: {DateTime.Now.ToString("yyyyMMddHHmmss")}; Tổng giá: {orderInfo.TotalPrice}",
                vnp_ReturnUrl = vnPayConfig.ReturnUrl,
                vnp_TxnRef = order.OrderId.ToString(),
                vnp_Command = "pay",
                vnp_Locale = vnPayConfig.Locale
            };

            var paymentUrl = await _vNPayService.GetPaymentLink(vnPayConfig.PaymentUrl, vnPayConfig.HashSecret, request);

            return new PaymentLinkResponse
            {
                OrderId = orderInfo.OrderId,
                PaymentUrl = paymentUrl,
            };
        }

        public async Task<PaymentReturnResponse> CheckPaymentResponse(VNPayResponse response)
        {
            VNPayConfig vnPayConfig = VNPayHelper.GetConfigData();
            PaymentReturnResponse paymentResponse = new PaymentReturnResponse();

            paymentResponse.OrderId = response.vnp_TxnRef;
            paymentResponse.Amount = response.vnp_Amount;

            bool isValid = await _vNPayService.IsValidSignature(vnPayConfig.HashSecret, response);
            if (isValid)
            {
                if (await _orderService.GetOrderById(response.vnp_TxnRef) != null)
                {
                    if (response.vnp_ResponseCode == "00")
                    {
                        paymentResponse.PaymentStatus = "Success";
                    }
                    else
                    {
                        paymentResponse.PaymentStatus = "Unsuccess";
                    }

                    switch (response.vnp_ResponseCode)
                    {
                        case "00":
                            paymentResponse.PaymentMessage = "Successful transaction.";
                            break;
                        case "07":
                            paymentResponse.PaymentMessage = "Successful balance deduction. Suspicious transaction (Related to scam, abnormal transaction).";
                            break;
                        case "09":
                            paymentResponse.PaymentMessage = "Card/Banking account is not registered banking services.";
                            break;
                        case "10":
                            paymentResponse.PaymentMessage = "Incorrect Card/Banking Account infomation validation more than 3 times.";
                            break;
                        case "11":
                            paymentResponse.PaymentMessage = "Transaction duration expired. Please redo making transaction.";
                            break;
                        case "12":
                            paymentResponse.PaymentMessage = "Card/Banking Account is currently unavailable (Locked).";
                            break;
                        case "13":
                            paymentResponse.PaymentMessage = "Wrong OTP Code inputed.";
                            break;
                        case "24":
                            paymentResponse.PaymentMessage = "Transaction Canceled.";
                            break;
                        case "51":
                            paymentResponse.PaymentMessage = "Banking Account's balance is not enough for this transaction.";
                            break;
                        case "65":
                            paymentResponse.PaymentMessage = "Bankiing Account exceeds the transaction limitation per day.";
                            break;
                        case "75":
                            paymentResponse.PaymentMessage = "Bank in maintanance.";
                            break;
                        case "79":
                            paymentResponse.PaymentMessage = "Incorrect transaction's password inputed more than specified number of times.";
                            break;
                        case "99":
                            paymentResponse.PaymentMessage = "Other Transaction Error.";
                            break;
                    }
                }
                else
                {
                    paymentResponse.PaymentStatus = "Unsuccess";
                    paymentResponse.PaymentMessage = "Can't find order in DB.";
                }

            }
            else
            {
                paymentResponse.PaymentStatus = "Unsuccess";
                paymentResponse.PaymentMessage = "Invalid signature in response.";
            }

            return paymentResponse;
        }

        //Ultil Function
        //private async Task<PaymentOrderInfoResponse> TakeOrderInfo(string orderId)
        //{
        //    double totalPrice = 0;
        //    List<OrderDetail> orderDetails = await _orderDetailService.GetOrderDetaiListlInOrder(orderId);

        //    if (orderDetails.Count > 0)
        //    {
        //        foreach (OrderDetail orderDetail in orderDetails)
        //        {
        //            var course = await _courseService.GetCourseById(orderDetail.CourseId);
        //            totalPrice += course.Price;
        //        }
        //    }


        //    return new PaymentOrderInfoResponse
        //    {
        //        ItemAmount = orderDetails.Count,
        //        TotalPrice = totalPrice
        //    };
        //}
    }
}
