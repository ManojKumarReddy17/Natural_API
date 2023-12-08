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


        [HttpGet]
        public async Task<IEnumerable<ExecutiveGetResource>> GetExecutives()
        {
            var exec = await _executiveService.GetAllExecutives();
            var execget=_mapper.Map<IEnumerable<Executive>,IEnumerable<ExecutiveGetResource>>(exec);
            return execget;
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ExecutiveResponse>> GetExecutiveById(string id)
        {
            var executive = await _executiveService.GetExecutiveById(id);
            var exec = _mapper.Map<Executive, ExecutiveGetResource>(executive);
            return Ok(exec);
        }


        [HttpGet("details/{id}")]
  
        public async Task<ActionResult<ExecutiveResponse>> GetExecutiveDetailsById(string id)
        {
            var executive = await _executiveService.GetExecutiveDetailsById(id);
            var exec = _mapper.Map<Executive, ExecutiveGetResource>(executive);
            return Ok(exec);
        }

    

        [HttpPut("{id}")]
        public async Task<ActionResult<ExecutiveResponse>> UpdateExecutive(string id , [FromBody] InsertUpdateResource saveExecutiveResource)
        {
            var exectutivetobrupade = await _executiveService.GetExecutiveById(id);
            
            var executive = _mapper.Map<InsertUpdateResource, Executive>(saveExecutiveResource);
          var Updateresponse =  await _executiveService.UpadateExecutive(exectutivetobrupade, executive);
           
            return StatusCode(Updateresponse.StatusCode,Updateresponse);
        }

        [HttpPost]
        public async Task<ActionResult<ExecutiveResponse>> InsertExecutiveWithAssociations([FromBody] InsertUpdateResource executiveResource)
        {
            var createexecu = _mapper.Map<InsertUpdateResource, Executive>(executiveResource);
            var exe = await _executiveService.CreateExecutiveWithAssociationsAsync(createexecu);

            return StatusCode(exe.StatusCode, exe);
        }
        [HttpDelete("{execId}")]
        public async Task<ActionResult<ExecutiveResponse>> DeleteExecutive(string execId)
        {

            var response = await _executiveService.DeleteExecutive(execId);
            return Ok(response);
        }

    }
}

