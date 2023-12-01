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

        public async Task<ActionResult<ExecutiveResponse>> GetByIdExecutive( string Id)
        {
            var executive=await _executiveService.GetExecutiveById(Id);
            var exec = _mapper.Map<Executive, ExecutiveResource>(executive);
            return Ok(exec);
        }

        [HttpPost]
        public async Task<ActionResult<ExecutiveResource>> InsertExecutiveWithAssociations([FromBody] ExecutiveResource executiveResource)
        {
            var createexecu = _mapper.Map<ExecutiveResource, Executive>(executiveResource);
            var exe = await _executiveService.CreateExecutiveWithAssociationsAsync(createexecu, executiveResource.Area, executiveResource.State,executiveResource.City);

            return StatusCode(exe.StatusCode, exe);
        }
    }
}
