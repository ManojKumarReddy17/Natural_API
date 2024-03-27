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
        public async Task<ActionResult<CategoryInsertResource>> InsertCategories([FromBody] CategoryInsertResource category)
        {
            var mapresult = _mapper.Map<CategoryInsertResource, Category>(category);
            var categoreis = await _categoryService.CreateCategory(mapresult);
            return StatusCode(categoreis.StatusCode, categoreis);
        }

        /// <summary>
        /// GETTING CATEGORY BY ID
        /// </summary>
       
        [HttpGet("{CategoryId}")]

        public async Task<ActionResult<CategoryResource>> GetCategoryById(string CategoryId)
        {

            var categories = await _categoryService.GetCategoryById(CategoryId);
            var categoryResource = _mapper.Map<Category, CategoryResource>(categories);

            return Ok(categoryResource);
        }

        /// <summary>
        /// UPDATING CATEGORY BY ID
        /// </summary>
       
        [HttpPut("{CategoryId}")]
        public async Task<ActionResult<CategoryInsertResource>> UpdateCategory(string CategoryId, [FromBody] CategoryInsertResource categorytoUpdate)
        {
            var existcategory = await _categoryService.GetCategoryById(CategoryId);
            var category = _mapper.Map(categorytoUpdate,existcategory);
            var result = await _categoryService.UpdateCategory(category);

            return StatusCode(result.StatusCode, result);

        }

        /// <summary>
        /// DELETING CATEGORY BY ID
        /// </summary>

        [HttpDelete("{CategoryId}")]
        public async Task<ActionResult> DeleteCategory(string CategoryId)
        {
            var response = await _categoryService.DeleteCategory(CategoryId);

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

