﻿using API.BO.DTOs.Order;
using API.BO.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace API.Service.Interfaces
{
    public interface IOrderService
    {
        public Task<List<Order>> GetAllOrder();
        public Task<Order> GetOrderById(string id);
        public Task CreateOrder(OrderDTO order);
        public Task ChangeOrderStatus(string id, string status);
    }
}