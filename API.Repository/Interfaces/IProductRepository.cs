using API.BO.Models;
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
        Task Create(Product product);
        Task<bool> Update(Product product);
        Task<bool> Delete(Product product);
    }
}
