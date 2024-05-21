using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Natural_API.Resources;
using Natural_Core.IServices;
using Natural_Core.Models;
using Natural_Services;

#nullable disable

namespace Natural_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CityController : ControllerBase
    {
        private readonly ICityService _cityService;
        private readonly IMapper _mapper;
        public CityController(ICityService cityService, IMapper mapper)
        {
            _mapper = mapper;
            _cityService = cityService;
        }

        /// <summary>
        /// GETTING LIST OF CITIES
        /// </summary>

        [HttpGet]
        public async Task<ActionResult<IEnumerable<City>>> GetCitiesList(string? StateId)
        {
            var cities = await _cityService.GetCitiesAsync(StateId);
            var CitiesList = _mapper.Map<IEnumerable<City>, IEnumerable<CityResource>>(cities);
            return Ok(CitiesList);
        }

        [HttpPost]
        public async Task<ActionResult<ProductResource>> InsertWithCity(CityResource city)
        {
            var ar = _mapper.Map<CityResource, City>(city);
            var exe = await _cityService.InsertWithCity(ar);

            return StatusCode(exe.StatusCode, exe);
        }
        [HttpGet("getbyid/{CityId}")]
        public async Task<ActionResult<CityResource>> GetCityId(String CityId)
        {
            var cities = await _cityService.GetCityWithId(CityId);
            var CityList = _mapper.Map<City, CityResource>(cities);
            return Ok(CityList);
        }
        [HttpPut("{CityId}")]
        public async Task<ActionResult<CityResource>> UpdateWithCity(String CityId, [FromBody] CityResource citytoupdate)
        {
            var existingCity = await _cityService.GetCityWithId(CityId);
            var city = _mapper.Map(citytoupdate, existingCity);
            var result = await _cityService.UpdateWithCity(city);

            return StatusCode(result.StatusCode, result);
        }
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteCity(string id)
        {
            var response = await _cityService.DeleteCity(id);

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

