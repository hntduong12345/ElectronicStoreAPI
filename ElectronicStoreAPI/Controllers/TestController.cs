using API.BO.DTOs.Category;
using API.BO.DTOs.Product;
using API.BO.Models;
using API.Repository.Repositories;
using API.Service.Interfaces;
using API.Service.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System.Reflection;

namespace ElectronicStoreAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TestController : ControllerBase
    {
        private readonly IImageModificationService _modificationService;
        private readonly IUploadImageService _uploadImageService;
        private readonly CategoryRepository _categoryRepository ;
        private readonly ProductRepository _productRepository;

        public TestController(IImageModificationService modificationService, IUploadImageService uploadImageService, IOptions<MongoDBContext> setting)
        {
            _modificationService = modificationService;
            _uploadImageService = uploadImageService;
            _categoryRepository = new CategoryRepository(setting);
            _productRepository = new ProductRepository(setting);

        }

        [HttpPost]
        public async Task<ActionResult> TestUpload( IFormFile imageFile, CancellationToken cancellationToken = default)
        {
            //var generatedName = Guid.NewGuid().ToString();
            var fileName = imageFile.FileName;
            var contentType = imageFile.ContentType;
            using var imageStreamFile = imageFile.OpenReadStream();
            var returnStream = await _modificationService.ResizeImage(imageStreamFile, fileName, 300, 300);
            var url = await _uploadImageService.UploadImage(returnStream, fileName, contentType, cancellationToken);
            return Ok();
        }
        [HttpPost("product")]
        public async Task<ActionResult> TestInsertProduct([FromForm]CreateProductDto createProductDto)
        {
            var tryGetCate = _categoryRepository.Get(createProductDto.CategoryId);
            if(tryGetCate == null ) 
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
            return Ok(getAll);
        }
        [HttpGet("product-all")]
        public async Task<ActionResult> TestGetAllProduct()
        {
            var getAll = await _productRepository.GetAll();
            return Ok(getAll);
        }
        [HttpPost("category")]
        public async Task<ActionResult> TestInsertCategory([FromForm]CreateCategoryDto createCategoryDto)
        {
            var newCate = new Category()
            {
                CategoryDescription = createCategoryDto.CategoryDescription,
                CategoryName = createCategoryDto.CategoryName,
            };
            await _categoryRepository.Create(newCate);
            var getAll = await _categoryRepository.GetAll();
            return Ok(getAll);
        }
        [HttpGet("category-all")]
        public async Task<ActionResult> TestGetAllCategory()
        {
            var getAll = await  _categoryRepository.GetAll();
            return Ok(getAll);
        }
     
    }
}
