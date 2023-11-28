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
    public class CityController : ControllerBase
    {
        private readonly ICityService _cityService;
        private readonly IMapper _mapper;
        public CityController(ICityService cityService, IMapper mapper)
        {
            _mapper = mapper;
            _cityService = cityService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<City>>> GetCitiesList()
        {
            var cities = await _cityService.GetCitiesAsync();
            var CitiesList = _mapper.Map<IEnumerable<City>, IEnumerable<CityResource>>(cities);
            return Ok(CitiesList);
        }

        [HttpGet("{StateId}")]

        public async Task<ActionResult<IEnumerable<City>>> GetCitywithStateId(string StateId)
        {
            var city = await _cityService.GetCitywithStateId(StateId);
            var CitiesList = _mapper.Map<IEnumerable<City>, IEnumerable<CityResource>>(city);
            return Ok(CitiesList);
        }

    }
}

