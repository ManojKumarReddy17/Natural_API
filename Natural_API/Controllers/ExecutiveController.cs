using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Natural_API.Resources;
using Natural_Core.IServices;
using Natural_Core.Models;
using Natural_Data.Repositories;
using Natural_Services;

namespace Natural_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ExecutiveController : ControllerBase
    {
        private readonly IExecutiveService _executiveService;
        private readonly IMapper _mapper;
        public ExecutiveController(IExecutiveService executiveService, IMapper mapper)
        {
            _executiveService = executiveService;
            _mapper = mapper;
        }

        /// <summary>
        /// GETTING LIST OF EXECUTIVE DETAILS
        /// </summary>
        [HttpGet]
        public async Task<IEnumerable<ExecutiveGetResource>> GetAllExecutives()
        {
            var exec = await _executiveService.GetAllExecutives();
            var execget=_mapper.Map<IEnumerable<Executive>,IEnumerable<ExecutiveGetResource>>(exec);
            return execget;
        }

        /// <summary>
        /// GETTING EXECUTIVE BY ID
        /// </summary>
    
        [HttpGet("{ExeId}")]
        public async Task<ActionResult<ExecutiveResponse>> GetExecutiveById(string ExeId)
        {
            var executive = await _executiveService.GetExecutiveById(ExeId);
            var exec = _mapper.Map<Executive, ExecutiveGetResource>(executive);
            return Ok(exec);
        }

        /// <summary>
        /// GETTING EXECUTIVE DETAILS BY ID
        /// </summary>
       
        [HttpGet("details/{ExeId}")]
  
        public async Task<ActionResult<ExecutiveResponse>> GetExecutiveDetailsById(string ExeId)
        {
            var executive = await _executiveService.GetExecutiveDetailsById(ExeId);
            var exec = _mapper.Map<Executive, ExecutiveGetResource>(executive);
            return Ok(exec);
        }

        /// <summary>
        /// CREATING NEW EXECUTIVE
        /// </summary>


        [HttpPost]
        public async Task<ActionResult<ExecutiveResponse>> InsertExecutiveWithAssociations([FromBody] InsertUpdateResource executiveResource)
        {
            var createexecu = _mapper.Map<InsertUpdateResource, Executive>(executiveResource);
            var exe = await _executiveService.CreateExecutiveWithAssociationsAsync(createexecu);

            return StatusCode(exe.StatusCode, exe);
        }

        /// <summary>
        /// UPDATING EXECUTIVE BY ID
        /// </summary>

        [HttpPut("{ExeId}")]
        public async Task<ActionResult<ExecutiveResponse>> UpdateExecutive(string ExeId, [FromBody] InsertUpdateResource saveExecutiveResource)
        {
            var exectutivetobrupade = await _executiveService.GetExecutiveById(ExeId);

            var executive = _mapper.Map<InsertUpdateResource, Executive>(saveExecutiveResource);
            var Updateresponse = await _executiveService.UpadateExecutive(exectutivetobrupade, executive);

            return StatusCode(Updateresponse.StatusCode, Updateresponse);
        }

        /// <summary>
        /// DELETING EXECUTIVE BY ID
        /// </summary>

        [HttpDelete("{ExeId}")]
        public async Task<ActionResult<ExecutiveResponse>> DeleteExecutive(string ExeId)
        {

            var response = await _executiveService.DeleteExecutive(ExeId);
            return Ok(response);
        }

    }
}

