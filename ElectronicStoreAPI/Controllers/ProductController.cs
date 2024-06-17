using API.BO.DTOs;
using API.BO.Models;
using API.Repository.Repositories;
using API.Service.Interfaces;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace ElectronicStoreAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {

        private readonly IImageModificationService _modificationService;
        private readonly IUploadImageService _uploadImageService;
        private readonly CategoryRepository _categoryRepository;
        private readonly ProductRepository _productRepository;
        private readonly IMapper _mapper;
        public ProductController(IImageModificationService modificationService, IUploadImageService uploadImageService, IOptions<MongoDBContext> setting, IMapper mapper)
        {
            _modificationService = modificationService;
            _uploadImageService = uploadImageService;
            _mapper = mapper;
            _categoryRepository = new CategoryRepository(setting);
            _productRepository = new ProductRepository(setting);


        }
        [HttpPost]
        public async Task<ActionResult> CreateProduct([FromForm] CreateProductDto createProductDto)
        {
            var tryGetCate = _categoryRepository.Get(createProductDto.CategoryId);
            if (tryGetCate == null)
            {
                return BadRequest();
            }
            var imageName = createProductDto.ImageFile.FileName;
            var imageContentType = createProductDto.ImageFile.ContentType;
            var imageStream = createProductDto.ImageFile.OpenReadStream();
            var returnStream = await _modificationService.ResizeImage(imageStream, imageName, 300, 300);
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
            var getAll = await _productRepository.GetAll();
            var result  = _mapper.Map<IList<ProductDto>>(getAll);
            return Ok(result);
        }
        [HttpGet("all")]
        public async Task<ActionResult> TestGetAllProduct()
        {
            var getAll = await _productRepository.GetAll();
            var result = _mapper.Map<IList<ProductDto>>(getAll);
            return Ok(result);
        }
        [HttpPut("{productId}")]
        public async Task<ActionResult> TestGetAllProduct([FromRoute] string productId, [FromForm] UpdateProductDto updateProductDto)
        {
            var tryGetProduct = await _productRepository.Get(productId);
            if (tryGetProduct == null)
                return BadRequest("No product found");
            var tryGetCate = _categoryRepository.Get(updateProductDto.CategoryId);
            if (tryGetCate == null)
            {
                return BadRequest("no such category");
            }
            string newImageUrl = null;
            if(updateProductDto.ImageFile is not null)
            {
                var imageName = updateProductDto.ImageFile.FileName;
                var imageContentType = updateProductDto.ImageFile.ContentType;
                var imageStream = updateProductDto.ImageFile.OpenReadStream();
                var returnStream = await _modificationService.ResizeImage(imageStream, imageName, 300, 300);
                var url = await _uploadImageService.UploadImage(returnStream, DateTime.UtcNow.Ticks.ToString(), imageContentType);
                if (url is null)
                    throw new Exception("fail to upload image and get its url for create product, try again");
                newImageUrl = url;
                var oldImageUrl = tryGetProduct.RelativeUrl;
                var removeOldImage = await _uploadImageService.DeleteImage(oldImageUrl);

            }
            tryGetProduct.ProductName = updateProductDto.ProductName is null ? tryGetProduct.ProductName : updateProductDto.ProductName;
            tryGetProduct.DefaultPrice = updateProductDto.DefaultPrice ;
            //tryGetProduct.CurrentPrice = ...;
            tryGetProduct.ProductName = updateProductDto.Description is null ? tryGetProduct.Description : updateProductDto.Description ;
            tryGetProduct.ProductName = updateProductDto.Manufacturer is null ? tryGetProduct.Manufacturer : updateProductDto.Manufacturer;
            tryGetProduct.ProductName = updateProductDto.CategoryId is null ? tryGetProduct.CategoryId : updateProductDto.CategoryId;
            tryGetProduct.StorageAmount = updateProductDto.StorageAmount;
            tryGetProduct.IsOnSale = updateProductDto.IsOnSale;
            tryGetProduct.SaleEndDate = updateProductDto.SaleEndDate is null ? tryGetProduct.SaleEndDate : updateProductDto.SaleEndDate;
            tryGetProduct.RelativeUrl = newImageUrl is null ? tryGetProduct.RelativeUrl : newImageUrl;
            var updateResult = await _productRepository.Update(tryGetProduct);
            if (updateResult is false)
                throw new Exception("server error with update");
            var getAll = await _productRepository.GetAll();
            var result = _mapper.Map<IList<ProductDto>>(getAll);
            return Ok(result);
        }
    }
}
