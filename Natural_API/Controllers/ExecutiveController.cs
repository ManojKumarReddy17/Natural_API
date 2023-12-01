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
        }
    }
}
