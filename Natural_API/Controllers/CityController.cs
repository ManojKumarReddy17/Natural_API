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
        public async Task<ActionResult<IEnumerable<City>>> GetCitiesList()
        {
            var cities = await _cityService.GetCitiesAsync();
            var CitiesList = _mapper.Map<IEnumerable<City>, IEnumerable<CityResource>>(cities);
            return Ok(CitiesList);
        }

        /// <summary>
        /// GETTING CITIES BY STATE ID
        /// </summary>

        [HttpGet("{StateId}")]

        public async Task<ActionResult<IEnumerable<City>>> GetCitieswithStateId(string StateId)
        {
            var city = await _cityService.GetCitywithStateId(StateId);
            var CitiesList = _mapper.Map<IEnumerable<City>, IEnumerable<CityResource>>(city);
            return Ok(CitiesList);
        }


        [HttpGet("getbyid/{CityId}")]
        public async Task<ActionResult<CityResource>> GetCityId(String CityId)
        {
            var City = await _cityService.GetCityWithId(CityId);
            var CityList = _mapper.Map<City, CityResource>(City);
            return Ok(CityList);
        }
        [HttpDelete]

        public async Task<ActionResult> DeleteCity(string CityId)
        {
            var response = await _cityService.DeleteCity(CityId);

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

