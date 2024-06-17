using API.Repository.Interfaces;
using API.Service.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ElectronicStoreAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoryController : ControllerBase
    {
        private readonly IImageModificationService _modificationService;
        private readonly IUploadImageService _uploadImageService;
        private readonly ICategoryRepository _categoryRepository;
    }
}
