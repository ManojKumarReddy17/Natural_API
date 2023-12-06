using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Natural_API.Resources;
using Natural_Core.IServices;
using Natural_Core.Models;
using Natural_Data.Repositories;

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
        public async Task<ActionResult<IEnumerable<ExecutiveResource>>> GetExecutive()
        {
            var execu = await _executiveService.GetAllExecutives();
            var executiveresource = _mapper.Map<IEnumerable<Executive>, IEnumerable<ExecutiveResource>>(execu);
            return Ok(executiveresource);
        }
      

        [HttpGet("{id}")]
        public async Task<ActionResult<ExecutiveResponse>> GetExecutiveById(string id)
        {
            var executive = await _executiveService.GetExecutiveById(id);
            var exec = _mapper.Map<Executive, ExecutiveResource>(executive);
            return Ok(exec);
        }


        [HttpGet("{id}/details")]
        public async Task<ActionResult<ExecutiveResponse>> GetDetailsById(string id)
        {
            var executive = await _executiveService.GetDetailsById(id);
            var exec = _mapper.Map<Executive, ExecutiveResource>(executive);
            return Ok(exec);
        }

    

        [HttpPut("{id}")]
        public async Task<ActionResult<ExecutiveResource>> UpdateExecutive(string id , [FromBody]ExecutiveResource saveExecutiveResource)
        {
            var exectutivetobrupade = await _executiveService.GetExecutiveById(id);
            
            var executive = _mapper.Map<ExecutiveResource, Executive>(saveExecutiveResource);
          var Updateresponse =  await _executiveService.UpadateExecutive(exectutivetobrupade, executive);
           
            return StatusCode(Updateresponse.StatusCode,Updateresponse);
        }


    }
}
