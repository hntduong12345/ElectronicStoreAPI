using API.BO.DTOs.Category;
using API.Repository.Interfaces;
using API.Service.Interfaces;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ElectronicStoreAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoryController : ControllerBase
    {
        private readonly ICategoryServices _categoryServices;
        private readonly IMapper _mapper;

        public CategoryController( ICategoryServices categoryServices, IMapper mapper)
        {
            _categoryServices = categoryServices;
            _mapper = mapper;
        }
        [HttpGet("all")]
        public async Task<ActionResult> GetAll()
        {
            var getResult = await _categoryServices.GetAll();
            return Ok(_mapper.Map<IList<CategoryDto>>(getResult));
        }
        [HttpGet("{categoryId}")]
        public async Task<ActionResult> Get([FromRoute] string categoryId)
        {
            var getResult = await _categoryServices.Get(categoryId);
            if (getResult == null)
                return BadRequest("cannot found categoy");
            return Ok(_mapper.Map<CategoryDto>(getResult));
        }
        [HttpPost]
        public async Task<ActionResult> Create([FromForm] CreateCategoryDto createCategoryDto)
        {
            var createResult = await _categoryServices.Create(createCategoryDto);
            if (createResult == null)
                return BadRequest("canot create");
            return Ok(_mapper.Map<CategoryDto>(createResult));
        }
        [HttpPut("{categoryId}")]
        public async Task<ActionResult> Update(
            [FromRoute] string categoryId,
            [FromForm] UpdateCategoryDto updateCategoryDto)
        {
            var updateresult = await _categoryServices.Update(categoryId, updateCategoryDto);
            if (updateresult is false)
                return BadRequest("cannot update now");
            return Ok();
        }
        [HttpDelete("{categoryId}")]
        public async Task<ActionResult> Delete([FromRoute] string categoryId)
        {
            var deleteResult = await _categoryServices.Delete(categoryId);
            if (deleteResult is false)
                return BadRequest();
            return Ok();
        }



    }
}
