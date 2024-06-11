using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Natural_API.Resources;
using Natural_Core;
using Natural_Core.IServices;
using Natural_Core.Models;
using Natural_Services;
using System.Collections.Generic;
using System.Linq.Expressions;


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
        private readonly ILogger<RetailorController> _logger;


        public RetailorController(IRetailorService retailorservice, IMapper mapper, IAreaService areaservice, ICityService cityService, IDistributorService distributorService, ILogger<RetailorController> logger)
        {
            _mapper = mapper;
            _retailorservice = retailorservice;
            _cityservice = cityService;
            _areaservice = areaservice;
            _DistributorService = distributorService;
            _logger = logger;
        }

        /// <summary>
        /// GETTING LIST OF RETAILORS
        /// </summary>
        
        [HttpGet]
        public async Task<IEnumerable<GetRetailor>> GetAllRetailorDetails([FromQuery] SearchModel? search, bool? nonAssign, string? prefix)
        {
            try
            {
                var retailor = await _retailorservice.GetAllRetailorDetailsAsync(search, nonAssign, prefix);
                return retailor;

            }
            catch(Exception ex)
            {
                _logger.LogError("RetailorController"+"GetAllRetailorDetails"+ex.Message);
                return ( IEnumerable < GetRetailor >) StatusCode(500, "An error occured while processing your request");
            }
            
      
         
        }



        /// <summary>
        /// GETTING RETAILOR DETAILS BY ID
        /// </summary>
        /// 
        [HttpGet("details/{RetailorId}")]

        public async Task<ActionResult<ResultResponse>> GetDetailsById(string RetailorId)
        {
            try
            {
                var retailor = await _retailorservice.GetRetailorDetailsById(RetailorId);

                return Ok(retailor);

            }
            catch (Exception ex)
            {
                _logger.LogError("RetailorController" + "GetDetailsById" + ex.Message);
                return StatusCode(500, "An error occured while processing your request");
            }

        }

        /// <summary>
        /// CREATING NEW RETAILOR
        /// </summary>

        [HttpPost]
        public async Task<ActionResult<ResultResponse>> InsertDistributorWithAssociations([FromForm] RetailorPostResource retailorResource, string? prefix)
        {
            try
            {
                var file = retailorResource.UploadImage;
                var retailor = _mapper.Map<RetailorPostResource, Retailor>(retailorResource);
                if (file != null)
                {
                    var result = await _DistributorService.UploadFileAsync(file, prefix);
                    retailor.Image = result.Message;

                }
                var createretailorResponse = await _retailorservice.CreateRetailorWithAssociationsAsync(retailor);
                return StatusCode(createretailorResponse.StatusCode, createretailorResponse);

            }
            catch (Exception ex)
            {
                _logger.LogError("RetailorController" + "InsertDistributorWithAssociations" + ex.Message);
                return StatusCode(500, "An error occured while processing your request");
            }

        }

        /// <summary>
        /// UPDATING RETAILOR BY ID
        /// </summary>

        [HttpPut]
        public async Task<ActionResult<RetailorPostResource>> UpdateRetailor(string RetailorId, [FromForm] RetailorPostResource updatedRetailorResource, string? prefix)
        {
            try
            {
                var existingRetailor = await _retailorservice.GetRetailorsById(RetailorId);

                var file = updatedRetailorResource.UploadImage;
                var retailorToUpdate = _mapper.Map(updatedRetailorResource, existingRetailor);
                if (file != null && file.Length > 0)
                {
                    var result = await _DistributorService.UploadFileAsync(file, prefix);
                    retailorToUpdate.Image = result.Message;
                }
                var update = await _retailorservice.UpdateRetailors(existingRetailor, retailorToUpdate);
                return StatusCode(update.StatusCode, update);

            }
            catch (Exception ex)
            {
                _logger.LogError("RetailorController" + "UpdateRetailor" + ex.Message);
                return StatusCode(500, "An error occured while processing your request");
            }



        }


        /// <summary>
        /// DELETING RETAILOR BY ID
        /// </summary>
       
        [HttpDelete("{RetailorId}")]
       
       
        public async Task<ActionResult<ResultResponse>> DeleteDistributor(string RetailorId)
        {
            try
            {
                var response = await _retailorservice.SoftDelete(RetailorId);

                return response;

            }
            catch (Exception ex)
            {
                _logger.LogError("RetailorController"+"DeleteDistributor for RetailorId{RetailorId}", ex.Message);
                return StatusCode(500, "An error occured while processing your request");

            }

        }

        [HttpGet("areaId/{areaid}")]
        public async Task<ActionResult<IEnumerable<RetailorDetailsByArea>>> GetRetailorDetailsByAreaId(string areaid)
        {
            var arearetailor = await _retailorservice.GetRetailordetailsByAreaId(areaid);
            return Ok(arearetailor);

        }
    }
}

        

 
 

