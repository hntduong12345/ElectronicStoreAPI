using API.BO.DTOs.Category;
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
        Task<Category> Create(CreateCategoryDto createCategoryDto);
        Task<bool> Update(string categoryId,UpdateCategoryDto updateCategoryDto);
        Task<bool> Delete(string categoryId);

        
    }
}
