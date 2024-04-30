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
    public class ExecutiveGPSController : ControllerBase
    {
        private readonly IExecutiveGpsService _executiveGpsService;
        private readonly IMapper _mapper;

        public ExecutiveGPSController(IExecutiveGpsService executiveGpsService, IMapper mapper)
        {
            _executiveGpsService = executiveGpsService;
            _mapper = mapper;
        }

        [HttpGet("{ExecutiveId}")]
        public async Task<ActionResult<ResultResponse>> GetExecutiveById(string ExecutiveId)
        {
            var executive = await _executiveGpsService.GetExeId(ExecutiveId);
            //var exec = _mapper.Map<ExecutiveGpsResource, ExecutiveGp>(executive);
            return Ok(executive);
        }


        [HttpPost("CreateOrUpdateExe")]
        public async Task<ActionResult<ResultResponse>> CreateOrUpdateExecutive([FromBody] ExecutiveGpsResource executive)
        {

            //  var executiveGp = _mapper.Map<IEnumerable<ExecutiveGp>, IEnumerable<ExecutiveGpsResource>>(executive);

             var executiveGp = _mapper.Map<ExecutiveGpsResource, ExecutiveGp>(executive);
            var result = await _executiveGpsService.CreateOrUpdate(executiveGp);
            return StatusCode(result.StatusCode, result);
        }


    }

}

