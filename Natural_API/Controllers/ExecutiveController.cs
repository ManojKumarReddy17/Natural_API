using System.Collections.Generic;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Natural_API.Resources;
using Natural_Core.IServices;
using Natural_Core.Models;
using Natural_Core.S3_Models;
using Natural_Core.S3Models;
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
        public async Task<IEnumerable<ExecutiveGetResource>> GetAllExecutives(string? prefix, [FromQuery] SearchModel? search)


        {
            var executive = await _executiveService.GetAllExecutiveDetailsAsync(prefix, search);
            var execget = _mapper.Map<IEnumerable<InsertUpdateModel>, IEnumerable<ExecutiveGetResource>>(executive);
            return execget;

        }


        /// <summary>
        /// GETTING EXECUTIVE DETAILS BY ID
        /// </summary>

        [HttpGet("details/{ExecutiveId}")]

        public async Task<ActionResult<ResultResponse>> GetExecutiveDetailsById(string ExecutiveId)
        {

            var executive = await _executiveService.GetExecutiveDetailsById(ExecutiveId);
            var executivearea = await _executiveService.GetExectiveAreaDetailsByIdAsync(ExecutiveId);
            var exectivearealist = _mapper.Map<List<ExecutiveArea>, List<ExecutiveAreaResource>>(executivearea).Select(a => a.Area).ToList();
            executive.Area = exectivearealist;
            var executiveareaId = await _executiveService.GetExecutiveAreaById(ExecutiveId);
            var exectiveareaIdlist = _mapper.Map<List<ExecutiveArea>, List<ExecutiveAreaResource>>(executiveareaId).Select(a => a.Area).ToList();
            executive.AreaId = exectiveareaIdlist;
            return Ok(executive);

        }

        /// <summary>
        /// CREATING NEW EXECUTIVE
        /// </summary>


        [HttpPost]

        public async Task<ActionResult<ProductResponse>> InsertExecutiveWithAssociations([FromForm] InsertUpdateResource executiveResource, string? prefix)

        {
            var file = executiveResource.UploadImage;
            if (file != null && file.Length > 0)
            {

                var result = await _executiveService.UploadFileAsync(file, prefix);
                var createexecu = _mapper.Map<InsertUpdateResource, Executive>(executiveResource);

                createexecu.Image = result.Message;

                var executivearea = executiveResource.Area;
                var exectivearealist = _mapper.Map<List<ExecutiveAreaResource>, List<ExecutiveArea>>(executivearea);
                var exe = await _executiveService.CreateExecutiveAsync(createexecu, exectivearealist);


                return StatusCode(exe.StatusCode, exe);

            }
            else

            {
                var createexecu = _mapper.Map<InsertUpdateResource, Executive>(executiveResource);
                var executivearea = executiveResource.Area;
                var exectivearealist = _mapper.Map<List<ExecutiveAreaResource>, List<ExecutiveArea>>(executivearea);
                var exe = await _executiveService.CreateExecutiveAsync(createexecu, exectivearealist);


                return StatusCode(exe.StatusCode, exe);


            }
        }

        /// <summary>
        /// UPDATING EXECUTIVE BY ID
        /// </summary>





        [HttpPut]

        public async Task<ActionResult<InsertUpdateResource>> UpdateExecutive([FromForm] InsertUpdateResource updatedexecutive, string? prefix)
        {

            var file = updatedexecutive.UploadImage;
            if (file != null && file.Length > 0)
            {
                var result = await _executiveService.UploadFileAsync(file, prefix);
                var updaeexecu = _mapper.Map<InsertUpdateResource, Executive>(updatedexecutive);
                updaeexecu.Image = result.Message;
                var executivearea = updatedexecutive.Area;
                var exectivearealist = _mapper.Map<List<ExecutiveAreaResource>, List<ExecutiveArea>>(executivearea);

                var exe = await _executiveService.UpadateExecutive(updaeexecu, exectivearealist, updaeexecu.Id);
                return StatusCode(exe.StatusCode, exe);

            }
            else
            {
                var updaeexecu1 = _mapper.Map<InsertUpdateResource, Executive>(updatedexecutive);
                var executivearea = updatedexecutive.Area;
                var exectivearealist = _mapper.Map<List<ExecutiveAreaResource>, List<ExecutiveArea>>(executivearea);
                var exe = await _executiveService.UpadateExecutive(updaeexecu1, exectivearealist, updaeexecu1.Id);
                return StatusCode(exe.StatusCode, exe);
            }
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


        [HttpPost("Login")]
        public async Task<ActionResult<AngularLoginResponse>> Login([FromBody] AngularLoginResourse loginModel)
        {
            var credentials = _mapper.Map<AngularLoginResourse, Executive>(loginModel);
            var user = await _executiveService.LoginAsync(credentials);

            return StatusCode(user.Statuscode, user);

        }



    }
}

