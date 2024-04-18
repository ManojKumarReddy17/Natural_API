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
        private readonly IDistributorService _DistributorService;


        public RetailorController(IRetailorService retailorservice, IMapper mapper, IAreaService areaservice,ICityService cityService, IDistributorService distributorService)
        {
            _mapper = mapper;
            _retailorservice = retailorservice;
            _cityservice = cityService;
            _areaservice = areaservice;
            _DistributorService = distributorService;
        }

        /// <summary>
        /// GETTING LIST OF RETAILORS
        /// </summary>
        
        [HttpGet]
        public async Task<IEnumerable<GetRetailor>> GetAllRetailorDetails(string? prefix)
        {
            var retailor = await _retailorservice.GetAllRetailorDetailsAsync(prefix);
            return retailor;
        }



        /// <summary>
        /// GETTING LIST OF NON-ASSIGNED RETAILORS
        /// </summary>

        [HttpGet("Assign")]
        public async Task<ActionResult<IEnumerable<RetailorResource>>> GetNonAssignedRetailors()
        {
            var retailor = await _retailorservice.GetNonAssignedRetailors();
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
            var retailor = await _retailorservice.GetRetailorPresignedUrlbyId(RetailorId);
            return Ok(retailor);
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
        public async Task<ActionResult<ResultResponse>> InsertDistributorWithAssociations([FromForm] RetailorPostResource retailorResource, string? prefix)
        {
            var file = retailorResource.UploadImage;
            var result = await _DistributorService.UploadFileAsync(file, prefix);
            var retailor = _mapper.Map<RetailorPostResource, Retailor>(retailorResource);
            retailor.Image = result.Message;
            var createretailorResponse = await _retailorservice.CreateRetailorWithAssociationsAsync(retailor);
            return StatusCode(createretailorResponse.StatusCode, createretailorResponse);
        }

        /// <summary>
        /// UPDATING RETAILOR BY ID
        /// </summary>

        [HttpPut]
        public async Task<ActionResult<RetailorPostResource>> UpdateRetailor(string RetailorId, [FromForm] RetailorPostResource updatedRetailorResource, string? prefix)
        {

            var existingRetailor = await _retailorservice.GetRetailorsById(RetailorId);

            var file = updatedRetailorResource.UploadImage;
            if (file != null && file.Length > 0)
            {
                var result = await _DistributorService.UploadFileAsync(file, prefix); 
                var mappeddis = _mapper.Map(updatedRetailorResource, existingRetailor);
                mappeddis.Image = result.Message;
                var Updateresponse = await _retailorservice.UpdateRetailors(existingRetailor, mappeddis);
                return StatusCode(Updateresponse.StatusCode, Updateresponse);
            }



            var distributorToUpdate = _mapper.Map(updatedRetailorResource, existingRetailor);
            var update = await _retailorservice.UpdateRetailors(existingRetailor, distributorToUpdate);

            return StatusCode(update.StatusCode, update);

        }


        /// <summary>
        /// DELETING RETAILOR BY ID
        /// </summary>

        [HttpDelete("{RetailorId}")]
       
       
        public async Task<ActionResult<ResultResponse>> DeleteDistributor(string RetailorId)
        {
            var response = await _retailorservice.SoftDelete(RetailorId);

            return response;
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

        [HttpPost("SearchNonAssign")]
        public async Task<IEnumerable<DistributorGetResource>> SearchNonAssignDistributor([FromBody] SearchModel SearchNonAssign)
        {
            var exe = await _retailorservice.SearchNonAssignedDistributors(SearchNonAssign);
            var execget = _mapper.Map<IEnumerable<Distributor>, IEnumerable<DistributorGetResource>>(exe);
            return execget;
        }


    }
}

        

 
 
