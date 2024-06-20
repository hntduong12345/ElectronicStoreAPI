using API.BO.Models;
using API.Repository.Interfaces;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace API.Repository.Repositories
{
    public class VoucherRepository : BaseRepository<Voucher>, IVoucherRepository
    {
        private readonly IMongoCollection<Voucher> _vouchers;
        public VoucherRepository(IOptions<MongoDBContext> setting) : base(setting)
        {
            _vouchers = _database.GetCollection<Voucher>("Voucher");
        }
        private IAggregateFluent<Voucher> IncludeAccounts(IAggregateFluent<Voucher> aggregate)
        {
            var accountCollection = _database.GetCollection<Account>("Account");
            var lookup = aggregate.Lookup<Voucher, Account, Voucher>(
                accountCollection,
                a => a.AccountId,
                b => b.AccountId,
                a => a.Account
                );

            return lookup;
        }
        public async Task<List<Voucher>> GetAll()
        {
            var aggregate = _vouchers.Aggregate();
            var included = IncludeAccounts(aggregate);
            included = included.Unwind(field => field.Account, new AggregateUnwindOptions<Voucher> { PreserveNullAndEmptyArrays = true });
            return await included.ToListAsync();
        }

        public async Task<List<Voucher>> GetByCondition(
            int? skip = null, int? take = null,
            params (Expression<Func<Voucher, object>> field, object value)[] filters
            )
        {
            var aggregate = _vouchers.Aggregate();

            foreach (var filter in filters)
            {
                aggregate = aggregate.Match(Builders<Voucher>.Filter.Eq(filter.field, filter.value));
            }
            var included = IncludeAccounts(aggregate);
            included = included.Unwind(field => field.Account, new AggregateUnwindOptions<Voucher> { PreserveNullAndEmptyArrays = true });
            return await included.ToListAsync();
        }

        public async Task<bool> Add(Voucher entity)
        {
            try
            {
                await _vouchers.InsertOneAsync(entity);
                return true;
            }
            catch
            {
                return false;
            }
        }
        public async Task<bool> Remove(Voucher entity)
        {
            try
            {
                await _vouchers.DeleteOneAsync(p => p.VoucherId == entity.VoucherId);
                return true;
            }
            catch
            {
                return false;
            }
        }
        public async Task<bool> Update(Voucher entity)
        {
            try
            {
                await _vouchers.ReplaceOneAsync(p => p.VoucherId == entity.VoucherId,entity);
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}
