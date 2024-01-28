using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Natural_API.Resources;
using Natural_Core.IServices;
using Natural_Core.Models;

namespace Natural_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AssignRetailorToDistributorController : ControllerBase
    {
        private readonly IAssignRetailorToDistributorService _retailortodistributorservice;
        private readonly IMapper _mapper;
        public AssignRetailorToDistributorController(IAssignRetailorToDistributorService service, IMapper mapper)
        {
            _retailortodistributorservice = service;
            _mapper = mapper;
        }

        [HttpGet("{distributorId}")]
        public async Task<ActionResult<IEnumerable<GetRTDResource>>> GetRetailorByDistributorIdAsync(string distributorId)
        {
            var retailers = await _retailortodistributorservice.GetRetailorsIdByDistributorId(distributorId);
            var rtdresources = _mapper.Map<IEnumerable<RetailorToDistributor>, IEnumerable<GetRTDResource>>(retailers);
            return Ok(rtdresources);
        }

        [HttpGet("details/{distributorId}")]
        public async Task<ActionResult<IEnumerable<GetRTDResource>>> GetRetailorsDetailsByDistributorId(string distributorId)
        {
            var retailers = await _retailortodistributorservice.GetRetailorsDetailsByDistributorId(distributorId);
            var rtdresources = _mapper.Map<IEnumerable<RetailorToDistributor>, IEnumerable<GetRTDResource>>(retailers);
            return Ok(rtdresources);
        }

        [HttpPost]
        public async Task<ActionResult<ResultResponse>> AssignRetailorToDistributor([FromBody] AssignRetailorToDistributorResource retailortodistributorResources)
        {
            var AssignedResult = new ResultResponse();
            foreach (var retailor in retailortodistributorResources.RetailorIds)
            {
                var DistributorId = retailortodistributorResources.DistributorId;
                var lst = new InsertRTDResource();
                lst.DistributorId = DistributorId;
                lst.RetailorId = retailor;
                var mappedResult = _mapper.Map<InsertRTDResource, RetailorToDistributor>(lst);
                AssignedResult = await _retailortodistributorservice.AssignRetailorsToDistributor(mappedResult);
            }
            return StatusCode(AssignedResult.StatusCode, AssignedResult);
        }
    }
}
