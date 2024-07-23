using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Natural_API.Resources;
using Natural_Core.IServices;
using Natural_Core.Models;
using Natural_Core.S3_Models;
using Natural_Services;
using System.Net.WebSockets;

#nullable disable

namespace Natural_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DistributorController : ControllerBase
    {
        private readonly IDistributorService _DistributorService;
        private readonly IMapper _mapper;
        public DistributorController(IDistributorService DistributorService, IMapper mapper)
        {
            _DistributorService = DistributorService;
            _mapper = mapper;

        }

        /// <summary>
        ///GETTING LIST OF DISTRIBUTOR DETAILS
        /// </summary>

        [HttpGet]
        public async Task<ActionResult<Pagination<DistributorGetResource>>> GetAllDistributorDetails(string? prefix, [FromQuery] SearchModel? search, bool? nonAssign,int? page)
        {
            var getAllDistributors = await _DistributorService.GetAllDistributorDetailsAsync(prefix, search, nonAssign,page);
            var distributors = _mapper.Map<Pagination<Distributor>, Pagination<DistributorGetResource>>(getAllDistributors);
            return Ok(distributors);
        }

        /// <summary>
        ///GETTING DISTRIBUTOR DETAILS BY ID
        /// </summary>

        [HttpGet("Details/{DistributorId}")]

        public async Task<ActionResult<DistributorGetResource>> GetDistributorDetailsById(string DistributorId)
        {
            var distributor = await _DistributorService.GetDistributorDetailsById(DistributorId);
            
            return Ok(distributor);
        }

        /// <summary>
        /// CREATING NEW DISTRIBUTOR
        /// </summary>

        [HttpPost]
        public async Task<ActionResult<ResultResponse>> InsertDistributorWithAssociations([FromForm] InsertUpdateResource distributorResource, string? prefix)
        {
            var file = distributorResource.UploadImage;
            var distributor = _mapper.Map<InsertUpdateResource, Distributor>(distributorResource);
            //distributor.Area = distributorResource.Area[0].Area;
            if (file != null)
            {
                var result = await _DistributorService.UploadFileAsync(file, prefix);
                distributor.Image = result.Message;
            }
            var createDistributorResponse = await _DistributorService.CreateDistributorWithAssociationsAsync(distributor);
            return StatusCode(createDistributorResponse.StatusCode, createDistributorResponse);
        }

        /// <summary>
        /// UPDATING DISTRIBUTOR BY ID
        /// </summary>

        [HttpPut]
        public async Task<ActionResult<InsertUpdateResource>> UpdateDistributor(string DistributorId, [FromForm] InsertUpdateResource updatedistributor, string? prefix)
        {

            var ExistingDistributor = await _DistributorService.GetDistributorById(DistributorId);

            var file = updatedistributor.UploadImage;
            var distributorToUpdate = _mapper.Map(updatedistributor, ExistingDistributor);
            //distributorToUpdate.Area = updatedistributor.Area[0].Area;
            distributorToUpdate.Id = DistributorId;
            if (file != null && file.Length > 0)
            {
                var result = await _DistributorService.UploadFileAsync(file, prefix); //change uploadfile to image
                distributorToUpdate.Image = result.Message;
            }
            var update = await _DistributorService.UpdateDistributor(distributorToUpdate);
            return StatusCode(update.StatusCode, update);

        }


        /// <summary>
        /// DELETING DISTRIBUTOR BY ID
        /// </summary>

        [HttpDelete("{DistributorId}")]
        public async Task<ActionResult<ResultResponse>> DeleteDistributor(string DistributorId)
        {
            var response = await _DistributorService.SoftDelete(DistributorId);

            return response;
        }

        [HttpPost("Login")]
        public async Task<ActionResult<AngularDistributor>> Login([FromBody] AngularLoginResourse loginModel)
        {
            var credentials = _mapper.Map<AngularLoginResourse, Distributor>(loginModel);
            var user = await _DistributorService.LoginAsync(credentials);

            return StatusCode(user.Statuscode, user);
        }

        }

}





