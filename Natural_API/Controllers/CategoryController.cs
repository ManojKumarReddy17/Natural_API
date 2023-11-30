using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualBasic;
using Natural_API.Resources;
using Natural_Core;
using Natural_Core.IServices;
using Natural_Core.Models;
using Natural_Data.Repositories;
using Natural_Services;

namespace Natural_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoryController : ControllerBase
    {

        private ICategoryService _categoryService;
        private IMapper _mapper;
        public CategoryController(ICategoryService categoryService, IMapper mapper)
        {
            _categoryService = categoryService;
            _mapper = mapper;
        }

        [HttpGet]

        public async Task<ActionResult<IEnumerable<CategoryResource>>> GetCategories()
        {
            var categories = await _categoryService.GetAllCategories();
            var mapped = _mapper.Map<IEnumerable<Category>, IEnumerable<CategoryResource>>(categories);
            return Ok(mapped);
        }

        [HttpGet("{Id}")]

        public async Task<ActionResult<CategoryResource>> GetCategoryById(string Id)
        {

            var categories = await _categoryService.GetCategoryById(Id);
            var categoryResource = _mapper.Map<Category,CategoryResource>(categories);

            return Ok(categoryResource);
        }

        //[HttpPost("{id}")
        //public async Task<ActionResult<IEnumerable<CategoryResponse>>> UpdateCategory(string Id)
        //{
        //    var mapresult = _mapper.Map<CategoryResource, Category>(category);
        //    var response = await _categoryService.UpdateCategory(Id, mapresult);
        //    return StatusCode(response.StatusCode, response);
        //}

        //[HttpPost("{id}")]
        //public async Task<ActionResult<CategoryResponse>> DeleteCategory(string id)
        //{
        //    var response = await _categoryService.DeleteCategory(id);
        //    return StatusCode(response.StatusCode, response);
        //}

    }
}