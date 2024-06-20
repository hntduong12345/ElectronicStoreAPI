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
        public OrderController(IOrderService orderService)
        {
            _orderService = orderService;
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
            await _orderService.CreateOrder(order);
            return Ok("Action success");
        }

        [HttpPatch(ApiEndpointConstant.Order.OrderStatusEndpoint)]
        public async Task<IActionResult> ChangeOrderStatus(string id, string status)
        {
            await _orderService.ChangeOrderStatus(id, status);
            return Ok();
        }
    }
}
