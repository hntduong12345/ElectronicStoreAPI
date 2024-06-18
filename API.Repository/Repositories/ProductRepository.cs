using API.BO.Models;
using API.Repository.Interfaces;
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
    public class ProductRepository : BaseRepository<ProductRepository>, IProductRepository
    {
        private readonly IMongoCollection<Product> _productsCollection;
        public ProductRepository(IOptions<MongoDBContext> setting) : base(setting)
        {
            _productsCollection = _database.GetCollection<Product>("Product");
        }
        private  IAggregateFluent<Product> IncludeCategory( IAggregateFluent<Product> pipeline)
        {
            var categoryCollection = _database.GetCollection<Category>("Category");
            var lookup = pipeline.Lookup<Product, Category, Product>(categoryCollection,
                p => p.CategoryId,
                c => c.CategoryId,
                result => result.Category);
            return lookup;
        }
        public async Task<IList<Product>> GetAll()
        {
            var productAggregate = _productsCollection.Aggregate();
            productAggregate = IncludeCategory(productAggregate);
            productAggregate = productAggregate.Unwind(field => field.Category, new AggregateUnwindOptions<Product> { PreserveNullAndEmptyArrays = true}) ;
            var result = await  productAggregate.ToListAsync();
            return result;
        }
        public async Task<Product>? Get(string id)
        {
            var productAggregate = _productsCollection.Aggregate();
            productAggregate = productAggregate.Match(p => p.ProductId == id);
            productAggregate = IncludeCategory(productAggregate);
            productAggregate = productAggregate.Unwind(field => field.Category, new AggregateUnwindOptions<Product> { PreserveNullAndEmptyArrays = true });
            var result = await productAggregate.FirstOrDefaultAsync();
            //return _productsCollection.Find(c => c.ProductId == id).FirstOrDefault();
            return result;
        }
        public async Task<IList<Product>> GetRange(int start, int take)
        {
            var pipeline = _productsCollection.Aggregate();
            pipeline = pipeline.Skip(start).Limit(take);
            pipeline = IncludeCategory(pipeline);
            pipeline = pipeline.Unwind(field => field.Category, new AggregateUnwindOptions<Product> { PreserveNullAndEmptyArrays = true });
            var result = await pipeline.ToListAsync();
            //return _productsCollection.Find(c => c.ProductId == id).FirstOrDefault();
            return result;
        }
        public Task Create(Product product)
        {
            _productsCollection.InsertOne(product);
            return Task.CompletedTask;
        }
        public Task<bool> Update(Product product)
        {
            var updateResult = _productsCollection.ReplaceOne(c => c.CategoryId.Equals(product.CategoryId), product);
            if (updateResult.IsAcknowledged is false)
                return Task.FromResult(false);
            return Task.FromResult(true);
        }
        public Task<bool> Delete(Product category)
        {
            var deleteResult = _productsCollection.DeleteOne(c => c.CategoryId.Equals(category.CategoryId));
            if (deleteResult.IsAcknowledged is false)
                return Task.FromResult(false);
            if (deleteResult.DeletedCount <= 0)
                return Task.FromResult(false);
            return Task.FromResult(true);
        }

        public Task<IAggregateFluent<Product>> GetAggregatePipeline()
        {
            return Task.FromResult(_productsCollection.Aggregate());
        }
    }
}
