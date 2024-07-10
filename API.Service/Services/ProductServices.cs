using Amazon.Runtime.CredentialManagement.Internal;
using API.BO.DTOs;
using API.BO.DTOs.Product;
using API.BO.Models;
using API.Repository.Interfaces;
using API.Service.Interfaces;
using Microsoft.EntityFrameworkCore.Diagnostics;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace API.Service.Services
{
    public class ProductServices : IProductServices
    {
        private readonly IProductRepository _productRepository;
        private readonly ICategoryRepository _categoryRepository;
        private readonly IImageModificationService _imageModificationService;
        private readonly IUploadImageService _uploadImageService;
        private const int IMAGE_WIDTH = 300;
        private const int IMAGE_HEIGHT = 300;


        public ProductServices(IProductRepository productRepository, ICategoryRepository categoryRepository, IImageModificationService imageModificationService, IUploadImageService uploadImageService)
        {
            _productRepository = productRepository;
            _categoryRepository = categoryRepository;
            _imageModificationService = imageModificationService;
            _uploadImageService = uploadImageService;
        }

        public async Task<Product> Create(CreateProductDto createProductDto)
        {
            var tryGetCate = _categoryRepository.Get(createProductDto.CategoryId);
            if (tryGetCate == null)
                throw new Exception("cannot found category with this id");
            var imageName = createProductDto.ImageFile.FileName;
            var imageContentType = createProductDto.ImageFile.ContentType;
            var imageStream = createProductDto.ImageFile.OpenReadStream();
            var returnStream = await _imageModificationService.ResizeImage(imageStream, imageName, IMAGE_WIDTH, IMAGE_HEIGHT);
            var url = await _uploadImageService.UploadImage(returnStream, DateTime.UtcNow.Ticks.ToString(), imageContentType);
            if (url is null)
                throw new Exception("fail to upload image and get its url for create product, try again");
            var newProduct = new Product()
            {
                CategoryId = createProductDto.CategoryId,
                Manufacturer = createProductDto.Manufacturer,
                ProductName = createProductDto.ProductName,
                CurrentPrice = createProductDto.DefaultPrice,
                DefaultPrice = createProductDto.DefaultPrice,
                Description = createProductDto.Description,
                //IsOnSale = createProductDto.IsOnSale,   
                //RelativeUrl = createProductDto.RelativeUrl,
                //SaleAmount = createProductDto.SaleAmount,
                StorageAmount = createProductDto.StorageAmount,
                //SaleEndDate = createProductDto.SaleEndDate,
                RelativeUrl = url,
            };
            await _productRepository.Create(newProduct);
            return await Get(newProduct.ProductId);
        }

        public async Task Delete(string productId)
        {
            var getProduct = await Get(productId);
            if (getProduct is null)
                throw new Exception("cannot found product with the provided id");
            await _productRepository.Delete(getProduct);
        }
        public async Task<bool> DeleteRange(IList<Product> productsTobeDeleted)
        {
            var listImagesToDelete = productsTobeDeleted.Select(p => p.RelativeUrl);
            var deleteResult =  await _productRepository.DeleteRange(productsTobeDeleted);
		    if(deleteResult is false)
            {
                return false;
            }
            foreach(var imagesLinkToDelete in listImagesToDelete)
            {
                Task.Run( () => { _uploadImageService.DeleteImage(imagesLinkToDelete); });
            }
            return true;
        }
        public async Task<Product?> Get(string id)
        {
            return await _productRepository.Get(id);
        }

        public async Task<IList<Product>> GetAll()
        {
            return await _productRepository.GetAll();
        }

        public async Task<PagingResponseDto<Product>> GetProductsInCategory(string categoryId, int start, int take)
        {
            if (start < 0 || take < 0)
                return new PagingResponseDto<Product>(); 
            var pipeline = await _productRepository.GetAggregatePipeline();
            var filter = Builders<Product>.Filter.Eq(p => p.CategoryId,categoryId);
            pipeline = pipeline.Match(filter);

            var countFacet = AggregateFacet.Create("count", PipelineDefinition<Product, AggregateCountResult>.Create(new[]
            {
                PipelineStageDefinitionBuilder.Count<Product>(),
            }))  ;
            var dataFacet = AggregateFacet.Create("values", PipelineDefinition<Product, Product>.Create(new[] {
                PipelineStageDefinitionBuilder.Skip<Product>((long) start),
                PipelineStageDefinitionBuilder.Limit<Product>((long) take),
            }));
            var facetResult = pipeline.Facet(countFacet, dataFacet).ToList();
            var data = facetResult.First().Facets.First(f => f.Name == "values").Output<Product>().ToList();
            var count = facetResult.First().Facets.First(f => f.Name == "count").Output<AggregateCountResult>().FirstOrDefault()?.Count;
            //var result = await pipeline.ToListAsync();
            return new PagingResponseDto<Product>
            {
                Total = count == null ? 0 :(int) count,
                Values = data
            };
        }

        public async Task<PagingResponseDto<Product>> GetRange(int start, int take)
        {
            if (start < 0 || take < 0)
                return new PagingResponseDto<Product>(); ;//new List<Product>();
            var pipeline = await _productRepository.GetAggregatePipeline();
            var countFacet = AggregateFacet.Create("count", PipelineDefinition<Product, AggregateCountResult>.Create(new[]
            {
                PipelineStageDefinitionBuilder.Count<Product>(),
            }));
            var dataFacet = AggregateFacet.Create("values", PipelineDefinition<Product, Product>.Create(new[] {
                PipelineStageDefinitionBuilder.Skip<Product>((long) start),
                PipelineStageDefinitionBuilder.Limit<Product>((long) take),
            }));
            var facetResult = pipeline.Facet(countFacet, dataFacet).ToList();
            var data = facetResult.First().Facets.First(f => f.Name == "values").Output<Product>().ToList();
            var count = facetResult.First().Facets.First(f => f.Name == "count").Output<AggregateCountResult>().FirstOrDefault()?.Count;
            return new PagingResponseDto<Product>
            {
                Total = count == null ? 0 :(int)count,
                Values = data
            };//await _productRepository.GetRange(start, take);  
        }

        public async Task Update(string productId, UpdateProductDto updateProductDto)
        {
            var getProduct = await Get(productId);
            if(getProduct is null)
                throw new Exception("product is not correct");
            var tryGetCate = _categoryRepository.Get(updateProductDto.CategoryId);
            if (tryGetCate == null)
                throw new Exception("category id is not correct");
            string newImageUrl = null;
            if (updateProductDto.ImageFile is not null)
            {
                var imageName = updateProductDto.ImageFile.FileName;
                var imageContentType = updateProductDto.ImageFile.ContentType;
                var imageStream = updateProductDto.ImageFile.OpenReadStream();
                var returnStream = await _imageModificationService.ResizeImage(imageStream, imageName, IMAGE_WIDTH, IMAGE_HEIGHT);
                var url = await _uploadImageService.UploadImage(returnStream, DateTime.UtcNow.Ticks.ToString(), imageContentType);
                if (url is null)
                    throw new Exception("fail to upload image and get its url for create product, try again");
                newImageUrl = url;
                var oldImageUrl = getProduct.RelativeUrl;
                var removeOldImage = await _uploadImageService.DeleteImage(oldImageUrl);

            }
            getProduct.ProductName = updateProductDto.ProductName is null ? getProduct.ProductName : updateProductDto.ProductName;
            getProduct.DefaultPrice = updateProductDto.DefaultPrice;
            getProduct.CurrentPrice = updateProductDto.DefaultPrice ;
            getProduct.Description = updateProductDto.Description is null ? getProduct.Description : updateProductDto.Description;
            getProduct.Manufacturer = updateProductDto.Manufacturer is null ? getProduct.Manufacturer : updateProductDto.Manufacturer;
            getProduct.CategoryId = updateProductDto.CategoryId is null ? getProduct.CategoryId : updateProductDto.CategoryId;
            getProduct.StorageAmount = updateProductDto.StorageAmount;
            //getProduct.IsOnSale = updateProductDto.IsOnSale;
            //getProduct.SaleEndDate = updateProductDto.SaleEndDate is null ? getProduct.SaleEndDate : updateProductDto.SaleEndDate;
            getProduct.RelativeUrl = newImageUrl is null ? getProduct.RelativeUrl : newImageUrl;
            var updateResult = await _productRepository.Update(getProduct);
            if (updateResult is false)
                throw new Exception("server error with update");
        }
        public async Task<bool> SetProductSales(Product product, int newCurrentPrice, DateTime saleEndDate)
        {
            //if(newCurrentPrice <= 0 || newCurrentPrice > product.DefaultPrice)
            //{
            //    return false;
            //}
            //if(saleEndDate <= DateTime.Now) 
            //{
            //    return false;
            //}
            //product.CurrentPrice = newCurrentPrice;
            //product.SaleEndDate = saleEndDate;
            //await _productRepository.Update(product);
            //return true;
            throw new NotImplementedException();
        }
        public async Task<bool> OnBuyProduct(Product product, int amount)
        {
            var currentAmount = product.StorageAmount - amount ;
            if( currentAmount < 0) 
            {
                throw new Exception("cannot buy product, the amount is over limit in storage");
            }
            product.StorageAmount = currentAmount;
            product.SaleAmount = product.SaleAmount + amount ;
            await _productRepository.Update(product);
            return true;
        }
        private void UpdateCurrentPriceOnDefaultPrice(Product currentProduct, decimal updatedPrice)
        {
            var currentPrice = currentProduct.CurrentPrice;
            var defaultPrice = updatedPrice;
            if(currentProduct.IsOnSale == false)
            {
                currentProduct.CurrentPrice = defaultPrice;
            }
            // GET VOUCHER AND DO STUFF TO IT
        }

    }
}
