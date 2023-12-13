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

        /// <summary>
        /// GETTING LIST OF CATEGORIES
        /// </summary>

        [HttpGet]
        public async Task<ActionResult<IEnumerable<CategoryResource>>> GetAllCategories()
        {
            var categories = await _categoryService.GetAllCategories();
            var mapped = _mapper.Map<IEnumerable<Category>, IEnumerable<CategoryResource>>(categories);
            return Ok(mapped);
        }
        /// <summary>
        /// CREATING NEW CATEGORY
        /// </summary>

        [HttpPost]
        public async Task<ActionResult<CategoryResponse>> InsertCategories([FromBody] CategoryResource category)
        {
            var mapresult = _mapper.Map<CategoryResource, Category>(category);
            var categoreis = await _categoryService.CreateCategory(mapresult);
            return StatusCode(categoreis.StatusCode, categoreis);
        }

        /// <summary>
        /// GETTING CATEGORY BY ID
        /// </summary>
       
        [HttpGet("{catId}")]

        public async Task<ActionResult<CategoryResource>> GetCategoryById(string catId)
        {

            var categories = await _categoryService.GetCategoryById(catId);
            var categoryResource = _mapper.Map<Category, CategoryResource>(categories);

            return Ok(categoryResource);
        }

        /// <summary>
        /// UPDATING CATEGORY BY ID
        /// </summary>
       
        [HttpPut("{catId}")]
        public async Task<ActionResult<CategoryInsertResource>> UpdateCategory(string catId, [FromBody] CategoryInsertResource categorytoUpdate)
        {
            var existcategory = await _categoryService.GetCategoryById(catId);
            var category = _mapper.Map(categorytoUpdate,existcategory);
            var result = await _categoryService.UpdateCategory(category);

            return StatusCode(result.StatusCode, result);

        }

        /// <summary>
        /// DELETING CATEGORY BY ID
        /// </summary>
       
        [HttpDelete("{catId}")]
        public async Task<ActionResult> DeleteCategory(string catId)
        {
            var response = await _categoryService.DeleteCategory(catId);

            if (response.StatusCode == 200)
            {
                return Ok(response);
            }
            else
            {
                return BadRequest(response);
            }
        }
    }
}

