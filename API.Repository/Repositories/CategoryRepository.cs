using API.BO.Models;
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
    public class CategoryRepository : BaseRepository<CategoryRepository>
    {
        private IMongoCollection<Category> _category;
        public CategoryRepository(IOptions<MongoDBContext> setting) : base(setting)
        {
            _category = _database.GetCollection<Category>("Category");
        }
        public IList<Category> GetAll()
        {
            return _category.Find(c => true).ToList() ;
        }
        public Category? Get(string id)
        {
            _category.Aggregate().Lookup<Category,Category>(null,null,null,null);
            return _category.Find(c => c.CategoryId == id).FirstOrDefault();
        }
        public IList<Category>? GetRange(int start, int take)
        {
            return _category.Find(c => true).Skip(start).Limit(take).ToList();
        }
        public IList<Category>? GetCondition(Func<Category,bool> filter, SortDefinition<Category> sort)
        {
            return null;
        }
        public void Create(Category category)
        {
            _category.InsertOne(category);
        }
        public bool Update(Category category) 
        {
            var updateResult = _category.ReplaceOne(c => c.CategoryId.Equals(category.CategoryId), category);
            if (updateResult.IsAcknowledged is false)
                return false;
            return true;
        }
        public bool Delete(Category category)
        {
            var deleteResult = _category.DeleteOne(c => c.CategoryId.Equals(category.CategoryId));
            if (deleteResult.IsAcknowledged is false)
                return false;
            if (deleteResult.DeletedCount <= 0)
                return false;
            return true;
        }

    }
}
