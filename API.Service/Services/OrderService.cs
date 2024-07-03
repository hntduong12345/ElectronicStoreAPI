using API.BO.DTOs.Order;
using API.BO.Models;
using API.Repository.Interfaces;
using API.Service.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace API.Service.Services
{
    public class OrderService : IOrderService
    {
        private readonly IOrderRepository _orderRepository;

        public OrderService(IOrderRepository orderRepository)
        {
            _orderRepository = orderRepository;
        }
        public async Task ChangeOrderStatus(string id, string status)
        {
            await _orderRepository.ChangeOrderStatus(id, status);
        }

        public async Task<Order> CreateOrder(OrderDTO order)
        {
            return await _orderRepository.CreateOrder(order);
        }

        public async Task<List<Order>> GetAllOrder()
        {
            return await _orderRepository.GetAllOrder();
        }

        public async Task<Order> GetOrderById(string id)
        {
            return await _orderRepository.GetOrderById(id);
        }

        public async Task<List<Order>> GetOrdersByAccount(string accountId)
        {
            return await _orderRepository.GetOrdersByAccount(accountId);
        }
    }
}
