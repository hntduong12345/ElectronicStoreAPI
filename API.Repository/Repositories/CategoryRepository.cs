using API.BO.Models;
using API.Repository.Interfaces;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace API.Repository.Repositories
{
    public class CategoryRepository : BaseRepository<CategoryRepository> , ICategoryRepository
    {
        private IMongoCollection<Category> _category;
        public IClientSessionHandle _session { get; set; }
        public CategoryRepository(IOptions<MongoDBContext> setting) : base(setting)
        {
            _category = _database.GetCollection<Category>("Category");
            _session = _client.StartSession();
        }
        public async Task<IList<Category>> GetAll()
        {
            return await _category.Find(c => true).ToListAsync() ;
        }
        public async Task<Category?> Get(string id)
        {
            //_category.Aggregate().Lookup<Category,Category>(null,null,null,null);
            return await _category.Find(c => c.CategoryId == id).FirstOrDefaultAsync();
        }
        public async Task<IList<Category>>? GetRange(int start, int take)
        {
            return await _category.Find(c => true).Skip(start).Limit(take).ToListAsync();
        }
        public Task Create(Category category)
        {
            _category.InsertOne(category);
            return Task.CompletedTask;
        }
        public Task<bool> Update(Category category) 
        {
            var updateResult = _category.ReplaceOne(c => c.CategoryId.Equals(category.CategoryId), category);
            if (updateResult.IsAcknowledged is false)
                return Task.FromResult(false);
            return Task.FromResult(true);
        }
        public Task<bool> Delete(Category category)
        {
            var deleteResult = _category.DeleteOne(c => c.CategoryId.Equals(category.CategoryId));
            if (deleteResult.IsAcknowledged is false)
                return Task.FromResult(false);
            if (deleteResult.DeletedCount <= 0)
                return Task.FromResult(false);
            return Task.FromResult(true);
        }

        public  Task<IAggregateFluent<Category>> GetAggregatePipeline()
        {
            return Task.FromResult(_category.Aggregate());
        }
    }
}
