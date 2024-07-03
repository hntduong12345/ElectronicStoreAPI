using API.BO.DTOs.Payment;
using API.Service.Interfaces;
using ElectronicStoreAPI.Constants;
using Microsoft.AspNetCore.Mvc;
using Payment.Domain.VNPay.Response;

namespace ElectronicStoreAPI.Controllers
{
    public class PaymentController : ControllerBase
    {
        private readonly IPaymentService _paymentService;
        private readonly IOrderService _orderService;

        public PaymentController(IPaymentService paymentService, IOrderService orderService)
        {
            _paymentService = paymentService;
            _orderService = orderService;
        }

        [HttpGet(ApiEndpointConstant.Payment.PyamentReturnEndPoint)]
        public async Task<IActionResult> VnpayReturn([FromQuery] VNPayResponse response)
        {
            try
            {
                var result = await _paymentService.CheckPaymentResponse(response);

                var order = await _orderService.GetOrderById(result.OrderId);
                string accountId = order.AccountId;

                if (result.PaymentStatus.Equals("Success"))
                {
                    await _orderService.ChangeOrderStatus(result.OrderId, "Paid");
                }
                //string returnUrl = $"http://localhost:3000/learner?paymentStatus={result.PaymentStatus}&message={result.PaymentMessage}&amount={result.Amount}";

                PaymentDTO.PaymentReturnResponse paymentReturnResponse = new PaymentDTO.PaymentReturnResponse
                {
                    OrderId = result.OrderId,
                    PaymentStatus = result.PaymentStatus,
                    PaymentMessage = result.PaymentMessage,
                    Amount = result.Amount
                };

                return /*Redirect(returnUrl)*/ Ok(paymentReturnResponse);
            }
            catch (Exception ex)
            {
                if (ex.GetType() == typeof(BadHttpRequestException))
                {
                    return BadRequest(ex.Message);
                }
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }
    }
}
