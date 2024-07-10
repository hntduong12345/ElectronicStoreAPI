using API.BO.Models;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace API.Repository.Interfaces
{
    public interface IProductRepository
    {
        Task<IList<Product>> GetAll();
        Task<Product?> Get(string id);
        Task<IList<Product>> GetRange(int start, int take);
        Task<IAggregateFluent<Product>> GetAggregatePipeline();
        Task Create(Product product);
        Task<bool> Update(Product product);
        Task<bool> Delete(Product product);
        Task<bool> DeleteRange(IList<Product> products);
    }
}
