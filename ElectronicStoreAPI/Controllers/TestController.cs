using API.Service.Interfaces;
using API.Service.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Reflection;

namespace ElectronicStoreAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TestController : ControllerBase
    {
        private readonly IImageModificationService _modificationService;
        private readonly IUploadImageService _uploadImageService;

        public TestController(IImageModificationService modificationService, IUploadImageService uploadImageService)
        {
            _modificationService = modificationService;
            _uploadImageService = uploadImageService;
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
    }
}
