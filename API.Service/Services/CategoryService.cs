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
        private readonly IProductServices _productServices;
        public CategoryService(ICategoryRepository categoryRepository, IProductRepository productRepository, IProductServices productServices)
        {
            _categoryRepository = categoryRepository;
            _productRepository = productRepository;
            _productServices = productServices;
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
            
            try
            {
				var tryGetAllProductWithThisCategory = await _productServices.GetProductsInCategory(categoryId, 0, 300000);
                if(tryGetAllProductWithThisCategory.Total == 0)
                {
					return  await _categoryRepository.Delete(tryGetCategory);
					//return true;
                }
				_categoryRepository._session.StartTransaction();
                var deleteProductResult = await _productServices.DeleteRange(tryGetAllProductWithThisCategory.Values);
                if(deleteProductResult is false)
                {
                    throw new Exception("fail to delete product in category, so category is not deleted also");
                }
				var deleteResult = await _categoryRepository.Delete(tryGetCategory);
				if (deleteResult is false)
				{
					throw new Exception("fail to delete category, revert all changes ");
				}
				_categoryRepository._session.CommitTransaction();
                return deleteResult;
			}
			catch (Exception ex) 
            {   
                _categoryRepository._session.AbortTransaction();
                throw new Exception(ex.Message,ex);
            }

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
