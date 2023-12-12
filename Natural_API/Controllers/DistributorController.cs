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

        // Get All Distributors

        [HttpGet]
        public async Task<ActionResult<IEnumerable<DistributorGetResource>>> GetAllDistributorDetails()
        
        {
            var distributors = await _DistributorService.GetAllDistributors();
            var distributorResources = _mapper.Map<IEnumerable<Distributor>, IEnumerable<DistributorGetResource>>(distributors);
            return Ok(distributorResources);
        }

        // Get Distributor by Id

        [HttpGet("{id}")]
        

        public async Task<ActionResult<DistributorGetResource>> GetDistributorById(string id)
        {
            var distributor = await _DistributorService.GetDistributorById(id);
            var distributorResource = _mapper.Map<Distributor, DistributorGetResource>(distributor);
            return Ok(distributorResource);
        }


        // Get Distributor Details by Id

        [HttpGet("Details/{id}")]

        public async Task<ActionResult<DistributorGetResource>> GetDistributorDetailsById(string id)
        {
            var distributor = await _DistributorService.GetDistributorDetailsById(id);
            var distributorResource = _mapper.Map<Distributor, DistributorGetResource>(distributor);
            return Ok(distributorResource);
        }

      

        // Create Distributor

        [HttpPost]
        public async Task<ActionResult<DistributorResponse>> InsertDistributorWithAssociations([FromBody] InsertUpdateResource distributorResource)
        {

            var distributor = _mapper.Map<InsertUpdateResource, Distributor>(distributorResource);

            var createDistributorResponse = await _DistributorService.CreateDistributorWithAssociationsAsync(distributor);
            return StatusCode(createDistributorResponse.StatusCode, createDistributorResponse);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<InsertUpdateResource>> UpdateDistributor(string id, [FromBody] InsertUpdateResource updatedistributor)
        {

            var ExistingDistributor = await _DistributorService.GetDistributorById(id);
            var distributorToUpdate = _mapper.Map(updatedistributor, ExistingDistributor);
            var update=  await _DistributorService.UpdateDistributor(distributorToUpdate);
            return StatusCode(update.StatusCode, update);
            
        }

        // Delete Distributor


        [HttpDelete("{distributorId}")]

        public async Task<ActionResult<DistributorResponse>> DeleteDistributor(string distributorId)
        {            
            var response = await _DistributorService.DeleteDistributor(distributorId);           

            return Ok(response);
        }
    }
}





