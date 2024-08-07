﻿using API.BO.DTOs.Enum;
using API.BO.DTOs.Order;
using API.BO.Models;
using API.Repository.Interfaces;
using API.Repository.Utils;
using Microsoft.Extensions.Options;
using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace API.Repository.Repositories
{
    public class OrderRepository : BaseRepository<Order>, IOrderRepository
    {
        private readonly IMongoCollection<Order> _orders;

        public OrderRepository(IOptions<MongoDBContext> setting, IMongoClient client) : base(setting, client)
        {
            _orders = _database.GetCollection<Order>("Order");
        }

        public async Task ChangeOrderStatus(string id, string status)
        {
            var order = _orders.Find(o => o.OrderId == id).FirstOrDefaultAsync();
            order.Result.Status = EnumUtils.ParseEnum<OrderStatusEnum>(status).ToString();
            await _orders.ReplaceOneAsync(o => o.OrderId == id, order.Result);
        }

        public async Task<Order> CreateOrder(OrderDTO order)
        {
            Order newOrder = new Order
            {
                TotalPrice = order.TotalPrice,
                AccountId = order.AccountId,
                OrderDetails = order.OrderDetails,
                TruePrice = order.TruePrice,
                Status = OrderStatusEnum.Pending.ToString()
            };
            await _orders.InsertOneAsync(newOrder);
            return newOrder;
        }

        public Task<List<Order>> GetAllOrder()
        {
            return _orders.Find(new BsonDocument()).ToListAsync();
        }

        public Task<Order> GetOrderById(string id)
        {
            return _orders.Find(o => o.OrderId == id).FirstOrDefaultAsync();
        }

        public async Task<List<Order>> GetOrdersByAccount(string accountId)
        {
            List<Order> orders = await _orders.Find(o => o.AccountId == accountId).ToListAsync();
            return orders;
        }
    }
}
