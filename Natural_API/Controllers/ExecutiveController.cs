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
            var executive = await _executiveService.GetAllExecutives();
            var execget=_mapper.Map<IEnumerable<Executive>,IEnumerable<ExecutiveGetResource>>(executive);
            return execget;
        }

        /// <summary>
        /// GETTING EXECUTIVE BY ID
        /// </summary>
    
        [HttpGet("{ExecutiveId}")]
        public async Task<ActionResult<ResultResponse>> GetExecutiveById(string ExecutiveId)
        {
            var executive = await _executiveService.GetExecutiveById(ExecutiveId);
            var exec = _mapper.Map<Executive, ExecutiveGetResource>(executive);
            return Ok(exec);
        }

        /// <summary>
        /// GETTING EXECUTIVE DETAILS BY ID
        /// </summary>
       
        [HttpGet("details/{ExecutiveId}")]
  
        public async Task<ActionResult<ResultResponse>> GetExecutiveDetailsById(string ExecutiveId)
        {
            var executive = await _executiveService.GetExecutiveDetailsById(ExecutiveId);
            var exec = _mapper.Map<Executive, ExecutiveGetResource>(executive);
            return Ok(exec);
        }

        /// <summary>
        /// CREATING NEW EXECUTIVE
        /// </summary>

        [HttpPost]
        public async Task<ActionResult<ResultResponse>> InsertExecutiveWithAssociations([FromBody] InsertUpdateResource executiveResource)
        {
            var createexecu = _mapper.Map<InsertUpdateResource, Executive>(executiveResource);
            var exe = await _executiveService.CreateExecutiveWithAssociationsAsync(createexecu);

            return StatusCode(exe.StatusCode, exe);
        }

        /// <summary>
        /// UPDATING EXECUTIVE BY ID
        /// </summary>

        [HttpPut("{ExecutiveId}")]
        public async Task<ActionResult<InsertUpdateResource>> UpdateExecutive(string ExecutiveId, [FromBody] InsertUpdateResource updatedexecutive)
        {
            var existingexectutive= await _executiveService.GetExecutiveById(ExecutiveId);

            var mappedexecutive = _mapper.Map(updatedexecutive,existingexectutive);
            var Updateresponse = await _executiveService.UpadateExecutive(mappedexecutive);

            return StatusCode(Updateresponse.StatusCode, Updateresponse);
        }

        /// <summary>
        /// DELETING EXECUTIVE BY ID
        /// </summary>

        [HttpDelete("{ExecutiveId}")]
        public async Task<ActionResult<ResultResponse>> DeleteExecutive(string ExecutiveId)
        {
            var response = await _executiveService.DeleteExecutive(ExecutiveId);
            return Ok(response);
        }


        /// <summary>
        /// SEARCH EXECUTIVE 
        /// </summary>
        /// 
        [HttpPost("Search")]
        public async Task<IEnumerable<ExecutiveGetResource>> SearchExecutive([FromBody] SearchModel search)
        {
           var exe = await _executiveService.SearchExecutives(search);
            var execget = _mapper.Map<IEnumerable<Executive>, IEnumerable<ExecutiveGetResource>>(exe);
            return execget;
        }

        [HttpPost("Login")]
        public async Task<ActionResult<AngularLoginResponse>> Login([FromBody] AngularLoginResourse loginModel)
        {
            var credentials = _mapper.Map<AngularLoginResourse, Executive>(loginModel);
            var user = await _executiveService.LoginAsync(credentials);

            return StatusCode(user.Statuscode, user);

        }



    }
}

