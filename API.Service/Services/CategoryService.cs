using API.BO.DTOs.Category;
using API.BO.Models;
using API.Repository.Interfaces;
using API.Service.Interfaces;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace API.Service.Services
{
    public class CategoryService : ICategoryServices
    {
        private readonly ICategoryRepository _categoryRepository;
        private readonly IProductRepository _productRepository;

        public CategoryService(ICategoryRepository categoryRepository, IProductRepository productRepository)
        {
            _categoryRepository = categoryRepository;
            _productRepository = productRepository;
        }

        public async Task<Category> Create(CreateCategoryDto createCategoryDto)
        {
            var newName = createCategoryDto.CategoryName;
            var pipeline = await _categoryRepository.GetAggregatePipeline();
            var filter = Builders<Category>.Filter.Eq(c => c.CategoryName,newName);
            var result = pipeline.Match(filter).FirstOrDefault();
            if (result is not null)
                throw new Exception("the category name exist before");
            var newCategory = new Category()
            {
                CategoryName = createCategoryDto.CategoryName,
                CategoryDescription =createCategoryDto.CategoryDescription,
            };
            await _categoryRepository.Create(newCategory);
            return await Get(newCategory.CategoryId);
        }

        public async Task<bool> Delete(string categoryId)
        {
            var tryGetCategory = await Get(categoryId);
            if(tryGetCategory == null) 
                return false;
            var deleteResult = await _categoryRepository.Delete(tryGetCategory);
            return deleteResult;
        }

        public async Task<Category?> Get(string id)
        {
            return await _categoryRepository.Get(id);
        }

        public async Task<IList<Category>> GetAll()
        {
            return await _categoryRepository.GetAll();
        }


        public async Task<IList<Category>> GetRange(int start, int take)
        {
            if(start  < 0 || take < 0)
                return new List<Category>();
            return await _categoryRepository.GetRange(start, take); 
        }

        public async Task<bool> Update(string categoryId, UpdateCategoryDto updateCategoryDto)
        {
            var tryGetCategory = await Get(categoryId);
            if (tryGetCategory is null)
                return false;
            tryGetCategory.CategoryDescription = updateCategoryDto.CategoryDescription;
            tryGetCategory.CategoryName = updateCategoryDto.CategoryName;
            var updateResult = await _categoryRepository.Update(tryGetCategory);
            return updateResult;
        }
    }
}
