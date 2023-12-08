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
        public async Task<IEnumerable<ExecutiveResource>> GetExecutives()
        {
            var exec = await _executiveService.GetAllExecutive();
            var execget=_mapper.Map<IEnumerable<Executive>,IEnumerable<ExecutiveResource>>(exec);
            return execget;
        }
        [HttpPost]
        public async Task<ActionResult<ExecutiveResponse>> InsertRetailorWithAssociations([FromBody] ExecutiveResource executiveResource)
        {
            var insertexec = _mapper.Map<ExecutiveResource, Executive>(executiveResource);
            var crtexec = await _executiveService.CreateExecutiveWithAssociationsAsync(insertexec, executiveResource.Area, executiveResource.City, executiveResource.State);
            return StatusCode(crtexec.StatusCode, crtexec);
        public async Task<ActionResult<IEnumerable<ExecutiveResource>>> GetExecutive()
        public async Task<ActionResult<IEnumerable<ExecutiveGetResource>>> GetExecutive()
        {
            var execu = await _executiveService.GetAllExecutives();
            var executiveresource = _mapper.Map<IEnumerable<Executive>, IEnumerable<ExecutiveGetResource>>(execu);
            return Ok(executiveresource);
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
        public async Task<ActionResult<ExecutiveResponse>> UpdateExecutive(string id , [FromBody] ExecutiveInsertUpdateResource saveExecutiveResource)
        {
            var exectutivetobrupade = await _executiveService.GetExecutiveById(id);
            
            var executive = _mapper.Map<ExecutiveInsertUpdateResource, Executive>(saveExecutiveResource);
          var Updateresponse =  await _executiveService.UpadateExecutive(exectutivetobrupade, executive);
           
            return StatusCode(Updateresponse.StatusCode,Updateresponse);
        }

        [HttpPost]
        public async Task<ActionResult<ExecutiveResponse>> InsertExecutiveWithAssociations([FromBody] ExecutiveInsertUpdateResource executiveResource)
        {
            var createexecu = _mapper.Map<ExecutiveInsertUpdateResource, Executive>(executiveResource);
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

