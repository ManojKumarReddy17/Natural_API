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

        [HttpPost]

        public async Task<ActionResult<CategoryResponse>> InsertCategories([FromBody]CategoryResource category)
        {
            var mapresult =  _mapper.Map<CategoryResource,Category>(category);
            var categoreis = await _categoryService.CreateCategory(mapresult);
            return StatusCode(categoreis.StatusCode, categoreis);

        }
    }
}