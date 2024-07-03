using API.BO.DTOs.Order;
using API.Service.Interfaces;
using ElectronicStoreAPI.Constants;
using Microsoft.AspNetCore.Mvc;

namespace ElectronicStoreAPI.Controllers
{
    [ApiController]
    public class OrderController : ControllerBase
    {
        private readonly IOrderService _orderService;
        private readonly IPaymentService _paymentService;
        public OrderController(IOrderService orderService, IPaymentService paymentService)
        {
            _orderService = orderService;
            _paymentService = paymentService;
        }

        [HttpGet(ApiEndpointConstant.Order.OrdersByUserEndpoint)]
        public async Task<IActionResult> GetOrdersByUser(string accountId)
        {
            var result = await _orderService.GetOrdersByAccount(accountId);
            return Ok(result);
        }

        [HttpGet(ApiEndpointConstant.Order.OrdersEndpoint)]
        public async Task<IActionResult> GetAllOrders()
        {
            var result = await _orderService.GetAllOrder();
            return Ok(result);
        }

        [HttpGet(ApiEndpointConstant.Order.OrderEndpoint)]
        public async Task<IActionResult> GetOrderById(string id)
        {
            var result = await _orderService.GetOrderById(id);
            return Ok(result);
        }

        [HttpPost(ApiEndpointConstant.Order.OrdersEndpoint)]
        public async Task<IActionResult> CreateOrder(OrderDTO order)
        {
            try
            {
                var createdOrder = await _orderService.CreateOrder(order);

                API.BO.DTOs.Payment.PaymentDTO.PaymentLinkResponse response = await _paymentService.CreatePayment(createdOrder);

                return Ok(response);
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

        [HttpPatch(ApiEndpointConstant.Order.OrderStatusEndpoint)]
        public async Task<IActionResult> ChangeOrderStatus(string id, string status)
        {
            await _orderService.ChangeOrderStatus(id, status);
            return Ok();
        }
    }
}
