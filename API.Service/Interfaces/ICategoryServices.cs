using API.BO.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace API.Service.Interfaces
{
    public interface ICategoryServices
    {
        Task<IList<Category>> GetAll();
        Task<IList<Category>> GetRange(int start, int take);
        Task<Category?> Get(string id);
        Task Create();
        Task<bool> Update();
        Task<bool> Delete();
        Task UpdateStorage();
        Task SetSales();
        
    }
}
