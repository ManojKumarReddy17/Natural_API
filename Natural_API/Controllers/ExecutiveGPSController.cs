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
        private readonly ILogger<ExecutiveGPSController> _logger;

        public ExecutiveGPSController(IExecutiveGpsService executiveGpsService, IMapper mapper, ILogger <ExecutiveGPSController>logger)
        {
            _executiveGpsService = executiveGpsService;
            _mapper = mapper;
            _logger = logger;
        }
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ExecutiveGp>>> GetAll()
        {
            try
            {
                var cat = await _executiveGpsService.GetAllLatLung();
                return Ok(cat);

            }
            catch (Exception ex)
            {
                _logger.LogError("ExecutiveGPSController" + "GetAll" + ex.Message);
                return StatusCode(500, "An error occured while processing your request");
            }

        }

        [HttpGet("{ExecutiveId}")]
        public async Task<ActionResult<ResultResponse>> GetExecutiveById(string ExecutiveId)
        {
            try
            {


                var executive = await _executiveGpsService.GetExeId(ExecutiveId);
                return Ok(executive);
            }
            catch (Exception ex)
            {
                _logger.LogError("ExecutiveGPSController" + "GetExecutiveById" + ex.Message);
                return StatusCode(500, "An error occured while processing your request");
            }
        }


        [HttpPost("CreateOrUpdateExe")]
        public async Task<ActionResult<ResultResponse>> CreateOrUpdateExecutive([FromBody] ExecutiveGpsResource executive)
        {
            try
            {




                var executiveGp = _mapper.Map<ExecutiveGpsResource, ExecutiveGp>(executive);
                var result = await _executiveGpsService.CreateOrUpdate(executiveGp);
                return StatusCode(result.StatusCode, result);
            }
            catch (Exception ex)
            {
                _logger.LogError("ExecutiveGPSController" + "CreateOrUpdateExecutive" + ex.Message);
                return StatusCode(500, "An error occured while processing your request");
            }
        }


    }

}

