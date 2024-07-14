using Amazon.Runtime.Internal;
using API.BO.DTOs;
using API.BO.DTOs.Product;
using API.BO.Models;
using API.Repository.Repositories;
using API.Service.Interfaces;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;

namespace ElectronicStoreAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {

        private readonly IImageModificationService _modificationService;
        private readonly IUploadImageService _uploadImageService;
        private readonly IProductServices _productServices;
        private readonly ICategoryServices _categoryServices;
        private readonly IMapper _mapper;

        public ProductController(IImageModificationService modificationService, IUploadImageService uploadImageService, IProductServices productServices, ICategoryServices categoryServices, IMapper mapper)
        {
            _modificationService = modificationService;
            _uploadImageService = uploadImageService;
            _productServices = productServices;
            _categoryServices = categoryServices;
            _mapper = mapper;
            //var getAllResult = client.GetDatabase("ElectronicStore").GetCollection<Product>("Product").Find(filter => true).ToList();
        }
        [HttpGet("all")]
        public async Task<ActionResult> GetAll()
        {
            var getAll = await _productServices.GetAll();
            var result = _mapper.Map<IList<ProductDto>>(getAll);
            return Ok(result);
        }
        [HttpGet("{productId}")]
        public async Task<ActionResult> Get([FromRoute] string productId)
        {
            var getResult = await _productServices.Get(productId);
            if (getResult is null)
                return BadRequest("cannot found");
            var mappedResult = _mapper.Map<ProductDto>(getResult);
            return Ok(mappedResult);
        }
        [HttpGet("range")]
        public async Task<ActionResult> GetRange(
            [FromQuery] int start, 
            [FromQuery] int? pageSize = 10)
        {
            if (start < 0 || pageSize < 0)
                return BadRequest("start and take must > 0");
            var take = start * pageSize.Value;
            var getResult = await _productServices.GetRange(start, take);
            var mappedResult = _mapper.Map<IList<ProductDto>>(getResult.Values);
            return Ok(new PagingResponseDto<ProductDto>
            {
                Total = getResult.Total,
                Values = mappedResult
            });
        }
        [HttpGet("category")]
        public async Task<ActionResult> GetInCategory(
            [FromQuery] string categoryId,
            [FromQuery] int start,  
            [FromQuery] int? pageSize = 10)
        {
            if (start < 0 || pageSize.Value <= 0)
                return BadRequest("start and take must > 0");
            var trueStart = start * pageSize.Value;
            var getResult = await _productServices.GetProductsInCategory(categoryId, trueStart, pageSize.Value);
            var mappedResult = _mapper.Map<IList<ProductDto>>(getResult.Values);
            return Ok(new PagingResponseDto<ProductDto>
            {
                Total = getResult.Total,
                Values = mappedResult
            }) ;
        }
        [HttpPost]
        public async Task<ActionResult> Create([FromForm] CreateProductDto createProductDto)
        {
            var createdProduct = await _productServices.Create(createProductDto);
            var mappedResult = _mapper.Map<ProductDto>(createdProduct);
            return Ok(mappedResult);
        }
        [HttpPut("{productId}")]
        public async Task<ActionResult> Update([FromRoute] string productId, [FromForm] UpdateProductDto updateProductDto)
        {
            await _productServices.Update(productId, updateProductDto);
            return Ok();
        }
        [HttpPut("Storage/{productId}")]
        public async Task<ActionResult> UpdateInventory([FromRoute] string productId, [FromForm] int StorageAmount)
        {
            if(StorageAmount < 0 || StorageAmount > 100000)
            {
                return BadRequest("Storage amount is invalid, it can be >=0 and less than 100mil ");
            }
            var tryGetProduct = await _productServices.Get(productId);
            if (tryGetProduct == null)
                return BadRequest("not found product to update storage ");
            tryGetProduct.StorageAmount = StorageAmount;
            var updateProductDto = new UpdateProductDto()
            {
                CategoryId = tryGetProduct.CategoryId,
                DefaultPrice = tryGetProduct.DefaultPrice,
                Description = tryGetProduct.Description,
                //IsOnSale = tryGetProduct.IsOnSale,
                //SaleEndDate = tryGetProduct.SaleEndDate,
                ProductName = tryGetProduct.ProductName,
                Manufacturer = tryGetProduct.Manufacturer,
                StorageAmount = tryGetProduct.StorageAmount,
            };
            await _productServices.Update(tryGetProduct.ProductId, updateProductDto);
            return Ok();
        }
		[HttpPut("Sale/{productId}")]
		public async Task<ActionResult> UpdateSale([FromRoute] string productId, [FromBody] UpdateSale updateDto)
		{
			var tryGetProduct = await _productServices.Get(productId);
			if (tryGetProduct == null)
				return BadRequest("not found product to update storage ");
            bool parseResult = DateTime.TryParseExact(updateDto.SaleEndDate,"yyyy-MM-dd HH:mm",null,DateTimeStyles.None, out DateTime dateTimeFromDto);
            if(parseResult is false)
            {
                return BadRequest("fail to parse date tiem from client");
            }
            if(DateTime.Compare(dateTimeFromDto,DateTime.Now) <= 0) 
            {
                return BadRequest("date time not valid");
            }
            if(updateDto.CurrentPrice <= 0 || updateDto.CurrentPrice > tryGetProduct.DefaultPrice)
            {
                return BadRequest("price not valid");
            }
            tryGetProduct.CurrentPrice = updateDto.CurrentPrice;
            tryGetProduct.SaleEndDate = dateTimeFromDto;
            tryGetProduct.IsOnSale = true;

            await _productServices.Update(tryGetProduct);
			//await _productServices.Update(tryGetProduct.ProductId, updateProductDto);
			return Ok();
		}
		[HttpDelete("{productId}")]
        public async Task<ActionResult> Delete([FromRoute] string productId)
        {
            await _productServices.Delete(productId);
            return Ok();
        }
    }
}
