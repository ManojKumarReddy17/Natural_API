using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Natural_API.Resources;
using Natural_Core;
using Natural_Core.IServices;
using Natural_Core.Models;
using Natural_Services;


namespace Natural_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RetailorController : ControllerBase
    {
        private readonly IRetailorService _retailorservice;
        private readonly IMapper _mapper;
        private readonly ICityService _cityservice; 
        private readonly IAreaService _areaservice;


        public RetailorController(IRetailorService retailorservice, IMapper mapper, IAreaService areaservice,ICityService cityService)
        {
            _mapper = mapper;
            _retailorservice = retailorservice;
            _cityservice = cityService;
            _areaservice = areaservice;
        }

        // Get All Retailors

        [HttpGet]
        public async Task<ActionResult<IEnumerable<RetailorResource>>> GetRetailors()
        {
            var retailor = await _retailorservice.GetAllRetailors();
            var retailorResource = _mapper.Map<IEnumerable<Retailor>, IEnumerable<RetailorResource>>(retailor);
            return Ok(retailorResource);
        }

        //Get Retailors Details

        [HttpGet("{id}")]
        public async Task<ActionResult<RetailorResponce>> GetByIdRetailor(string id)
        {
            var retailor = await _retailorservice.GetRetailorsById(id);
            var retailorResource = _mapper.Map<Retailor, RetailorResource>(retailor);
            return Ok(retailorResource);
        }
        // Get Retailor by Id

   

        [HttpGet("details/{id}")]
        public async Task<ActionResult<RetailorResponce>> GetDetailsById(string id)
        {
            var retailor = await _retailorservice.GetRetailorDetailsById(id);
            var ret = _mapper.Map<Retailor, RetailorResource>(retailor);
            return Ok(ret);
        }

        // Create Retailor

        [HttpPost]
        public async Task<ActionResult<RetailorResponce>> InsertRetailorWithAssociations([FromBody] RetailorPostResource retailorResource)
        
        {

            var retailor = _mapper.Map<RetailorPostResource, Retailor>(retailorResource);
            var createretailorResponse = await _retailorservice.CreateRetailorWithAssociationsAsync(retailor);
            return StatusCode(createretailorResponse.StatusCode, createretailorResponse);
        }

    
        [HttpPut("{id}")]
        public async Task<ActionResult<RetailorPostResource>> UpdateRetailor(string id, [FromBody] RetailorPostResource updatedRetailorResource)
        {


            var existingRetailor = await _retailorservice.GetRetailorsById(id);
            var retailor = _mapper.Map<RetailorPostResource, Retailor>(updatedRetailorResource);
            var result = await _retailorservice.UpdateRetailors(existingRetailor, retailor);
            return StatusCode(result.StatusCode, result);


        }
        [HttpDelete("{retailorId}")]
        public async Task<ActionResult<RetailorResponce>> DeleteRetailor(string retailorId)
        {

            var response = await _retailorservice.DeleteRetailor(retailorId);
            return Ok(response);
        }
    }
}
        

 
 
