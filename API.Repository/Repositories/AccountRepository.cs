using API.BO.Models;
using API.Repository.Interfaces;
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
        public async Task<IList<Account>> GetAll()
        {
            return await _accounts.Find(new BsonDocument()).ToListAsync();
        }
  
        public async Task<IList<Account>> GetByCondition(
            Expression<Func<Account, bool>> filter = null,
            Func<IQueryable<Account>,
                IOrderedQueryable<Account>> orderBy = null,
            string[] includeProperties = null, int? skip = null, int? take = null)
        {
            IFindFluent<Account, Account> query;
            if (filter != null) query = _accounts.Find(filter);
            else query = _accounts.Find(new BsonDocument());
            return await query.ToListAsync();
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
