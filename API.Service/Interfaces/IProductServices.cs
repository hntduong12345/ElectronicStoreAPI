﻿using API.BO.DTOs;
using API.BO.DTOs.Combo;
using API.BO.DTOs.Product;
using API.BO.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace API.Service.Interfaces
{
    public interface IProductServices
    {
        Task<IList<Product>> GetAll();
        Task<PagingResponseDto<Product>> GetRange(int start, int take);
        Task<Product?> Get(string id);
        Task<PagingResponseDto<Product>> GetProductsInCategory(string categoryId, int start, int take);
        Task<Product> Create(CreateProductDto createCategoryDto);
        Task Update(string categoryId, UpdateProductDto updateCategoryDto);
		Task Update(Product productToUpdate);
		Task Delete(string categoryId);
        Task<bool> DeleteRange(IList<Product> productsTobeDeleted);

		Task<bool> SetProductSales(Product product, int newCurrentPrice, DateTime saleEndDate);
        Task BuyProduct(Product product, int amount);
		Task CancelProduct(Product product, int amount);


	}
}
