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
using Serilog;

namespace Natural_API.Controllers
{   
    [Route("api/[controller]")]
    [ApiController]
    public class ExecutiveController : ControllerBase
    {
        private readonly IExecutiveService _executiveService;
        private readonly IMapper _mapper;
        private readonly ILogger<ExecutiveController> _logger;
        public ExecutiveController(IExecutiveService executiveService, IMapper mapper, ILogger<ExecutiveController> logger)
        {
            _executiveService = executiveService;
            _mapper = mapper;
            _logger = logger;
        }

        /// <summary>
        /// GETTING LIST OF EXECUTIVE DETAILS
        /// </summary>

        [HttpGet]
        public async Task<IEnumerable<ExecutiveGetResource>> GetAllExecutives(string? prefix, [FromQuery] SearchModel? search)


        {
            Log.Information("Starting GetAllExecutives method with prefix: {Prefix} and search model: {SearchModel}", prefix, search);

            try
            {
                var executiveDetails = await _executiveService.GetAllExecutiveDetailsAsync(prefix, search);

                if (executiveDetails == null || !executiveDetails.Any())
                {
                    Log.Warning("No executives found for prefix: {Prefix} and search model: {SearchModel}", prefix, search);
                    return Enumerable.Empty<ExecutiveGetResource>();
                }

                var execget = _mapper.Map<IEnumerable<InsertUpdateModel>, IEnumerable<ExecutiveGetResource>>(executiveDetails);
                Log.Information("Successfully retrieved and mapped executive details for prefix: {Prefix} and search model: {SearchModel}", prefix, search);
                return execget;
            }
            catch (Exception ex)
            {
                Log.Error(ex.Message,"Executive Controller"+ "Error occurred while retrieving executives for prefix: {Prefix} and search model: {SearchModel}", prefix, search);
                throw;
            }
            finally
            {
                Log.Information("Completed GetAllExecutives method for prefix: {Prefix} and search model: {SearchModel}", prefix, search);
            }

            

        }


        /// <summary>
        /// GETTING EXECUTIVE DETAILS BY ID
        /// </summary>

        [HttpGet("details/{ExecutiveId}")]

        public async Task<ActionResult<ResultResponse>> GetExecutiveDetailsById(string ExecutiveId)
        {
            Log.Information("Starting GetExecutiveDetailsById method with ExecutiveId: {ExecutiveId}", ExecutiveId);
            try
            {



                var executive = await _executiveService.GetExecutiveDetailsById(ExecutiveId);
                var executivearea = await _executiveService.GetExectiveAreaDetailsByIdAsync(ExecutiveId);
                var exectivearealist = _mapper.Map<List<ExecutiveArea>, List<ExecutiveAreaResource>>(executivearea).Select(a => a.Area).ToList();
                executive.Area = exectivearealist;
                var executiveareaId = await _executiveService.GetExecutiveAreaById(ExecutiveId);
                var exectiveareaIdlist = _mapper.Map<List<ExecutiveArea>, List<ExecutiveAreaResource>>(executiveareaId).Select(a => a.Area).ToList();
                executive.AreaId = exectiveareaIdlist;
                Log.Information("Successfully retrieved executive details for ExecutiveId: {ExecutiveId}", ExecutiveId);

                return Ok(executive);
            }
            catch (Exception ex)
            {
                Log.Error(ex.Message, "Executive Controller" + "Error occurred while retrieving executive details for ExecutiveId: {ExecutiveId}", ExecutiveId);
                return StatusCode(500);
            }
            finally
            {
                Log.Information("Completed DeleteExecutive method for ExecutiveId: {ExecutiveId}", ExecutiveId);
            }

        }

        /// <summary>
        /// CREATING NEW EXECUTIVE
        /// </summary>


        [HttpPost]

        public async Task<ActionResult<ProductResponse>> InsertExecutiveWithAssociations([FromForm] InsertUpdateResource executiveResource, string? prefix)

        {
            Log.Information("Starting InsertExecutiveWithAssociations method");

            try
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
            catch (Exception ex)
            {
                Log.Error(ex.Message, "Executive Controller" + "Error occurred while inserting executive with associations");
                return StatusCode(500);
            }
        }

        /// <summary>
        /// UPDATING EXECUTIVE BY ID
        /// </summary>





        [HttpPut]

        public async Task<ActionResult<InsertUpdateResource>> UpdateExecutive([FromForm] InsertUpdateResource updatedexecutive, string? prefix)
        {
            Log.Information("Starting UpdateExecutive method");
            try
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
            catch (Exception ex)
            {
                Log.Error(ex.Message, "Executive Controller" + "Error occurred while updating executive");
                return StatusCode(500);
            }
        }


        /// <summary>
        /// DELETING EXECUTIVE BY ID
        /// </summary>

        [HttpDelete("{ExecutiveId}")]

        public async Task<ActionResult<ResultResponse>> DeleteExecutive(string ExecutiveId)
        {
            Log.Information("Starting DeleteExecutive method for ExecutiveId: {ExecutiveId}", ExecutiveId);

            try
            {
                var response = await _executiveService.DeleteExecutive(ExecutiveId);
                Log.Information("Successfully deleted executive with ExecutiveId: {ExecutiveId}", ExecutiveId);
                return Ok(response);
            }
            catch (Exception ex)
            {
                Log.Error(ex.Message, "Executive Controller" + "Error occurred while deleting executive with ExecutiveId: {ExecutiveId}", ExecutiveId);
                return StatusCode(500);
            }
            finally
            {
                Log.Information("Completed DeleteExecutive method for ExecutiveId: {ExecutiveId}", ExecutiveId);
            }
          
        }


        [HttpPost("Login")]
        public async Task<ActionResult<AngularLoginResponse>> Login([FromBody] AngularLoginResourse loginModel)
        {
            try
            {


                var credentials = _mapper.Map<AngularLoginResourse, Executive>(loginModel);
                var user = await _executiveService.LoginAsync(credentials);

                return StatusCode(user.Statuscode, user);
            }
            catch (Exception ex)
            {
                Log.Error(ex.Message,"Executive Controller"+"Error Occured While login:{login}",loginModel);
                return StatusCode(500);
            }

        }



    }
}

