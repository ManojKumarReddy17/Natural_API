using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Natural_API.Resources;
using Natural_Core.IServices;
using Natural_Core.Models;
using Natural_Services;

namespace Natural_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AssignDistributorToExecutiveController : ControllerBase
    {

        private readonly IAssignDistributorToExecutiveService _distributorToExecutiveService;

        private readonly IMapper _mapper;

        public AssignDistributorToExecutiveController(IAssignDistributorToExecutiveService distributorToExecutiveService, IMapper mapper)
        {
            _distributorToExecutiveService = distributorToExecutiveService;
            _mapper = mapper;
        }

        /// <summary>
        /// GETTING ASSIGNED DISTRIBUTORS BY EXECUTIVE ID
        /// </summary>

        [HttpGet("{ExecutiveId}")]
        public async Task<ActionResult<IEnumerable<DistributorToExecutiveResource>>> GetDistributorsByExecutiveId(string ExecutiveId)
        {
            var result = await _distributorToExecutiveService.AssignedDistributorsByExecutiveId(ExecutiveId);
            var mapped = _mapper.Map<IEnumerable<DistributorToExecutive>, IEnumerable<DistributorToExecutiveResource>>(result);
            return Ok(mapped);
        }

        /// <summary>
        /// GETTING ASSIGNED DISTRIBUTOR DETAILS BY EXECUTIVE ID
        /// </summary>


        [HttpGet("Details/{ExecutiveId}")]
        public async Task<ActionResult<IEnumerable<DistributorToExecutiveResource>>> GetAssignedDistributorDetailsByExecutiveId(string ExecutiveId)
        {
            var result = await _distributorToExecutiveService.AssignedDistributorDetailsByExecutiveId(ExecutiveId);
            var mapped = _mapper.Map<IEnumerable<DistributorToExecutive>, IEnumerable<DistributorToExecutiveResource>>(result);
            return Ok(mapped);
        }

        /// <summary>
        /// ASSGINING DISTRIBUTOR TO EXECUTIVE
        /// </summary>


        [HttpPost]
        public async Task<ActionResult<ResultResponse>> AssignDistributortoExecutive([FromBody] AssignDistributorToExecutiveResource distributorToExecutiveResources)
        {
            var AssignedResult = new ResultResponse();

            foreach (var distributor in distributorToExecutiveResources.DistributorIds)
            {
                var ExecutiveId = distributorToExecutiveResources.ExecutiveId;
                var lst = new InsertDEmapper();
                lst.ExecutiveId = ExecutiveId;
                lst.DistributorId = distributor;
                var mappedResult = _mapper.Map<InsertDEmapper, DistributorToExecutive>(lst);
                AssignedResult = await _distributorToExecutiveService.AssignDistributorsToExecutive(mappedResult);
            }
            return StatusCode(AssignedResult.StatusCode, AssignedResult);
        }

    }
}