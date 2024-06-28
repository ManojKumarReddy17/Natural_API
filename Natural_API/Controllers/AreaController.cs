using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Natural_API.Resources;
using Natural_Core.IServices;
using Natural_Core.Models;
using Natural_Core.S3Models;
using Natural_Services;


#nullable disable

namespace Natural_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AreaController : ControllerBase
    {
        private readonly IAreaService _areaService;
        private readonly IMapper _mapper;
        

        public AreaController(IAreaService areaService, IMapper mapper)
        {
            _areaService = areaService;
            _mapper = mapper;
        }


        /// <summary>
        /// GETTING LIST PF AREAS
        /// </summary>

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Area>>> GetAreasList(string? CityId, int page=1)
        {
            var areas = await _areaService.GetAreasAsync(CityId,page);
            var AreasList = _mapper.Map<Pagination<Area>, Pagination<AreaResource>>(areas);
            
            return Ok(AreasList);
        }

        [HttpGet("areaById/{AreaId}")]
        public async Task<ActionResult<AreaResource>> GetAreasId(String AreaId)
        {
            var areas = await _areaService.GetAreasWithId(AreaId);
            var AreaList = _mapper.Map<Area, AreaResource>(areas);
            return Ok(AreaList);
        }
        [HttpPost]
        public async Task<ActionResult<AreaResource>> Insert([FromBody] AreaResource area)
        {
           var maper=_mapper.Map<AreaResource,Area>(area);
            var categories=await _areaService.Insert(maper);
            

            return StatusCode(categories.StatusCode, categories);
        }
        



        [HttpPut("AreaId/{Id}")]

        public async Task<ActionResult<AreaUpdateResources>> UpdateArea(string Id, [FromBody] AreaUpdateResources areatoupdate)
        {

            var existingarea = await _areaService.GetAreasWithId(Id);
            var Area = _mapper.Map(areatoupdate, existingarea);
            var result = await _areaService.updateArea(Area);

            return StatusCode(result.StatusCode, result);

        }
        [HttpDelete("{AreaId}")]
        public async Task<ActionResult> DeleteArea(String AreaId)
        {
            var response = await _areaService.DeleteArea(AreaId);
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
