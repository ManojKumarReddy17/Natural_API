using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Natural_API.Resources;
using Natural_Core.IServices;
using Natural_Core.Models;
using Natural_Core.S3_Models;
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
        public async Task<IEnumerable<GetExecutive>> GetAllExecutiveDetailsAsync(string? prefix)
        {
            var executive = await _executiveService.GetAllExecutiveDetailsAsync(prefix);
            return executive;
        }

        /// <summary>
        /// GETTING EXECUTIVE BY ID
        /// </summary>

        [HttpGet("{ExecutiveId}")]
        public async Task<ActionResult<Executive>> GetExecutiveById(string ExecutiveId)
        {
            var executive = await _executiveService.GetExecutivePresignedUrlbyId(ExecutiveId);
            return Ok(executive);
        }

        
        /// <summary>
        /// GETTING EXECUTIVE DETAILS BY ID
        /// </summary>
       
        [HttpGet("details/{ExecutiveId}")]
  
        public async Task<ActionResult<ResultResponse>> GetExecutiveDetailsById(string ExecutiveId)
        {
            var executive = await _executiveService.GetExecutiveDetailsPresignedUrlById(ExecutiveId);
            return Ok(executive);
        }

        /// <summary>
        /// CREATING NEW EXECUTIVE
        /// </summary>

        
        [HttpPost]
        public async Task<ActionResult<ResultResponse>> InsertExecutiveWithAssociations([FromForm] InsertUpdateResource executiveResource, string? prefix)
        {
            var file = executiveResource.UploadImage;
            var result = await _executiveService.UploadFileAsync(file, prefix);
            var createexecu = _mapper.Map<InsertUpdateResource, Executive>(executiveResource);
            createexecu.Image = result.Message;
            var exe = await _executiveService.CreateExecutiveWithAssociationsAsync(createexecu);

            return StatusCode(exe.StatusCode, exe);
        }

        /// <summary>
        /// UPDATING EXECUTIVE BY ID
        /// </summary>


        [HttpPut]
        public async Task<ActionResult<InsertUpdateResource>> UpdateExecutive(string ExecutiveId, [FromForm] InsertUpdateResource updatedexecutive, string? prefix)
        {
            
            var existingexectutive = await _executiveService.GetExecutiveByIdAsync(ExecutiveId);

            var file = updatedexecutive.UploadImage;
            if (file != null && file.Length > 0)
            {
                var result = await _executiveService.UploadFileAsync(file, prefix); //change uploadfile to image
                var mappedexecutive = _mapper.Map(updatedexecutive, existingexectutive);
                mappedexecutive.Image = result.Message;
                var Updateresponse = await _executiveService.UpadateExecutive(mappedexecutive);
                return StatusCode(Updateresponse.StatusCode, Updateresponse);
            }

            var mappedexecutive1 = _mapper.Map(updatedexecutive, existingexectutive);
            var Updateresponse1 = await _executiveService.UpadateExecutive(mappedexecutive1);

            return StatusCode(Updateresponse1.StatusCode, Updateresponse1);
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

