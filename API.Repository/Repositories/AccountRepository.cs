using API.BO.DTOs.Account;
using API.BO.Models;
using API.Repository.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace API.Repository.Repositories
{
    public class AccountRepository : BaseRepository<Account>, IAccountRepository
    {
        private readonly IMongoCollection<Account> _accounts;
        public AccountRepository(IOptions<MongoDBContext> setting) : base(setting)
        {
            _accounts = _database.GetCollection<Account>("Account");
        }
        private IAggregateFluent<Account> IncludeOrders(IAggregateFluent<Account> aggregate)
        {
            var ordersCollection = _database.GetCollection<Order>("Order");
            var lookup = aggregate.Lookup<Account, Order, Account>(
                ordersCollection,
                a => a.OrdersId,
                b => b.OrderId,
                a => a.Orders
                );
            return lookup;
        }
        public async Task<List<Account>> GetAll()
        {
            var aggregate = _accounts.Aggregate();
            var included = IncludeOrders(aggregate);
            included = included.Unwind(field => field.Orders, new AggregateUnwindOptions<Account> { PreserveNullAndEmptyArrays = true });
            return await included.ToListAsync();
        }
  
        public async Task<List<Account>> GetByCondition(
            int? skip = null, int? take = null,
            params (Expression<Func<Account, object>> field, object value)[] filters
            )
        {
            var aggregate = _accounts.Aggregate();

            foreach (var filter in filters)
            {
                aggregate = aggregate.Match(Builders<Account>.Filter.Eq(filter.field, filter.value));
            }
            var included = IncludeOrders(aggregate);
            included = included.Unwind(field => field.Orders, new AggregateUnwindOptions<Account> { PreserveNullAndEmptyArrays = true });
            return await included.ToListAsync();
        }
        public async Task<bool> Add(Account entity)
        {
            await _accounts.InsertOneAsync(entity);
            return true;
        }

        public async Task<bool> Delete(Account entity)
        {
            await _accounts.DeleteOneAsync(p => p.AccountId == entity.AccountId);
            return true;
        }

        public async Task<bool> Update(Account entity)
        {
            await _accounts.ReplaceOneAsync(p => p.AccountId == entity.AccountId, entity);
            return true;
        }
    }
}
