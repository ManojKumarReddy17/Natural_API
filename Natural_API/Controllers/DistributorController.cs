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
        public async Task<ActionResult<IEnumerable<DistributorResource>>> GetDistributors()
        {
            var distributors = await _DistributorService.GetAllDistributors();
            var distributorResources = _mapper.Map<IEnumerable<Distributor>, IEnumerable<DistributorResource>>(distributors);
            return Ok(distributorResources);
        }

        // Get Distributor by Id

        [HttpGet("{id}")]

        public async Task<ActionResult<DistributorResource>> GetByIdDistributor(string id)
        {
            var distributor = await _DistributorService.GetDistributorById(id);
            var distributorResource = _mapper.Map<Distributor,DistributorResource>(distributor);
            return Ok(distributorResource);
        }


        [HttpGet("{onlyid}")]

        public async Task<ActionResult<DistributorResource>> GetDistributorByonlId(string id)
        {
            var distributor = await _DistributorService.GetDistributorById(id);
            var distributorResource = _mapper.Map<Distributor, DistributorResource>(distributor);
            return Ok(distributorResource);

        }
        // Create Distributor

        [HttpPost]
        public async Task<ActionResult<DistributorResponse>> InsertDistributorWithAssociations([FromBody] DistributorResource distributorResource)
        {

            var distributor = _mapper.Map<DistributorResource, Distributor>(distributorResource);
            var createDistributorResponse = await _DistributorService.CreateDistributorWithAssociationsAsync(distributor, distributorResource.Area, distributorResource.City, distributorResource.State);
            return StatusCode(createDistributorResponse.StatusCode, createDistributorResponse);
        }

    }
}


