using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Natural_API.Resources;
using Natural_Core.IServices;
using Natural_Core.Models;


#nullable disable

namespace Natural_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StateController : ControllerBase
    {
        private readonly IStateService _stateService;
        private readonly IMapper _mapper;

        public StateController(IStateService stateService, IMapper mapper)
        {
            _stateService = stateService;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<State>>> GetStatesList()
        {
            var states = await _stateService.GetStatesAsync();
            var StatesList = _mapper.Map<IEnumerable<State>, IEnumerable<StateResource>>(states);
            return Ok(StatesList);



        }
    }
}

