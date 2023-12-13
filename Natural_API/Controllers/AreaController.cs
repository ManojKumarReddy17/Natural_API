using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Natural_API.Resources;
using Natural_Core.IServices;
using Natural_Core.Models;


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
        public async Task<ActionResult<IEnumerable<Area>>> GetAreasList()
        {
            var areas = await _areaService.GetAreasAsync();
            var AreasList = _mapper.Map<IEnumerable<Area>, IEnumerable<AreaResource>>(areas);
            return Ok(AreasList);
        }

        /// <summary>
        /// GETTING AREAS BY CITY ID
        /// </summary>
       
        [HttpGet("{CityId}")]

        public async Task<ActionResult<IEnumerable<City>>> GetAreaswithCityId(string CityId)
        {
            var areas = await _areaService.GetAreasWithCityID(CityId);
            var AreaList = _mapper.Map<IEnumerable<Area>, IEnumerable<AreaResource>>(areas);
            return Ok(AreaList);
        }

    }
}
