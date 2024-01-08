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

        /// <summary>
        /// GETTING LIST OF RETAILORS
        /// </summary>

        [HttpGet]
        public async Task<ActionResult<IEnumerable<RetailorResource>>> GetRetailors()
        {
            var retailor = await _retailorservice.GetAllRetailors();
            var retailorResource = _mapper.Map<IEnumerable<Retailor>, IEnumerable<RetailorResource>>(retailor);
            return Ok(retailorResource);
        }

        /// <summary>
        /// GETTING RETAILOR BY ID
        /// </summary>
        /// 
        [HttpGet("{RetailorId}")]
        public async Task<ActionResult<ResultResponse>> GetByIdRetailor(string RetailorId)
        {
            var retailor = await _retailorservice.GetRetailorsById(RetailorId);
            var retailorResource = _mapper.Map<Retailor, RetailorResource>(retailor);
            return Ok(retailorResource);
        }

        /// <summary>
        /// GETTING RETAILOR DETAILS BY ID
        /// </summary>
        /// 
        [HttpGet("details/{RetailorId}")]

        public async Task<ActionResult<ResultResponse>> GetDetailsById(string RetailorId)
        {
            var retailor = await _retailorservice.GetRetailorDetailsById(RetailorId);
            var ret = _mapper.Map<Retailor, RetailorResource>(retailor);
            return Ok(ret);
        }

        /// <summary>
        /// CREATING NEW RETAILOR
        /// </summary>

        [HttpPost]
        public async Task<ActionResult<ResultResponse>> InsertRetailorWithAssociations([FromBody] RetailorPostResource retailorResource)
        
        {

            var retailor = _mapper.Map<RetailorPostResource, Retailor>(retailorResource);
            var createretailorResponse = await _retailorservice.CreateRetailorWithAssociationsAsync(retailor);
            return StatusCode(createretailorResponse.StatusCode, createretailorResponse);
        }

        /// <summary>
        /// UPDATING RETAILOR BY ID
        /// </summary>
        
        [HttpPut("{RetailorId}")]
        public async Task<ActionResult<RetailorPostResource>> UpdateRetailor(string RetailorId, [FromBody] RetailorPostResource updatedRetailorResource)
        {


            var existingRetailor = await _retailorservice.GetRetailorsById(RetailorId);
            var retailor = _mapper.Map<RetailorPostResource, Retailor>(updatedRetailorResource);
            var result = await _retailorservice.UpdateRetailors(existingRetailor, retailor);
            return StatusCode(result.StatusCode, result);


        }
        /// <summary>
        /// DELETING RETAILOR BY ID
        /// </summary>
        
        [HttpDelete("{RetailorId}")]
        public async Task<ActionResult<ResultResponse>> DeleteRetailor(string RetailorId)
        {

            var response = await _retailorservice.DeleteRetailor(RetailorId);
            return Ok(response);
        }

        /// <summary>
        /// SEARCH RETAILOR 
        /// </summary>

        [HttpPost("Search")]
        public async Task<IEnumerable<RetailorResource>> SearchRetailor([FromBody] SearchModel search)
        {
            var exe = await _retailorservice.SearcRetailors(search);
            var execget = _mapper.Map<IEnumerable<Retailor>, IEnumerable<RetailorResource>>(exe);
            return execget;
        }


    }
}

        

 
 
