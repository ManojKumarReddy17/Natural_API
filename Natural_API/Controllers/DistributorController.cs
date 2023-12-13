using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Natural_API.Resources;
using Natural_Core.IServices;
using Natural_Core.Models;
using Natural_Services;
using System.Net.WebSockets;

#nullable disable

namespace Natural_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DistributorController : ControllerBase
    {

        private readonly IDistributorService _DistributorService;
        private readonly IMapper _mapper;
        public DistributorController(IDistributorService DistributorService, IMapper mapper)
        {
            _DistributorService = DistributorService;
            _mapper = mapper;

        }

        /// <summary>
        ///GETTING LIST OF DISTRIBUTOR DETAILS
        /// </summary>

        [HttpGet]
        public async Task<ActionResult<IEnumerable<DistributorGetResource>>> GetAllDistributorDetails()
        
        {
            var distributors = await _DistributorService.GetAllDistributors();
            var distributorResources = _mapper.Map<IEnumerable<Distributor>, IEnumerable<DistributorGetResource>>(distributors);
            return Ok(distributorResources);
        }

        /// <summary>
        /// GETTING DISTRIBUTOR BY ID
        /// </summary>
      

        [HttpGet("{DistributorId}")]
        

        public async Task<ActionResult<DistributorGetResource>> GetDistributorById(string DistributorId)
        {
            var distributor = await _DistributorService.GetDistributorById(DistributorId);
            var distributorResource = _mapper.Map<Distributor, DistributorGetResource>(distributor);
            return Ok(distributorResource);
        }

        /// <summary>
        ///GETTING DISTRIBUTOR DETAILS BY ID
        /// </summary>
      
        [HttpGet("Details/{DistributorId}")]

        public async Task<ActionResult<DistributorGetResource>> GetDistributorDetailsById(string DistributorId)
        {
            var distributor = await _DistributorService.GetDistributorDetailsById(DistributorId);
            var distributorResource = _mapper.Map<Distributor, DistributorGetResource>(distributor);
            return Ok(distributorResource);
        }   

        /// <summary>
        /// CREATING NEW DISTRIBUTOR
        /// </summary>
       

        [HttpPost]
        public async Task<ActionResult<DistributorResponse>> InsertDistributorWithAssociations([FromBody] InsertUpdateResource distributorResource)
        {

            var distributor = _mapper.Map<InsertUpdateResource, Distributor>(distributorResource);

            var createDistributorResponse = await _DistributorService.CreateDistributorWithAssociationsAsync(distributor);
            return StatusCode(createDistributorResponse.StatusCode, createDistributorResponse);
        }

        /// <summary>
        /// UPDATING DISTRIBUTOR BY ID
        /// </summary>
      
        [HttpPut("{DistributorId}")]
        public async Task<ActionResult<InsertUpdateResource>> UpdateDistributor(string DistributorId, [FromBody] InsertUpdateResource updatedistributor)
        {

            var ExistingDistributor = await _DistributorService.GetDistributorById(DistributorId);
            var distributorToUpdate = _mapper.Map(updatedistributor, ExistingDistributor);
            var update=  await _DistributorService.UpdateDistributor(distributorToUpdate);
            return StatusCode(update.StatusCode, update);
            
        }

      /// <summary>
      /// DELETING DISTRIBUTOR BY ID
      /// </summary>


        [HttpDelete("{DistributorId}")]

        public async Task<ActionResult<DistributorResponse>> DeleteDistributor(string DistributorId)
        {            
            var response = await _DistributorService.DeleteDistributor(DistributorId);           

            return Ok(response);
        }
    }
}





