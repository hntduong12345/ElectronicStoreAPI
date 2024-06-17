using API.BO.Models;
using API.Repository.Interfaces;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace API.Repository.Repositories
{
    public class ProductRepository : BaseRepository<ProductRepository>, IProductRepository
    {
        private readonly IMongoCollection<Product> _productsCollection;
        public ProductRepository(IOptions<MongoDBContext> setting) : base(setting)
        {
            _productsCollection = _database.GetCollection<Product>("Product");
        }
        public IList<Product> GetAll()
        {
            return _productsCollection.Find(c => true).ToList();
        }
        public void Create(Product product)
        {
            _productsCollection.InsertOne(product);
        }
        public bool Update(Product product)
        {
            var updateResult = _productsCollection.ReplaceOne(c => c.CategoryId.Equals(product.CategoryId), product);
            if (updateResult.IsAcknowledged is false)
                return false;
            return true;
        }
        public bool Delete(Product category)
        {
            var deleteResult = _productsCollection.DeleteOne(c => c.CategoryId.Equals(category.CategoryId));
            if (deleteResult.IsAcknowledged is false)
                return false;
            if (deleteResult.DeletedCount <= 0)
                return false;
            return true;
        }
    }
}
